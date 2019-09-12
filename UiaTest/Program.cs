using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIAutomationClient;
using static UIAutomationClient.UIA_PatternIds;

namespace UiaTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //WinApi.SetComboxItemValue((IntPtr)721740, "是");
            //SetEditValue((IntPtr)394026,"222");
            //SetComboxCommon((IntPtr) 918118, "7");

            var sb = new StringBuilder(255);
           
            Console.ReadKey();
        }


        /// <summary>
        /// 获取edit控件的值
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns></returns>
        public static string GetEditValue(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero) return "";
            try
            {
                var editControl = UiaHelper.GetUIAutomation().ElementFromHandle(hwnd);
                var editPt = (IUIAutomationValuePattern)editControl.GetCurrentPattern(UIA_ValuePatternId);
                return editPt.CurrentValue;
            }
            catch (Exception e)
            {
                
            }
            return "";
        }
        /// <summary>
        /// 判断是否是指定的UI类型
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="controlType"></param>
        /// <returns></returns>
        public static bool IsUiType(IntPtr hwnd, int controlType)
        {
            try
            {
                var editControl = UiaHelper.GetUIAutomation().ElementFromHandle(hwnd);
                var pattern = editControl.GetCurrentPattern(controlType);
                return pattern != null;
            }
            catch (Exception e)
            {
                
            }
            return false;
        }

        /// <summary>
        /// UI自动化设置edit值
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="setValue"></param>
        /// <returns></returns>
        public static bool SetEditValue(IntPtr hwnd, string setValue)
        {
            if (hwnd == IntPtr.Zero) return false;
            try
            {
                var editControl = UiaHelper.GetUIAutomation().ElementFromHandle(hwnd);
                var editPt = (IUIAutomationValuePattern)editControl.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                editPt?.SetValue(setValue);
                return true;
            }
            catch (Exception e)
            {
                
            }
            return false;
        }

        public static void SetComboxCommon(IntPtr comboxBar, string item)
        {
            try
            {
                if (comboxBar == IntPtr.Zero) return;


                WinApi.ClickLocation(comboxBar, 10, 10);
                Thread.Sleep(1000);

                var comBoxMation = UiaHelper.GetUIAutomation().ElementFromHandle(comboxBar);
                //var childs = comBoxMation.FindAll(TreeScope.TreeScope_Descendants,
                //    UiaHelper.GetUIAutomation().CreateTrueCondition());
                //for (var i = 0; i < childs.Length; i++)
                //{
                //    var cmpt = (IUIAutomationExpandCollapsePattern)childs.GetElement(i).GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                //    if (cmpt == null) continue;
                //    Thread.Sleep(500);
                //    cmpt.Expand();
                //}
                //var pattern = (IUIAutomationExpandCollapsePattern)comBoxMation.GetCurrentPattern(UIA_PatternIds.UIA_ExpandCollapsePatternId);
                //if (pattern == null) return;
                //Thread.Sleep(500);
                //pattern.Expand();
                var selectItem = comBoxMation?.FindFirst(TreeScope.TreeScope_Descendants,
                    UiaHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, item));
                var pt = (IUIAutomationLegacyIAccessiblePattern)selectItem?.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                if (pt == null) return;
                pt.DoDefaultAction();
                //pattern.Collapse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void openinfo()
        {
            var element= UiaHelper.GetUIAutomation().ElementFromHandle((IntPtr) 722784).
                FindFirst(TreeScope.TreeScope_Descendants,UiaHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, "红字增值税专用发票信息表查询导出"));
            var pt = (IUIAutomationInvokePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
            Task.Factory.StartNew(()=>pt?.Invoke());
        }


        public static void SetCombox(IntPtr comboxBar, string item)
        {
            try
            {
                if (comboxBar == IntPtr.Zero) return;
                WinApi.ClickLocation(comboxBar, 10, 10);
                Thread.Sleep(1500);
                for (var i = 0; i < 15; i++)
                {
                    WinApi.SendKey(comboxBar,WinApi.VK_UP);
                    Thread.Sleep(10);
                }
                var comBoxMation = UiaHelper.GetUIAutomation().ElementFromHandle(comboxBar);
                var selectItem = comBoxMation?.FindFirst(TreeScope.TreeScope_Descendants,
                    UiaHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, item));
                var comboxRect = comBoxMation.CurrentBoundingRectangle;
                var selectRect = selectItem.CurrentBoundingRectangle;
                var index = (selectRect.top - comboxRect.top) / (selectRect.bottom - selectRect.top);

                for (var i = 0; i < index - 1; i++)
                {
                    WinApi.SendKey(comboxBar,WinApi.VK_DOWN);
                    Thread.Sleep(10);
                }
                WinApi.SendKey(comboxBar,WinApi.VK_RETURN);
                Thread.Sleep(500);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void Test()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票");
            var childs = WinApi.EnumChildWindowsCallback(winBar);
            var toolStrip = childs[childs.Count - 1].hWnd;
            for (var i = 0; i < 1000; i++)
            {
                ClickBtnByName(toolStrip, "红字");
                for (var j = 0; j < 100; j++)
                {
                    Thread.Sleep(100);
                    var cwinBar = WinApi.FindWindow(null, "销项正数发票代码号码填写、确认");
                    if (cwinBar != IntPtr.Zero)
                    {
                        WinApi.CloseWinForm(cwinBar);
                        break;
                    }
                }
                
                File.AppendAllText("cc.txt",$"{i}\r\n");
            }
        }

        /// <summary>
        /// 点击toolBar句柄下某个按钮--这个有可能点不中--需要优化
        /// </summary>
        /// <param name="toolBar"></param>
        /// <param name="name"></param>
        public static bool ClickBtnByName(IntPtr toolBar, string name)
        {
            if (toolBar == IntPtr.Zero)
                return false;
            //AmLogger.Info("ClickBtnByName", $"toolBar:{toolBar},name:{name}");
            try
            {
                var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(toolBar);
                var element = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().
                    CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, name));
                //var x = element.CurrentBoundingRectangle.left - winBarUia.CurrentBoundingRectangle.left + 30;
                //WinApi.ClickLocation(toolBar, x, 25);
                //Console.WriteLine($"{element.CurrentNativeWindowHandle}");
                var pattern = (IUIAutomationInvokePattern)element?.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                //var pattern = (IUIAutomationLegacyIAccessiblePattern)element?.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        pattern.Invoke();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                });
                //for (var j = 0; j < 100; j++)
                //{
                //    return WinApi.FindWindow(null, "销项正数发票代码号码填写、确认")!=IntPtr.Zero;
                //}
                //pattern?.DoDefaultAction();
                Console.WriteLine("success....");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"error {e.Message}");
                //AmLogger.Error("ClickBtnUiaByName", $"{name},message:{e.Message},stacktrace:{e.StackTrace}");
            }
            return false;

        }

        public static bool OpenBarMenu()
        {
            try
            {
                //197170
                var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle((IntPtr)197170);
                var elements = winBarUia.FindAll(UIAutomationClient.TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().
                    CreateTrueCondition());
                IUIAutomationElement mation = null;
                for (var i = 0; i < elements.Length; i++)
                {
                    if (elements.GetElement(i).CurrentName == "增值税普通发票填开")
                        mation = elements.GetElement(i);
                    Console.WriteLine($"name:{elements.GetElement(i).CurrentName},handler:{elements.GetElement(i).CurrentNativeWindowHandle}");
                }
                var pt = (IUIAutomationInvokePattern)mation.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                
                Task.Factory.StartNew(() => { pt.Invoke();});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return false;
        }
    }
}
