using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace UiTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string>()
            {
                "开具增值税专用发票",
                "开具增值税普通发票",
                "开具机动车销售统一发票",
                "开具收购发票"
            };
            for (var i = 0; i < list.Count; i++)
            {
                var bar = WinApi.FindWindow(null, list[i]);
                Console.WriteLine($"{list[i]},bar:{bar}");
                if (bar == IntPtr.Zero)
                {
                    Console.WriteLine("bar is zero");
                    return;
                }

                var winBar = AutomationElement.FromHandle((IntPtr)bar);
                Console.WriteLine($"{list[i]} IsOffscreen:{winBar.Current.IsOffscreen}");
                Console.WriteLine($"关闭{list[i]}窗体");
                WinApi.PostMessage(bar, 16, 0, 0);
            }

            Console.ReadKey();
        }


        static void Test()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票");
            var childs = WinApi.EnumChildWindowsCallback(winBar);
            var toolStrip = childs[childs.Count - 1].hWnd;
            ClickBtnByName(toolStrip, "红字");
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
                var winBarUi = AutomationElement.FromHandle(toolBar);

                var element = winBarUi.FindFirst(TreeScope.Children,
                    new PropertyCondition(AutomationElement.NameProperty,name));
                element.TryGetCurrentPattern(InvokePattern.Pattern,out var pt);
                //var pattern = (IUIAutomationLegacyIAccessiblePattern)element?.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                ((InvokePattern) pt).Invoke();
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
    }
}
