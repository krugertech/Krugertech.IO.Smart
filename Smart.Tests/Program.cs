using System;
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
                    Console.WriteLine(" DRIVE ({0}): " + drive.Serial + " - " + drive.Model + " - " + drive.Type, ((drive.IsOK) ? "OK" : "BAD"));
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("");

                    Console.WriteLine("Attribute\t\t\tCurrent  Worst  Threshold  Data  Status");
                    foreach (var attr in drive.SmartAttributeAttributes)
                    {
                        if (attr.HasData)
                            Console.WriteLine("{0}\t {1}\t {2}\t {3}\t " + attr.Data + " " + ((attr.IsOK) ? "OK" : "BAD"), attr.Name, attr.Current, attr.Worst, attr.Threshold);
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
