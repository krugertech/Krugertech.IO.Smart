using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Net
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
    
    public class Drive
    {
        public Drive()
        {
            SmartAttributeAttributes = new SmartAttributeCollection();
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
        public SmartAttributeCollection SmartAttributeAttributes { get; set; }
    }

    public class DriveCollection : List<Drive>
    {
        
    }
}
