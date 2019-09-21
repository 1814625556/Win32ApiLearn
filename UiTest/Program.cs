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
            var bar = Convert.ToInt32(args[0]);
            var menuBar = AutomationElement.FromHandle((IntPtr)bar);
            var menu = menuBar.FindFirst(
                TreeScope.Descendants,
                new PropertyCondition(AutomationElement.NameProperty, "已开发票查询"));
            menu.TryGetCurrentPattern(InvokePattern.Pattern, out var pt);
            ((InvokePattern)pt).Invoke();

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
