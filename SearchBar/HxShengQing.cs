using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using User32Test;
using System.Threading;

namespace SearchBar
{
    /// <summary>
    /// 航信红字信息表申请
    /// </summary>
    public static class HxShengQing
    {
        #region 测试语句
        //WinApi.ClickLocation((IntPtr) 7145062, 10, 37);//可以点中第一行第一块
        #endregion
        /// <summary>
        /// 点击toolBar句柄下某个按钮
        /// </summary>
        /// <param name="toolBar"></param>
        /// <param name="name"></param>
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
        /// 商品编码页面模拟
        /// </summary>
        public static void WriteGoodsTaxNoAdd()
        {
            Thread.Sleep(1000);
            int goodNoAddHw = (int)WinApi.FindWindow(null, "商品编码添加");
            if (goodNoAddHw == 0)
            {
                return;
            }

            //写入分类编码
            var ssflName = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "税收分类名称");
            var temp1 = WinApi.FindWindowEx((IntPtr)goodNoAddHw, ssflName, null, null);
            var temp2 = WinApi.FindWindowEx(temp1, IntPtr.Zero, null, null);
            var ssflBar = WinApi.FindWindowEx(temp1, temp2, null, null); //获取税收分类编码句柄
            
            var stripHw = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "toolStrip1");
            Thread.Sleep(500);
            WinApi.ClickLocation(stripHw, 40, 13); //点击保存
            WinApi.SendMessage(ssflBar, 0X0C, IntPtr.Zero, "101010101"); //对文本框进行赋值
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
            var zhuanRedBar = WinApi.FindWindow(null, "红字增值税专用发票信息表信息选择");
            var allBars = WinApi.EnumChildWindowsCallback(zhuanRedBar);
            if (allBars == null)
            {
                return;
            }

            var checkBar = IntPtr.Zero;
            allBars.ForEach(bar =>
            {
                if (bar.szWindowName.Contains("开具红字增值税专用发票信息表信息选择"))
                {
                    checkBar = bar.hWnd;
                }
            });


            checkBar = WinApi.FindWindowEx(checkBar, IntPtr.Zero, null, null);
            var radioList = WinApi.FindChildBar(checkBar);
            WinApi.LeftClick(radioList[0]);//可以点中 rediobutton
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
        /// 获取信息表流水账号
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
                HxShengQing.WriteGoodsTaxNoAdd();
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
        /// 提示框测试
        /// </summary>
        /// <param name="btnName"></param>
        public static void SystemOpera(string btnName)
        {
            var sysBar = WinApi.FindWindow(null, "SysMessageBox");
            var sysListBars = WinApi.EnumChildWindowsCallback(sysBar);
            var cancel = sysListBars.Find(bar => bar.szWindowName == btnName);
            WinApi.LeftClickMsg(cancel.hWnd);
        }


        //===============================================================================================
        /// <summary>
        /// 红字信息表专票查询导出--第二个接口了
        /// </summary>
        public static void ChaXunIsLoadingSuccess()
        {
            //获取所有桌面窗体句柄
            var alldeskBar = WinApi.FindChildInfo(IntPtr.Zero);

            //获取开票软件句柄
            var kprjBar = alldeskBar.Find(bar => bar.szWindowName != null && bar.szWindowName.Contains("开票软件"));

            ShowForm(kprjBar.hWnd);

            ClickBtnByName(kprjBar.hWnd, "发票管理");//点击发票管理

            //找信息表管理父句柄
            var barInfoList = WinApi.EnumChildWindowsCallback(kprjBar.hWnd);
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
            WinApi.keybd_event(Keys.Down, 0, 0, 0);
            //SendKeys.SendWait("{down}");
            Thread.Sleep(100);
            WinApi.keybd_event(Keys.Down, 0, 0, 0);
            //SendKeys.SendWait("{down}");
            Thread.Sleep(100);
            WinApi.keybd_event(Keys.Enter, 0, 0, 0);
            //SendKeys.SendWait("{enter}");

            Thread.Sleep(2000);
            //获取开票软件下面的所有子句柄
            var kprjList = WinApi.EnumChildWindowsCallback(kprjBar.hWnd);

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
            //WinApi.SendMessage(riqiList[1].hWnd, 0X0C, IntPtr.Zero, "2018-05-01");
            //WinApi.SendMessage(riqiList[1].hWnd, 0X0C, IntPtr.Zero, "2019-05-31");
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
        }

        static void ShowForm(IntPtr bar)
        {
            WinApi.ShowWindow(bar, 2);
            Thread.Sleep(100);
            WinApi.ShowWindow(bar, 3);
            Thread.Sleep(100);
        }
    }
}
