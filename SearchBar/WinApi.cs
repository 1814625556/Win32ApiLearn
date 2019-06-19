using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace User32Test
{
    public static class WinApi
    {
        internal const int WmGettext = 13;
        internal const int WmSettext = 12;
        internal const int BM_CLICK = 245;
        internal const int BM_TEXT = 12;
        internal const int WM_CLOSE = 16;
        internal const uint BmGetcheck = 240;
        internal const uint BstChecked = 241;
        internal const uint CbShowdropdown = 335;
        internal const uint CbSettopindex = 348;
        internal const uint WmLbuttondown = 513;
        internal const uint WmLbuttonup = 514;
        internal const int MouseeventfLeftdown = 2;
        internal const int MouseeventfMove = 1;
        internal const int MouseeventfLeftup = 4;
        internal const int EM_GETTEXTEX = 1118;
        public const int CB_GETLBTEXT = 328;
        public const int CB_SETCURSEL = 334;
        public const int CB_GETCOUNT = 326;
        public const int CB_GETLBTEXTLEN = 329;
        public const int VK_LEFT = 37;
        public const int VK_UP = 38;
        public const int VK_RIGHT = 39;
        public const int VK_DOWN = 40;
        public const int VK_TAB = 9;
        public const int VK_RETURN = 13;
        public const int VK_ESCAPE = 27;
        public const int VK_BACK = 8;
        public const int VK_NEXT = 34;
        public const int VK_END = 35;
        public const int WM_KEYDOWN = 256;
        public const int WM_KEYUP = 257;
        public const int WM_SYSKEYDOWN = 260;
        public const int WM_SYSKEYUP = 261;
        public const int VK_DELETE = 46;
        internal const uint VbKeyZ = 90;

        /// <summary>
        /// 窗体前置
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        //获取焦点
        [DllImport("User32.dll")]
        public static extern int SetFocus(IntPtr hwnd);
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
        /// 发送消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg">等于16的时候就是点击关闭窗体</param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);

        /// <summary>
        /// 关闭windown窗体
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static bool CloseWinForm(IntPtr hwnd)
        {
            return PostMessage(hwnd, 0X10, 0,0);
        }

        /// <summary>
        /// 窗体最大最小化2：最小，3：最大
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nCmdShow"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowWindow", CharSet = CharSet.Auto)]
        public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);

        /// <summary>
        /// 根据句柄获取大小位置
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(HandleRef hwnd, out RECT rect);

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
        /// 给Text发送信息0x0C，操作下拉框0x014e
        /// </summary>
        /// <param name="hWnd">文本控件句柄</param>
        /// <param name="Msg">0x0C固定</param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        internal static extern int SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, string lParam);



        [DllImport("USER32.DLL", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// 获取文本文件操作
        /// </summary>
        /// <param name="hDlg"></param>
        /// <param name="nIDDlgItem"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "GetDlgItemTextA")]
        public static extern int GetDlgItemText(IntPtr hDlg, int nIDDlgItem, [Out]StringBuilder lpString, int nMaxCount);

        /// <summary>
        /// 向直定句柄的窗体发送键盘键值
        /// 所对应的的键盘值：  https://docs.microsoft.com/en-us/windows/desktop/inputdev/virtual-key-codes
        /// </summary>
        internal static void SendKey(IntPtr intPtr, int key)
        {
            //这种方式发送的是双字符
            WinApi.PostMessage(intPtr, KeySnap.WM_KEYDOWN, key, 0);
            WinApi.PostMessage(intPtr, KeySnap.WM_KEYUP, key, 0);

            //这种方式发送的是单字符
            //WinApi.PostMessage(intPtr, KeySnap.WM_SYSKEYDOWN, key, 0);
            //WinApi.PostMessage(intPtr, KeySnap.WM_SYSKEYUP, key, 0);
        }

        //模拟键盘按键--没有防止遮挡
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        /// <summary>
        /// 组合按键
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        /// <summary>
        /// radio按钮操作
        /// </summary>
        /// <param name="hDlg"></param>
        /// <param name="nIDFirstButton"></param>
        /// <param name="nIDLastButton"></param>
        /// <param name="nIDCheckButton"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool CheckRadioButton(
            IntPtr hDlg,
            int nIDFirstButton,
            int nIDLastButton,
            int nIDCheckButton
        );

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hDlg"></param>
        /// <param name="nIDButton"></param>
        /// <param name="uCheck"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern bool CheckDlgButton(
                IntPtr hDlg,      // handle to dialog box
                int nIDButton,  // button-control identifier
                int uCheck     // check state
            );


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

        public static bool LeftClick(IntPtr hWnd)
        {
            var btDownResult = WinApi.PostMessage(hWnd, 0x0201, 0, 0);
            Thread.Sleep(10);
            var btUpResult = WinApi.PostMessage(hWnd, 0x0202, 0, 0);
            return btDownResult && btUpResult;
        }

        internal static int MAKEPARAM(int l, int h)
        {
            return ((l & 0xffff) | (h << 0x10));
        }

        #region 获取子控件句柄
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
        /// 获取所有子控件的句柄
        /// </summary>
        public static List<IntPtr> EnumChilWindowsIntptr(IntPtr intPtr)
        {
            var intPtrs = new List<IntPtr>();
            EnumChildWindows(intPtr, delegate (IntPtr hWnd, int lParam)
            {
                intPtrs.Add(hWnd);
                return true;
            }, 0);
            return intPtrs;
        }
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
        #endregion
        /// <summary>
        /// 查找直接子控件信息
        /// </summary>
        /// <param name="parentBar"></param>
        /// <returns></returns>
        public static List<WindowInfo> FindChildInfo(IntPtr parentBar)
        {
            var wndList = new List<WindowInfo>();
            var temp = IntPtr.Zero;
            while (true)
            {
                temp = WinApi.FindWindowEx(parentBar, temp, null, null);
                if (temp == IntPtr.Zero)
                {
                    break;
                }
                WindowInfo wnd = new WindowInfo();
                StringBuilder sb = new StringBuilder(256);
                wnd.hWnd = temp;
                GetWindowTextW(temp, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();
                GetClassName(temp, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();
                wnd.parentHWnd = GetParent(temp);
                wnd.id = (int)temp;
                wndList.Add(wnd);
            }
            return wndList;
        }

        public static List<IntPtr> FindChildBar(IntPtr parentBar)
        {
            var listChildBars = new List<IntPtr>();
            var temp = IntPtr.Zero;
            while (true)
            {
                temp = WinApi.FindWindowEx(parentBar, temp, null, null);
                if (temp == IntPtr.Zero)
                {
                    break;
                }
                listChildBars.Add(temp);
            }
            return listChildBars;
        }

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto)]
        public static extern IntPtr SendRefMessage(IntPtr hWnd, uint Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, string lParam);

        /// <summary>
        /// 通过句柄和值设置下拉框
        /// </summary>
        /// <param name="intPtr"></param>
        /// <param name="itemValue"></param>
        /// <returns></returns>
        public static bool SetComboxItemValue(IntPtr intPtr, string itemValue)
        {
            int num1 = SendMessage(intPtr, 326, 0, 0);
            for (int wParam = 0; wParam < num1; ++wParam)
            {
                StringBuilder lParam = new StringBuilder(100);
                SendRefMessage(intPtr, 328U, wParam, lParam);
                if (lParam.ToString().Contains(itemValue))
                {
                    int num2 = SendMessage(intPtr, 334, wParam, "");
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 左键点击-2
        /// </summary>
        /// <param name="intPtr"></param>
        public static void LeftClickMsg(IntPtr intPtr)
        {
            WinApi.PostMessage(intPtr, 245, 0, 0);
            Thread.Sleep(10);
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

    public class KeySnap
    {
        public const int VK_CAPITAL = 0x14;//Caps按键

        public const int VK_SHIFT = 0x10;// shift按键
        public const int VK_UP = 0x26; // UP ARROW 键
        public const int VK_RIGHT = 0x27; // RIGHT ARROW 键
        public const int VK_DOWN = 0x28; // DOWN ARROW 键
        public const int VK_TAB = 0x09; // TAB 键
        public const int VK_ENTER = 0x0D; // ENTER 键
        public const int WM_KEYDOWN = 0x100; // DOWN 鼠标事件
        public const int WM_KEYUP = 0x101; // UP 鼠标事件
        public const int WM_SYSKEYDOWN = 0x104; // DOWN 鼠标事件
        public const int WM_SYSKEYUP = 0x105; // UP 鼠标事件
        //下面是字母按键
        public const int VK_A = 0x41;
        public const int VK_B = 0x42;
        public const int VK_C = 0x43;

        public const int VK_a = 0x61;
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

    #region  知识点总结
    //修改下拉框
    //int selected = WinApi.SendMessage(guiGeSelectBar, 0x014e, (IntPtr)0, "");

    //发送键盘按键:
    //SendKeys.SendWait("{DOWN}")
    //SendKeys.SendWait("{ENTER}")


    /// doc:  https://docs.microsoft.com/en-us/windows/desktop/inputdev/virtual-key-codes
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
