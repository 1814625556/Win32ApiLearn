using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace SimulationMouseKeyboard
{
    public class WinApi
    {
        //结构体布局 本机位置
        [StructLayout(LayoutKind.Sequential)]
        public struct NativeRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        internal const int WmGettext = 0x000D;
        internal const int WmSettext = 0x000C;
        internal const int WmClick = 0x00F5;
        internal const int WmClose = 16;
        internal const uint BmGetcheck = 0xF0;
        internal const uint BstChecked = 0xF1;
        internal const uint CbShowdropdown = 0x014F;
        internal const uint CbSettopindex = 0x015c;
        internal const uint WmLbuttondown = 0x0201;
        internal const uint WmLbuttonup = 0x0202;
        internal const int MouseeventfLeftdown = 0x0002; //模拟鼠标移动
        internal const int MouseeventfMove = 0x0001; //模拟鼠标左键按下
        internal const int MouseeventfLeftup = 0x0004; //模拟鼠标左键抬起

        internal const uint VbKeyZ = 90;

        //public delegate bool CallBack(IntPtr hwnd, int lParam);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 模拟键盘输入
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>

        [DllImport("user32.dll", EntryPoint = "keybd_event")]

        public static extern void keybd_event(

            byte bVk,    //虚拟键值

            byte bScan,// 一般为0

            int dwFlags,  //这里是整数类型  0 为按下，2为释放

            int dwExtraInfo  //这里是整数类型 一般情况下设成为 0

        );

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool mouse_event(int dwFlags, int dx, int dy, int cButtons, 
            int dwExtraInfo);

        /// <summary>
        /// 根据句柄获取大小位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(HandleRef hwnd, out NativeRECT rect);

        /// <summary>
        /// 获取窗体的句柄
        /// </summary>
        /// <param name="strClass">窗口类名--这个需要查找（精易编程助手）</param>
        /// <param name="strWindow">窗口标题-对应winform控件的Text属性</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClass, string strWindow);

        //该函数获取一个窗口句柄,该窗口类名和窗口名与给定字符串匹配 ,
        //这个函数查找子窗口，从排在给定的子窗口后面的下一个子窗口开始
        //在查找时不区分大小写
        /// <param name="strClass">窗口类名--这个需要查找（精易编程助手）</param>
        /// <param name="strWindow">窗口标题-对应winform控件的Text属性</param>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter,
            string strClass, string strWindow);

        /// <summary>
        /// 给Text发送信息0x0C，操作下拉框0x014e
        /// </summary>
        /// <param name="hWnd">文本控件句柄</param>
        /// <param name="Msg">0x0C固定</param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);




        /// <summary>
        /// 获取窗体标题名称
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 设置窗体标题名称
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern bool SetWindowText(IntPtr hwnd, StringBuilder lpString);


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
        /// 获取子控件回调函数
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        internal delegate bool CallBack(IntPtr hwnd, int lParam);
        /// <summary>
        /// 获取子控件api
        /// </summary>
        /// <param name="hWndParent">窗体句柄</param>
        /// <param name="lpfn">回调函数</param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL")]
        internal static extern int EnumChildWindows(IntPtr hWndParent, CallBack lpfn, int lParam);

        //获取窗口Text 
        [DllImport("user32.dll")]
        internal static extern int GetWindowTextW(IntPtr hWnd, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 创建截屏图片
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("USER32.DLL", EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);

        /// <summary>
        /// 获取父类句柄
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        internal static extern IntPtr GetParent(IntPtr hWnd);

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Bitmap GetWindowCapture(IntPtr hWnd)
        {
            IntPtr intp;
            Bitmap bmp = null;

            if (hWnd == IntPtr.Zero)
            {
                intp = Win32Stuff.GetDesktopWindow();
            }
            else
            {
                intp = (IntPtr)hWnd;
            }

            IntPtr hscrdc = Win32Stuff.GetWindowDC((int)intp);
            IntPtr hmemdc = Win32Stuff.CreateCompatibleDC(hscrdc);
            try
            {
                Win32Stuff.GetClientRect(intp, out var rect);
                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;
                IntPtr hbitmap = Win32Stuff.CreateCompatibleBitmap(hscrdc, width, height);
                Win32Stuff.SelectObject(hmemdc, hbitmap);
                Win32Stuff.PrintWindow(intp, hmemdc, 0);
                bmp = Image.FromHbitmap(hbitmap);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e);
            }
            finally
            {
                Win32Stuff.DeleteDC(hmemdc);
                Win32Stuff.DeleteDC(hscrdc);
            }

            return bmp;
        }

        /// <summary>
        /// 鼠标点击某窗体,相对位置
        /// </summary>
        public static bool ClickLocation(IntPtr hWnd, int x, int y)
        {
            Point point = new Point(x, y);
            var btDownResult = WinApi.PostMessage(hWnd, WmLbuttondown, 0, MAKEPARAM(point.X, point.Y));
            Thread.Sleep(50);
            var btUpResult = WinApi.PostMessage(hWnd, WmLbuttonup, 0, MAKEPARAM(point.X, point.Y));
            return btDownResult && btUpResult;
        }

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
        /// 鼠标左击
        /// </summary>
        /// <param name="hWnd1"></param>
        public static void leftClick(int hWnd1)
        {
            IntPtr hWnd = (IntPtr)hWnd1;
            if (hWnd != IntPtr.Zero)
            {
                PostMessage(hWnd, WmLbuttondown, 0, 0);
                Thread.Sleep(10);
                PostMessage(hWnd, WmLbuttonup, 0, 0);
            }
        }

        internal static int MAKEPARAM(int l, int h)
        {
            return ((l & 0xffff) | (h << 0x10));
        }
    }
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
