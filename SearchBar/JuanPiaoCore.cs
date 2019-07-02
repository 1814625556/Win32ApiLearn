using SearchBar.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using User32Test;

namespace SearchBar
{
    public class JuanPiaoCore
    {
        static string GetText(IntPtr txtBar)
        {
            StringBuilder sb = new StringBuilder(256);
           
            WinApi.GetWindowTextW(txtBar, sb, sb.Capacity);

            return sb.ToString();
        }

        public static TR TryRetry<T, TR>(Func<T, TR> func, T arg, int count = 20, int sleepMilliTimeout = 500)
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
        public static void JuanPiaoTianKai(Entitys.InvoiceInfo invoiceInfo)
        {
            //有时候会有插件加载
            Thread.Sleep(3000);

            var winJuanBar = IntPtr.Zero;
            var toolBar = IntPtr.Zero;
            var dataBar = IntPtr.Zero;
            var FPtiankai_new = IntPtr.Zero;

            //购方名称
            var gfmcBar = IntPtr.Zero;

            //纳税人识别号
            var nsrsbhBar = IntPtr.Zero;

            //收款员
            var skyBar = IntPtr.Zero;

            //备注
            var remarkBar = IntPtr.Zero;

            var flag = TryRetry(str =>
            {
                winJuanBar = WinApi.FindWindow(null, str);
                if (winJuanBar == IntPtr.Zero)
                {
                    return false;
                }
                var child = WinApi.FindWindowEx(winJuanBar, IntPtr.Zero, null, "FPtiankai_new");
                var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
                toolBar = WinApi.FindWindowEx(child, child1, null, null);

                var parentBar = WinApi.FindWindowEx(child1, IntPtr.Zero, null, null);
                var childs = WinApi.FindChildBar(parentBar);

                skyBar = childs[18];
                remarkBar = childs[19];
                gfmcBar = childs[childs.Count - 1];
                nsrsbhBar = childs[childs.Count - 2];
                dataBar = childs[childs.Count - 5];

                if (toolBar == IntPtr.Zero || skyBar == IntPtr.Zero || remarkBar == IntPtr.Zero ||
                    gfmcBar == IntPtr.Zero || nsrsbhBar == IntPtr.Zero || dataBar == IntPtr.Zero)
                {
                    return false;
                }
                return true;
            }, "开具增值税普通发票(卷票)", 20, 500);

            if (flag == false)
            {
                Console.Write("句柄查找错误");
                return;
            }

            //窗体前置
            SetForHead(winJuanBar, false);
            Thread.Sleep(100);

            HxShengQing.ClickBtnByName(toolBar, "减行");
            
            //填写备注
            if (!string.IsNullOrEmpty(invoiceInfo?.Head.Remark))
            {
                WinApi.SendMessage(remarkBar, WinApi.BM_TEXT, IntPtr.Zero, invoiceInfo?.Head.Remark);
            }

            //收款人
            if (!string.IsNullOrEmpty(invoiceInfo?.Head.CashierName))
            {
                var nsMation = AutomationElement.FromHandle(skyBar);
                var temp = nsMation.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                if (temp != null)
                {
                    temp.TryGetCurrentPattern(ValuePattern.Pattern, out var objPattern);
                    ((ValuePattern)objPattern)?.SetValue(invoiceInfo?.Head.CashierName);
                }
                else
                {
                    var list = WinApi.FindChildBar(skyBar);
                    foreach (var b in list)
                    {
                        WinApi.SendMessage(b, WinApi.BM_TEXT, IntPtr.Zero, invoiceInfo?.Head.CashierName);
                    }
                }
            }

            //填写购买方名称
            if (!string.IsNullOrEmpty(invoiceInfo?.Head.PurchaserName))
            {
                var nsMation = AutomationElement.FromHandle(gfmcBar);
                var temp = nsMation.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                if (temp != null)
                {
                    temp.TryGetCurrentPattern(ValuePattern.Pattern, out var objPattern);
                    ((ValuePattern)objPattern)?.SetValue(invoiceInfo?.Head.PurchaserName);
                }
                else
                {
                    var list = WinApi.FindChildBar(gfmcBar);
                    foreach (var b in list)
                    {
                        WinApi.SendMessage(b, WinApi.BM_TEXT, IntPtr.Zero, invoiceInfo?.Head.PurchaserName);
                    }
                }
                Thread.Sleep(500);
                WinApi.keybd_event(Keys.Down, 0, 0, 0);
                Thread.Sleep(500);
                WinApi.keybd_event(Keys.Enter, 0, 0, 0);
            }

            //填写纳税人识别号
            if (!string.IsNullOrEmpty(invoiceInfo?.Head.PurchaserTaxNo))
            {
                //var txtbar = WinApi.FindChildInfo(nsrsbhBar);

                var nsMation = AutomationElement.FromHandle(nsrsbhBar);
                var temp = nsMation.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
                if (temp != null)
                {
                    temp.TryGetCurrentPattern(ValuePattern.Pattern, out var objPattern);
                    if (((ValuePattern)objPattern)?.Current.Value != invoiceInfo?.Head.PurchaserTaxNo)
                        ((ValuePattern)objPattern)?.SetValue(invoiceInfo?.Head.PurchaserTaxNo);
                }
                else
                {
                    var list = WinApi.FindChildBar(nsrsbhBar);
                    foreach (var b in list)
                    {
                        WinApi.SendMessage(b, WinApi.BM_TEXT, IntPtr.Zero, invoiceInfo?.Head.PurchaserTaxNo);
                    }
                }
                WinApi.ClickLocation(FPtiankai_new, 100, 100);
            }

            //填写明细
            for (var i = 0; i < invoiceInfo.InvoiceDetails.Count; i++)
            {
                if (invoiceInfo.InvoiceDetails[i].DetailKind == "1")
                {
                    continue;
                }

                HxShengQing.ClickBtnByName(toolBar, "增行");
                
                Thread.Sleep(500);
                //WinApi.ClickLocation(dataBar, 40, 24 + 30 * i + 15); //可以点中第一行第一块
                //Thread.Sleep(200);
                if (invoiceInfo.InvoiceDetails[i].DetailKind == "2")
                {
                    var next = invoiceInfo.InvoiceDetails.Find(detail => detail.DetailKind == "1" &&
                                                                         detail.ItemName ==
                                                                         invoiceInfo.InvoiceDetails[i].ItemName);
                    JuanPiaoMingXi(toolBar, invoiceInfo.InvoiceDetails[i], next); //填写卷票明细
                }
                else
                {
                    JuanPiaoMingXi(toolBar, invoiceInfo.InvoiceDetails[i]); //填写卷票明细    
                }
            }

            Thread.Sleep(500);
            

        }

        /// <summary>
        /// 卷票明细
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="next"></param>
        /// <param name="toolBar"></param>
        public static void JuanPiaoMingXi(IntPtr toolBar, Entitys.InvoiceDetail detail, Entitys.InvoiceDetail next = null)
        {
            try
            {
                
                GetTableFocus();
                //填写货物名称
                SendKeys.SendWait(detail.ItemName);
                Thread.Sleep(500);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);

                var ssbmszBar = IntPtr.Zero;
                var ssbmtjBar = IntPtr.Zero;
                TryRetry(str =>
                {
                    ssbmszBar = WinApi.FindWindow(null, "税收分类编码设置");
                    ssbmtjBar = WinApi.FindWindow(null, "商品编码添加");
                    return ssbmszBar != IntPtr.Zero || ssbmtjBar != IntPtr.Zero;
                }, "", 4, 500);

                if (ssbmszBar != IntPtr.Zero)
                {
                   WriteGoodsSetting(ssbmszBar,
                        detail.TaxPer, detail.TaxperCon, detail.TaxRate);
                }
                if (ssbmtjBar != IntPtr.Zero)
                {
                    WriteGoodsTaxNoAdd(ssbmtjBar, detail.GoodsTaxNo,
                        detail.TaxPer, detail.TaxperCon, detail.TaxRate);
                }

                if (!string.IsNullOrWhiteSpace(detail.Quantity))
                {
                    GetTableFocus();
                    //填写数量
                    SendKeys.SendWait(detail.Quantity);
                }

                Thread.Sleep(500);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                if (!string.IsNullOrWhiteSpace(detail.UnitPrice))
                {
                    GetTableFocus();
                    //填写单价
                    if (detail.PriceMethod != "1")
                        detail.UnitPrice = ((1 + Convert.ToDouble(detail.TaxRate))
                                            * Convert.ToDouble(detail.UnitPrice)).ToString();
                    SendKeys.SendWait(detail.UnitPrice);
                }

                Thread.Sleep(500);
                WinApi.keybd_event(Keys.Tab, 0, 0, 0);
                Thread.Sleep(500);
                //AmLogger.Info("JuanPiaoMingXi", "一行明细填写成功");

                //添加折扣逻辑
                if (detail.DetailKind != "2" || next == null)
                {
                    return;
                }

                //点击折扣按钮
                HxShengQing.ClickBtnByName(toolBar, "折扣");
                Thread.Sleep(500);

                var jbar = IntPtr.Zero;
                for (var i = 0; i < 10; i++)
                {
                    jbar = WinApi.FindWindow(null, "添加折扣行");
                    if (jbar != IntPtr.Zero)
                    {
                        break;
                    }

                    Thread.Sleep(500);
                }

                var list = WinApi.EnumChilWindowsIntptr(jbar);
                if (list == null || list.Count < 8)
                {
                    return;
                }

                //发送折扣金额
                WinApi.SendMessage((IntPtr)list[6], WinApi.BM_TEXT, IntPtr.Zero, next.AmountWithTax.Substring(1, next.AmountWithTax.Length - 1));

                //点击确定按钮
                Thread.Sleep(500);
                WinApi.LeftClickMsg(list[4]);

            }
            catch (Exception e)
            {
                //CaptureScreen.TakeScreenShot("添加明细", $"{e.Message}");
                //AmLogger.Error("JuanPiaoMingXi", e);
            }
        }

        /// <summary>
        /// 获取焦点
        /// </summary>
        public static void GetTableFocus()
        {
            WinApi.keybd_event(Keys.Space, 0, 0, 0);
            Thread.Sleep(100);
            WinApi.keybd_event(Keys.Back, 0, 0, 0);
            Thread.Sleep(100);
        }

        public static void SetForHead(IntPtr bar, bool isMax = true)
        {
            var mation = AutomationElement.FromHandle(bar);
            mation.TryGetCurrentPattern(WindowPattern.Pattern, out var winobj);
            if (winobj!=null && isMax)
            {
                ((WindowPattern)winobj)?.SetWindowVisualState(WindowVisualState.Maximized);
            }
            else
            {
                ((WindowPattern)winobj)?.SetWindowVisualState(WindowVisualState.Normal);
            }
        }

        public static void WriteGoodsSetting(IntPtr goodNoSettingHw, string taxPer = "", string taxPerCon = "", string taxRate = "")
        {
            if (goodNoSettingHw == IntPtr.Zero)
            {
                return;
            }

            var suilvBar = IntPtr.Zero;
            var yhBar = IntPtr.Zero;
            var yhlBar = IntPtr.Zero;
            var toolStrip = IntPtr.Zero;

            TryRetry(str =>
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

            if (toolStrip == IntPtr.Zero || suilvBar == IntPtr.Zero || yhBar == IntPtr.Zero || yhlBar == IntPtr.Zero)
            {
                //AmLogger.Error("WriteGoodsSetting", $"税收分类编码设置窗体，控件句柄获取失败");
                //throw new AmExceptionCode("税收分类编码设置窗体，控件句柄获取失败");
            }

            if ("1".Equals(taxPer) && !string.IsNullOrEmpty(taxPerCon))
            {
                UIHelper.SetCombox(yhBar, "是");

                //等待优惠政策enable
                Thread.Sleep(1000);
                UIHelper.SetCombox(yhlBar, taxPerCon.Trim());
            }
            else
            {
                UIHelper.SetCombox(suilvBar, HxShengQing.GetByTaxRate(taxRate));
            }

            HxShengQing.ClickBtnByName(toolStrip, "保存");
            Thread.Sleep(1000);

            HxShengQing.SystemOpera("确认", out var message);
            if (message != "修改成功！")
            {
                throw new Exception(message);
            }
            //AmLogger.Info("WriteGoodsSetting", $"税收分类编码设置窗体，设置成功");
        }

        public static void WriteGoodsTaxNoAdd(IntPtr goodNoAddHw, string goodsTaxNo = "",
            string taxPer = "", string taxPerCon = "", string taxRate = "")
        {
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

            TryRetry(str =>
            {

                var childInfos = WinApi.FindChildInfo(goodNoAddHw);
                if (childInfos == null || childInfos.Count < 30)
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
            }, "", 20, 500);

            if (ssflbmBar == IntPtr.Zero || toolStrip == IntPtr.Zero ||
                suilvBar == IntPtr.Zero || yhBar == IntPtr.Zero || yhlBar == IntPtr.Zero)
            {
                return;
            }

            WinApi.SendMessage(ssflbmBar, WinApi.BM_TEXT, IntPtr.Zero,HxShengQing.TaxSub(goodsTaxNo));
            Thread.Sleep(100);
            WinApi.SendKey(ssflbmBar, WinApi.VK_DOWN);
            Thread.Sleep(100);
            WinApi.SendKey(ssflbmBar, WinApi.VK_RETURN);
            Thread.Sleep(100);
            WinApi.ClickLocation(goodNoAddHw, 300, 10);
            Thread.Sleep(500);

            if ("1".Equals(taxPer) && !string.IsNullOrEmpty(taxPerCon))
            {
                UIHelper.SetCombox(yhBar, "是");

                //等待优惠政策enable
                Thread.Sleep(1000);
                UIHelper.SetCombox(yhlBar, taxPerCon.Trim());
            }
            else
            {
                UIHelper.SetCombox(suilvBar, HxShengQing.GetByTaxRate(taxRate));
            }

            HxShengQing.ClickBtnByName(toolStrip, "保存");
            Thread.Sleep(500);
            
        }

       

    }
}
