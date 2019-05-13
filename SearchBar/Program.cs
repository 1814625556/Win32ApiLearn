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
    class Program
    {
        static void Main(string[] args)
        {


            //SelectTaxDemo();
            //TianXieShuiShouFenLeiBianMa();
            GetText();
            Console.ReadKey();
        }



        //点击button按钮获取 文本框,title------------需要测试获取文本信息
        static void GetControlTest()
        {
            var bar = WinApi.FindWindow(null, "发票号码确认");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "确认");
            //WinApi.LeftClick(child);
            //WinApi.SendMessage(child, 0x00F5, IntPtr.Zero, "");//按钮点击事件--亲测可行
            WinApi.PostMessage(child, 0x00F5, 5,5);//按钮点击事件
        }

        /// <summary>
        /// 操作规格型号
        /// </summary>
        static void GuiGeXingHao()
        {
            var spflbmBar = WinApi.FindWindow(null, "商品编码添加");
            var guiGebar = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "规格型号");
            var guiGeSelectBar = WinApi.FindWindowEx(spflbmBar, guiGebar, null, null);
            WinApi.ClickLocation(guiGeSelectBar, 10, 10);
            SendKeys.SendWait("{UP}");
            Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
            //int selected = WinApi.SendMessage(guiGeSelectBar, 0x014e, (IntPtr)0, null);

            Thread.Sleep(2000);

            var jianma = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "简码");
            var temp = WinApi.FindWindowEx(spflbmBar, jianma, null, null);
            temp = WinApi.FindWindowEx(spflbmBar, temp, null, null);
            temp = WinApi.FindWindowEx(spflbmBar, temp, null, null);
            //var selected1 = WinApi.SendMessage(temp, 0x014e, (IntPtr)1, null);
            WinApi.ClickLocation(temp, 10, 10);
            SendKeys.SendWait("{UP}");
            Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
        }

        //填写税收分类编码--填写text文本
        static void TianXieShuiShouFenLeiBianMa()
        {
            var spflbmBar = WinApi.FindWindow(null, "商品编码添加");
            var ssflName = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "税收分类名称");
            var temp1 = WinApi.FindWindowEx(spflbmBar, ssflName, null, null);
            var temp2 = WinApi.FindWindowEx(temp1, IntPtr.Zero, null, null);
            var ssflBar = WinApi.FindWindowEx(temp1, temp2, null, null);                //获取税收分类编码句柄
            //Console.WriteLine($"spflbmBar:{spflbmBar},child:{child},child1:{child1},child2:{child2},child3:child3");
            WinApi.SendMessage(ssflBar, 0x0C, IntPtr.Zero, "110");
        }


        /// <summary>
        /// 根据索引选中下拉框
        /// </summary>
        static void SelectTaxDemo()
        {
            var bar = WinApi.FindWindow(null, "商品编码添加");
            var child = WinApi.FindWindowEx(bar,IntPtr.Zero,null, "*税率");
            var child1 = WinApi.FindWindowEx(bar, child, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1}");
            //通过索引设置下拉框选项
            int selected = WinApi.SendMessage(child1, 0x014e, (IntPtr)6, "");

        }
        static void JieTuCeShi()
        {
            var bar = WinApi.FindWindow(null, "开具增值税普通发票");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);
            //Console.WriteLine($"bar:{bar},child:{child},child1:{child1},child2:{child2}");
            var map = WinApi.GetWindowCapture(child2);

            Color destColor1 = Color.FromArgb(156, 175, 51);//
            Color destColor2 = Color.FromArgb(240, 132, 60);//
            bool isNoTax = false;
            var X = 0;
            for (var i = map.Width - 350; i < map.Width - 310; i++)//310,350,10,40
            {
                for (var j = 11; j < 39; j++)
                {
                    var color = map.GetPixel(i, j);
                    if (color.Equals(destColor1))
                    {
                        Console.WriteLine($"no tax success,(x:{i},y:{j})");
                        isNoTax = true;
                        X = i;
                        break;
                    }
                    if (color.Equals(destColor2))
                    {
                        Console.WriteLine($"tax success,(x:{i},y:{j})");
                        isNoTax = false;
                        X = i;
                        break;
                    }
                }
            }
            if (!isNoTax)
                WinApi.ClickLocation(child2, X + 20, 25);//点击价格，不含税

            Console.WriteLine("=========================================================");
            //WinApi.ClickLocation(child2, X + 20, 25);


            for (var i = 0; i < map.Width; i++)
            {
                var pixel = map.GetPixel(i, 25);

                var r3 = (pixel.R - destColor1.R) / 256.0;
                var g3 = (pixel.G - destColor1.G) / 256.0;
                var b3 = (pixel.B - destColor1.B) / 256.0;

                var diff = Math.Sqrt(r3 * r3 + g3 * g3 + b3 * b3);
                if (diff <= 0.05)
                {
                    Console.WriteLine($"success,(x:{i},y:25)");
                }
            }

            //map.GetPixel();
            //map.Save($"{DateTime.Now.Ticks}.png");
        }

        static void SendMessageTest()
        {

        }
        /// <summary>
        /// 电票 添加 减少按钮
        /// </summary>
        static void SearchAddLess()
        {
            var bar = WinApi.FindWindow(null, "开具增值税电子普通发票");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1},child2:{child2}");
            RECT rect;
            HandleRef ptrT = new HandleRef(null, child2);
            WinApi.GetWindowRect(ptrT,out rect);
            for (var i = 0; i < 4; i++)
            {
                WinApi.ClickLocation(child2, rect.right-rect.left-195, 25);//减少行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 195, 25);//减少
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 250, 25);//减少
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 查找红字句柄
        /// </summary>
        static void SearchRedChar()
        {
            var bar = WinApi.FindWindow(null, "开具增值税专用发票");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1},child2:{child2}");
        }

        /// <summary>
        /// 精度测试
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        private static string GetAmountWithTax(string number1, string number2, int precision = 2)
        {
            decimal num1 = 0;
            decimal num2 = 0;
            try
            {
                num1 = Convert.ToDecimal(number1);
                num2 = Convert.ToDecimal(number2);

                var sum = num1 + num2;
                sum = Math.Round(sum, precision, MidpointRounding.AwayFromZero);
                return sum.ToString();
            }
            catch (Exception e)
            {
               
            }
            return "0";
        }


        #region 2019年5月13日发送键盘值
        static void SendKey()
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var btnBar = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "button1");
            var txtBar = WinApi.FindWindowEx(bar, btnBar, null,null);

            //WinApi.SendKey(txtBar, KeySnap.VK_CAPITAL);
            //Thread.Sleep(500);
            WinApi.SendKey(txtBar, KeySnap.VK_A);
            Thread.Sleep(1000);
            WinApi.SendKey(txtBar, KeySnap.VK_B);
            Thread.Sleep(1000);
            WinApi.SendKey(txtBar, KeySnap.VK_C);
        }
        /// <summary>
        /// 模拟键盘操作
        /// </summary>
        static void SimulationKeyBoard()
        {
            WinApi.keybd_event(Keys.Shift, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.A, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.B, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.C, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Shift, 0, 2, 0);
        }

        /// <summary>
        /// 模拟键盘组合键
        /// </summary>
        static void SimulationKeyBoard2()
        {
            Thread.Sleep(2000);

            WinApi.keybd_event(VBKEY.vbKeyShift, 0, 0, 0);

            for (var i = 0; i < 10; i++)
            {
                #region 测试通过
                //WinApi.keybd_event(VBKEY.vbKeyA, 0, 0, 0);
                //WinApi.keybd_event(VBKEY.vbKeyA, 0, 2, 0);

                //WinApi.keybd_event(VBKEY.vbKeyB, 0, 0, 0);
                //WinApi.keybd_event(VBKEY.vbKeyB, 0, 2, 0);

                //WinApi.keybd_event(VBKEY.vbKeyTab, 0, 0, 0);
                //WinApi.keybd_event(VBKEY.vbKeyTab, 0, 2, 0);
                #endregion


                Thread.Sleep(1000);

            }

            WinApi.keybd_event(VBKEY.vbKeyShift, 0, 2, 0);
        }
        /// <summary>
        /// 改变文本文件的内容
        /// </summary>
        static void ChangeText()
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var btnBar = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "button1");
            var txtBar = WinApi.FindWindowEx(bar, btnBar, null, null);
            WinApi.SendMessage(txtBar, 0x0C, IntPtr.Zero, "AKKJJCCBBDDEE"); //对文本框进行赋值
        }

        /// <summary>
        /// 尚未成功
        /// </summary>
        static void GetText()
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var btnBar = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "button1");
            var txtBar = WinApi.FindWindowEx(bar, btnBar, null, null);

            StringBuilder sb = new StringBuilder();
            WinApi.GetDlgItemText(bar, (int)txtBar, sb, 255);
            Console.WriteLine(sb.ToString());

            WinApi.GetWindowText(txtBar, sb, 255);//获取标题
            Console.WriteLine(sb.ToString());
        }
        #endregion
    }
}
