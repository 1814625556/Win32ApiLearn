using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//引用新命名空间
using System.Runtime.InteropServices;
using System.Threading;

//StructLayout

namespace SimulationMouseKeyboard
{
    public partial class CCForm1 : Form
    {


        #region  ============方法===================================================
        /// <summary>
        /// 获取标题
        /// </summary>
        /// <param name="hander"></param>
        /// <returns></returns>
        private string GetTitle(IntPtr hander)
        {
            StringBuilder sb = new StringBuilder(255);
            WinApi.GetWindowText(hander, sb, 255);
            return sb.ToString();
        }
        /// <summary>
        /// 设置标题
        /// </summary>
        /// <param name="hander"></param>
        private void SetTitle(IntPtr hander)
        {
            StringBuilder sb = new StringBuilder(255);
            sb.Append("chenchang is a good man...");
            bool flag = WinApi.SetWindowText(hander, sb);
        }
        /// <summary>
        /// 发送字符串(操作文本文件)
        /// </summary>
        /// <param name="hWnd"></param>
        private void sendMessage(IntPtr hWnd)
        {
            WinApi.SendMessage(hWnd, 0x0C, IntPtr.Zero, "测试发送数据");
        }
        #endregion


        public CCForm1()
        {
            InitializeComponent();
        }
        //结构体布局 本机位置
        [StructLayout(LayoutKind.Sequential)]
        struct NativeRECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        //将枚举作为位域处理
        [Flags]
        enum MouseEventFlag : uint //设置鼠标动作的键值
        {
            Move = 0x0001,               //发生移动
            LeftDown = 0x0002,           //鼠标按下左键
            LeftUp = 0x0004,             //鼠标松开左键
            RightDown = 0x0008,          //鼠标按下右键
            RightUp = 0x0010,            //鼠标松开右键
            MiddleDown = 0x0020,         //鼠标按下中键
            MiddleUp = 0x0040,           //鼠标松开中键
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,              //鼠标轮被移动
            VirtualDesk = 0x4000,        //虚拟桌面
            Absolute = 0x8000
        }
        
        [DllImport("USER32.DLL", EntryPoint = "PostMessage", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool PostMessage(IntPtr hwnd, UInt32 wMsg, int wParam, int lParam);

        internal const uint WmLbuttondown = 0x0201;
        internal const uint WmLbuttonup = 0x0202;

        internal static int MAKEPARAM(int l, int h)
        {
            return ((l & 0xffff) | (h << 0x10));
        }

        //设置鼠标位置
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        //设置鼠标按键和动作
        [DllImport("user32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy,
            uint data, UIntPtr extraInfo); //UIntPtr指针多句柄类型
        
        [DllImport("user32.dll")]
        static extern bool GetWindowRect(HandleRef hwnd, out NativeRECT rect);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(IntPtr hwnd, uint wMsg, int wParam, int lParam);

        //模拟键盘发送案按键
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(Keys bVk, byte bScan, uint dwFlags, uint dwExtraInfo);


        private Point endPosition;
        private int count;

        /// <summary>
        /// 点击按钮操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //获取主窗体句柄
            //WindowsForms10.Window.8.app.0.141b42a_r16_ad1
            IntPtr ptrTaskbar = WinApi.FindWindow(null,"Form1Text");
            if (ptrTaskbar == IntPtr.Zero)
            {
                MessageBox.Show("No windows found!");
                return;
            }
            //获取窗体中"button1"按钮
            IntPtr ptrStartBtn = WinApi.FindWindowEx(ptrTaskbar, IntPtr.Zero, null, "button1");
            if (ptrStartBtn == IntPtr.Zero)
            {
                MessageBox.Show("No button found!");
                return;
            }
            SendMessage(ptrStartBtn, 0xF5, 0, 0);
            Thread.Sleep(2000);
            SendMessage(ptrTaskbar, 0x10, 0, 0);//关闭窗体
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //获取主窗体句柄
            //WindowsForms10.Window.8.app.0.141b42a_r16_ad1
            IntPtr ptrTaskbar = WinApi.FindWindow(null, "Form1Text");
            if (ptrTaskbar == IntPtr.Zero)
            {
                MessageBox.Show("No windows found!");
                return;
            }
            IntPtr ccPtr = IntPtr.Zero;
            //获取窗体中"textBox1"按钮
            IntPtr ptrStartTxt = WinApi.FindWindowEx(ptrTaskbar, ccPtr, "WindowsForms10.EDIT.app.0.141b42a_r16_ad1", null);
            var title = GetTitle(ptrStartTxt);
            sendMessage(ptrStartTxt);
        }
        
        /// <summary>
        /// 获取窗体句柄-获取窗体title-改变窗体title
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            IntPtr ptrTaskbar = WinApi.FindWindow(null, "Form1Text");
            if (ptrTaskbar == IntPtr.Zero)
            {
                MessageBox.Show("No windows found!");
                return;
            }
            var title = GetTitle(ptrTaskbar);
            SetTitle(ptrTaskbar);
        }
        /// <summary>
        /// 查找子窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            IntPtr ptrTaskbar = WinApi.FindWindow(null, "Form1Text");
            if (ptrTaskbar == IntPtr.Zero)
            {
                MessageBox.Show("No windows found!");
                return;
            }
            IntPtr child = IntPtr.Zero;
            int i = 0;
            do
            {
                child = WinApi.FindWindowEx(ptrTaskbar, child, null, null);
                var title = GetTitle(child);
                TxtResult.Text += $"控件{i}:{title}\r\n";
                var flag = child.Equals(IntPtr.Zero);
                i++;
            } while (!child.Equals(IntPtr.Zero));
        }
        /// <summary>
        /// 获取窗体大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            IntPtr ptrTaskbar = WinApi.FindWindow(null, "Form1Text");
            if (ptrTaskbar == IntPtr.Zero)
            {
                MessageBox.Show("No windows found!");
                return;
            }
            NativeRECT rect;
            HandleRef ptrT = new HandleRef(null,ptrTaskbar);
            GetWindowRect(ptrT, out rect);
            var width = rect.right - rect.left;
        }
    }
}
