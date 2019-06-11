using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        private static string serviceFilePath = @"C:\MyDatas\VsProjects\demo\SimulationMouseKeyboard\CCWinServiceLearn\bin\Debug\CCWinServiceLearn.exe";
        static void Main(string[] args)
        {

            //InstallService();
            Thread.Sleep(100);
            SearchService();
            Console.ReadKey();
        }
        /// <summary>
        /// 获取所有服务
        /// </summary>
        static void SearchService()
        {
            var service = ServiceController.GetServices().OrderBy(S=>S.ServiceName).ToList();
            for (var i = 0; i < service.Count; i++)
            {
                Console.WriteLine($"ServiceName:{service[i].ServiceName},DisplayName:{service[i].DisplayName}," +
                                  $"MachineName{service[i].MachineName}");

            }
        }
        /// <summary>
        /// 安装指定服务
        /// </summary>
        static void InstallService()
        {
            try
            {
                using (AssemblyInstaller assemblyInstaller = new AssemblyInstaller())
                {
                    assemblyInstaller.UseNewContext = true;
                    assemblyInstaller.Path = serviceFilePath;
                    IDictionary dictionary = (IDictionary)new Hashtable();
                    assemblyInstaller.Install(dictionary);
                    assemblyInstaller.Commit(dictionary);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
           
        }

        static void UninstallService()
        {
            using (AssemblyInstaller assemblyInstaller = new AssemblyInstaller())
            {
                assemblyInstaller.UseNewContext = true;
                assemblyInstaller.Path = serviceFilePath;
                assemblyInstaller.Uninstall((IDictionary)null);
            }
        }
    }
}
