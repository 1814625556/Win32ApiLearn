using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace UploadNuget
{
    class Program
    {
        static void Main(string[] args)
        {



            args = new [] {@"C:\MyDatas\VsProjects\demo\SimulationMouseKeyboard\NugetClass1\NugetClass1.csproj",
                "1.0.0.0" };

            XmlHelper.UpdateXml(@"C:\MyDatas\VsProjects\demo\SimulationMouseKeyboard\NugetClass1\NugetClass1.csproj.nuspec", 
                "id", "chenchang");

            RunCmd("nuget.exe", $"spec {args[0]}");
            Console.ReadKey();
        }

        static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"cmd 调用失败：{e.Message}");
            }
            return result;
        }
    }
}
