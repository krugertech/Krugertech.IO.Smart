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
