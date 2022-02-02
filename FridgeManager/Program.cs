using System;
using System.Configuration;

namespace FridgeManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var libName = ConfigurationManager.AppSettings["dbName"];
            BLC.BLC blc = BLC.BLC.GetBLC();

            foreach ( Interfaces.IFood c in blc.GetFood())
            {
                Console.WriteLine(c.Name);
            }
            Console.ReadLine();
        }
    }
}
