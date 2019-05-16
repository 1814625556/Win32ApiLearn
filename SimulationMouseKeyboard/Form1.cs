using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

//引用新命名空间
using System.Runtime.InteropServices;
using System.Threading;

//StructLayout

namespace SimulationMouseKeyboard
{
    public partial class CCForm1 : Form
    {
        #region 属性

        private static int MOUSEEVENTF_MOVE = 0x0001;      //移动鼠标 
        private static int MOUSEEVENTF_LEFTDOWN = 0x0002; //模拟鼠标左键按下 
        private static int MOUSEEVENTF_LEFTUP = 0x0004; //模拟鼠标左键抬起 
        private static int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        private static int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        private static int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        private static int MOUSEEVENTF_MIDDLEUP = 0x0040; //模拟鼠标中键抬起 
        private static int MOUSEEVENTF_ABSOLUTE = 0x8000; //标示是否采用绝对坐标 

        #endregion

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
        /// 获取窗体内容
        /// </summary>
        /// <param name="hander"></param>
        /// <returns></returns>
        private string GetText(IntPtr hander)
        {
            StringBuilder sb = new StringBuilder(255);
            WinApi.GetWindowTextW(hander, sb, 255);
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

            Thread.Sleep(1000);
            SendMessage(WinApi.FindWindow(null, "CCMessageBox"), 0x10, 0, 0);
            Thread.Sleep(2000);
            SendMessage(WinApi.FindWindow(null, "Form2"), 0x10, 0, 0);//关闭窗体
            Thread.Sleep(2000);
            //关闭窗体--这个时候一旦关闭了，
            //顶层父类窗体，生成的子窗体也会自动关闭
            SendMessage(ptrTaskbar, 0x10, 0, 0);
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
            WinApi.NativeRECT rect;
            HandleRef ptrT = new HandleRef(null,ptrTaskbar);
            WinApi.GetWindowRect(ptrT, out rect);
            var width = rect.right - rect.left;
        }
        /// <summary>
        /// 模拟鼠标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 50; i++)
            {
                Thread.Sleep(5000);

                
                //WinApi.SetCursorPos(840, 350);
                WinApi.mouse_event(2, 640, 360, 0, 0);

                WinApi.mouse_event(4, 640, 640, 0, 0);
                //Thread.Sleep(300);
                //WinApi.mouse_event(16, i * 20, i * 20, 0, 0);
            }
        }
        /// <summary>
        /// 获取窗体相对位置，点击该位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            var hxBar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");
            var toolBar = WinApi.FindWindowEx(hxBar, IntPtr.Zero, null, null);

            WinApi.ClickLocation(toolBar, 40, 29);
            Thread.Sleep(1000);
            WinApi.ClickLocation(toolBar, 110, 29);
            Thread.Sleep(1000);
            //WinApi.ClickLocation(toolBar, 190, 29);
            var flag = WinApi.ClickLocation(toolBar, 190, 29);
            //var flag1 = WinApi.ClickLocation(ptrTaskbar, 421, 89);
        }


        /// <summary>
        /// 点击红字测试成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            // 测试红字
            //var barInt = 2230240;
            //var bar = (IntPtr) barInt;
            //Thread.Sleep(1000);
            //WinApi.ClickLocation(bar, 471, 25);
            //Thread.Sleep(1000);
            //WinApi.ClickLocation(bar, 481, 25);
            //Thread.Sleep(1000);
            //WinApi.ClickLocation(bar, 491, 25);
            //Thread.Sleep(1000);


            //
            WinApi.NativeRECT rect;
            HandleRef ptrT = new HandleRef(null, (IntPtr)262822);
            WinApi.GetWindowRect(ptrT, out rect);
            var width = rect.right - rect.left;
            WinApi.ClickLocation((IntPtr)262822, width-435, 25);
        }
        /// <summary>
        /// 测试开票软件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            
            
            var bar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");

            WinApi.ShowWindow(bar, 2);//最小
            Thread.Sleep(1000);
            WinApi.ShowWindow(bar, 3);//最大

            //bool flag = WinApi.SetForegroundWindow(bar);
            int fpglHw = WinApi.getHwByTitle((int)bar, "发票管理");

            

            int fpglHw1 = (int)WinApi.FindWindowEx((IntPtr)fpglHw, IntPtr.Zero, null, null);
            int fpglHw2 = (int)WinApi.FindWindowEx((IntPtr)fpglHw1, IntPtr.Zero, null, null);
            int fpglHw3 = (int)WinApi.FindWindowEx((IntPtr)fpglHw1, (IntPtr)fpglHw2, null, null);
            //WinApi.SetForegroundWindow(bar);
            Thread.Sleep(100);
            //点击发票填开
            WinApi.leftClick(fpglHw3);



            //KeyBoardDown((IntPtr)fpglHw1, 0x100);

            //return;


            Thread.Sleep(500);
            keybd_event(Keys.Down, 0, 0, 0);
            keybd_event(Keys.Down, 0, 2, 0);

            Thread.Sleep(500);
            keybd_event(Keys.Down, 0, 0, 0);
            keybd_event(Keys.Down, 0, 2, 0);

            Thread.Sleep(500);
            keybd_event(Keys.Down, 0, 0, 0);
            keybd_event(Keys.Down, 0, 2, 0);

            Thread.Sleep(500);
            keybd_event(Keys.Enter, 0, 0, 0);
            keybd_event(Keys.Enter, 0, 2, 0);

            Thread.Sleep(1000);
            var form1 = WinApi.FindWindow(null, "发票号码确认");
            Thread.Sleep(1000);
            var confirm = WinApi.FindWindowEx(form1, IntPtr.Zero, null, "确认");
            WinApi.leftClick((int)confirm);
        }

        private void KeyBoardDown(IntPtr intPtr, int key)
        {
            Thread.Sleep(1000);
            WinApi.PostMessage(intPtr, 0x100, key, 0);
            Thread.Sleep(1000);
            WinApi.PostMessage(intPtr, 0x100, key, 0);
            Thread.Sleep(1000);
            WinApi.PostMessage(intPtr, 0x100, key, 0);
            Thread.Sleep(1000);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //ShuiPanTest.InputText();
            ShuiPanTest.SelectTest();
        }
        /// <summary>
        /// 操作下拉框--成功
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            string selectStr = "";
            IntPtr bar = (IntPtr) 132678;

            //var control = Control.FromHandle((IntPtr)198216);


            //通过索引设置下拉框选项
            int selected = WinApi.SendMessage(bar, 0x014e, (IntPtr)9, "");
            //通过索引获取下拉框选项
            Thread.Sleep(1000);
            string selectedStr = GetText(bar);

            //获得选项数量
            int count = SendMessage(bar, 0x0146, 0, 0);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string text = GetText((IntPtr)1508892);
        }
        /// <summary>
        /// 临时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button13_Click(object sender, EventArgs e)
        {
            //关闭窗体
            var bar = WinApi.FindWindow(null, "开具增值税电子普通发票");

            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                var closeBar = WinApi.FindWindow(null, "SysMessageBox");
                var noBar = WinApi.getHwByTitle((int)closeBar, "是");
                WinApi.leftClick(noBar);
                //MessageBox.Show($"closeBar:{closeBar}; noBar:{noBar}");

            });
            SendMessage(bar, 0x10, 0, 0);
            Thread.Sleep(100);
            //MessageBox.Show("哈哈~，关闭开具增值税电子普通发票fail");
            //var closeBar = WinApi.FindWindow(null, "SysMessageBox");
            //var noBar = WinApi.FindWindow(null, "否");
        }
        /// <summary>
        /// 截图操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button14_Click(object sender, EventArgs e)
        {
            var jpg = WinApi.GetWindowCapture(WinApi.FindWindow(null, "Form1Text"));
            var name = $"Form1{GetTimeStamp()}.png";
            jpg.Save(Path.Combine(@"C:\MyDatas\公司\Pictures",name),ImageFormat.Png);
        }
        /// <summary> 
        /// 获取时间戳 
        /// </summary> 
        /// <returns></returns> 
        public static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var listPtr = WinApi.EnumChildWindowsCallback(bar);
            for (var i = 0; i < listPtr.Count; i++)
            {
                this.TxtResult.Text += $"intpr:{listPtr[i].hWnd}    ";
            }
        }
    }
}
