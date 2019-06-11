using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using Newtonsoft.Json;
using User32Test;

namespace SearchBar
{
    public class Bug
    {
        public static void ClickBtnByName(IntPtr toolBar, string name)
        {
            var parentAutomation = AutomationElement.FromHandle(toolBar);
            var listAutomation = parentAutomation.
                FindAll(TreeScope.Children, Condition.TrueCondition);
            for (int i = 0; i < listAutomation.Count; i++)
            {
                if (listAutomation[i]?.Current.ControlType != ControlType.Button)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(listAutomation[i]?.Current.Name))
                {
                    continue;
                }
                if (listAutomation[i].Current.Name == name)
                {
                    object patternObject;
                    var pattern = listAutomation[i].TryGetCurrentPattern
                        (InvokePattern.Pattern, out patternObject);
                    ((InvokePattern)patternObject).Invoke();
                    break;
                }
            }
        }
        /// <summary>
        /// 红字信息表选择
        /// </summary>
        public static void InfomationChoose()
        {
            try
            {
                var redbar = WinApi.FindWindow(null, "开具增值税专用发票");

                Console.WriteLine("开具增值税专用发票:" + redbar);

                var list = WinApi.EnumChildWindowsCallback(redbar);
                Console.WriteLine($"list.Count:{list.Count}");

                File.AppendAllText("ccbar.txt", JsonConvert.SerializeObject(list));
                for (var i = 1; i < list.Count; i++)
                {
                    var temp = list[list.Count - i].hWnd;
                    
                    Console.WriteLine($"i:{i}");

                    var parentAutomation = AutomationElement.FromHandle(temp);

                    var listAutomation = parentAutomation.FindAll(TreeScope.Children, Condition.TrueCondition);

                    if (listAutomation == null || listAutomation.Count < 10)
                    {
                        Console.WriteLine("continue...");
                        File.AppendAllText("ccbar.txt", "continue...");
                        continue;
                    }
                    File.AppendAllText("ccbar.txt", $"\r\n打印输出{JsonConvert.SerializeObject(listAutomation)}\r\n");
                    for (int j = 0; j < listAutomation.Count; j++)
                    {
                        Console.WriteLine("进入循环");
                        File.AppendAllText("ccbar.txt", "进入循环");
                        if (listAutomation[j]?.Current.ControlType != ControlType.Button)
                        {
                            continue;
                        }
                        Console.WriteLine(listAutomation[j]?.Current.Name);
                        File.AppendAllText("ccbar.txt", listAutomation[j]?.Current.Name);
                        if (listAutomation[j].Current.Name == "红字")
                        {
                            listAutomation[j].TryGetCurrentPattern(InvokePattern.Pattern, out var patternObject);
                            ((InvokePattern)patternObject).Invoke();
                            Console.WriteLine($"第几{list.Count - i}行");
                            File.AppendAllText("ccbar.txt", $"第几{list.Count - i}行");
                            break;
                        }
                    }
                    break;
                }

                //var temp = list[list.Count - 1].hWnd;

                //List<AutomationElement> listauto = new List<AutomationElement>(listAutomation.Count);
                
                Thread.Sleep(1000);
                WinApi.keybd_event(Keys.Up, 0, 0, 0);
                Thread.Sleep(1000);
                WinApi.keybd_event(Keys.Enter, 0, 0, 0);

                Thread.Sleep(2000);

                var barInfoChoose = WinApi.FindWindow(null, "信息表选择");
                if (barInfoChoose == IntPtr.Zero)
                {
                    Console.WriteLine("信息表选择未找到");
                    return;
                }
                var childs = WinApi.FindChildInfo(barInfoChoose);
                var toolbar = childs.Find(b => b.szWindowName == "toolStrip1");

                //ClickBtnByName(toolbar.hWnd, "选择");
                //ClickBtnByName(toolbar.hWnd, "查找");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            //ClickBtnByName(toolbar.hWnd, "下载");

            //RECT rect;
            //WinApi.GetClientRect(toolbar.hWnd, out rect);

            ////点击查找
            //WinApi.ClickLocation(toolbar.hWnd, rect.right - rect.left - 165, 25);

            ////点击选择
            //WinApi.ClickLocation(toolbar.hWnd, rect.right - rect.left - 45, 25);
        }

        /// <summary>
        /// 点击红字成功~
        /// </summary>
        public static void ClickRedChar()
        {
            var fpkj = WinApi.FindWindow(null, "开具增值税专用发票");
            var list = WinApi.EnumChildWindowsCallback(fpkj);
            var redBar = list[list.Count - 1].hWnd;
            var rect = new RECT();
            WinApi.GetClientRect(redBar, out rect);
            WinApi.ClickLocation(redBar, rect.right - rect.left - 500, 25);
        }

        /// <summary>
        /// 关闭客户选择页面--测试成功
        /// </summary>
        public static void GuanBiKeHuXuanZe()
        {
            var khxz = WinApi.FindWindow(null, "客户选择");
            //点掉窗体
            WinApi.PostMessage(khxz, 16, 0, 0);
        }

    }
}
