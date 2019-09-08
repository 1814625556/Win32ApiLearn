using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UIAutomationClient;

namespace UiaTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.ReadKey();
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
