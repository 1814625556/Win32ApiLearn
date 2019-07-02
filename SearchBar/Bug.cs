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
using SearchBar.RequestRed;
using User32Test;

namespace SearchBar
{
    public class Bug
    {
        /// <summary>
        /// 窗体的最大最小化，正常大小
        /// </summary>
        public static void WindownType()
        {
            var bar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427 - [红字发票信息表填开]");
            if (bar == IntPtr.Zero)
                bar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");

            var list = WinApi.EnumChildWindowsCallback(bar);
            var fpoInfoBar = list.Find(b => b.szWindowName == "红字发票信息表填开").hWnd;

            //WinApi.ShowWindow(fpoInfoBar,1);

            //ShowForm(bar);
            if (fpoInfoBar != IntPtr.Zero)
            {
                var winMation = AutomationElement.FromHandle(fpoInfoBar);
                winMation.TryGetCurrentPattern(WindowPattern.Pattern, out var wobj);

                var property1 = ((WindowPattern)wobj).Current.IsModal;
                //((WindowPattern)wobj).SetWindowVisualState(WindowVisualState.Minimized);
                ((WindowPattern)wobj).SetWindowVisualState(WindowVisualState.Maximized);
                ((WindowPattern)wobj).SetWindowVisualState(WindowVisualState.Normal);
            }

            
            var infoBar = WinApi.FindWindow(null, "红字增值税专用发票信息表信息选择");
            if (infoBar != IntPtr.Zero)
            {
                var infoBarMation = AutomationElement.FromHandle(infoBar);
                infoBarMation.TryGetCurrentPattern(WindowPattern.Pattern, out var infoPattern);
            }



            
        }


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


        public static void WriteGoodsSetting(IntPtr goodNoSettingHw, RednotificationDetail detail)
        {
            if (goodNoSettingHw == IntPtr.Zero)
            {
                return;
            }

            if (detail == null)
            {
                detail = new RednotificationDetail()
                {
                    GoodsTaxNo = "101010102",
                    //TaxPer = "1",
                    TaxPerCon = "不征税",
                    TaxRate = "0.17"
                };
            }

            var suilvBar = IntPtr.Zero;
            var yhBar = IntPtr.Zero;
            var yhlBar = IntPtr.Zero;
            var toolStrip = IntPtr.Zero;

            HxShengQing.TryRetry(str =>
            {
                var childInfos = WinApi.FindChildInfo(goodNoSettingHw);
                if (childInfos == null || childInfos.Count < 11)
                {
                    return false;
                }

                //获取toolStrip
                toolStrip = childInfos.Find(b => b.szWindowName == "toolStrip1").hWnd;

                //税率句柄
                suilvBar = childInfos[10].hWnd;

                //享受优惠政策
                yhBar = childInfos[8].hWnd;

                //优惠政策类型
                yhlBar = childInfos[5].hWnd;

                return toolStrip != IntPtr.Zero && suilvBar != IntPtr.Zero 
                && yhBar != IntPtr.Zero && yhlBar != IntPtr.Zero;
            }, "", 20, 500);

            if ("1".Equals(detail.TaxPer) && !string.IsNullOrEmpty(detail.TaxPerCon))
            {
                UIHelper.SetCombox(yhBar, "是");

                //等待优惠政策enable
                Thread.Sleep(1000);
                UIHelper.SetCombox(yhlBar, detail.TaxPerCon.Trim());
            }
            else
            {
                UIHelper.SetCombox(suilvBar, GetByTaxRate(detail.TaxRate));
            }

            ClickBtnByName(toolStrip, "保存");
            Thread.Sleep(1000);

            HxShengQing.SystemOpera("确认",out var message);
            if (message != "修改成功！")
            {
                throw new Exception(message);
            }
        }


        public static void WriteGoodsTaxNoAdd(IntPtr goodNoAddHw, RednotificationDetail detail)
        {
            if (detail == null)
            {
                detail = new RednotificationDetail()
                {
                    GoodsTaxNo = "101010102",
                    TaxPer = "1",
                    TaxPerCon = "不征税",
                    TaxRate = "0.17"
                };
            }
            if (goodNoAddHw == IntPtr.Zero)
            {
                return;
            }

            //税收分类编码
            var ssflbmBar = IntPtr.Zero;
            var toolStrip = IntPtr.Zero;
            var suilvBar = IntPtr.Zero;
            var yhBar = IntPtr.Zero;
            var yhlBar = IntPtr.Zero;

            HxShengQing.TryRetry(str =>
            {
                var childInfos = WinApi.FindChildInfo(goodNoAddHw);
                if (childInfos==null || childInfos.Count < 30)
                {
                    return false;
                }

                //获取分类名称
                var flmnBar = childInfos.Find(b => b.szWindowName == "税收分类名称").hWnd;
                var temp1 = WinApi.FindWindowEx(goodNoAddHw, flmnBar, null, null);
                var temp2 = WinApi.FindWindowEx(temp1, IntPtr.Zero, null, null);
                ssflbmBar = WinApi.FindWindowEx(temp1, temp2, null, null);

                //获取toolStrip
                toolStrip = childInfos.Find(b => b.szWindowName == "toolStrip1").hWnd;

                //获取税率句柄
                var suilv = childInfos.Find(b => b.szWindowName == "*税率").hWnd;
                suilvBar = WinApi.FindWindowEx(goodNoAddHw, suilv, null, null);

                //获取享受优惠政策
                var yh = childInfos.Find(b => b.szWindowName == "规格型号").hWnd;
                yhBar = WinApi.FindWindowEx(goodNoAddHw, yh, null, null);

                //优惠政策类型
                yhlBar = childInfos[8].hWnd;

                return ssflbmBar != IntPtr.Zero && toolStrip != IntPtr.Zero && 
                suilvBar != IntPtr.Zero && yhBar != IntPtr.Zero && yhlBar != IntPtr.Zero;
            },"",20,500);

            if (ssflbmBar == IntPtr.Zero || toolStrip == IntPtr.Zero ||
                suilvBar == IntPtr.Zero || yhBar == IntPtr.Zero || yhlBar == IntPtr.Zero)
            {
                throw new Exception("商品编码窗体，句柄获取失败");
            }

            WinApi.SendMessage(ssflbmBar, WinApi.BM_TEXT, IntPtr.Zero, TaxSub(detail.GoodsTaxNo));
            Thread.Sleep(100);
            WinApi.SendKey(ssflbmBar, WinApi.VK_DOWN);
            Thread.Sleep(100);
            WinApi.SendKey(ssflbmBar, WinApi.VK_RETURN);
            Thread.Sleep(100);
            WinApi.ClickLocation(goodNoAddHw, 300, 10);
            Thread.Sleep(500);

            if ("1".Equals(detail.TaxPer) && !string.IsNullOrEmpty(detail.TaxPerCon))
            {
                UIHelper.SetCombox(yhBar,"是");

                //等待优惠政策enable
                Thread.Sleep(1000); 
                UIHelper.SetCombox(yhlBar, detail.TaxPerCon.Trim());
            }
            else
            {
                UIHelper.SetCombox(suilvBar, GetByTaxRate(detail.TaxRate));
            }

            ClickBtnByName(toolStrip, "保存");
            Thread.Sleep(500);
        }

       
        /// <summary>
        ///根据税率获取相应索引
        /// </summary>
        /// <param name="taxRate"></param>
        /// <returns></returns>
        public static string GetByTaxRate(string taxRate = "")
        {
            taxRate = taxRate.Trim();
            switch (taxRate)
            {
                case "0":
                    return "0%";
                case "0.0":
                    return "0%";
                case "0.00":
                    return "0%";
                case "0.03":
                    return "3%";
                case "0.04":
                    return "4%";
                case "0.05":
                    return "5%";
                case "0.06":
                    return "6%";
                case "0.09":
                    return "9%";
                case "0.10":
                    return "10%";
                case "0.11":
                    return "11%";
                case "0.13":
                    return "13%";
                case "0.16":
                    return "16%";
                case "0.17":
                    return "17%";
                case "减按1.5%计算":
                    return "减按1.5%计算";
                case "中外合作油气田":
                    return "中外合作油气田";
                default:
                    return "";
            }
        }

        public static string TaxSub(string taxStr)
        {
            string result = "";
            for (var i = taxStr.Length - 1; i > 1; i--)
            {
                if (taxStr[i] == '0')
                {
                    result = taxStr.Substring(0, i + 1);
                    continue;
                }
                else
                {
                    result = taxStr.Substring(0, i + 1);
                }

                break;
            }

            return result;
        }

        public static void GetTableFocus()
        {
            WinApi.keybd_event(Keys.Space, 0, 0, 0);
            Thread.Sleep(100);
            WinApi.keybd_event(Keys.Back, 0, 0, 0);
            Thread.Sleep(100);
        }


    }
}
