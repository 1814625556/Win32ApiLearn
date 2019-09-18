using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Session
{
    public class SessionChange
    {
        public static void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLogon:
                    Console.WriteLine("用户登录");
                    break;

                case SessionSwitchReason.SessionUnlock:
                    Console.WriteLine("解锁");
                    break;

                case SessionSwitchReason.SessionLock:
                    Console.WriteLine("锁屏");
                    break;
                case SessionSwitchReason.SessionLogoff:
                    Console.WriteLine("注销");
                    break;
            }
        }
    }
}
