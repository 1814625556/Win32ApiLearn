using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SingCheckTool
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args == null || args.Length < 2) return 1;
            Thread.Sleep(1000);

            var directory = args[0];
            var pattern = args[1];

            Console.WriteLine($"directory:{directory},pattern:{pattern}");

            //var files = Directory.GetFiles(@"C:\MyDatas\VsProjects\demo\打包程序临时文件\releasebak", "RK*.dll");
            var files = Directory.GetFiles(directory, pattern);

            if (files.Select(CheckHelper.CheckSignture).Any(flag => flag == false))
            {
                Console.WriteLine("有文件签名失败");
                return 1;
            }
            //CheckHelper.CheckSingle(@"C:\MyDatas\VsProjects\demo\SimulationMouseKeyboard\bat1\bin\Debug\bat1.exe");
            else
            {
                Console.WriteLine("所有文件都已签名认证");
                return 0;
            }
            
        }
    }
}
