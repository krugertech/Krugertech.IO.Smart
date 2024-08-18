#region License
// Copyright (c) 2017, Llewellyn Kruger
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Diagnostics;
using System.Linq;
using System.Management;

namespace Krugertech.IO.Smart.Controllers
{
    public sealed class WmiController
    {
        public static DriveCollection GetSmartInformation()
        {
            DriveCollection drives = new DriveCollection();
            try
            {
                // TODO: 2017-12-19 - Refactor regions into separate methods.
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
                        drive.IsOK = (bool)m.Properties["PredictFailure"].Value == false;
                    }

                    #endregion

                    #region Smart Registers

                    drive.SmartAttributes.AddRange(Helper.GetSmartRegisters(Resource.SmartAttributes));

                    searcher.Query = new ObjectQuery(@"Select * from MSStorageDriver_FailurePredictData Where InstanceName like ""%"
                                                     + drive.PnpDeviceID.Replace("\\", "\\\\") + @"%""");

                    foreach (ManagementObject data in searcher.Get())
                    {
                        byte[] bytes = (byte[])data.Properties["VendorSpecific"].Value;
                        for (int i = 0; i < 42; ++i)
                        {
                            try
                            {
                                int id = bytes[i * 12 + 2];

                                int flags = bytes[i * 12 + 4]; // least significant status byte, +3 most significant byte, but not used so ignored.
                                                               //bool advisory = (flags & 0x1) == 0x0;
                                bool failureImminent = (flags & 0x1) == 0x1;
                                //bool onlineDataCollection = (flags & 0x2) == 0x2;

                                uint value = bytes[i * 12 + 5];
                                uint worst = bytes[i * 12 + 6];
                                uint vendordata = BitConverter.ToUInt32(bytes, i * 12 + 7);
                                if (id == 0) continue;

                                var attr = drive.SmartAttributes.GetAttribute(id);
                                if (attr != null)
                                {
                                    attr.Current = value;
                                    attr.Worst = worst;
                                    attr.Data = vendordata;
                                    attr.IsOK = failureImminent == false;
                                }
                            }
                            catch (Exception ex)
                            {
                                // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }

                    searcher.Query = new ObjectQuery(@"Select * from MSStorageDriver_FailurePredictThresholds Where InstanceName like ""%"
                                                     + drive.PnpDeviceID.Replace("\\", "\\\\") + @"%""");
                    foreach (ManagementObject data in searcher.Get())
                    {
                        byte[] bytes = (byte[])data.Properties["VendorSpecific"].Value;
                        for (int i = 0; i < 42; ++i)
                        {
                            try
                            {
                                int id = bytes[i * 12 + 2];
                                uint thresh = bytes[i * 12 + 3];
                                if (id == 0) continue;

                                var attr = drive.SmartAttributes.GetAttribute(id);
                                if (attr != null)
                                {
                                    attr.Threshold = thresh;

                                    // Debug
                                    // Console.WriteLine("{0}\t {1}\t {2}\t {3}\t " + attr.Data + " " + ((attr.IsOK) ? "OK" : ""), attr.Name, attr.Current, attr.Worst, attr.Threshold);
                                }
                            }
                            catch (Exception ex)
                            {
                                // given key does not exist in attribute collection (attribute not in the dictionary of attributes)
                                Debug.WriteLine(ex.Message);
                            }
                        }
                    }

                    #endregion

                    drive.IsSupported = drive.SmartAttributes.Where(sa => sa.HasData).Any();

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
