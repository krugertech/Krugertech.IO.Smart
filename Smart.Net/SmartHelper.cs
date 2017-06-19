using System;
using System.Collections.Generic;
using System.Text;

namespace Smart.Net
{
    public static class SmartHelper
    {
        public static DriveCollection GetDrives()
        {
            return WmiController.GetSmartInformation();
        }
    }
}
