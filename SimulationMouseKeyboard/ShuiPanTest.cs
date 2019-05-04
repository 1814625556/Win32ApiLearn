using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SimulationMouseKeyboard
{
    public class ShuiPanTest
    {
        //模拟键盘发送案按键
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);
        /// <summary>
        /// 税盘最大化最小化
        /// </summary>
        public static void test1()
        {
            var bar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");
            Thread.Sleep(1000);
            WinApi.ShowWindow(bar, 2);
            Thread.Sleep(1000);
            WinApi.ShowWindow(bar, 3);
            Thread.Sleep(1000);
            WinApi.ShowWindow(bar, 2);
            Thread.Sleep(1000);
            WinApi.ShowWindow(bar, 3);
        }
        /// <summary>
        /// 点击成品油测试
        /// </summary>
        public static void test2()
        {
            var bar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");
            var barChild = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);//获取句柄成功

            for (var i = 0; i < 10; i++)
            {
                WinApi.ClickLocation(barChild, 30, 28);
                Thread.Sleep(1000);
                WinApi.ClickLocation(barChild, 90, 28);
                Thread.Sleep(1000);
                //var childone = WinApi.FindWindowEx(barChild, IntPtr.Zero, null, null);
            }

            int fpglHw = WinApi.getHwByTitle((int)bar, "成品油");//这里改下标题就好了

        }

        /// <summary>
        /// 获取子控件句柄
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static List<WindowInfo> EnumChildWindowsCallback(IntPtr handle)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            WinApi.EnumChildWindows(handle, delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                wnd.hWnd = hWnd;
                WinApi.GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                WinApi.GetClassName(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                wnd.parentHWnd = WinApi.GetParent(hWnd);
                wnd.id = (int)hWnd;
                wndList.Add(wnd);
                return true;
            }, 0);
            return wndList;
        }

        /// <summary>
        /// 根据句柄获取title
        /// </summary>
        /// <param name="loginHw"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static int getHwByTitle(int loginHw, string title)
        {
            List<WindowInfo> list = EnumChildWindowsCallback((IntPtr)loginHw);
            foreach (WindowInfo item in list)
            {
                string szWindowName = item.szWindowName;
                IntPtr hWnd = item.hWnd;
                if (title.Equals(szWindowName))
                {
                    return (int)hWnd;
                }
            }
            return (int)IntPtr.Zero;
        }

        public static void BoardChooseSuccess()
        {
            var bar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");

            WinApi.ShowWindow(bar, 2);
            Thread.Sleep(100);
            WinApi.ShowWindow(bar, 3);

            //bool flag = WinApi.SetForegroundWindow(bar);
            int fpglHw = WinApi.getHwByTitle((int)bar, "发票管理");



            int fpglHw1 = (int)WinApi.FindWindowEx((IntPtr)fpglHw, IntPtr.Zero, null, null);
            int fpglHw2 = (int)WinApi.FindWindowEx((IntPtr)fpglHw1, IntPtr.Zero, null, null);
            int fpglHw3 = (int)WinApi.FindWindowEx((IntPtr)fpglHw1, (IntPtr)fpglHw2, null, null);
            WinApi.SetForegroundWindow(bar);
            Thread.Sleep(100);
            //点击发票填开
            WinApi.leftClick(fpglHw3);
            Thread.Sleep(500);
            keybd_event(Keys.Down, 0, 0, 0);
            keybd_event(Keys.Down, 0, 2, 0);
            for (var i = 0; i < 10; i++)
            {
                Thread.Sleep(500);
                keybd_event(Keys.Down, 0, 0, 0);
                keybd_event(Keys.Down, 0, 2, 0);
                Thread.Sleep(500);
                keybd_event(Keys.Down, 0, 0, 0);
                keybd_event(Keys.Down, 0, 2, 0);
                Thread.Sleep(500);
                keybd_event(Keys.Up, 0, 0, 0);
                keybd_event(Keys.Up, 0, 2, 0);
                Thread.Sleep(500);
                keybd_event(Keys.Up, 0, 0, 0);
                keybd_event(Keys.Up, 0, 2, 0);
                Thread.Sleep(500);
            }
        }


        //写入文本文件
        public static void InputText()
        {
            //句柄是用精易助手获取的
            //WinApi.leftClick(7669360);
            //Thread.Sleep(100);
            //keybd_event(Keys.A, 0, 0, 0);
            //keybd_event(Keys.A, 0, 2, 0);
            //Thread.Sleep(100);
            //keybd_event(Keys.A, 0, 0, 0);
            //keybd_event(Keys.A, 0, 2, 0);

            WinApi.SendMessage((IntPtr)7669360, 0x0C, IntPtr.Zero, "1234567890");
        }

        public static void SelectTest()
        {
            WinApi.SendMessage((IntPtr)395158, 0x0C, IntPtr.Zero, "kkk");
        }
    }
}
