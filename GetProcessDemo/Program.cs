using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GetProcessDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var processes = Process.GetProcesses();
            var list = processes.OrderBy(pro => pro.ProcessName);

            var entity = list.FirstOrDefault(p => p.ProcessName == "explorer11111");

            var sb = new StringBuilder();
            foreach (var process in list)
            {
                sb.Append($"ProcessName:{process.ProcessName}\r\n");
                Console.WriteLine(process.ProcessName);
            }
            File.WriteAllText("processName.txt",sb.ToString());
            Console.ReadKey();
        }
    }
}
