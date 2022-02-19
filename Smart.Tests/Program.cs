using System;
using System.Linq;
using Krugertech.IO;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Demo of Krugertech.IO.Smart";

            try
            {
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
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }

            Console.ReadLine();
        }
    }
}
