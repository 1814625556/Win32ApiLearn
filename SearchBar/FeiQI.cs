using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using User32Test;

namespace SearchBar
{
    class FeiQI
    {
        #region 测试语句
        //WinApi.ClickLocation((IntPtr) 7145062, 10, 37);//可以点中第一行第一块

        //赋值成功
        //cc.TryGetCurrentPattern
        //    (ValuePattern.Pattern, out var patternObject);
        //((ValuePattern)patternObject).SetValue("mingchenghaha~");

        #endregion

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
            HxShengQing.ClickBtnByName(bar, "发票管理");//点击发票管理

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

            zhuanRedBar = HxShengQing.TryRetry<string, IntPtr>(
                str => WinApi.FindWindow(null, str), "红字增值税专用发票信息表信息选择",
                20, 500);
            if (zhuanRedBar == IntPtr.Zero)
            {
                //日志--红字增值税专用发票信息表信息选择查找失败
                return;
            }

            Thread.Sleep(500);

            HxShengQing.TryRetry<IntPtr, bool>(bar =>
            {
                var list = WinApi.EnumChildWindowsCallback(bar);
                var PurchaserApplicationBar = list.Find(b => b.szWindowName == "一、购买方申请");
                var SalesAllicationBar = list.Find(b => b.szWindowName == "二、销售方申请");
                var DeductedBar = list.Find(b => b.szWindowName == "1、已抵扣");
                var UnDeductedBar = list.Find(b => b.szWindowName == "2、未抵扣");
                var FpInputBar = list.Find(b => b.szWindowName == "对应蓝字增值税专用发票信息");
                if (PurchaserApplicationBar.hWnd == IntPtr.Zero || SalesAllicationBar.hWnd == IntPtr.Zero ||
                    DeductedBar.hWnd == IntPtr.Zero || UnDeductedBar.hWnd == IntPtr.Zero || FpInputBar.hWnd == IntPtr.Zero)
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
            }, zhuanRedBar, 20, 500);


        }

        /// <summary>
        /// 对名称和识别号进行赋值
        /// </summary>
        public static void step3()
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

            var hzxxtkInfoList = WinApi.FindChildInfo(hzxxtklist[0].hWnd);
            var hzxxtkInfoChild = WinApi.FindChildInfo(hzxxtkInfoList[2].hWnd);
            var sfxxlist = WinApi.FindChildInfo(hzxxtkInfoChild[1].hWnd);

            //对销方名称进行赋值
            var mclist = WinApi.FindChildBar(sfxxlist[1].hWnd);
            mclist.ForEach(mc => WinApi.SendMessage(mc, 0x0C, IntPtr.Zero, "名称测试"));

            //对纳税人识别号进行赋值
            var bmlist = WinApi.FindChildBar(sfxxlist[0].hWnd);
            bmlist.ForEach(mc => WinApi.SendMessage(mc, 0x0C, IntPtr.Zero, "12345678901234567"));

            //点击价格

            //ClickBtnByName(hzxxtklist[hzxxtklist.Count - 1].hWnd, "价格");//这里貌似不需要点击 默认都是不含税
            HxShengQing.ClickBtnByName(hzxxtklist[hzxxtklist.Count - 1].hWnd, "价格");//这里貌似不需要点击 默认都是不含税
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
            HxShengQing.ClickBtnByName(hzxxtklist[hzxxtklist.Count - 1].hWnd, "差额");

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
            HxShengQing.SystemOpera("取消");
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
            HxShengQing.ClickBtnByName(toolStripBar.hWnd, "上传");
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
                HxShengQing.GetTableFocus();
                Thread.Sleep(100);
                SendKeys.SendWait($"{i}aaaaa");
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                HxShengQing.WriteGoodsTaxNoAdd(IntPtr.Zero, null);
                Thread.Sleep(1000);
                HxShengQing.ClickBtnByName((IntPtr)7472786, "增行");
                Thread.Sleep(1000);
            }
        }
    }
}
