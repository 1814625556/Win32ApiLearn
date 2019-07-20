using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace bat1
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length != 0)
            {
                foreach (var s in args)
                {
                    Console.WriteLine(s);
                }
            }
            Console.WriteLine("hello bat1");
            Thread.Sleep(2000);
            //throw new Exception("Hello error");
            return 0;
        }
    }
}
