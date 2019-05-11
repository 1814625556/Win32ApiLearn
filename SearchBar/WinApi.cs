using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace User32Test
{
    public static class WinApi
    {
        public const int VK_UP = 0x26; // UP ARROW 键
        public const int VK_RIGHT = 0x27; // RIGHT ARROW 键
        public const int VK_DOWN = 0x28; // DOWN ARROW 键
        public const int VK_TAB = 0x09; // TAB 键
        public const int VK_ENTER = 0x0D; // ENTER 键
        public const int WM_KEYDOWN = 0x100; // DOWN 鼠标事件
        public const int WM_KEYUP = 0x101; // UP 鼠标事件
        public const int WM_SYSKEYDOWN = 0x104; // DOWN 鼠标事件
        public const int WM_SYSKEYUP = 0x105; // UP 鼠标事件
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

        [DllImport("USER32.DLL", EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);

        /// <summary>
        /// 根据句柄获取大小位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(HandleRef hwnd, out RECT rect);

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
        /// 向直定句柄的窗体发送键盘键值
        /// </summary>
        internal static void SendKey(IntPtr intPtr, int key)
        {
            WinApi.PostMessage(intPtr, WinApi.WM_KEYDOWN, key, 0);
            Thread.Sleep(20);
            WinApi.PostMessage(intPtr, WinApi.WM_KEYUP, key, 0);
            Thread.Sleep(20);
        }

        #region 截图 winapi
        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);
        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);
        #endregion
        /// <summary>
        /// 截图Method
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static Bitmap GetWindowCapture(IntPtr hWnd)
        {
            IntPtr intp;
            Bitmap bmp = null;

            if (hWnd == IntPtr.Zero)
            {
                intp = GetDesktopWindow();
            }
            else
            {
                intp = (IntPtr)hWnd;
            }

            IntPtr hscrdc = GetWindowDC((int)intp);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            try
            {
                GetClientRect(intp, out var rect);
                int width = rect.right - rect.left;
                int height = rect.bottom - rect.top;
                IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, width, height);
                SelectObject(hmemdc, hbitmap);
                PrintWindow(intp, hmemdc, 0);
                bmp = Image.FromHbitmap(hbitmap);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message, e);
            }
            finally
            {
                DeleteDC(hmemdc);
                DeleteDC(hscrdc);
            }

            return bmp;
        }

        /// <summary>
        /// 鼠标点击某窗体,相对位置
        /// </summary>
        public static bool ClickLocation(IntPtr hWnd, int x, int y)
        {
            Point point = new Point(x, y);
            var btDownResult = PostMessage(hWnd, 0x0201, 0, MAKEPARAM(point.X, point.Y));
            Thread.Sleep(10);
            var btUpResult = PostMessage(hWnd, 0x0202, 0, MAKEPARAM(point.X, point.Y));
            return btDownResult && btUpResult;
        }

        public static bool LeftClick(IntPtr hWnd, int x=5, int y=5)
        {
            Point point = new Point(x, y);
            var btDownResult = WinApi.PostMessage(hWnd, 0x0201, 0, 0);
            Thread.Sleep(10);
            var btUpResult = WinApi.PostMessage(hWnd, 0x0202, 0, 0);
            return btDownResult && btUpResult;
        }

        internal static int MAKEPARAM(int l, int h)
        {
            return ((l & 0xffff) | (h << 0x10));
        }
    }
    //结构体布局 本机位置
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    #region  知识点总结
    //修改下拉框
    //int selected = WinApi.SendMessage(guiGeSelectBar, 0x014e, (IntPtr)0, "");

    //发送键盘按键:
    //SendKeys.SendWait("{DOWN}")
    //SendKeys.SendWait("{ENTER}")


    /// doc: https://docs.microsoft.com/zh-cn/windows/desktop/Controls/bm-click
    /// public const int VK_LEFT = 0x25; // LEFT ARROW 键
    //public const int VK_UP = 0x26; // UP ARROW 键
    //public const int VK_RIGHT = 0x27; // RIGHT ARROW 键
    //public const int VK_DOWN = 0x28; // DOWN ARROW 键
    //public const int VK_TAB = 0x09; // TAB 键
    //public const int VK_ENTER = 0x0D; // ENTER 键
    //public const int WM_KEYDOWN = 0x100; // DOWN 鼠标事件
    //public const int WM_KEYUP = 0x101; // UP 鼠标事件
    //public const int WM_SYSKEYDOWN = 0x104; // DOWN 鼠标事件
    //public const int WM_SYSKEYUP = 0x105; // UP 鼠标事件


    //WinApi.SendKey(yhzclxBar, WinApi.VK_DOWN);//向下移动
    //WinApi.SendKey(yhzclxBar, WinApi.VK_UP);//向上移动
    //WinApi.SendKey(yhzclxBar, WinApi.VK_ENTER);//向上移动
    //WinApi.ClickLocation(yhzclxBar, 10, 10);//点击句柄对应控件指定位置
    //var jianma = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "简码");//查找下一个窗体

    //int selected = WinApi.SendMessage(guiGeSelectBar, 0x014e, (IntPtr)0, "");//选择下拉框

    //WinApi.SendMessage(ssflBar, 0x0C, IntPtr.Zero, TaxSub(detail.GoodsTaxNo));//对文本框进行赋值
    #endregion
}
