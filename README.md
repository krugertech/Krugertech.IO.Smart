# Krugertech.IO.Smart

[![NuGet version (Krugertech.IO.Smart)](https://badge.fury.io/nu/Krugertech.IO.Smart.svg)](https://badge.fury.io/nu/Krugertech.IO.Smart)

A class library for the reading of HDD and SSD SMART registers.

Note: Formerly known as SMART.NET

## Breaking Changes 

1. Upgraded to .NET Standard 2.0, so older .NET Framework versions 4.6.1+ are supported
2. Name space changed from Simpliefied.IO to Krugertech.IO.Smart
3. Entry point class changed From "Smart" to "SmartDrive"

## Usage
```cs
using Krugertech.IO.Smart;
var drives = SmartDrive.GetDrives();

foreach (Drive drive in drives)
{
    string driveStatus = (!drive.IsSupported)? "NOT SUPPORTED" : ((drive.IsOK) ? "OK" : "BAD");

    Console.WriteLine("-----------------------------------------------------");
    Console.WriteLine($" DRIVE ({driveStatus}): {drive.Serial} - {drive.Model} - {drive.Type}");
    Console.WriteLine("-----------------------------------------------------");
    Console.WriteLine("");

    if (drive.IsSupported)
    {
        Console.WriteLine("Attribute\t\t\tCurrent  Worst  Threshold  Data  Status");
        int maxNameLen = drive.SmartAttributes.Max(s => s.Name.Length);
        foreach (var attr in drive.SmartAttributes)
        {
            if (attr.HasData)
                Console.WriteLine($"{attr.Name.PadRight(maxNameLen, ' ')} {attr.Current}\t {attr.Worst}\t {attr.Threshold}\t {attr.Data.ToString().PadRight(9, ' ')} {((attr.IsOK) ? "OK" : "BAD")}");
        }
    }
    Console.WriteLine();
}               
```       

![alt text](https://raw.githubusercontent.com/krugertech/SMART.Net/master/Exhibit.A.png)

## Supported Drives

| Interface | Support                         
|-----------------------|---------------------
| IDE       | :heavy_check_mark: 
| SATA      | :heavy_check_mark: 
| SCSI      | :heavy_check_mark:
| NVME      | :exclamation:
| M2        | :exclamation:
| USB       | :exclamation:


## Runtime Compatibility

| Platform | .NET runtime version | Support                         
|-----------------------|----------------------|-----------------------------------------
| Windows      | .NET Framework 4.6.1+ | :heavy_check_mark: 
| Windows      | .NET Core 2.1+      | :heavy_check_mark:                      
| Linux        | .NET Core           | :exclamation:  Not Supported         

## License
Copyright (c) 2017 Llewellyn Kruger

MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
