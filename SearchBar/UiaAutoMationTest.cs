using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Newtonsoft.Json;
using UIAutomationClient;
using User32Test;
using TreeScope = UIAutomationClient.TreeScope;

namespace SearchBar
{
    public class UiaAutoMationTest
    {
        //卷票调整
        public static void Method13()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var winBarUia = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);
            var dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation()
                .CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var tableBar = (IntPtr) dataGridUia.CurrentNativeWindowHandle;
            var childs = dataGridUia.FindAll(TreeScope.TreeScope_Children,
                UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            var count = childs.Length;

            for (var i = 0; i < count; i++)
            {
                dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation()
                    .CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

                childs = dataGridUia.FindAll(TreeScope.TreeScope_Children,
                    UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

                var uiarow = dataGridUia.FindFirst(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, $"行 {i}"));

                Console.WriteLine($"uiarow.CurrentName:{uiarow.CurrentName}");

                var uiaxiangmu = uiarow.FindFirst(TreeScope.TreeScope_Children,
                    UiaAutoMationHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, $"项目 行 {i}"));

                Console.WriteLine($"uiaxiangmu.CurrentName:{uiaxiangmu.CurrentName}");

                var uiashuilv = uiarow.FindFirst(TreeScope.TreeScope_Children,
                    UiaAutoMationHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, $"税率 行 {i}"));

                Console.WriteLine($"uiashuilv.CurrentName:{uiashuilv.CurrentName}");
                //var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                //    UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

                //对名称进行赋值
                var pt3 = (IUIAutomationLegacyIAccessiblePattern) uiaxiangmu
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                pt3.DoDefaultAction();
                Thread.Sleep(500);
                var childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, "Hello world");

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
                var pt2 = (IUIAutomationLegacyIAccessiblePattern) uiashuilv
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt2.DoDefaultAction();
                Thread.Sleep(100);
                pt2.DoDefaultAction();
                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                UIHelper.SetCombox(childinfos1.Find(b => b.szClassName.Contains("COMBOBOX")).hWnd, "17%");
                Thread.Sleep(500);

                //for (var j = 2; j < 5; j++)
                //{
                //    var pt1 = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(j)
                //        .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                //    if (j == 2)
                //    {
                //        pt1.DoDefaultAction();
                //        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                //        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "2");
                //        Thread.Sleep(100);
                //    }
                //    else if (j == 3)
                //    {
                //        pt1.DoDefaultAction();
                //        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                //        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "25");
                //        Thread.Sleep(100);
                //    }
                //    else if (j == 4)
                //    {
                //        pt1.DoDefaultAction();
                //        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                //        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "50");
                //        Thread.Sleep(100);
                //    }
                //}
            }
        }

        //根据条件查找控件
        public static void Method12()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var winBarUia = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);
            var dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var winMation = AutomationElement.FromHandle(winBar);
            var dataGridMation = winMation.FindFirst(System.Windows.Automation.TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, "DataGrid1"));
        }

        /// <summary>
        /// 直接操作日历控件--探究中
        /// </summary>
        public static void Method11()
        {
            var winBar = WinApi.FindWindow(null, "红字发票信息表查询条件");

            var childs = WinApi.FindChildInfo(winBar);
            var rizhiBar = childs.Find(b => b.szWindowName == "填开日期");

            var subChilds = WinApi.EnumChildWindowsCallback(rizhiBar.hWnd);


            WinApi.GetWindowRect(new HandleRef(null, subChilds[0].hWnd), out var rect);
            WinApi.ClickLocation(subChilds[0].hWnd, rect.right - rect.left - 10, 11);

            Thread.Sleep(1000);
            //日历控件
            var winBarUia = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);

            var allUiaMations = winBarUia.FindAll(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            IUIAutomationElement calendarUia = null;
            for (var i = 0; i < allUiaMations.Length; i++)
            {
                var mation = allUiaMations.GetElement(i);
                Console.WriteLine($"Num:{i}, name:{mation.CurrentName}");
                if (mation.CurrentName.Contains("日历控件"))
                {
                    calendarUia = mation;
                    break;
                }
            }
            if (calendarUia != null)
            {
                var invokePt =
                    (IUIAutomationLegacyIAccessiblePattern)calendarUia.GetCurrentPattern(UIA_PatternIds
                        .UIA_LegacyIAccessiblePatternId);
                invokePt.SetValue("2017-09-08");
            }
        }


        /// <summary>
        /// 卷票测试--通过--有点小问题需要调整--调整完毕，大于5的话从4开始，其他从3开始
        /// </summary>
        public static void Method10()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var winBarUia = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);
            var dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var tableBar = (IntPtr) dataGridUia.CurrentNativeWindowHandle; 
            var childs = dataGridUia.FindAll(TreeScope.TreeScope_Children, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            var count = childs.Length;

            for (var i = 4; i < count; i++)
            {

                var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                    UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

                //对名称进行赋值
                var pt3 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(1)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                Console.WriteLine($"name :{subChilds.GetElement(1).CurrentName}");
                
                pt3.DoDefaultAction();
                Thread.Sleep(500);
                var childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, "UUUU");

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
                var pt2 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(5)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                Console.WriteLine($"name :{subChilds.GetElement(5).CurrentName}");

                pt2.DoDefaultAction();
                Thread.Sleep(100);
                pt2.DoDefaultAction();
                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                UIHelper.SetCombox(childinfos1.Find(b=>b.szClassName.Contains("COMBOBOX")).hWnd, "17%");
                Thread.Sleep(500);

                for (var j = 2; j < 5; j++)
                {
                    var pt1 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(j)
                        .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                    //数量
                    if (j == 2)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "2");
                        Thread.Sleep(100);
                    }
                    //含税单价
                    else if (j == 3)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "25");
                        Thread.Sleep(100);
                    }
                    //含税金额
                    else if (j == 4)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "50");
                        Thread.Sleep(100);
                    }
                }

            }
        }


        /// <summary>
        /// 日期控件操作---思路--操作日期控件成功
        /// </summary>
        public static void Method9()
        {

            var winBar = WinApi.FindWindow(null, "红字发票信息表查询条件");

            var childs = WinApi.FindChildInfo(winBar);
            var rizhiBar = childs.Find(b => b.szWindowName == "填开日期");

            var subChilds = WinApi.EnumChildWindowsCallback(rizhiBar.hWnd);


            WinApi.GetWindowRect(new HandleRef(null, subChilds[0].hWnd), out var rect);
            WinApi.ClickLocation(subChilds[0].hWnd, rect.right - rect.left - 10, 11);

            Thread.Sleep(1000);

            #region UI操作失败

            //var allMaitons = AutomationElement.FromHandle(winBar)
            //    .FindAll(System.Windows.Automation.TreeScope.Descendants, Condition.TrueCondition);

            //for (var i = 0; i < allMaitons.Count; i++)
            //{
            //    Console.WriteLine($"Name:{allMaitons[i].Current.Name}");
            //}
            //allMaitons[3].TryGetCurrentPattern(InvokePattern.Pattern, out var upPt);
            //for (var i = 0; i < 24; i++)
            //{
            //    ((InvokePattern)upPt).Invoke();
            //    Thread.Sleep(500);
            //}
            //WinApi.SendMessage(dataHwnd, 12, IntPtr.Zero, "2018-01-01");
            #endregion

            #region 采用uia方式--成功
            var winBarUia = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);

            var allUiaMations = winBarUia.FindAll(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            IUIAutomationElement upbtnUia = null;
            for (var i = 0; i < allUiaMations.Length; i++)
            {
                var mation = allUiaMations.GetElement(i);
                Console.WriteLine($"Num:{i}, name:{mation.CurrentName}");
                if (mation.CurrentName.Contains("上一个按钮"))
                {
                    upbtnUia = mation;
                    break;
                }
            }
            if (upbtnUia != null)
            {
                var invokePt =
                    (IUIAutomationLegacyIAccessiblePattern) upbtnUia.GetCurrentPattern(UIA_PatternIds
                        .UIA_LegacyIAccessiblePatternId);
                for (var i = 0; i < 24; i++)
                {
                    invokePt.DoDefaultAction();
                    Thread.Sleep(500);
                }
            }

            Thread.Sleep(1000);


            winBarUia = UiaAutoMationHelper.GetUIAutomation().ElementFromHandle(winBar);

            allUiaMations = winBarUia.FindAll(TreeScope.TreeScope_Descendants, UiaAutoMationHelper.GetUIAutomation().CreateTrueCondition());

            IUIAutomationElement ttbtn = null;
            for (var i = 0; i < allUiaMations.Length; i++)
            {
                var mation = allUiaMations.GetElement(i);
                Console.WriteLine($"Num:{i}, name:{mation.CurrentName}");
                if (mation.CurrentName.Contains("23"))
                {
                    ttbtn = mation;
                    break;
                }
            }

            //选择时间
            if (ttbtn != null)
            {
                var invokePt =
                    (IUIAutomationLegacyIAccessiblePattern)ttbtn.GetCurrentPattern(UIA_PatternIds
                        .UIA_LegacyIAccessiblePatternId);
                invokePt.DoDefaultAction();
            }

            #endregion

        }

        //普票测试通过
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
        /// 成功操作红字申请测试通过
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
