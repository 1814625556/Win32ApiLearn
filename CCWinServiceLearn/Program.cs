using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace CCWinServiceLearn
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1(){ServiceName = "CcWinService1 test"},
            };
            ServiceBase.Run(ServicesToRun);
        }

        //安装window 服务  InstallUtil.exe E:/test.exe
    }
}
