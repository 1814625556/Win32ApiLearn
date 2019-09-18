using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Session
{
    class Program
    {
        static void Main(string[] args)
        {

            GetUserInfo.Demo();


            //监听鼠标键盘事件
            //SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SessionChange.SystemEvents_SessionSwitch);
            Console.ReadKey();
        }

        static bool isExist()
        {
            return false;
        }
    }
}
