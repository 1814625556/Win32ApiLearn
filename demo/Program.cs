using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
                var winBar = FindWindow(null, "Adobe Reader");
                Console.WriteLine($"Adobe Reader bar is:{winBar}");
                if (winBar != IntPtr.Zero)
                {
                    var childs = EnumChildWindowsCallback(winBar);
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
        /// <summary>
        /// 获取窗体的句柄
        /// </summary>
        /// <param name="strClass">窗口类名--这个需要查找（精易编程助手）</param>
        /// <param name="strWindow">窗口标题-对应winform控件的Text属性</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClass, string strWindow);

        /// <summary>
        /// 获取子控件句柄
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static List<WindowInfo> EnumChildWindowsCallback(IntPtr handle)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumChildWindows(handle, delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                wnd.hWnd = hWnd;
                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                GetClassName(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                wnd.parentHWnd = GetParent(hWnd);
                wnd.id = (int)hWnd;
                wndList.Add(wnd);
                return true;
            }, 0);
            return wndList;
        }

        /// <summary>
        /// 获取子控件api
        /// </summary>
        /// <param name="hWndParent">窗体句柄</param>
        /// <param name="lpfn">回调函数</param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        internal static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);

        /// <summary>
        /// 获取子控件回调函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        internal delegate bool CallBack(IntPtr hwnd, int lParam);

        //获取窗口Text 
        [DllImport("user32.dll")]
        internal static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 获取窗体类名称
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpstring"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr hwnd, StringBuilder lpstring, int nMaxCount);

        /// <summary>
        /// 获取父类句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        public struct WindowInfo
        {
            internal int id;
            public IntPtr hWnd;
            internal IntPtr parentHWnd;
            public string szWindowName;
            public string szClassName;
            internal string szTextName;
        }
    }
}
