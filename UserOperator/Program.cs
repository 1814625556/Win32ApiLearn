using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace UserOperator
{
    class Program
    {
        static void Main(string[] args)
        {
          
            
           Console.ReadKey();
        }


        public static void SessionEvent()
        {
            //session切换事件
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);

            //session注销，切换事件
            SystemEvents.SessionEnded += new SessionEndedEventHandler(SystemEvents_SessionEnd);
        }

        static void SystemEvents_SessionEnd(object sender, SessionEndedEventArgs e)
        {
            File.AppendAllText("SessionEnd.txt", $"{DateTime.Now:yyyyMMddhhmmss},HasShutdownStarted:{Environment.HasShutdownStarted}");
        }

        static void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
           File.AppendAllText("SessionSwitch.txt", $"{DateTime.Now:yyyyMMddhhmmss}");
        }
    }
}
