using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var winBar = WinApi.FindWindow(null, "Adobe Reader");
                Console.WriteLine($"Adobe Reader bar is:{winBar}");
                if (winBar != IntPtr.Zero)
                {
                    var childs = WinApi.EnumChildWindowsCallback(winBar);
                    WinApi.LeftClickMsg(childs.Find(b=>b.szWindowName.Contains("不在显示本消息")).hWnd);
                    Console.WriteLine("选择不在显示本消息");
                    Thread.Sleep(500);
                    WinApi.LeftClickMsg(childs.Find(b=>b.szWindowName=="确定").hWnd);
                    Console.WriteLine("点击确定");
                }
                Console.WriteLine("没有找到Adobe Reader窗体");
                Thread.Sleep(10000);
            }
        }
    }
}
