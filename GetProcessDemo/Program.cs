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
            var list = processes.OrderBy(pro => pro.ProcessName).ToList();

            var entity = list.FirstOrDefault(p => p.ProcessName == "explorer11111");

            var sb = new StringBuilder();
            foreach (var process in list)
            {
                sb.Append($"ProcessName:{process.ProcessName}\r\n");
                sb.Append($"MainWindowHandle:{process.MainWindowHandle}\r\n");
                sb.Append($"SessionId:{process.SessionId}\r\n");
                //sb.Append($"ProcessName:{process.}\r\n");
                Console.WriteLine($"MainWindowHandle:{process.MainWindowHandle}--SessionId:{process.SessionId}");
            }
            File.WriteAllText("processName.txt",sb.ToString());
            Console.ReadKey();
        }
    }
}
