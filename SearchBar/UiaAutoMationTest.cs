using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms.VisualStyles;
using UIAutomationClient;
using User32Test;

namespace SearchBar
{
    public class UiaAutoMationTest
    {
        //获取异常报错
        public static void Method3()
        {
            var winbar = WinApi.FindWindow(null, "CusMessageBox");
            var childinfos = WinApi.EnumChildWindowsCallback(winbar);
            for (var i = 0; i < childinfos.Count; i++)
            {
                if (42730990 == (int)childinfos[i].hWnd)
                {
                    var txtMation = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(childinfos[i].hWnd);

                    var txtPt = (IUIAutomationValuePattern)txtMation.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    Console.WriteLine(txtPt.CurrentValue);
                }

                //Console.WriteLine($"No:{i},hwnd:{(int)childinfos[i].hWnd},szWindowName:{childinfos[i].szWindowName},szTextName{childinfos[i].szTextName}");
            }
        }

        public static void Method2()
        {
            var uiaInstance = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle((IntPtr)18615182);
            var allChilds = uiaInstance.FindAll(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            var pt = uiaInstance.GetCurrentPattern(UIA_ControlTypeIds
                .UIA_TableControlTypeId);


            //var item = (IUIAutomationGridItemPattern)pt.GetItem(3, 4).GetCurrentPattern(UIA_ControlTypeIds.UIA_TabItemControlTypeId);

            


            for (var i = 0; i < allChilds.Length; i++)
            {
                var element = allChilds.GetElement(i);

                //if (element.CurrentName == "单价(不含税) 行 3")
                //{
                //    var pattern = (IUIAutomationValuePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                //    if (pattern == null)
                //    {
                //        break;
                //    }
                //    pattern.SetValue("asf");
                //}

                //Console.WriteLine($"element.CurrentControlType:{element.CurrentControlType}--{UIA_ControlTypeIds.UIA_ButtonControlTypeId}");

                //if (element.CurrentControlType == UIA_ControlTypeIds.UIA_CustomControlTypeId)
                //{
                //    var pattern = (IUIAutomationValuePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                //    if (pattern == null)
                //    {
                //        break;
                //    }
                //    pattern.SetValue("asf");
                //    return;
                //}


                //Console.WriteLine();
            }
        }

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

                if(element.CurrentControlType == UIA_ControlTypeIds.UIA_EditControlTypeId)
                {
                    var pattern = (IUIAutomationValuePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    if (pattern == null)
                    {
                        break;
                    }
                    pattern.SetValue("1234");
                    
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
