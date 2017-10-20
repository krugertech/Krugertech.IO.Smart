using System;
using System.Linq;
using Simplified.IO;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Demo of Simplified.IO.Smart class";
                 
            try
            {                                
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
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);                
            }

            Console.ReadLine();
        }
    }
}
