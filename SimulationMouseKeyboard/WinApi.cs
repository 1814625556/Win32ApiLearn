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
        /// 给Text发送信息
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
        /// 设置窗体类名称
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
        public static bool ClickLocation(int hWnd, int x, int y)
        {
            Point point = new Point(x, y);
            var btDownResult = WinApi.PostMessage((IntPtr)hWnd, WmLbuttondown, 0, MAKEPARAM(point.X, point.Y));
            Thread.Sleep(50);
            var btUpResult = WinApi.PostMessage((IntPtr)hWnd, WmLbuttonup, 0, MAKEPARAM(point.X, point.Y));
            return btDownResult && btUpResult;
        }

        internal static int MAKEPARAM(int l, int h)
        {
            return ((l & 0xffff) | (h << 0x10));
        }
    }
}
