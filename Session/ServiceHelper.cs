using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.Text;
using System.ServiceProcess;

namespace Session
{
    public class ServiceHelper
    {
        /// <summary>判断服务是否存在</summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>是否存在</returns>
        public static bool IsExisted(string serviceName)
        {
            return ((IEnumerable<ServiceController>)ServiceController.GetServices()).
                Count<ServiceController>((Func<ServiceController, bool>)(c =>
                string.Compare(c.ServiceName, serviceName, StringComparison.OrdinalIgnoreCase) == 0)) > 0;
        }

        /// <summary>安装服务</summary>
        /// <param name="serviceFilePath">服务路径</param>
        public static void Install(string serviceFilePath)
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

        /// <summary>卸载服务</summary>
        /// <param name="serviceFilePath">服务路径</param>
        public static void Uninstall(string serviceFilePath)
        {
            using (AssemblyInstaller assemblyInstaller = new AssemblyInstaller())
            {
                assemblyInstaller.UseNewContext = true;
                assemblyInstaller.Path = serviceFilePath;
                assemblyInstaller.Uninstall((IDictionary)null);
            }
        }

        /// <summary>启动服务</summary>
        /// <param name="serviceName">服务名称</param>
        public static void Start(string serviceName)
        {
            using (ServiceController serviceController = new ServiceController(serviceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Stopped)
                    return;
                serviceController.Start();
            }
        }

        /// <summary>停止服务</summary>
        /// <param name="serviceName">服务名称</param>
        public static void Stop(string serviceName)
        {
            using (ServiceController serviceController = new ServiceController(serviceName))
            {
                if (serviceController.Status != ServiceControllerStatus.Running)
                    return;
                serviceController.Stop();
            }
        }
    }
}
