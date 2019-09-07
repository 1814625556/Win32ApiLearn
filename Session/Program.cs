using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Session
{
    class Program
    {
        static void Main(string[] args)
        {
            //SessionHelper.CreateProcess(@"C:\Windows\System32\calc.exe");
            //var cc = WinApi.WTSQueryUserToken(1, out var token);
            //OverStackSession.ExecuteAppAsLoggedOnUser(@"C:\Windows\System32\calc.exe", null);
            //SessionOne.CreateProcess(@"C:\Windows\System32\calc.exe");
            //Interops.CreateProcess("calc.exe", @"C:\Windows\System32");
            //Session3.CreateProcess(@"C:\Windows\System32\calc.exe",null,2);
            Console.ReadKey();
        }

        static bool isExist()
        {
            return false;
        }
    }
}
