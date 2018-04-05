# SMART.Net
A class library for the reading of HDD and SSD SMART registers.

# Usage
```cs

var drives = Smart.GetDrives();
                
foreach (var drive in drives)
{
    Console.WriteLine("-----------------------------------------------------");
    Console.WriteLine($" DRIVE ({((drive.IsOK) ? "OK" : "BAD")}): {drive.Serial} - {drive.Model} - {drive.Type}");
    Console.WriteLine("-----------------------------------------------------");
    Console.WriteLine("");

    Console.WriteLine("Attribute\t\t\tCurrent  Worst  Threshold  Data  Status");
    int maxNameLen = drive.SmartAttributes.Max(s => s.Name.Length);
    foreach (var attr in drive.SmartAttributes)
    {
        if (attr.HasData)
            Console.WriteLine($"{attr.Name.PadRight(maxNameLen, ' ')} {attr.Current}\t {attr.Worst}\t {attr.Threshold}\t {attr.Data.ToString().PadRight(9, ' ')} {((attr.IsOK) ? "OK" : "BAD")}");
    }
    Console.WriteLine();
}                
```


![alt text](https://raw.githubusercontent.com/krugertech/SMART.Net/master/Exhibit.A.png)

# License
Copyright (c) 2017 Llewellyn Kruger

MIT License

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
