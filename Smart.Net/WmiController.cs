/*
Copyright (c) 2017, Llewellyn Kruger
All rights reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS “AS IS” AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Text;

namespace Simplified.IO
{
    public sealed class WmiController
    {
        public static DriveCollection GetSmartInformation()
        {
            DriveCollection drives = new DriveCollection();
            try
            {                                
                foreach (var device in new ManagementObjectSearcher(@"SELECT * FROM Win32_DiskDrive").Get())
                {
                    #region Drive Info

                    Drive drive = new Drive
                    {
                        DeviceID = device.GetPropertyValue("DeviceID").ToString(),
                        PnpDeviceID = device.GetPropertyValue("PNPDeviceID").ToString(),
                        Model = device["Model"]?.ToString().Trim(),
                        Type = device["InterfaceType"]?.ToString().Trim(),
                        Serial = device["SerialNumber"]?.ToString().Trim()

                    };

                    #endregion

                    #region Get drive letters

                    foreach (var partition in new ManagementObjectSearcher(
                        "ASSOCIATORS OF {Win32_DiskDrive.DeviceID='" + device.Properties["DeviceID"].Value
                        + "'} WHERE AssocClass = Win32_DiskDriveToDiskPartition").Get())
                    {

                        foreach (var disk in new ManagementObjectSearcher(
                            "ASSOCIATORS OF {Win32_DiskPartition.DeviceID='"
                            + partition["DeviceID"]
                            + "'} WHERE AssocClass = Win32_LogicalDiskToPartition").Get())
                        {
                            drive.DriveLetters.Add(disk["Name"].ToString());
                        }

                    }

                    #endregion

                    #region Overall Smart Status       

                    ManagementScope scope = new ManagementScope("\\\\.\\ROOT\\WMI");
                    ObjectQuery query = new ObjectQuery(@"SELECT * FROM MSStorageDriver_FailurePredictStatus Where InstanceName like ""%"
                                                        + drive.PnpDeviceID.Replace("\\", "\\\\") + @"%""");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
                    ManagementObjectCollection queryCollection = searcher.Get();
                    foreach (ManagementObject m in queryCollection)
                    {
                        drive.IsOK = (bool) m.Properties["PredictFailure"].Value == false;
                    }

                    #endregion

                    #region Smart Registers

                    drive.SmartAttributeAttributes.AddRange(Helper.GetSmartRegisters(Resource.SmartAttributes));

                    searcher.Query = new ObjectQuery(@"Select * from MSStorageDriver_FailurePredictData Where InstanceName like ""%"
                                                     + drive.PnpDeviceID.Replace("\\", "\\\\") + @"%""");
                    
                    //foreach (ManagementObject data in searcher.Get())
                    //{
                    //    Byte[] bytes = (Byte[]) data.Properties["VendorSpecific"].Value;
                    //    foreach (var attribute in drive.SmartAttributeAttributes)
                    //    {
                    //        try
                    //        {
                    //            int id = bytes[attribute.Register * 12 + 2];

                    //            int flags = bytes[attribute.Register * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                    //            bool advisory = (flags & 0x1) == 0x0;
                    //            bool failureImminent = (flags & 0x1) == 0x1;
                    //            bool onlineDataCollection = (flags & 0x2) == 0x2;

                    //            int value = bytes[attribute.Register * 12 + 5];
                    //            int worst = bytes[attribute.Register * 12 + 6];
                    //            int vendordata = BitConverter.ToInt32(bytes, attribute.Register * 12 + 7);
                    //            if (id == 0) continue;

                    //            attribute.Current = value;
                    //            attribute.Worst = worst;
                    //            attribute.Data = vendordata;
                    //            attribute.IsOK = failureImminent == false;

                    //            //Console.WriteLine("{0}\t {1}\t {2}\t {3}\t " + attribute.Data + " " + ((attribute.IsOK) ? "OK" : ""), attribute.Name, attribute.Current, attribute.Worst, attribute.Threshold);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Debug.WriteLine($"Error resolving attribute data [{attribute.Name}].");
                    //        }
                    //    }
                    //}
                                        
                    foreach (ManagementObject data in searcher.Get())
                    {
                        Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                        for (int i = 0; i < 42; ++i)
                        {
                            try
                            {
                                int id = bytes[i * 12 + 2];

                                int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                                                               //bool advisory = (flags & 0x1) == 0x0;
                                bool failureImminent = (flags & 0x1) == 0x1;
                                //bool onlineDataCollection = (flags & 0x2) == 0x2;

                                int value = bytes[i * 12 + 5];
                                int worst = bytes[i * 12 + 6];
                                int vendordata = BitConverter.ToInt32(bytes, i * 12 + 7);
                                if (id == 0) continue;

                                var attr = drive.SmartAttributeAttributes.GetAttribute(id);
                                attr.Current = value;
                                attr.Worst = worst;
                                attr.Data = vendordata;
                                attr.IsOK = failureImminent == false;
                            }
                            catch
                            {
                                // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                            }
                        }                        
                    }

                    searcher.Query = new ObjectQuery(@"Select * from MSStorageDriver_FailurePredictThresholds Where InstanceName like ""%"
                                                     + drive.PnpDeviceID.Replace("\\", "\\\\") + @"%""");
                    foreach (ManagementObject data in searcher.Get())
                    {
                        Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;
                        for (int i = 0; i < 42; ++i)
                        {
                            try
                            {
                                int id = bytes[i * 12 + 2];
                                int thresh = bytes[i * 12 + 3];
                                if (id == 0) continue;

                                var attr = drive.SmartAttributeAttributes.GetAttribute(id);
                                attr.Threshold = thresh;

                                //Console.WriteLine("{0}\t {1}\t {2}\t {3}\t " + attr.Data + " " + ((attr.IsOK) ? "OK" : ""), attr.Name, attr.Current, attr.Worst, attr.Threshold);
                            }
                            catch
                            {
                                // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                            }
                        }
                    }

                    //searcher.Query = new ObjectQuery(@"Select * from MSStorageDriver_FailurePredictThresholds Where InstanceName like ""%"
                    //                                 + drive.PnpDeviceID.Replace("\\", "\\\\") + @"%""");
                    //foreach (ManagementObject data in searcher.Get())
                    //{
                    //    Byte[] bytes = (Byte[])data.Properties["VendorSpecific"].Value;

                    //    foreach (var attribute in drive.SmartAttributeAttributes)
                    //    {
                    //        try
                    //        {
                    //            int id = bytes[attribute.Register * 12 + 2];
                    //            int thresh = bytes[attribute.Register * 12 + 3];
                    //            if (id == 0) continue;

                    //            var attr = drive.SmartAttributeAttributes.GetAttribute(id);
                    //            attr.Threshold = thresh;

                    //            //Console.WriteLine("{0}\t {1}\t {2}\t {3}\t " + attribute.Data + " " + ((attribute.IsOK) ? "OK" : ""), attribute.Name, attribute.Current, attribute.Worst, attribute.Threshold);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            Debug.WriteLine($"Error resolving attribute data [{attribute.Name}].");
                    //        }
                    //    }
                    //}

                    #endregion

                    drives.Add(drive);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving Smart data for one or more drives. " + ex.Message);
            }

            return drives;
        }
    }
}
