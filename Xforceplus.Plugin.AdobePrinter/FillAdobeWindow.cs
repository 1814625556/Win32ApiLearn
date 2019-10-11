using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Xforceplus.Plugin.AdobePrinter
{
    internal class FillAdobeWindow
    {
        /// <summary>
        ///     将程序嵌入窗体
        /// </summary>
        /// <param name="pW">容器</param>
        /// <param name="appname">程序名</param>
        public FillAdobeWindow(Panel pW, IntPtr hwnd)
        {
            panel1 = pW;
            intptr = hwnd;
            LoadEvent(hwnd);
            pane();
        }

        #region 相应事件

        private void LoadEvent(IntPtr appHandle)
        {
            // 设置目标应用程序的主窗体的父亲(为我们的窗体).
            SetParent(appHandle, panel1.Handle);

            // 除去窗体边框.
            var wndStyle = GetWindowLong(appHandle, GWL_STYLE);
            wndStyle &= ~WS_BORDER;
            wndStyle &= ~WS_THICKFRAME;
            SetWindowLong(appHandle, GWL_STYLE, wndStyle);
            SetWindowPos(appHandle, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);

            // 在Resize事件中更新目标应用程序的窗体尺寸.
            panel1_Resize(panel1, null);
        }

        #endregion 相应事件

        #region 函数和变量声明

        /*
        * 声明 Win32 API
        */

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild,
            IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd,
            int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd,
            int nIndex,
            int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int SetWindowPos(IntPtr hWnd,
            IntPtr hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            uint uFlags);

        /*
         * 定义 Win32 常数
         */

        private const int GWL_STYLE = -16;

        private const int WS_BORDER = (int)0x00800000L;

        private const int WS_THICKFRAME = (int)0x00040000L;

        private const int SWP_NOMOVE = 0x0002;

        private const int SWP_NOSIZE = 0x0001;

        private const int SWP_NOZORDER = 0x0004;

        private const int SWP_FRAMECHANGED = 0x0020;

        private const int SW_MAXIMIZE = 3;

        private IntPtr HWND_NOTOPMOST = new IntPtr(-2);

        #endregion 函数和变量声明

        #region 容器

        private readonly IntPtr intptr = new IntPtr(0);

        public Panel panel1 { set; get; }

        private void pane()
        {
            panel1.Anchor = AnchorStyles.Left | AnchorStyles.Top |
                            AnchorStyles.Right | AnchorStyles.Bottom;

            panel1.Resize += panel1_Resize;
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            // 设置目标应用程序的窗体样式.
            SetWindowPos(intptr, IntPtr.Zero, 0, 0, panel1.ClientSize.Width, panel1.ClientSize.Height, SWP_NOZORDER);
        }

        #endregion 容器
    }
}
