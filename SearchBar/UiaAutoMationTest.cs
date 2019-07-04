using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIAutomationClient;
using User32Test;

namespace SearchBar
{
    public class UiaAutoMationTest
    {
        public static void Method1()
        {
            var winBar = WinApi.FindWindow(null, "Form1Text");
            if (winBar == IntPtr.Zero) return;
            if (UiaAutoMationHelper.IsAvailable == false) return;

            var uiaInstance = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);
            var allChilds = uiaInstance.FindAll(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());
            for (var i = 0; i < allChilds.Length; i++)
            {
                var element = allChilds.GetElement(i);

                if(element.CurrentControlType == UIA_ControlTypeIds.UIA_ButtonControlTypeId)
                {
                    var pattern = (IUIAutomationInvokePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                    if (pattern == null)
                    {
                        break;
                    }
                    pattern.Invoke();
                }


                Console.WriteLine();
            }

        }

        /// <summary>
        /// 打印所有属性
        /// </summary>
        /// <param name="element"></param>
        static void ConsoleAllProperty(IUIAutomationElement element)
        {
            Console.WriteLine($"element.CurrentClassName:{element.CurrentClassName}");

            Console.WriteLine($"element.CurrentName:{element.CurrentName}");

            Console.WriteLine($"element.CurrentControlType:{element.CurrentControlType.ToString()}");

            Console.WriteLine($"element.CurrentAutomationId:{element.CurrentAutomationId}");

            Console.WriteLine($"element.CurrentIsEnabled:{element.CurrentIsEnabled}");

        }

        /// <summary>
        /// 编辑控件
        /// </summary>
        /// <param name="element"></param>
        static void EditControl(IUIAutomationElement element)
        {
            if (element?.CurrentControlType != UIA_ControlTypeIds.UIA_EditControlTypeId) return;

            var pattern = (IUIAutomationValuePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
            if (pattern == null)
            {
                return;
            }
            Console.WriteLine(pattern.CurrentValue);

            pattern.SetValue("chenchang");
        }

        /// <summary>
        /// 点击控件
        /// </summary>
        /// <param name="element"></param>
        static void ButtonControl(IUIAutomationElement element)
        {
            if (element?.CurrentControlType != UIA_ControlTypeIds.UIA_ButtonControlTypeId) return;

            var pattern = (IUIAutomationInvokePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
            pattern?.Invoke();
        }

    }
}
