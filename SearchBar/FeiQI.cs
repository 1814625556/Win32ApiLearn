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

        //public static void WriteGoodsTaxNoSetting(IntPtr goodNoSettingHw, RednotificationDetail detail)
        //{
        //    try
        //    {
        //        if (goodNoSettingHw == IntPtr.Zero)
        //        {
        //            //日志--税收分类编码设置页面查找失败
        //            return;
        //        }
        //        var goodNoSettingChildBars = new List<WinApi.WindowInfo>();
        //        var flag = RetryUtil.TryRetry(str =>
        //        {
        //            goodNoSettingChildBars = WinApi.EnumChildWindowsCallback(goodNoSettingHw);
        //            return goodNoSettingChildBars != null && goodNoSettingChildBars.Count >= 11;
        //        }, goodNoSettingHw, 20, 500);

        //        if ("1".Equals(detail.TaxPer) && !string.IsNullOrEmpty(detail.TaxPerCon))
        //        {
        //            //把优惠政策标识设置成 “是”
        //            var child = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "享受优惠政策");
        //            var yhzcBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, child, null, null);
        //            WinApi.leftClick(yhzcBar);
        //            WinApi.SendKey(yhzcBar, 38);//上
        //            Thread.Sleep(30);
        //            WinApi.SendKey(yhzcBar, 13);//enter

        //            Thread.Sleep(300);

        //            int index = detail.TaxPerCon.Trim() == "免税" ? 1 : 2;
        //            //设置优惠政策内容
        //            var ssflbmBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "税收分类编码");
        //            var yhlxBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, ssflbmBar, null, null);
        //            WinApi.leftClick(yhlxBar);
        //            for (var i = 0; i < index; i++)
        //            {
        //                WinApi.SendKey(yhlxBar, 40);//下
        //                Thread.Sleep(300);
        //            }

        //            WinApi.SendKey(yhlxBar, 13);//enter
        //        }
        //        else
        //        {
        //            var suilvBar = goodNoSettingChildBars[goodNoSettingChildBars.Count - 1].hWnd;
        //            //AmLogger.Info("WriteGoodsTaxNoAdd", $"suilvBar{suilvBar}");
        //            //通过索引设置下拉框选项
        //            Thread.Sleep(500);
        //            WinApi.SetComboxItemValue(suilvBar, GetByTaxRate(detail.TaxRate));
        //        }

        //        var stripHw = goodNoSettingChildBars.Find(b => b.szWindowName == "toolStrip1").hWnd;
        //        WinApi.ClickLocation((int)stripHw, 40, 13); //点击保存--之后会有弹框
        //        Thread.Sleep(500);
        //        var bar = WinApi.FindWindow(null, "SysMessageBox");
        //        if (bar == IntPtr.Zero)
        //        {
        //            return;
        //        }

        //        var list = WinApi.EnumChildWindowsCallback((IntPtr)bar);
        //        if (list == null || list.Count <= 7)
        //        {
        //            return;
        //        }

        //        if (list[4].szWindowName != "修改成功！")
        //        {
        //            throw new Exception(list[4].szWindowName);
        //        }

        //        WinApi.leftClick(list[7].hWnd); //点击确定返回
        //        return;
        //    }
        //    catch (Exception e)
        //    {
        //        //AmLogger.Error("WriteGoodsTaxNoSetting", e);
        //    }
        //}

        ///// <summary>
        ///// 商品编码添加
        ///// </summary>
        //public static void WriteGoodsTaxNoAdd(IntPtr goodNoAddHw, RednotificationDetail detail)
        //{
        //    try
        //    {
        //        if (goodNoAddHw == IntPtr.Zero)
        //        {
        //            return;
        //        }

        //        //写入分类编码
        //        var ssflName = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "税收分类名称");
        //        var temp1 = WinApi.FindWindowEx((IntPtr)goodNoAddHw, ssflName, null, null);
        //        var temp2 = WinApi.FindWindowEx(temp1, IntPtr.Zero, null, null);
        //        var ssflBar = WinApi.FindWindowEx(temp1, temp2, null, null); //获取税收分类编码句柄

        //        WinApi.SendMessage(ssflBar, WinApi.BM_TEXT, IntPtr.Zero, TaxSub(detail.GoodsTaxNo)); //对文本框进行赋值
        //        Thread.Sleep(200);
        //        WinApi.SendKey(ssflBar, WinApi.VK_DOWN);
        //        Thread.Sleep(30);
        //        WinApi.SendKey(ssflBar, WinApi.VK_RETURN);
        //        WinApi.ClickLocation((IntPtr)goodNoAddHw, 300, 10);
        //        Thread.Sleep(200);

        //        //优惠政策
        //        if ("1".Equals(detail.TaxPer) && !string.IsNullOrEmpty(detail.TaxPerCon))
        //        {
        //            //AmLogger.Info("WriteGoodsTaxNoAdd", "享受优惠政策");
        //            var spflbmBar = WinApi.FindWindow(null, "商品编码添加");
        //            var guiGebar = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "规格型号");
        //            var guiGeSelectBar = WinApi.FindWindowEx(spflbmBar, guiGebar, null, null);

        //            WinApi.ClickLocation(guiGeSelectBar, 10, 10);
        //            Thread.Sleep(30);
        //            int selected = WinApi.SendMessage(guiGeSelectBar, WinApi.CB_SETCURSEL, (IntPtr)0, "");

        //            Thread.Sleep(1000); //等待优惠政策enable

        //            var jianma = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "简码");
        //            var temp = WinApi.FindWindowEx(spflbmBar, jianma, null, null);
        //            temp = WinApi.FindWindowEx(spflbmBar, temp, null, null);
        //            var yhzclxBar = WinApi.FindWindowEx(spflbmBar, temp, null, null);
        //            int index = detail.TaxPerCon.Trim() == "免税" ? 1 : 2;
        //            WinApi.ClickLocation(yhzclxBar, 10, 10);
        //            for (var i = 0; i < index; i++)
        //            {
        //                WinApi.SendKey(yhzclxBar, WinApi.VK_DOWN);
        //                Thread.Sleep(30);
        //            }

        //            Thread.Sleep(100);
        //            WinApi.SendKey(ssflBar, WinApi.VK_RETURN);
        //            //AmLogger.Info("WriteGoodsTaxNoAdd", $"享受优惠政策:{guiGeSelectBar},yhzclxBar{yhzclxBar}");
        //        }

        //        //正常税率采用平台传过来的税率
        //        else
        //        {
        //            //修改实际传入的税率
        //            var child = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "*税率");
        //            var suilvBar = WinApi.FindWindowEx((IntPtr)goodNoAddHw, child, null, null);
        //            //AmLogger.Info("WriteGoodsTaxNoAdd", $"suilvBar{suilvBar}");
        //            //通过索引设置下拉框选项
        //            Thread.Sleep(500);
        //            //WinApi.SendMessage(suilvBar, WinApi.CB_SETCURSEL, (IntPtr) GetIndexByTaxRate(detail.TaxRate), ""); //调整税率为传入税率
        //            WinApi.SetComboxItemValue(suilvBar, GetByTaxRate(detail.TaxRate));
        //        }

        //        var stripHw = WinApi.FindWindowEx((IntPtr)goodNoAddHw, IntPtr.Zero, null, "toolStrip1");
        //        Thread.Sleep(500);
        //        WinApi.ClickLocation(stripHw, 40, 13); //点击保存
        //        //点击保存的时候可能弹出错误提示
        //        Thread.Sleep(500);
        //        //GetErrorInfo();
        //        //AmLogger.Info("WriteGoodsTaxNoAdd", "商品编码添加填写成功");
        //    }
        //    catch (Exception e)
        //    {
        //        //CaptureScreen.TakeScreenShot("商品编码添加", $"{e.Message}");
        //        //AmLogger.Error("WriteGoodsTaxNoAdd", e);
        //    }
        //}

        /// <summary>
        ///根据税率获取相应索引
        /// </summary>
        /// <param name="taxRate"></param>
        /// <returns></returns>
        //public static string GetByTaxRate(string taxRate = "")
        //{
        //    taxRate = taxRate.Trim();
        //    switch (taxRate)
        //    {
        //        case "0":
        //            return "0%";
        //        case "0.0":
        //            return "0%";
        //        case "0.00":
        //            return "0%";
        //        case "0.03":
        //            return "3%";
        //        case "0.04":
        //            return "4%";
        //        case "0.05":
        //            return "5%";
        //        case "0.06":
        //            return "6%";
        //        case "0.09":
        //            return "9%";
        //        case "0.10":
        //            return "10%";
        //        case "0.11":
        //            return "11%";
        //        case "0.13":
        //            return "13%";
        //        case "0.16":
        //            return "16%";
        //        case "0.17":
        //            return "17%";
        //        case "减按1.5%计算":
        //            return "减按1.5%计算";
        //        case "中外合作油气田":
        //            return "中外合作油气田";
        //        default:
        //            return "";
        //    }
        //}

        //public static string TaxSub(string taxStr)
        //{
        //    string result = "";
        //    for (var i = taxStr.Length - 1; i > 1; i--)
        //    {
        //        if (taxStr[i] == '0')
        //        {
        //            result = taxStr.Substring(0, i + 1);
        //            continue;
        //        }
        //        else
        //        {
        //            result = taxStr.Substring(0, i + 1);
        //        }

        //        break;
        //    }

        //    return result;
        //}






    }
}
