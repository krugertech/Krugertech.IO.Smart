using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart.Net;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {            
            try
            {                                
                var drives = SmartHelper.GetDrives();
                
                foreach (var drive in drives)
                {
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine(" DRIVE ({0}): " + drive.Serial + " - " + drive.Model + " - " + drive.Type, ((drive.IsOK) ? "OK" : "BAD"));
                    Console.WriteLine("-----------------------------------------------------");
                    Console.WriteLine("");

                    Console.WriteLine("ID                   Current  Worst  Threshold  Data  Status");
                    foreach (var attr in drive.SmartAttributeAttributes)
                    {
                        if (attr.HasData)
                            Console.WriteLine("{0}\t {1}\t {2}\t {3}\t " + attr.Data + " " + ((attr.IsOK) ? "OK" : "BAD"), "(" + attr.Register +")" + attr.Name, attr.Current, attr.Worst, attr.Threshold);
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                }

                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }
    }
}
