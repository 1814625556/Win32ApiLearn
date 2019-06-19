using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            for (var i = 0; i < 100; i++)
            {
                var bar = WinApi.FindWindow(null, "信息表下载中");
                Console.WriteLine($"第{i}秒，bar:{bar}");
                Thread.Sleep(1000);
            }
            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
