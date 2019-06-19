using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using User32Test;
using System.Threading;
using SearchBar.RequestRed;

namespace SearchBar
{
    /// <summary>
    /// 航信红字信息表申请
    /// </summary>
    public static class HxShengQing
    {
        #region 测试语句
        //WinApi.ClickLocation((IntPtr) 7145062, 10, 37);//可以点中第一行第一块

        //赋值成功
        //cc.TryGetCurrentPattern
        //    (ValuePattern.Pattern, out var patternObject);
        //((ValuePattern)patternObject).SetValue("mingchenghaha~");

        #endregion


        //Ui自动化操作单选按钮
        public static void RadioButtonTest()
        {
            var zsBar = WinApi.FindWindow(null, "红字增值税专用发票信息表信息选择");
            var automationElement = AutomationElement.FromHandle(zsBar);

            var childs1 = automationElement.FindAll(TreeScope.Descendants, Condition.TrueCondition);

            for (var i = 0; i < childs1.Count; i++)
            {
                if (childs1[i].Current.ControlType != ControlType.RadioButton)
                {
                    continue;
                }
                Console.WriteLine($"Name:{childs1[i].Current.Name}");
                if (childs1[i].Current.Name.Contains("未抵扣"))
                {
                    var autoMation = childs1[i];

                    autoMation.TryGetCurrentPattern(SelectionItemPattern.Pattern, out var obj);
                    if (obj != null)
                    {
                        ((SelectionItemPattern)obj).Select();
                    }
                }
            }
            //var childs2 = automationElement.FindAll(TreeScope.Descendants, Condition.TrueCondition);

        }
        /// <summary>
        /// 复选框操作
        /// </summary>
        public static void CheckBoxTest()
        {
            var zsBar = WinApi.FindWindow(null, "红字增值税专用发票信息表信息选择");
            var automationElement = AutomationElement.FromHandle(zsBar);

            var childs1 = automationElement.FindAll(TreeScope.Descendants, Condition.TrueCondition);

            for (var i = 0; i < childs1.Count; i++)
            {
                if (childs1[i].Current.ControlType != ControlType.CheckBox)
                {
                    continue;
                }
                Console.WriteLine($"Name:{childs1[i].Current.Name}");
                if (childs1[i].Current.Name.Contains("成品油"))
                {
                    var autoMation = childs1[i];

                    autoMation.TryGetCurrentPattern(TogglePattern.Pattern, out var obj);
                    //autoMation.TryGetCurrentPattern(DockPattern.Pattern, out var obj2);
                    if (obj != null)
                    {
                        ((TogglePattern)obj).Toggle();
                        //((DockPattern)obj).;
                    }
                }
            }
        }

        /// <summary>
        /// 下拉框选项
        /// </summary>
        public static void ComboxSelected()
        {
            var zsBar = WinApi.FindWindow(null, "商品编码添加");
            var automationElement = AutomationElement.FromHandle(zsBar);

            var childs1 = automationElement.FindAll(TreeScope.Descendants, Condition.TrueCondition);

            for (var i = 0; i < childs1.Count; i++)
            {
                if (childs1[i].Current.ControlType != ControlType.ComboBox)
                {
                    continue;
                }
                Console.WriteLine($"Name:{childs1[i].Current.Name}");
                if (childs1[i].Current.Name.Contains("商品名称"))
                {
                    var autoMation = childs1[i];

                    UIHelper.SetSelectedComboBoxItem(autoMation, "17%");

                    //autoMation.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out var obj);
                    ////autoMation.TryGetCurrentPattern(DockPattern.Pattern, out var obj2);
                    //if (obj != null)
                    //{
                    //    ((ExpandCollapsePattern)obj).Expand();
                    //    ((ExpandCollapsePattern)obj).Collapse();
                    //    //((DockPattern)obj).;
                    //}
                }
            }
        }




        public static void TestBar()
        {
            //获取开票软件主窗体
            var kprjBar = GetKprjMainPageBar();
            if (kprjBar == IntPtr.Zero)
            {
                return;
            }
            //前置主窗体
            ShowForm(kprjBar);

            var kprjToolBar = WinApi.FindChildInfo(kprjBar)[1].hWnd;

            InvokeMenuItem(kprjToolBar, "红字增值税专用发票信息表填开");

            InvokeMenuItem(kprjToolBar, "红字增值税专用发票信息表查询导出");

        }


        /// <summary>
        /// 点击toolBar句柄下某个按钮--这个有可能点不中--需要优化
        /// </summary>
        /// <param name="toolBar"></param>
        /// <param name="name"></param>
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
                    if (patternObject == null)
                    {
                        throw new Exception($"{name},按钮转换失败");
                    }
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

       /// <summary>
       /// 使用UI自动化的方式操作 菜单栏
       /// </summary>
       /// <param name="hWnd"></param>
       /// <param name="name"></param>
        public static void InvokeMenuItem(IntPtr hWnd, string name)
        {
            if (hWnd == IntPtr.Zero)
                throw new Exception("句柄不能为空Zero");
            var automationElement = AutomationElement.FromHandle(hWnd);
            var propertyCondition1 = new PropertyCondition(AutomationElement.NameProperty, (object)name);
            var first = automationElement.FindFirst(TreeScope.Descendants, (Condition)propertyCondition1);
            var pattern = InvokePattern.Pattern;
            first.TryGetCurrentPattern(pattern, out var patternObject);
            if (!(patternObject is InvokePattern))
            {
                throw new Exception("xx");
            }
            ((InvokePattern)patternObject).Invoke();
        }

        public static void InvokeEditItem(IntPtr Hwnd, string name)
        {
            var bar = WinApi.FindWindow(null, "开具增值税普通发票");
            var list = WinApi.EnumChildWindowsCallback(bar);

            var listChild = WinApi.FindChildInfo(list[2].hWnd);
            var tableBar = listChild[14];

            AutomationElement automationElement = AutomationElement.FromHandle(tableBar.hWnd);
            var tableList = automationElement.FindAll(TreeScope.Children, Condition.TrueCondition);

            var zuhekuan = tableList[0];//组合框
            UIHelper.SetSelectedComboBoxItem(zuhekuan,"17%");

            var zhhekuangList = zuhekuan.FindAll(TreeScope.Children, Condition.TrueCondition);


            var cczhuhe = zhhekuangList[0].FindAll(TreeScope.Children, Condition.TrueCondition);

            //PropertyCondition propertyCondition1 = new PropertyCondition(AutomationElement.NameProperty, (object)"文本");
            //AutomationElement first = zhhekuangList[0].FindFirst(TreeScope.Children, (Condition)propertyCondition1);

            //automationElement.TryGetCurrentPattern(TablePattern.Pattern, out var pb);
            //AutomationElement audemo = ((TablePattern) pb).GetItem(2, 7);

            //var hangling = tableList[0].FindAll(TreeScope.Children, Condition.TrueCondition);
            //var hangyi = tableList[1].FindAll(TreeScope.Children, Condition.TrueCondition);
            var hanger = tableList[2].FindAll(TreeScope.Children, Condition.TrueCondition);
            //var hangsan = tableList[3].FindAll(TreeScope.Children, Condition.TrueCondition);

            //UIHelper.InsertTextUsingUIAutomation(hanger[2], "大份");

            var shuilvlist = hanger[7].FindAll(TreeScope.Children, Condition.TrueCondition);
            

            //hanger[7].TryGetCurrentPattern
            //    (InvokePattern.Pattern, out var patternObject);
            //((InvokePattern)patternObject).Invoke();

            //UIHelper.InsertTextUsingUIAutomation(hangling[2], "大份");
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

        /// <summary>
        /// 商品编码添加
        /// </summary>
        public static void WriteGoodsTaxNoAdd(IntPtr goodNoAddHw, RednotificationDetail detail)
        {
            try
            {
                if (goodNoAddHw == IntPtr.Zero)
                {
                    //AmLogger.Info("WriteGoodsTaxNoAdd", "未找到：商品编码添加界面");
                    return;
                }

                //写入分类编码
                var ssflName = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "税收分类名称");
                var temp1 = WinApi.FindWindowEx((IntPtr)goodNoAddHw, ssflName, null, null);
                var temp2 = WinApi.FindWindowEx(temp1, IntPtr.Zero, null, null);
                var ssflBar = WinApi.FindWindowEx(temp1, temp2, null, null); //获取税收分类编码句柄
                //AmLogger.Info("WriteGoodsTaxNoAdd", $"税收分类编码句柄获取成功ssflBar{ssflBar}");
                WinApi.SendMessage(ssflBar, WinApi.BM_TEXT, IntPtr.Zero, TaxSub(detail.GoodsTaxNo)); //对文本框进行赋值
                Thread.Sleep(200);
                WinApi.SendKey(ssflBar, WinApi.VK_DOWN);
                Thread.Sleep(30);
                WinApi.SendKey(ssflBar, WinApi.VK_RETURN);
                WinApi.ClickLocation((IntPtr)goodNoAddHw, 300, 10);
                Thread.Sleep(200);

                //优惠政策
                if ("1".Equals(detail.TaxPer) && !string.IsNullOrEmpty(detail.TaxPerCon))
                {
                    //AmLogger.Info("WriteGoodsTaxNoAdd", "享受优惠政策");
                    var spflbmBar = WinApi.FindWindow(null, "商品编码添加");
                    var guiGebar = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "规格型号");
                    var guiGeSelectBar = WinApi.FindWindowEx(spflbmBar, guiGebar, null, null);

                    WinApi.ClickLocation(guiGeSelectBar, 10, 10);
                    Thread.Sleep(30);
                    int selected = WinApi.SendMessage(guiGeSelectBar, WinApi.CB_SETCURSEL, (IntPtr)0, "");

                    Thread.Sleep(1000); //等待优惠政策enable

                    var jianma = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "简码");
                    var temp = WinApi.FindWindowEx(spflbmBar, jianma, null, null);
                    temp = WinApi.FindWindowEx(spflbmBar, temp, null, null);
                    var yhzclxBar = WinApi.FindWindowEx(spflbmBar, temp, null, null);
                    int index = detail.TaxPerCon.Trim() == "免税" ? 1 : 2;
                    WinApi.ClickLocation(yhzclxBar, 10, 10);
                    for (var i = 0; i < index; i++)
                    {
                        WinApi.SendKey(yhzclxBar, WinApi.VK_DOWN);
                        Thread.Sleep(30);
                    }

                    Thread.Sleep(100);
                    WinApi.SendKey(ssflBar, WinApi.VK_RETURN);
                    //AmLogger.Info("WriteGoodsTaxNoAdd", $"享受优惠政策:{guiGeSelectBar},yhzclxBar{yhzclxBar}");
                }

                //正常税率采用平台传过来的税率
                else
                {
                    //修改实际传入的税率
                    var child = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "*税率");
                    var suilvBar = WinApi.FindWindowEx((IntPtr)goodNoAddHw, child, null, null);
                    //AmLogger.Info("WriteGoodsTaxNoAdd", $"suilvBar{suilvBar}");
                    //通过索引设置下拉框选项
                    Thread.Sleep(500);
                    //WinApi.SendMessage(suilvBar, WinApi.CB_SETCURSEL, (IntPtr) GetIndexByTaxRate(detail.TaxRate), ""); //调整税率为传入税率
                    WinApi.SetComboxItemValue(suilvBar, GetByTaxRate(detail.TaxRate));
                }

                var stripHw = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "toolStrip1");
                Thread.Sleep(500);
                WinApi.ClickLocation(stripHw, 40, 13); //点击保存
                //点击保存的时候可能弹出错误提示
                Thread.Sleep(500);
                //GetErrorInfo();
                //AmLogger.Info("WriteGoodsTaxNoAdd", "商品编码添加填写成功");
            }
            catch (Exception e)
            {
                //CaptureScreen.TakeScreenShot("商品编码添加", $"{e.Message}");
                //AmLogger.Error("WriteGoodsTaxNoAdd", e);
            }
        }

        //这种方式不防遮挡
        public static void KeyBoard()
        {
            WinApi.keybd_event(Keys.Down, 0, 0, 0);
            Thread.Sleep(100);
            WinApi.keybd_event(Keys.Enter, 0, 0, 0);
        }

        /// <summary>
        /// 点击发票填开
        /// </summary>
        public static void Step1()
        {
            var mainBar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427");//需要改动，模糊查询所有窗体
            var bar = WinApi.FindWindowEx(mainBar, IntPtr.Zero, null, null);
            ClickBtnByName(bar, "发票管理");//点击发票管理

            //找信息表管理父句柄
            var barInfoList = WinApi.EnumChildWindowsCallback(mainBar);
            var infoParentBar = IntPtr.Zero;
            barInfoList.ForEach(i => {
                if (i.szWindowName == "发票管理")
                {
                    infoParentBar = i.hWnd;
                }
            });
            infoParentBar = WinApi.FindWindowEx(infoParentBar, IntPtr.Zero, null, null);
            var infoBar = WinApi.FindWindowEx(infoParentBar, IntPtr.Zero, null, null);
            WinApi.LeftClick(infoBar);//点击信息表成功

            //打开红字增值税专用发票信息表信息选择--没有防遮挡
            WinApi.keybd_event(VBKEY.vbKeyDown, 0, 0, 0);
            Thread.Sleep(100);
            WinApi.keybd_event(VBKEY.vbKeyReturn, 0, 0, 0);


        }

        /// <summary>
        /// 操作红字增值税专用发票信息表信息选择
        /// </summary>
        public static void Step2()
        {
            var zhuanRedBar = IntPtr.Zero;

            zhuanRedBar = TryRetry<string, IntPtr>(
                str => WinApi.FindWindow(null, str), "红字增值税专用发票信息表信息选择", 
                20, 500);
            if (zhuanRedBar == IntPtr.Zero)
            {
                //日志--红字增值税专用发票信息表信息选择查找失败
                return;
            }

            Thread.Sleep(500);

            TryRetry<IntPtr, bool>(bar =>
            {
                var list = WinApi.EnumChildWindowsCallback(bar);
                var PurchaserApplicationBar = list.Find(b => b.szWindowName == "一、购买方申请");
                var SalesAllicationBar = list.Find(b => b.szWindowName == "二、销售方申请");
                var DeductedBar = list.Find(b => b.szWindowName == "1、已抵扣");
                var UnDeductedBar = list.Find(b => b.szWindowName == "2、未抵扣");
                var FpInputBar = list.Find(b => b.szWindowName == "对应蓝字增值税专用发票信息");
                if (PurchaserApplicationBar.hWnd == IntPtr.Zero || SalesAllicationBar.hWnd == IntPtr.Zero ||
                    DeductedBar.hWnd == IntPtr.Zero || UnDeductedBar.hWnd == IntPtr.Zero || FpInputBar.hWnd==IntPtr.Zero)
                {
                    return false;
                }

                //这里是判断条件--真实业务的时候添加
                Thread.Sleep(1000);
                WinApi.LeftClick(SalesAllicationBar.hWnd);//点击rediobutton
                Thread.Sleep(1000);

                var FpInputList = WinApi.FindChildBar(FpInputBar.hWnd);
                WinApi.SendMessage(FpInputList[0], 0X0C, IntPtr.Zero, "123456");//发票号码
                WinApi.SendMessage(FpInputList[1], 0X0C, IntPtr.Zero, "654321");//发票代码


                //Thread.Sleep(1000);
                //WinApi.LeftClick(PurchaserApplicationBar.hWnd);
                //Thread.Sleep(1000);
                //WinApi.LeftClick(DeductedBar.hWnd);
                //Thread.Sleep(1000);
                //WinApi.LeftClick(UnDeductedBar.hWnd);

                return true;
            },zhuanRedBar, 20, 500);


        }

        /// <summary>
        /// 对名称和识别号进行赋值
        /// </summary>
        public static void step3()
        {
            //获取所有桌面窗体句柄
            var alldeskBar = WinApi.FindChildInfo(IntPtr.Zero);

            //获取开票软件句柄
            var kprjBar = alldeskBar.Find(bar =>bar.szWindowName != null && bar.szWindowName.Contains("开票软件"));

            //获取开票软件下面的所有子句柄
            var kprjList = WinApi.EnumChildWindowsCallback(kprjBar.hWnd);

            //获取红字信息填开句柄--模糊查询
            var hzxxtkBar = kprjList.Find(bar =>
                bar.szWindowName != null && bar.szWindowName.Contains("红字发票信息表填开"));

            //三个--最后一个对应toolBar
            var hzxxtklist = WinApi.FindChildInfo(hzxxtkBar.hWnd);

            var hzxxtkInfoList = WinApi.FindChildInfo(hzxxtklist[0].hWnd);
            var hzxxtkInfoChild = WinApi.FindChildInfo(hzxxtkInfoList[2].hWnd);
            var sfxxlist = WinApi.FindChildInfo(hzxxtkInfoChild[1].hWnd);

            //对销方名称进行赋值
            var mclist = WinApi.FindChildBar(sfxxlist[1].hWnd);
            mclist.ForEach(mc=> WinApi.SendMessage(mc, 0x0C, IntPtr.Zero, "名称测试"));
            
            //对纳税人识别号进行赋值
            var bmlist = WinApi.FindChildBar(sfxxlist[0].hWnd);
            bmlist.ForEach(mc => WinApi.SendMessage(mc, 0x0C, IntPtr.Zero, "12345678901234567"));

            //点击价格

            //ClickBtnByName(hzxxtklist[hzxxtklist.Count - 1].hWnd, "价格");//这里貌似不需要点击 默认都是不含税
            ClickBtnByName(hzxxtklist[hzxxtklist.Count - 1].hWnd, "价格");//这里貌似不需要点击 默认都是不含税
        }
        /// <summary>
        /// 折扣处理，正式环境下折扣行只会有一行。
        /// </summary>
        public static void step4()
        {
            //获取所有桌面窗体句柄
            var alldeskBar = WinApi.FindChildInfo(IntPtr.Zero);

            //获取开票软件句柄
            var kprjBar = alldeskBar.Find(bar => bar.szWindowName != null && bar.szWindowName.Contains("开票软件"));

            //获取开票软件下面的所有子句柄
            var kprjList = WinApi.EnumChildWindowsCallback(kprjBar.hWnd);

            //获取红字信息填开句柄--模糊查询
            var hzxxtkBar = kprjList.Find(bar =>
                bar.szWindowName != null && bar.szWindowName.Contains("红字发票信息表填开"));

            //三个--最后一个对应toolBar
            var hzxxtklist = WinApi.FindChildInfo(hzxxtkBar.hWnd);

            //点击差额征税按钮
            ClickBtnByName(hzxxtklist[hzxxtklist.Count - 1].hWnd,"差额");

            var zhekouWinBar = WinApi.FindWindow(null, "输入扣除额");
            var zhekouBar = WinApi.FindWindowEx(zhekouWinBar, IntPtr.Zero, null, null);
            var zhekouList = WinApi.FindChildBar(zhekouBar);
            
            //这是确定按钮
            var confirmBar = zhekouList[0];
            var amountBar = zhekouList[zhekouList.Count - 1];

            //输入折扣金额
            WinApi.SendMessage(amountBar, 0X0C, IntPtr.Zero, "10");
            Thread.Sleep(1000);
            //点击确定按钮
            WinApi.LeftClickMsg(confirmBar);
        }

        /// <summary>
        /// 点击不打印按钮
        /// </summary>
        public static void step5()
        {
            var dayinBar = WinApi.FindWindow(null, "打印");
            var dayinlist = WinApi.FindChildInfo(dayinBar);
            var budayinBar = dayinlist.Find(bar => bar.szWindowName == "不打印");
            WinApi.LeftClickMsg(budayinBar.hWnd);
        }
        /// <summary>
        /// 提示框点击取消按钮
        /// </summary>
        public static void step6()
        {
            SystemOpera("取消");
        }

        /// <summary>
        /// 根据信息表流水账号，查询对应的信息表流水账号账单并点击上传按钮
        /// </summary>
        public static void step7()
        {
            //获取所有桌面窗体句柄
            var alldeskBar = WinApi.FindChildInfo(IntPtr.Zero);

            //获取开票软件句柄
            var kprjBar = alldeskBar.Find(bar => bar.szWindowName != null && bar.szWindowName.Contains("开票软件"));

            //获取开票软件下面的所有子句柄
            var kprjList = WinApi.EnumChildWindowsCallback(kprjBar.hWnd);

            //获取toolStrip1句柄
            var toolStripBar = kprjList.Find(bar =>
                bar.szWindowName != null && bar.szWindowName == "toolStrip1");


            var inputTextBar = WinApi.FindWindowEx(toolStripBar.hWnd, IntPtr.Zero, null, null);
            WinApi.SendMessage(inputTextBar, 0X0C, IntPtr.Zero, "661735289405190530193705");
            Thread.Sleep(1000);
            WinApi.SendKey(inputTextBar, KeySnap.VK_ENTER);

            var indexClickBar = 0;
            for (var i = 0; i < kprjList.Count; i++)
            {
                if (kprjList[i].szWindowName == "红字发票信息表查询导出")
                    indexClickBar = i + 2;
            }
            //ClickBtnByName(kprjList[indexClickBar].hWnd, "");
            WinApi.ClickLocation(kprjList[indexClickBar].hWnd, 10, 55);
            ClickBtnByName(toolStripBar.hWnd,"上传");
        }
        /// <summary>
        /// 信息表上传中
        /// </summary>
        public static void GetInfoLoading()
        {
            while (true)
            {
                var bar = WinApi.FindWindow(null, "信息表上传中");
                Console.WriteLine(bar);
                if (bar != IntPtr.Zero)
                {
                    Thread.Sleep(200);
                }
                else
                {
                    Console.WriteLine("上传完毕");
                    break;
                }
            }
            

        }

        /// <summary>
        /// 获取信息表流水账号--已经使用UI自动化方式替代
        /// </summary>
        public static void Getsuihao()
        {
            //获取所有桌面窗体句柄
            var alldeskBar = WinApi.FindChildInfo(IntPtr.Zero);

            //获取开票软件句柄
            var kprjBar = alldeskBar.Find(bar => bar.szWindowName != null && bar.szWindowName.Contains("开票软件"));

            //获取开票软件下面的所有子句柄
            var kprjList = WinApi.EnumChildWindowsCallback(kprjBar.hWnd);

            //获取红字信息填开句柄--模糊查询
            var hzxxtkBar = kprjList.Find(bar =>
                bar.szWindowName != null && bar.szWindowName.Contains("红字发票信息表填开"));

            var hzxxtkListBars = WinApi.EnumChildWindowsCallback(hzxxtkBar.hWnd);
            var bhindex = 0;
            for (var i = 0; i < hzxxtkListBars.Count; i++)
            {
                if (hzxxtkListBars[i].szWindowName == "开具红字增值税专用发票信息表")
                    bhindex = i + 1;
            }
            var hzbm = hzxxtkListBars[bhindex].szWindowName;
        }


        /// <summary>
        /// 开票软件前置，detail填写，暂时这样--如果分辨率调整肯定会受到影响的
        /// </summary>
        static void InfoTianKai()
        {
            Thread.Sleep(100);
            WinApi.ShowWindow((IntPtr)1442640, 2);
            Thread.Sleep(100);
            WinApi.ShowWindow((IntPtr)1442640, 3);
            Thread.Sleep(100);
            WinApi.SetFocus((IntPtr)1442640);
            Thread.Sleep(100);
            WinApi.SetForegroundWindow((IntPtr)1442640);


            Thread.Sleep(2000);
            for (var i = 0; i < 5; i++)
            {
                WinApi.ClickLocation((IntPtr)7145062, 10, 25 + 23 * i + 12); //可以点中第一行第一块
                GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait($"{i}aaaaa");
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                HxShengQing.WriteGoodsTaxNoAdd(IntPtr.Zero, null);
                Thread.Sleep(1000);
                HxShengQing.ClickBtnByName((IntPtr)7472786, "增行");
                Thread.Sleep(1000);
            }
        }
        public static void GetTableFocus()
        {
            WinApi.keybd_event(Keys.Space, 0, 0, 0);
            Thread.Sleep(100);
            WinApi.keybd_event(Keys.Back, 0, 0, 0);
            Thread.Sleep(100);
        }
        /// <summary>
        /// 提示框弹出框
        /// </summary>
        /// <param name="btnName"></param>
        public static bool SystemOpera(string btnName)
        {
            //多次重复获取按钮
            var btn = TryRetry(str =>
            {
                var sysBar = WinApi.FindWindow(null, str);
                var sysListBars = WinApi.EnumChildWindowsCallback(sysBar);
                return sysListBars.Find(b => b.szWindowName == btnName);

            }, "SysMessageBox");

            if (btn.hWnd == IntPtr.Zero)
            {
                return false;
            }
            WinApi.LeftClickMsg(btn.hWnd);
            return true;
        }
        public static bool SystemOpera(string btnName,out string message)
        {
            message = "";
            var messageCopy = "";
            //多次重复获取按钮
            var btn = TryRetry(str =>
            {
                var sysBar = WinApi.FindWindow(null, str);
                var sysListBars = WinApi.EnumChildWindowsCallback(sysBar);
                messageCopy = sysListBars[4].szWindowName;
                return sysListBars.Find(b => b.szWindowName == btnName);
            }, "SysMessageBox");

            if (btn.hWnd == IntPtr.Zero)
            {
                return false;
            }
            message = messageCopy;
            WinApi.LeftClickMsg(btn.hWnd);
            return true;
        }


        //========================================第一个接口=======================================================
        /// <summary>
        /// 
        /// </summary>
        public static void DiYigeJieKou(RednotificationInfo redInfo)
        {
            //获取开票软件主窗体
            var kprjBar = GetKprjMainPageBar();
            if (kprjBar == IntPtr.Zero)
            {
                return;
            }
            //前置主窗体
            ShowForm(kprjBar);

            var kprjToolBar = WinApi.FindChildInfo(kprjBar)[0].hWnd;
            //1：点击发票管理
            ClickBtnByName(kprjToolBar, "发票管理");

            Thread.Sleep(1000);

            //2：主界面进入 - 增值税专用发票信息表填开
            var kprjToolBar2 = WinApi.FindChildInfo(kprjBar)[1].hWnd;
            InvokeMenuItem(kprjToolBar2, "红字增值税专用发票信息表填开");

            //3：操作-增值税专用发票信息表填开
            if (!RedInfoTianKai(redInfo))
            {
                //日志--操作增值税专用发票信息表填开页面失败
            }

            //4：操作红字发票信息表填开--里面有明细
            if (!RedInfoFpTianKai(kprjBar, redInfo,out var hzbm))
            {
                //日志--红字信息填开失败
            }
            //5：点击打印--选择不打印
            DaYin();
            //6：点击取消
            SystemOpera("取消");

            Thread.Sleep(1000);

            //7：进入查询导出页面
            InvokeMenuItem(kprjToolBar2, "红字增值税专用发票信息表查询导出");

            //8：上传红字信息表
            UploadHzInfo(kprjBar, hzbm);

            //9：等待信息表上传
            if (!WaitLoadingForm("信息表上传中"))
            {
                //throw new Exception("信息表上传失败");
                Console.WriteLine("信息表上传错误");
            }

            //10：获取对应的红字信息
            var entity = DateBaseHelper.GetHzscResult(hzbm);
            Console.WriteLine("success...");
        }

        //========================================第二个接口=======================================================
        /// <summary>
        /// 红字信息表专票查询导出--第二个接口了
        /// </summary>
        public static void ChaXunIsLoadingSuccess()
        {
            //获取开票软件主窗体
            var kprjBar = GetKprjMainPageBar();
            if (kprjBar == IntPtr.Zero)
            {
                return;
            }
            //前置主窗体
            ShowForm(kprjBar);

            var kprjToolBar = WinApi.FindChildInfo(kprjBar)[0].hWnd;
            //1：点击发票管理
            ClickBtnByName(kprjToolBar, "发票管理");

            Thread.Sleep(1000);

            //2：主界面进入-增值税专用发票信息表填开
            //if (!EnterRedInfoTianKai(kprjBar))
            //{
            //    //日志--进入增值税专用发票信息表填开页面失败
            //}

            //2：主界面进入 - 增值税专用发票信息表填开
            var kprjToolBar2 = WinApi.FindChildInfo(kprjBar)[1].hWnd;
            InvokeMenuItem(kprjToolBar2, "红字增值税专用发票信息表查询导出");


            Thread.Sleep(2000);
            //获取开票软件下面的所有子句柄
            var kprjList = WinApi.EnumChildWindowsCallback(kprjBar);

            //获取toolStrip1句柄
            var toolStripBar = kprjList.Find(bar =>
                bar.szWindowName != null && bar.szWindowName == "toolStrip1");

            //点击下载按钮
            ClickBtnByName(toolStripBar.hWnd, "下载");


            Thread.Sleep(1000);
            var hxshjgBar = WinApi.FindWindow(null, "红字发票信息表审核结果下载条件设置");//需要retry
            var hxshjgList = WinApi.EnumChildWindowsCallback(hxshjgBar);
            var tkrqBar = hxshjgList.Find(bar => bar.szWindowName == "填开日期");
            var confirmBtn = hxshjgList.Find(bar => bar.szWindowName == "确定");
            var xinxibiaoxinxiBar = hxshjgList.Find(bar => bar.szWindowName == "信息表信息");//信息表信息

            //日期修改
            var riqiList = WinApi.FindChildInfo(tkrqBar.hWnd);
            WinApi.LeftClick(riqiList[1].hWnd);
            SendKeys.SendWait("2018");
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Right, 0,0,0);
            Thread.Sleep(500);
            SendKeys.SendWait("05");
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Right, 0, 0, 0);
            Thread.Sleep(500);
            SendKeys.SendWait("01");
            Thread.Sleep(500);

            WinApi.LeftClick(riqiList[0].hWnd);
            SendKeys.SendWait("2019");
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Right, 0, 0, 0);
            Thread.Sleep(500);
            SendKeys.SendWait("05");
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Right, 0, 0, 0);
            Thread.Sleep(500);
            SendKeys.SendWait("31");
            Thread.Sleep(500);

            //信息表信息修改
            var xixinInfoList = WinApi.FindChildInfo(xinxibiaoxinxiBar.hWnd);
            //信息表编号
            WinApi.SendMessage(xixinInfoList[1].hWnd, 0X0C, IntPtr.Zero, "661735289405190530193705");
            //购方税号
            WinApi.SendMessage(xixinInfoList[xixinInfoList.Count-1].hWnd, 0X0C, IntPtr.Zero, "6217920170548015");
            //销方税号
            WinApi.SendMessage(xixinInfoList[xixinInfoList.Count-2].hWnd, 0X0C, IntPtr.Zero, "7217920170548015");

            //点击确定按钮
            WinApi.LeftClickMsg(confirmBtn.hWnd);

            //等待信息表下载
            TryRetry(str =>
            {
                var infoDownLoadBar = WinApi.FindWindow(null, "信息表下载中");
                var automation = AutomationElement.FromHandle(infoDownLoadBar);

                var sysAlertBar = WinApi.FindWindow(null, "SysMessageBox");
                if (sysAlertBar != IntPtr.Zero)
                {
                    SystemOpera("确认");
                }

                return automation.Current.IsOffscreen;
                //SystemOpera("")
            }, "信息表下载中",60,1000);

            
            Console.WriteLine("success...");
        }

        //========================================帮助方法=======================================================

        /// <summary>
        /// 获取开票软件句柄
        /// </summary>
        /// <returns></returns>
        public static IntPtr GetKprjMainPageBar()
        {
            try
            {
                //获取所有桌面窗体句柄
                var alldeskBar = WinApi.FindChildInfo(IntPtr.Zero);
                //获取开票软件句柄
                var kprjBar = alldeskBar.Find(bar => bar.szWindowName != null && bar.szWindowName.Contains("开票软件"));
                if (kprjBar.hWnd == IntPtr.Zero)
                {
                    Console.WriteLine("开票软件未运行");
                    return IntPtr.Zero;
                }
                return kprjBar.hWnd;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return IntPtr.Zero;
            }
            return IntPtr.Zero;
        }

        /// <summary>
        /// 进入增值税专用发票信息表填开
        /// </summary>
        /// <param name="mainBar"></param>
        public static bool EnterRedInfoTianKai(IntPtr mainBar)
        {
            if (mainBar == IntPtr.Zero)
            {
                //日志 -- 
                return false;
            }
            //找信息表管理父句柄
            var barInfoList = WinApi.EnumChildWindowsCallback(mainBar);
            var infoParentBar = IntPtr.Zero;
            barInfoList.ForEach(i => {
                if (i.szWindowName == "发票管理")
                {
                    infoParentBar = i.hWnd;
                }
            });
            
            infoParentBar = WinApi.FindWindowEx(infoParentBar, IntPtr.Zero, null, null);
            var infoBar = WinApi.FindWindowEx(infoParentBar, IntPtr.Zero, null, null);
            if (infoBar == IntPtr.Zero)
            {
                //日志--未找到发票管理句柄，打开增值税专用发票信息表填开页面失败
                return false;
            }
            //点击信息表成功
            WinApi.LeftClick(infoBar);

            //打开红字增值税专用发票信息表信息选择--没有防遮挡
            WinApi.keybd_event(VBKEY.vbKeyDown, 0, 0, 0);
            Thread.Sleep(100);
            WinApi.keybd_event(VBKEY.vbKeyReturn, 0, 0, 0);
            return true;
        }
        /// <summary>
        /// 操作增值税专用发票信息表填开页面
        /// </summary>
        /// <param name="redInfoEntity"></param>
        /// <returns></returns>
        public static bool RedInfoTianKai(RednotificationInfo redInfoEntity)
        {
            var zhuanRedBar = TryRetry<string, IntPtr>(
                str => WinApi.FindWindow(null, str), "红字增值税专用发票信息表信息选择",
                20, 500);
            if (zhuanRedBar == IntPtr.Zero)
            {
                //日志--红字增值税专用发票信息表信息选择查找失败
                return false;
            }
            //等待组件加载
            Thread.Sleep(500);

            var purchaserApplicationBar = new WindowInfo();
            var salesAllicationBar = new WindowInfo();
            var deductedBar = new WindowInfo();
            var unDeductedBar = new WindowInfo();
            var fpInputBar = new WindowInfo();
            var confirmBtnBar = new WindowInfo();
            var nextStepBar = new WindowInfo();

            var flag = TryRetry<IntPtr, bool>(bar =>
            {
                var list = WinApi.EnumChildWindowsCallback(bar);
                purchaserApplicationBar = list.Find(b => b.szWindowName == "一、购买方申请");
                salesAllicationBar = list.Find(b => b.szWindowName == "二、销售方申请");
                deductedBar = list.Find(b => b.szWindowName == "1、已抵扣");
                unDeductedBar = list.Find(b => b.szWindowName == "2、未抵扣");
                fpInputBar = list.Find(b => b.szWindowName == "对应蓝字增值税专用发票信息");
                confirmBtnBar = list.Find(b => b.szWindowName == "确定");
                nextStepBar = list.Find(b => b.szWindowName == "下一步"); 
                if (purchaserApplicationBar.hWnd == IntPtr.Zero || salesAllicationBar.hWnd == IntPtr.Zero ||
                    confirmBtnBar.hWnd==IntPtr.Zero || nextStepBar.hWnd == IntPtr.Zero ||
                    deductedBar.hWnd == IntPtr.Zero || unDeductedBar.hWnd == IntPtr.Zero || fpInputBar.hWnd == IntPtr.Zero)
                {
                    return false;
                }

                return true;
            }, zhuanRedBar, 20, 500);

            if (!flag)
            {
                //日志--句柄获取失败，打印上述句柄
                return false;
            }

            //销方申请
            if (redInfoEntity.notificationHead.RequestMemo.Trim() == "2")
            {
                WinApi.LeftClick(salesAllicationBar.hWnd);
                Thread.Sleep(1000);
                var FpInputList = WinApi.FindChildBar(fpInputBar.hWnd);
                WinApi.SendMessage(FpInputList[0], 0X0C, IntPtr.Zero, "02038375");//发票号码
                Thread.Sleep(100);
                WinApi.SendMessage(FpInputList[1], 0X0C, IntPtr.Zero, "4400081140");//发票代码
                Thread.Sleep(500);
                WinApi.LeftClick(nextStepBar.hWnd);
                if (!RedInfoConfirm(zhuanRedBar))
                {
                    //日志--不能开具红字专票
                    return false;
                }
                return true;
            }

            //购方申请
            WinApi.LeftClick(purchaserApplicationBar.hWnd);
            Thread.Sleep(100);

            //未抵扣
            if (redInfoEntity.notificationHead.RequestMemo.Trim() == "0")
            {
                WinApi.LeftClick(deductedBar.hWnd);
            }

            //已抵扣
            if (redInfoEntity.notificationHead.RequestMemo.Trim() == "1")
            {
                WinApi.LeftClick(unDeductedBar.hWnd);
                Thread.Sleep(500);

                var fpInputList = WinApi.FindChildBar(fpInputBar.hWnd);
                WinApi.SendMessage(fpInputList[0], 0X0C, IntPtr.Zero, "0203837");//发票号码
                Thread.Sleep(100);
                WinApi.SendMessage(fpInputList[1], 0X0C, IntPtr.Zero, "4400081140");//发票代码
            }

            Thread.Sleep(500);
            //点击确定按钮
            WinApi.LeftClick(confirmBtnBar.hWnd);

            return true;
        }

        /// <summary>
        /// 红字发票信息表填开--填写红字信息表
        /// </summary>
        /// <returns></returns>
        public static bool RedInfoFpTianKai(IntPtr kprjBar,RednotificationInfo redInfoEntity, out string hzbm)
        {
            hzbm = "";
            if (kprjBar == IntPtr.Zero)
            {
                //日志---开票软件句柄获取失败
                return false;
            }
            var hzxxtklist = new List<WindowInfo>();
            //红字信息编码
            TryRetry<IntPtr, bool>(b =>
            {
                //获取开票软件下面的所有子句柄
                var kprjList = WinApi.EnumChildWindowsCallback(kprjBar);
                //获取红字信息填开句柄--模糊查询
                var hzxxtkBar = kprjList.Find(bar =>
                    bar.szWindowName != null && bar.szWindowName.Contains("红字发票信息表填开"));
                hzxxtklist = WinApi.FindChildInfo(hzxxtkBar.hWnd);
                return hzxxtkBar.hWnd != IntPtr.Zero && hzxxtklist.Count >= 5;
            }, kprjBar, 20, 500);

            if (hzxxtklist.Count < 5)
            {
                //日志--句柄查找错误
                return false;
            }

            //销方申请进来的--已经填写完毕直接返回就好
            if (redInfoEntity.notificationHead.RequestMemo.Trim() == "2")
            {
                return true;
            }

            //三个--最后一个对应toolBar
            var hzxxtkInfoList = WinApi.FindChildInfo(hzxxtklist[0].hWnd);
            var hzxxtkInfoChild = WinApi.FindChildInfo(hzxxtkInfoList[2].hWnd);
            var sfxxlist = WinApi.FindChildInfo(hzxxtkInfoChild[1].hWnd);
            var toolBar = hzxxtklist[hzxxtklist.Count - 1].hWnd;

            //对销方名称进行赋值
            var mclist = WinApi.FindChildBar(sfxxlist[1].hWnd);
            mclist.ForEach(mc => WinApi.SendMessage(mc, 0x0C, IntPtr.Zero, "名称测试"));

            //对纳税人识别号进行赋值
            var bmlist = WinApi.FindChildBar(sfxxlist[0].hWnd);
            bmlist.ForEach(mc => WinApi.SendMessage(mc, 0x0C, IntPtr.Zero, "12345678901234567"));

            var taxKind = redInfoEntity.notificationHead.TaxKind.Trim();
            if (taxKind != "2" && taxKind != "0")
            {
                //日志--没有该业务场景
                return false;
            }

            //差额逻辑
            if (taxKind == "2")
            {
                if (redInfoEntity.notificationDetails?.Count != 1)
                {
                    //日志--差额征收--明细只能有一行
                    return false;
                }
                ClickBtnByName(toolBar, "差额");
                if (!ChaEPage("100"))
                {
                    //日志--扣除额界面过度失败
                    return false;
                }
                Thread.Sleep(200);
                //不含税
                ClickBtnByName(toolBar, "价格");
            }

            //获取table数据句柄
            var dataBar = WinApi.FindWindowEx(hzxxtklist[0].hWnd, IntPtr.Zero, null, null);
            dataBar = WinApi.FindWindowEx(dataBar, IntPtr.Zero, null, null);

            //填开数据---分类编码设置还没有走通--需要测试
            if (!RedInfoFpTianKaiData(dataBar, toolBar, redInfoEntity.notificationDetails))
            {
                //--日志数据填写异常
                return false;
            }
            //获取税号
            var hzshList = WinApi.FindChildInfo(hzxxtklist[3].hWnd);
            hzbm = hzshList[1].szWindowName;
            //点击打印
            ClickBtnByName(toolBar,"打印");
            return true;
        }

        /// <summary>
        /// 表格数据填写
        /// </summary>
        /// <param name="dataBar"></param>
        /// <param name="toolBar"></param>
        /// <param name="details"></param>
        /// <returns></returns>
        public static bool RedInfoFpTianKaiData(IntPtr dataBar,IntPtr toolBar, List<RednotificationDetail> details)
        {
            for (var i = 0; i < details.Count; i++)
            {
                WinApi.ClickLocation(dataBar, 10, 25 + 23 * i + 12); //可以点中第一行第一块
                GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait(details[i].ItemName);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                Thread.Sleep(200);

                var ssbmszBar = IntPtr.Zero;
                var ssbmtjBar = IntPtr.Zero;
                TryRetry(str =>
                {
                    ssbmszBar = WinApi.FindWindow(null, "税收分类编码设置");
                    ssbmtjBar = WinApi.FindWindow(null, "商品编码添加");
                    return ssbmszBar != IntPtr.Zero || ssbmtjBar != IntPtr.Zero;
                },"",4,500);

                
                WriteGoodsTaxNoSetting(ssbmszBar,details[i]);
                WriteGoodsTaxNoAdd(ssbmtjBar,details[i]);

                //规格型号
                GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait(details[i].ItemSpec);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                Thread.Sleep(500);

                //单位
                GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait(details[i].Unit);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                Thread.Sleep(500);

                //数量
                GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait(details[i].Quantity);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                Thread.Sleep(500);

                //单价不含税
                GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait(details[i].UnitPrice);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                Thread.Sleep(500);

                if (i == details.Count - 1)
                {
                    break;
                }
                ClickBtnByName(toolBar, "增行");
                Thread.Sleep(500);
            }

            return true;
        }

        /// <summary>
        /// 打印界面选择不打印
        /// </summary>
        /// <returns></returns>
        public static bool DaYin(string btnName="不打印")
        {
            var nodyBar = TryRetry(str =>
            {
                var daYinBar = WinApi.FindWindow(null, "打印");
                var childs = WinApi.EnumChildWindowsCallback(daYinBar);
                return childs.Find(b => b.szWindowName == str).hWnd;
            }, btnName, 20, 500);

            if (nodyBar==IntPtr.Zero)
            {
                //日志--未找到不打印按钮
                return false;
            }
            //点击不打印按钮
            WinApi.LeftClickMsg(nodyBar);
            return true;
        }

        /// <summary>
        /// 差额征收
        /// </summary>
        /// <param name="taxDedunction">扣除额</param>
        public static bool ChaEPage(string taxDedunction)
        {
            var zhekouWinBar = IntPtr.Zero;
            var zhekouBar = IntPtr.Zero;
            TryRetry<string, bool>(str =>
            {
                zhekouWinBar = WinApi.FindWindow(null, str);
                zhekouBar = WinApi.FindWindowEx(zhekouWinBar, IntPtr.Zero, null, null);
                return zhekouWinBar != IntPtr.Zero && zhekouBar != IntPtr.Zero;
            }, "输入扣除额",20, 500);
            if (zhekouBar == IntPtr.Zero)
            {
                //日志--差额界面打开失败
                return false;
            }

            var zhekouList = WinApi.FindChildBar(zhekouBar);

            //这是确定按钮
            var confirmBar = zhekouList[0];
            var amountBar = zhekouList[zhekouList.Count - 1];

            //输入折扣金额
            WinApi.SendMessage(amountBar, 0X0C, IntPtr.Zero, "10");
            Thread.Sleep(1000);
            //点击确定按钮
            WinApi.LeftClickMsg(confirmBar);
            return true;
        }

        /// <summary>
        /// 销方申请-增值税专用发票信息表填开页面-确定
        /// </summary>
        /// <returns></returns>
        public static bool RedInfoConfirm(IntPtr zhuanRedBar)
        {
            var cancelBtnBar = new WindowInfo();
            var redBar = new WindowInfo();
            var confirmBtnBar = new WindowInfo();

            var flag = TryRetry<IntPtr, bool>(bar =>
            {
                var list = WinApi.EnumChildWindowsCallback(bar);
                redBar = list.Find(b => b.szWindowName.Contains("本张发票可以开红字发票！"));
                confirmBtnBar = list.Find(b => b.szWindowName == "确定");
                cancelBtnBar = list.Find(b => b.szWindowName == "取消"); 
                return redBar.hWnd != IntPtr.Zero && cancelBtnBar.hWnd != IntPtr.Zero && confirmBtnBar.hWnd != IntPtr.Zero;
            }, zhuanRedBar, 20, 500);

            if (!flag)
            {
                //日志--句柄查找错误 打印句柄:redBar.szWindowName
            }
            if (redBar.szWindowName == "本张发票可以开红字发票！")
            {
                WinApi.LeftClick(confirmBtnBar.hWnd);
            }
            else
            {
                //日志--redBar.szWindowName
                Console.WriteLine(redBar.szWindowName);
                WinApi.LeftClick(cancelBtnBar.hWnd);
            }
            return flag;
        }

        /// <summary>
        /// 第一个接口最后一步，红字信息表上传
        /// </summary>
        /// <param name="hzbm"></param>
        /// <returns></returns>
        public static bool UploadHzInfoBak(IntPtr hwnd, string hzbm)
        {
            AutomationElement toolStripMation = null;
            AutomationElement tableGridMation = null;

            TryRetry(bar =>
            {
                var winMations = AutomationElement.FromHandle(bar);
                toolStripMation = winMations.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, "toolStrip1"));//toolStrip1

                tableGridMation = winMations.FindFirst(TreeScope.Descendants,
                    new PropertyCondition(AutomationElement.NameProperty, "DataGridView"));//DataGridView

                return toolStripMation.Current.NativeWindowHandle != 0 && tableGridMation.Current.NativeWindowHandle != 0;
            },hwnd,20,500);

            //输入数据
            var inputMation = toolStripMation.FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            inputMation.TryGetCurrentPattern
                (ValuePattern.Pattern, out var patternObject);
            if (patternObject == null)
            {
                throw new Exception("编辑按钮出错");
            }
            ((ValuePattern)patternObject).SetValue(hzbm);

            //var rowMations = tableGridMation.FindAll(TreeScope.Children, Condition.TrueCondition);
            Thread.Sleep(500);

            //点击选中
            WinApi.ClickLocation((IntPtr) tableGridMation.Current.NativeWindowHandle, 10, 50);

            //点击上传
            ClickBtnByName((IntPtr)toolStripMation.Current.NativeWindowHandle, "上传");

            SystemOpera("确认");
            return true;
        }


        public static bool UploadHzInfo(IntPtr hwnd, string hzbm,bool isDebug=true)
        {
            if (isDebug)
            {
                hwnd = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190427 - [红字发票信息表查询导出]");
            }
            var toolStripBar = IntPtr.Zero;
            var tableGridBar = IntPtr.Zero;

            TryRetry(bar =>
            {
                var ChildList = WinApi.EnumChildWindowsCallback(hwnd);

                toolStripBar = ChildList.Find(b=>b.szWindowName== "toolStrip1").hWnd;//toolStrip1

                var dataGridBar = ChildList.Find(b => b.szWindowName == "红字发票信息表查询导出").hWnd;
                dataGridBar = WinApi.FindWindowEx(dataGridBar, IntPtr.Zero, null, null);
                tableGridBar = WinApi.FindWindowEx(dataGridBar, IntPtr.Zero, null, null);
                //tableGridMation = winMations.FindFirst(TreeScope.Descendants,
                //    new PropertyCondition(AutomationElement.NameProperty, "DataGridView"));//DataGridView

                return toolStripBar != IntPtr.Zero;
            }, hwnd, 20, 500);

            //输入数据
            var inputMation = AutomationElement.FromHandle(toolStripBar).FindFirst(TreeScope.Children,
                new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            inputMation.TryGetCurrentPattern
                (ValuePattern.Pattern, out var patternObject);
            if (patternObject == null)
            {
                throw new Exception("编辑按钮出错");
            }
            ((ValuePattern)patternObject).SetValue(hzbm);

            //var rowMations = tableGridMation.FindAll(TreeScope.Children, Condition.TrueCondition);
            Thread.Sleep(500);

            //点击选中
            WinApi.ClickLocation(tableGridBar, 10, 50);

            //点击上传
            ClickBtnByName(toolStripBar, "上传");
            //点击确认
            SystemOpera("确认");
            return true;
        }

        /// <summary>
        /// 信息表上传，下载等待框
        /// </summary>
        /// <param name="waitFormName"></param>
        /// <param name="clickBtnName"></param>
        /// <returns></returns>
        public static bool WaitLoadingForm(string waitFormName,string clickBtnName= "确认")//信息表上传中
        {
            var result = true;
            Thread.Sleep(2000);
            TryRetry(str =>
            {
                var infoDownLoadBar = WinApi.FindWindow(null, str);
                var automation = AutomationElement.FromHandle(infoDownLoadBar);

                var sysAlertBar = WinApi.FindWindow(null, "SysMessageBox");
                if (sysAlertBar != IntPtr.Zero)
                {
                    SystemOpera(clickBtnName);
                    result = false;
                }
                return automation.Current.IsOffscreen;
            }, waitFormName, 60, 1000);
            return result;
        }

        /// <summary>
        /// 窗体前置
        /// </summary>
        /// <param name="bar"></param>
        static void ShowForm(IntPtr bar)
        {
            WinApi.ShowWindow(bar, 2);
            Thread.Sleep(100);
            WinApi.ShowWindow(bar, 3);
            Thread.Sleep(100);
        }
        /// <summary>
        /// 填写税收分类编码设置框
        /// </summary>
        /// <param name="detail"></param>
        public static void WriteGoodsTaxNoSetting(IntPtr goodNoSettingHw ,RednotificationDetail detail)
        {
            try
            {     
                if (goodNoSettingHw == IntPtr.Zero)
                {
                    //日志--税收分类编码设置页面查找失败
                    return;
                }
                var goodNoSettingChildBars = new List<WindowInfo>();
                var flag = TryRetry(str =>
                {
                    goodNoSettingChildBars = WinApi.EnumChildWindowsCallback(goodNoSettingHw);
                    return goodNoSettingChildBars != null && goodNoSettingChildBars.Count >= 11;
                }, goodNoSettingHw, 20, 500);

                if ("1".Equals(detail.TaxPer) && !string.IsNullOrEmpty(detail.TaxPerCon))
                {
                    //把优惠政策标识设置成 “是”
                    var child = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "享受优惠政策");
                    var yhzcBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, child, null, null);
                    WinApi.LeftClick(yhzcBar);
                    WinApi.SendKey(yhzcBar, 38);//上
                    Thread.Sleep(30);
                    WinApi.SendKey(yhzcBar, 13);//enter

                    Thread.Sleep(300);

                    int index = detail.TaxPerCon.Trim() == "免税" ? 1 : 2;
                    //设置优惠政策内容
                    var ssflbmBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "税收分类编码");
                    var yhlxBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, ssflbmBar, null, null);
                    WinApi.LeftClick(yhlxBar);
                    for (var i = 0; i < index; i++)
                    {
                        WinApi.SendKey(yhlxBar, 40);//下
                        Thread.Sleep(300);
                    }

                    WinApi.SendKey(yhlxBar, 13);//enter
                }
                else
                {
                    var suilvBar = goodNoSettingChildBars[goodNoSettingChildBars.Count - 1].hWnd;
                    //AmLogger.Info("WriteGoodsTaxNoAdd", $"suilvBar{suilvBar}");
                    //通过索引设置下拉框选项
                    Thread.Sleep(500);
                    WinApi.SetComboxItemValue(suilvBar, GetByTaxRate(detail.TaxRate));
                }

                var stripHw = goodNoSettingChildBars.Find(b=>b.szWindowName== "toolStrip1").hWnd;
                WinApi.ClickLocation(stripHw, 40, 13); //点击保存--之后会有弹框
                Thread.Sleep(500);
                var bar = WinApi.FindWindow(null, "SysMessageBox");
                if (bar == IntPtr.Zero)
                {
                    return;
                }

                var list = WinApi.EnumChildWindowsCallback((IntPtr)bar);
                if (list == null || list.Count <= 7)
                {
                    return;
                }

                if (list[4].szWindowName != "修改成功！")
                {
                    throw new Exception(list[4].szWindowName);
                }

                WinApi.LeftClick(list[7].hWnd); //点击确定返回
                return;
            }
            catch (Exception e)
            {
                //AmLogger.Error("WriteGoodsTaxNoSetting", e);
            }
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

        /// <summary>
        /// 窗体获取重试大法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="func"></param>
        /// <param name="arg"></param>
        /// <param name="count"></param>
        /// <param name="sleepMilliTimeout"></param>
        /// <returns></returns>
        public static TR TryRetry<T, TR>(Func<T, TR> func, T arg, int count=20, int sleepMilliTimeout=500)
        {
            if (count <= 0)
                return default(TR);
            if (sleepMilliTimeout < 0)
                return default(TR);

            for (int i = 0; i < count; ++i)
            {
                try
                {
                    //Console.WriteLine(i);
                    TR r = func(arg);
                    if (!r.Equals((object)default(TR)))
                        return r;
                    Thread.Sleep(sleepMilliTimeout);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return default(TR);
        }

    }
}
