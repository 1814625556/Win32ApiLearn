using System;
using System.CodeDom;
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
        //这种方式获取不到treeview的句柄信息
        public static void TreeIntpre()
        {
            var treeBar = (IntPtr)328902;
            var list = WinApi.EnumChildWindowsCallback(treeBar);
            list.ForEach(l=>Console.WriteLine(l.szWindowName));
        }

        /// <summary>
        /// 树类型的UI自动化
        /// </summary>
        public static void TreePattern()
        {
            var treeBar = (IntPtr) 328902;
            var treeMation = AutomationElement.FromHandle(treeBar);

            var propertyCondition = new PropertyCondition(AutomationElement.NameProperty, "节点0");

            var node0 = treeMation.FindFirst(TreeScope.Children, propertyCondition);

            node0.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out var obj);
            ((ExpandCollapsePattern)obj).Expand();
            var node2 = node0.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.NameProperty, "节点2"));

            Thread.Sleep(2000);
            node2.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out var node2Pattern);
            ((ExpandCollapsePattern)node2Pattern).Expand();
        }

        //关闭window窗体
        public static void CloseWindow()
        {
            //获取开票软件主窗体
            var kprjBar = HxShengQing.GetKprjMainPageBar();
            if (kprjBar == IntPtr.Zero)
            {
                return;
            }
            //前置主窗体
            ShowForm(kprjBar);


            var list = WinApi.EnumChildWindowsCallback(kprjBar);
            var bar = list.Find(b => b.szWindowName == "红字发票信息表查询导出").hWnd;

            var barMation = AutomationElement.FromHandle(bar);
            barMation.TryGetCurrentPattern(WindowPattern.Pattern, out var obj);
            ((WindowPattern) obj).Close();
        }

        /// <summary>
        /// 加载框判断
        /// </summary>
        /// <returns></returns>
        public static bool IsInfoLoading()
        {
            var infoBar = WinApi.FindWindow(null, "信息表下载中");
            var automation = AutomationElement.FromHandle(infoBar);
            var str = JsonConvert.SerializeObject(automation.Current.IsOffscreen);//区别在IsOffscreen
            return true;
        }

        public static bool ClickBtnByName(IntPtr toolBar, string name)
        {
            try
            {
                var parentAutomation = AutomationElement.FromHandle(toolBar);
                var listAutomation = parentAutomation.
                    FindAll(TreeScope.Children, Condition.TrueCondition);
                for (var i = 0; i < listAutomation.Count; i++)
                {
                    if (listAutomation[i]?.Current.ControlType != ControlType.Button)
                    {
                        continue;
                    }
                    if (string.IsNullOrEmpty(listAutomation[i]?.Current.Name))
                    {
                        continue;
                    }

                    if (listAutomation[i].Current.Name != name) continue;

                    listAutomation[i].TryGetCurrentPattern
                        (InvokePattern.Pattern, out var patternObject);
                    ((InvokePattern)patternObject).Invoke();
                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            return false;
        }

        public static void ShowForm(IntPtr bar)
        {
            WinApi.ShowWindow(bar, 2);
            Thread.Sleep(100);
            WinApi.ShowWindow(bar, 3);
            Thread.Sleep(100);
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
            RECT rect;
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

        /// <summary>
        /// 查找按钮点击
        /// </summary>
        public static void XuanZe()
        {
            var bar = WinApi.FindWindow(null, "信息表选择");
            ShowForm(bar);
            var list = WinApi.EnumChildWindowsCallback(bar);
            var tooltrip = list.Find(b => b.szWindowName == "toolStrip1");
            var flag = ClickBtnByName(tooltrip.hWnd, "查找");
            if (flag) return;
            RECT rect;
            WinApi.GetClientRect(tooltrip.hWnd, out rect);
            WinApi.ClickLocation(tooltrip.hWnd, rect.right - rect.left - 175, 25);//查找
        }

        /// <summary>
        /// 红冲信息表下载demo，缺少成功验证逻辑
        /// </summary>
        public static void downLoadInfo()
        {
            var infoBar = WinApi.FindWindow(null, "信息表选择");
            var list = WinApi.EnumChildWindowsCallback(infoBar);
            var toolStrip = list.Find(bar => bar.szWindowName == "toolStrip1");
            if (toolStrip.hWnd == IntPtr.Zero)
            {
                return;
            }

            ClickBtnByName(toolStrip.hWnd, "下载");
            Thread.Sleep(500);

            var beginDateBar = IntPtr.Zero;
            var endDateBar = IntPtr.Zero;

            var purTaxNoBar = IntPtr.Zero;
            var saleTaxNoBar = IntPtr.Zero;
            var infoBhBar = IntPtr.Zero;

            var confirmBar = IntPtr.Zero;

            HxShengQing.TryRetry(str =>
            {
                var downinfoBar = WinApi.FindWindow(null, str);
                var infoChilds = WinApi.FindChildInfo(downinfoBar);
                confirmBar = infoChilds.Find(b => b.szWindowName == "确定").hWnd;

                var infoTableInfoBar = infoChilds.Find(b => b.szWindowName == "信息表信息");
                var infoTableChilds = WinApi.FindChildBar(infoTableInfoBar.hWnd);
                purTaxNoBar = infoTableChilds[5];
                saleTaxNoBar = infoTableChilds[4];
                infoBhBar = infoTableChilds[1];

                var dateBar = infoChilds.Find(b => b.szWindowName == "填开日期");
                var dateChilds = WinApi.FindChildBar(dateBar.hWnd);
                beginDateBar = dateChilds[1];
                endDateBar = dateChilds[0];

                if (beginDateBar == IntPtr.Zero || endDateBar == IntPtr.Zero 
                    || purTaxNoBar == IntPtr.Zero || saleTaxNoBar == IntPtr.Zero
                    || infoBhBar == IntPtr.Zero || confirmBar == IntPtr.Zero)
                {
                    return false;
                }
                return true;
            }, "红字发票信息表审核结果下载条件设置");

            WinApi.LeftClick(beginDateBar);
            Thread.Sleep(500);
            //修改填开日期起为2018-1-1
            Thread.Sleep(100);
            SendKeys.SendWait("2018");
            Thread.Sleep(100);
            SendKeys.SendWait("{right}");
            Thread.Sleep(100);
            SendKeys.SendWait("01");
            Thread.Sleep(100);
            SendKeys.SendWait("{right}");
            Thread.Sleep(100);
            SendKeys.SendWait("01");

            Thread.Sleep(100);
            WinApi.SendMessage(purTaxNoBar, 0X0C, IntPtr.Zero, "1234567890");
            Thread.Sleep(100);
            WinApi.SendMessage(saleTaxNoBar, 0X0C, IntPtr.Zero, "0987654321");
            Thread.Sleep(100);
            WinApi.SendMessage(infoBhBar, 0X0C, IntPtr.Zero, "12345678901234567");
            Thread.Sleep(500);

            WinApi.LeftClick(confirmBar);

            var infoDownLoadBar = HxShengQing.TryRetry(str => WinApi.FindWindow(null, str), "信息表下载中");
            if (infoDownLoadBar != IntPtr.Zero)
            {
                for (var i = 0; i < 100; i++)
                {
                    infoDownLoadBar = WinApi.FindWindow(null, "信息表下载中");
                    var SysMessageBox = WinApi.FindWindow(null, "SysMessageBox");
                    if (SysMessageBox != IntPtr.Zero)
                    {
                        HxShengQing.SystemOpera("确认");
                        break;
                    }
                    if (infoDownLoadBar == IntPtr.Zero)
                        break;
                    Thread.Sleep(1000);
                }
            }

        }


        /// <summary>
        /// 蓝字发票校验页面过度
        /// </summary>
        public static void lanzifapiaoguodu()
        {
            var blueInvoiceInputtHw = HxShengQing.TryRetry(str => WinApi.FindWindow(null, str), "销项正数发票代码号码填写、确认");

            var fapiaodaimaBar1 = IntPtr.Zero;
            var fapiaodaimaBar2 = IntPtr.Zero;
            var fapiaohaomaBar1 = IntPtr.Zero;
            var fapiaohaomaBar2 = IntPtr.Zero;
            var nextStepBar = IntPtr.Zero;


            var flag = HxShengQing.TryRetry(bar =>
            {
                var list = WinApi.EnumChildWindowsCallback(bar);
                nextStepBar = list.Find(b => b.szWindowName == "下一步").hWnd;

                var tabPage2 = list.Find(b => b.szWindowName == "tabPage2").hWnd;
                var childs = WinApi.FindChildBar(tabPage2);

                fapiaodaimaBar1 = childs[6];
                fapiaodaimaBar2 = childs[0];
                fapiaohaomaBar1 = childs[3];
                fapiaohaomaBar2 = childs[1];

                if (fapiaodaimaBar1 == IntPtr.Zero || fapiaodaimaBar2 == IntPtr.Zero ||
                    fapiaohaomaBar1 == IntPtr.Zero ||
                    fapiaohaomaBar2 == IntPtr.Zero || nextStepBar == IntPtr.Zero)
                {
                    return false;
                }
                return true;
            }, blueInvoiceInputtHw);

            if (!flag)
            {
                throw new Exception("xxxx:蓝字发票校验页面句柄查找错误");
            }

            WinApi.SendMessage(fapiaodaimaBar1, 0X0C, IntPtr.Zero, "4400154620");
            Thread.Sleep(100);
            WinApi.SendMessage(fapiaodaimaBar2, 0X0C, IntPtr.Zero, "4400154620");
            Thread.Sleep(100);
            WinApi.SendMessage(fapiaohaomaBar1, 0X0C, IntPtr.Zero, "51365035");
            Thread.Sleep(100);
            WinApi.SendMessage(fapiaohaomaBar2, 0X0C, IntPtr.Zero, "51365035");
            WinApi.SendKey(fapiaohaomaBar2,WinApi.VK_RETURN);
            Thread.Sleep(100);

            //点击下一步按钮
            WinApi.LeftClick(nextStepBar);


            var confirmBtn = IntPtr.Zero;
            var cancelBtn = IntPtr.Zero;
            var isClickConfirm = IntPtr.Zero;

            var nextFlag = HxShengQing.TryRetry(bar =>
            {
                var list = WinApi.EnumChildWindowsCallback(bar);
                confirmBtn = list.Find(b => b.szWindowName == "确  定").hWnd;
                cancelBtn = list.Find(b => b.szWindowName == "取 消").hWnd;
                isClickConfirm = list.Find(b => b.szWindowName == "本张发票可以开红字发票！").hWnd;
                if (confirmBtn == IntPtr.Zero || cancelBtn == IntPtr.Zero)
                {
                    return false;
                }
                return true;
            }, blueInvoiceInputtHw);

            if (!nextFlag)
            {
                throw new Exception("xxxx:蓝字发票校验页面下一步之后，句柄查找错误");
            }

            //点击确定按钮
            if (isClickConfirm != IntPtr.Zero)
            {
                WinApi.LeftClick(confirmBtn);
            }
            else
            {
                throw new Exception("xxxx:本张发票不可以开红字发票！");
            }

        }

    }
}
