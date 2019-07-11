using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using UIAutomationClient;
using User32Test;

namespace SearchBar
{
    public class UiaAutoMationTest
    {

        //测试普票填开
        public static void Method8()
        {
            var tableBar = (IntPtr)21169748;

            var childs = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(tableBar)
                .FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());
            for (var i = 2; i < childs.Length; i++)
            {

                var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                    UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

                //对名称进行赋值
                var pt3 = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(1)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                pt3.DoDefaultAction();
                Thread.Sleep(500);
                var childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, $"AAA");

                try
                {
                    pt3.DoDefaultAction();
                }
                catch (Exception e)
                {
                    var noaddBar = WinApi.FindWindow(null, "商品编码添加");
                    Bug.WriteGoodsTaxNoAdd(noaddBar, "101010104");
                    Console.WriteLine(e);
                }

                //改变税率
                var pt2 = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(7)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt2.DoDefaultAction();

                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                UIHelper.SetCombox(childinfos1[1].hWnd,"17%");
                //WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, "17%");

                Thread.Sleep(500);

                for (var j = 2; j < 6; j++)
                {
                    var pt1 = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(j)
                        .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                   if (j == 2)
                    {
                        pt1.DoDefaultAction();
                        //pt1.SetValue("小");
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "小");
                        Thread.Sleep(100);
                    }
                    else if (j == 3)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "kg");
                        Thread.Sleep(100);
                    }
                    else if (j == 4)
                    {
                        pt1.DoDefaultAction();
                        //pt1.SetValue("小");
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "3");
                        Thread.Sleep(100);
                    }
                    else if (j == 5)
                    {
                        pt1.DoDefaultAction();
                        //pt1.SetValue("小");
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "100");
                        Thread.Sleep(100);
                        pt1.DoDefaultAction();
                        Thread.Sleep(1000);
                    }
                }
            }
        }


        /// <summary>
        /// 成功操作红字申请--税率问题解决了
        /// </summary>
        public static void Method7()
        {
            var tableBar = (IntPtr)6227812;

            var childs = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());
            for (var i = 1; i < childs.Length; i++)
            {

                var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                    UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

                //对名称进行赋值
                var pt3 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(0)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                pt3.DoDefaultAction();
                Thread.Sleep(500);
                var childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, $"AAA");

                try
                {
                    pt3.DoDefaultAction();
                }
                catch (Exception e)
                {
                    var noaddBar = WinApi.FindWindow(null, "商品编码添加");
                    Bug.WriteGoodsTaxNoAdd(noaddBar, "101010104");
                    Console.WriteLine(e);
                }

                //改变税率
                var pt2 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(6)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt2.DoDefaultAction();

                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, "17%");

                Thread.Sleep(500);

                for (var j = 1; j < 8; j++)
                {
                    var pt1 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(j)
                        .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                    pt1.Select(j);

                    if (j == 0)
                    {
                        pt1.DoDefaultAction();
                        Thread.Sleep(500);
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, $"{i}-{j}AAA");

                        try
                        {
                            pt1.DoDefaultAction();
                        }
                        catch (Exception e)
                        {
                            var noaddBar = WinApi.FindWindow(null, "商品编码添加");
                            Bug.WriteGoodsTaxNoAdd(noaddBar, "101010104");
                            Console.WriteLine(e);
                        }
                    }
                  
                    else if (j == 1)
                    {
                        pt1.DoDefaultAction();
                        //pt1.SetValue("小");
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "小");
                        Thread.Sleep(100);
                    }
                    else if (j == 2)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "kg");
                        Thread.Sleep(100);
                    }
                    else if (j == 3)
                    {
                        pt1.DoDefaultAction();
                        //pt1.SetValue("小");
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "-1");
                        Thread.Sleep(100);
                    }
                    else if (j == 4)
                    {
                        pt1.DoDefaultAction();
                        //pt1.SetValue("小");
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "100");
                        Thread.Sleep(100);
                        pt1.DoDefaultAction();
                        Thread.Sleep(1000);
                    }
                    //else if (j == 5)
                    //{
                    //    pt1.SetValue("-100.00");
                    //    Thread.Sleep(100);
                    //    //pt1.DoDefaultAction();
                    //}
                    //else if (j == 7)
                    //{
                    //    pt1.SetValue("-9");
                    //    Thread.Sleep(100);
                    //    //pt1.DoDefaultAction();
                    }

                }
            }

            

        

        /// <summary>
        /// 获取输入焦点
        /// </summary>
        public static void Method6()
        {
            var tableBar = (IntPtr)4852320;

            var bar = (IntPtr)4852320;
            var childinfos = WinApi.EnumChildWindowsCallback(bar);
            WinApi.SendMessage(childinfos[childinfos.Count-1].hWnd, 12, IntPtr.Zero, "wwwgoogle");

            var childs = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            var subChilds = childs.GetElement(4).FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            //弹出 税收分类编码添加
            try
            {
                for (var i = 2; i < 7; i++)
                {
                    var ptd = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(i)
                        .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                    ptd.SetValue("77");
                }
                var pt = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(1)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt.DoDefaultAction();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //处理税收分类编码添加
            }

            //出发税率事件
            var pt1 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(7)
                .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
            pt1.DoDefaultAction();

            //设置税率
            childinfos = WinApi.FindChildInfo(bar);
            UIHelper.SetCombox(childinfos[1].hWnd,"17%");
        }

        //dataGriddeep
        public static void Method5()
        {

            //可以采用winapi方式
            //var bar = (IntPtr)4852320;
            //var childs = WinApi.EnumChildWindowsCallback(bar);
            //WinApi.SendMessage(childs[4].hWnd, 12, IntPtr.Zero, "cc");


            var tableBar = (IntPtr)4852320;

            var childs = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            var subChilds = childs.GetElement(3).FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

           
            
            var pt = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(6)
                .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
            pt.SetValue("100.99");
            

            

            //WinApi.SendMessage((IntPtr)subChilds.GetElement(3).CurrentNativeWindowHandle, 12, IntPtr.Zero, "88");
        }
        //操作表格控件
        public static void Method4()
        {
            var tableBar = (IntPtr)4852320;

            var childs = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            var subChilds = childs.GetElement(3).FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());


            var pt = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(1)
                .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
            pt.SetValue("100.99");

            //WinApi.SendMessage((IntPtr)subChilds.GetElement(3).CurrentNativeWindowHandle, 12, IntPtr.Zero, "88");
        }

        //获取异常报错--成功
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
