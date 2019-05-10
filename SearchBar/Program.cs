using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace User32Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SelectTaxDemo();
            Console.ReadKey();
        }
        static void SelectTaxDemo()
        {
            var bar = WinApi.FindWindow(null, "商品编码添加");
            var child = WinApi.FindWindowEx(bar,IntPtr.Zero,null, "*税率");
            var child1 = WinApi.FindWindowEx(bar, child, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1}");
            //通过索引设置下拉框选项
            int selected = WinApi.SendMessage(child1, 0x014e, (IntPtr)3, "");

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
    }
}
