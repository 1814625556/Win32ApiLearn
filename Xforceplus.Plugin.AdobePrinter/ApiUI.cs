using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Xforceplus.Plugin.AdobePrinter
{
    internal class ApiUI
    {
        /// <summary>
        /// 获取窗体的句柄
        /// </summary>
        /// <param name="strClass">窗口类名--这个需要查找（精易编程助手）</param>
        /// <param name="strWindow">窗口标题-对应winform控件的Text属性</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string strClass, string strWindow);

        public delegate bool WNDENUMPROC(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, int lParam);

        [DllImport("user32.dll")]
        public static extern int GetClassNameW(IntPtr hWnd,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString,
            int nMaxCount);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

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
        internal static extern int GetWindowTextW(IntPtr hWnd,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpString,
            int nMaxCount);

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
        /// 发送消息
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg">等于16的时候就是点击关闭窗体</param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("USER32.DLL", EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);

        public struct RECT
        {
            public int Left; //最左坐标

            public int Top; //最上坐标

            public int Right; //最右坐标

            public int Bottom; //最下坐标
        }

        public struct WindowInfo2
        {
            public IntPtr hWnd;

            public string szWindowName;

            public string szClassName;

            public ApiUI.RECT lRect;
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

        /// <summary>
        /// 获取子控件句柄
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static List<WindowInfo> EnumChildWindowsCallback(IntPtr handle)
        {
            List<WindowInfo> wndList = new List<WindowInfo>();
            EnumChildWindows(handle,
                delegate (IntPtr hWnd, int lParam)
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
                },
                0);

            return wndList;
        }

        public static List<WindowInfo2> GetAllDesktopWindows()
        {
            List<WindowInfo2> wndList = new List<WindowInfo2>();

            EnumWindows(delegate (IntPtr hWnd, int lParam)
            {
                WindowInfo2 wnd = new WindowInfo2();
                StringBuilder sb = new StringBuilder(256);

                wnd.hWnd = hWnd;

                GetWindowTextW(hWnd, sb, sb.Capacity);
                wnd.szWindowName = sb.ToString();

                GetClassNameW(hWnd, sb, sb.Capacity);
                wnd.szClassName = sb.ToString();

                RECT rect = new RECT();
                GetWindowRect(hWnd, ref rect);
                wnd.lRect = rect;

                wndList.Add(wnd);

                return true;
            },
                0);

            return wndList;
        }
    }
}
