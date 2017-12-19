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
using System.Text;

namespace Simplified.IO
{
    #region Smart
    public sealed class SmartAttribute
    {        
        public SmartAttribute(int register, string attributeName)
        {
            this.Register = register;
            this.Name = attributeName;
        }

        public int Register { get; set; }
        public string Name { get; set; }

        public int Current { get; set; }
        public int Worst { get; set; }
        public int Threshold { get; set; }
        public int Data { get; set; }
        public bool IsOK { get; set; }

        public bool HasData
        {
            get
            {
                if (Current == 0 && Worst == 0 && Threshold == 0 && Data == 0)
                    return false;
                return true;
            }
        }
    }

    public class SmartAttributeCollection :List<SmartAttribute>
    {

        public SmartAttributeCollection()
        {
                  
        }

        public SmartAttribute GetAttribute(int registerID)
        {
            foreach (var item in this)
            {
                if (item.Register == registerID)
                    return item;
            }

            return null;
        }
    }
    #endregion
    
    #region Drive
    public class Drive
    {
        public Drive()
        {
            SmartAttributes = new SmartAttributeCollection();
            DriveLetters = new List<string>();
        }

        public int Index { get; set; }

        public string DeviceID { get; set; }
        public string PnpDeviceID { get; set; }

        public List<string> DriveLetters { get; set; }        
        public bool IsOK { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
        public string Serial { get; set; }
        public SmartAttributeCollection SmartAttributes { get; set; }
    }

    public class DriveCollection : List<Drive>
    {
        
    }
    #endregion
}
