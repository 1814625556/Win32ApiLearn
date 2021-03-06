﻿using System;
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
        /// <summary>
        /// Uia点击事件
        /// </summary>
        /// <param name="toolBar"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ClickBtnUiaByName(IntPtr toolBar, string name)
        {
            try
            {
                var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(toolBar);
                var element = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, name));
                var pattern = (IUIAutomationInvokePattern)element?.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                pattern?.Invoke();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 红字申请测试
        /// </summary>
        public static void Method14()
        {
            Thread.Sleep(10000);
            var winBar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190628 - [红字发票信息表填开]");
            var tkUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar).FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "SqdTianKai"));
            var toolBarUia = tkUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation()
                .CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "toolStrip2"));
            for (var i = 0; i < 7; i++)
            {
                HxShengQing.ClickBtnByName(toolBarUia.CurrentNativeWindowHandle, "增行");
                Thread.Sleep(100);
            }

            var tableUia = tkUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_NamePropertyId, "DataGridView"));
            var tableBar = tableUia.CurrentNativeWindowHandle;
            var shUia = tableUia.FindFirst(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation()
                .CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, "首行"));
            var shRect = shUia.CurrentBoundingRectangle;

            for (var i = 0; i < 8; i++)
            {
                var row = tableUia.FindFirst(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation()
                    .CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, $"行 {i}"));
                var cols = row.FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());

                Console.WriteLine($"{i}:col0:{cols.GetElement(0).CurrentName},col1:{cols.GetElement(1).CurrentName}");
                var nameRect = cols.GetElement(0).CurrentBoundingRectangle;
                WinApi.ClickLocation(tableBar, nameRect.left - shRect.left + 50, nameRect.top - shRect.top + 10);
                Thread.Sleep(100);
                WinApi.ClickLocation(tableBar, nameRect.left - shRect.left + 50, nameRect.top - shRect.top + 10);
                Thread.Sleep(500);
                var childs = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childs[childs.Count - 1].hWnd, 12, IntPtr.Zero, $"BBB{i}");

                WinApi.SendKey(tableBar, 9);//发送tab按键

                //var guigeRect = cols.GetElement(1).CurrentBoundingRectangle;
                //WinApi.ClickLocation(tableBar, guigeRect.left - shRect.left + 10, guigeRect.top - shRect.top + 10);
                Thread.Sleep(2000);
                var noaddBar = WinApi.FindWindow(null, "商品编码添加");
                if (noaddBar != IntPtr.Zero)
                {
                    Bug.WriteGoodsTaxNoAdd(noaddBar, "101010104");
                    Thread.Sleep(500);
                }

                var pt1 = (IUIAutomationLegacyIAccessiblePattern)cols.GetElement(1)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt1.DoDefaultAction();
                Thread.Sleep(500);
                childs = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childs[childs.Count - 1].hWnd, 12, IntPtr.Zero, "大");
                pt1.DoDefaultAction();
                Thread.Sleep(200);

                var pt6 = (IUIAutomationLegacyIAccessiblePattern)cols.GetElement(6)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt6.DoDefaultAction();
                Thread.Sleep(500);
                childs = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childs[childs.Count - 1].hWnd, 12, IntPtr.Zero, "10%");
                Thread.Sleep(500);
                pt6.DoDefaultAction();
                Thread.Sleep(500);

                //var danweiRect = cols.GetElement(2).CurrentBoundingRectangle;
                //WinApi.ClickLocation(tableBar, danweiRect.left - shRect.left + 10, danweiRect.top - shRect.top + 10);
                var pt2 = (IUIAutomationLegacyIAccessiblePattern)cols.GetElement(2)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt2.DoDefaultAction();
                Thread.Sleep(500);
                childs = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childs[childs.Count - 1].hWnd, 12, IntPtr.Zero, "kg");

                //var numRect = cols.GetElement(3).CurrentBoundingRectangle;
                //WinApi.ClickLocation(tableBar, numRect.left - shRect.left + 10, numRect.top - shRect.top + 10);
                var pt3 = (IUIAutomationLegacyIAccessiblePattern)cols.GetElement(3)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt3.DoDefaultAction();
                Thread.Sleep(500);
                childs = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childs[childs.Count - 1].hWnd, 12, IntPtr.Zero, "-3");

                //var priceRect = cols.GetElement(4).CurrentBoundingRectangle;
                //WinApi.ClickLocation(tableBar, priceRect.left - shRect.left + 10, priceRect.top - shRect.top + 10);
                var pt4 = (IUIAutomationLegacyIAccessiblePattern)cols.GetElement(4)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt4.DoDefaultAction();
                Thread.Sleep(500);
                childs = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childs[childs.Count - 1].hWnd, 12, IntPtr.Zero, "100.00");
                Thread.Sleep(100);
                pt4.DoDefaultAction();
                Thread.Sleep(500);

            }
        }

        //调整不含税金额
        public static void Taxtiaozheng()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票");
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);

            var toolBar = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "toolStrip3"));
            var dataGridUia = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var shElement = dataGridUia.FindFirst(TreeScope.TreeScope_Descendants,
                UiaHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, "首行"));

            var shChilds = shElement.FindAll(UIAutomationClient.TreeScope.TreeScope_Descendants,
                UiaHelper.GetUIAutomation().CreateTrueCondition());

            var danjia = shChilds.GetElement(5);
            if (danjia.CurrentName.Contains("不含税"))
            {

            }
            else
            {
                ClickBtnUia(toolBar.CurrentNativeWindowHandle, "价格");
            }
        }

        //卷票调整
        public static void Method13()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);
            var dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation()
                .CreatePropertyCondition(
                    UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var tableBar = (IntPtr) dataGridUia.CurrentNativeWindowHandle;
            var childs = dataGridUia.FindAll(TreeScope.TreeScope_Children,
                UiaHelper.GetUIAutomation().CreateTrueCondition());

            var count = childs.Length;

            for (var i = 0; i < count; i++)
            {
                dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation()
                    .CreatePropertyCondition(
                        UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

                childs = dataGridUia.FindAll(TreeScope.TreeScope_Children,
                    UiaHelper.GetUIAutomation().CreateTrueCondition());

                var uiarow = dataGridUia.FindFirst(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, $"行 {i}"));

                Console.WriteLine($"uiarow.CurrentName:{uiarow.CurrentName}");

                var uiaxiangmu = uiarow.FindFirst(TreeScope.TreeScope_Children,
                    UiaHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, $"项目 行 {i}"));

                Console.WriteLine($"uiaxiangmu.CurrentName:{uiaxiangmu.CurrentName}");

                var uiashuilv = uiarow.FindFirst(TreeScope.TreeScope_Children,
                    UiaHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, $"税率 行 {i}"));

                Console.WriteLine($"uiashuilv.CurrentName:{uiashuilv.CurrentName}");
                //var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                //    UiaHelper.GetUIAutomation().CreateTrueCondition());

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

        //根据条件查找控件--uia和ui的方式
        public static void Method12()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);
            var dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var winMation = AutomationElement.FromHandle(winBar);
            var dataGridMation = winMation.FindFirst(System.Windows.Automation.TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, "DataGrid1"));
        }

        /// <summary>
        /// 直接操作日历控件 成功
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
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);
            var riliUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_NamePropertyId, "日历控件"));
            var riliRect = riliUia.CurrentBoundingRectangle;
            var upBtnUia = riliUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_NamePropertyId, "上一个按钮"));
            var upBtnRect = upBtnUia.CurrentBoundingRectangle;
            //Console.WriteLine($"upBtnUia.CurrentNativeWindowHandle:{upBtnUia.CurrentNativeWindowHandle}");
            for (var i = 0; i < 1; i++)
            {
                WinApi.ClickLocation(riliUia.CurrentNativeWindowHandle, upBtnRect.left - riliRect.left + 5,
                    upBtnRect.top - riliRect.top + 5);
                Thread.Sleep(500);
            }
            Thread.Sleep(1000);
            var chooseRect = riliUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_NamePropertyId, "‎1")).CurrentBoundingRectangle;
            WinApi.ClickLocation(riliUia.CurrentNativeWindowHandle, chooseRect.left - riliRect.left + 5,
                chooseRect.top - riliRect.top + 5);
        }


        /// <summary>
        /// 卷票测试--通过--有点小问题需要调整--调整完毕，大于5的话从4开始，其他从3开始--默认五条数据
        /// </summary>
        public static void Method10()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);

            var toolBarUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "toolStrip3"));

            var dataGridUia = winBarUia.FindFirst(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));
            var tableBar = (IntPtr)dataGridUia.CurrentNativeWindowHandle;

            var shouhangRetc = dataGridUia.FindFirst(TreeScope.TreeScope_Descendants,
                UiaHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, "首行")).CurrentBoundingRectangle;
            for (var i = 0; i < 2; i++)
            {
                HxShengQing.ClickBtnByName(toolBarUia.CurrentNativeWindowHandle, "减行");
                Thread.Sleep(100);
            }
            for (var i = 0; i < 3; i++)
            {
                ClickBtnUia(toolBarUia.CurrentNativeWindowHandle, "增行");
                Thread.Sleep(500);
                //对名称进行赋值
                var childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, $"uuyy{i}");

                var element = dataGridUia.FindFirst(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation()
                    .CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, $"行 {i}"));

                var elementChilds = element.FindAll(TreeScope.TreeScope_Children,
                    UiaHelper.GetUIAutomation().CreateTrueCondition());

                var pt2 = (IUIAutomationLegacyIAccessiblePattern)elementChilds.GetElement(2)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                var rectcol2 = elementChilds.GetElement(2).CurrentBoundingRectangle;

                //触发商品编码添加窗体
                pt2.DoDefaultAction();
                Thread.Sleep(2000);
                var noaddBar = WinApi.FindWindow(null, "商品编码添加");
                if (noaddBar != IntPtr.Zero)
                {
                    Bug.WriteGoodsTaxNoAdd(noaddBar, "101010104");
                    Thread.Sleep(500);
                    
                    WinApi.ClickLocation(tableBar, rectcol2.left - shouhangRetc.left + 10,
                        rectcol2.top - shouhangRetc.top + 10);
                    Thread.Sleep(500);
                }
                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, "2.00");

                var pt3 = (IUIAutomationLegacyIAccessiblePattern)elementChilds.GetElement(3)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt3.DoDefaultAction();
                Thread.Sleep(500);
                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, "200.00");
                pt3.DoDefaultAction();
                Thread.Sleep(500);

                var rect = elementChilds.GetElement(5).CurrentBoundingRectangle;
                WinApi.ClickLocation(tableBar, rect.left - shouhangRetc.left + 10, rect.top - shouhangRetc.top + 10);
                Thread.Sleep(500);
                childinfos1 = WinApi.EnumChildWindowsCallback(tableBar);
                //修改税率
                UIHelper.SetCombox(childinfos1.Find(b => b.szClassName.Contains("COMBOBOX")).hWnd, "17%");
                
            }
            Console.WriteLine("success...");

        }


        /// <summary>
        /// 日期控件操作---思路--操作日期控件成功
        /// </summary>
        public static void Method9()
        {
            var infoBar = WinApi.FindWindow(null, "信息表选择");


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
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);

            var allUiaMations = winBarUia.FindAll(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreateTrueCondition());

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


            winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);

            allUiaMations = winBarUia.FindAll(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreateTrueCondition());

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
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票");
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);

            var toolBar = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "toolStrip3"));

            for (var i = 0; i < 4; i++)
            {
                HxShengQing.ClickBtnByName(toolBar.CurrentNativeWindowHandle, "减行");
                Thread.Sleep(100);
            }
            for (var i = 0; i < 4; i++)
            {
                ClickBtnUia(toolBar.CurrentNativeWindowHandle, "增行");
                Thread.Sleep(100);
            }

            var dataGridUia = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_AutomationIdPropertyId, "DataGrid1"));

            var rectdatatable = dataGridUia.CurrentBoundingRectangle;

            var childs = dataGridUia.FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());
            for (var i = 2; i < childs.Length; i++)
            {
                var comElement = childs.GetElement(i);
                var rect = comElement.CurrentBoundingRectangle;

                var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                    UiaHelper.GetUIAutomation().CreateTrueCondition());

                //对名称进行赋值
                var pt3 = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(1)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                //pt3.DoDefaultAction();
                Thread.Sleep(500);
                var childinfos1 = WinApi.EnumChildWindowsCallback(dataGridUia.CurrentNativeWindowHandle);
                WinApi.SendMessage(childinfos1[childinfos1.Count - 1].hWnd, 12, IntPtr.Zero, $"AAA");

                try
                {
                    //pt3.DoDefaultAction();
                }
                catch (Exception e)
                {
                    var noaddBar = WinApi.FindWindow(null, "商品编码添加");
                    Bug.WriteGoodsTaxNoAdd(noaddBar, "101010104");
                    Console.WriteLine(e);
                }

                //改变税率

                var subElement = subChilds.GetElement(7);
                var retcsub = subElement.CurrentBoundingRectangle;
                var pt2 = (IUIAutomationLegacyIAccessiblePattern) subChilds.GetElement(7)
                    .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
                pt2.DoDefaultAction();

                childinfos1 = WinApi.EnumChildWindowsCallback(dataGridUia.CurrentNativeWindowHandle);
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
                        var childinfos = WinApi.EnumChildWindowsCallback(dataGridUia.CurrentNativeWindowHandle);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "小");
                        Thread.Sleep(100);
                    }
                    else if (j == 3)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(dataGridUia.CurrentNativeWindowHandle);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "kg");
                        Thread.Sleep(100);
                    }
                    else if (j == 4)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(dataGridUia.CurrentNativeWindowHandle);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "3");
                        Thread.Sleep(100);
                    }
                    else if (j == 5)
                    {
                        pt1.DoDefaultAction();
                        var childinfos = WinApi.EnumChildWindowsCallback(dataGridUia.CurrentNativeWindowHandle);
                        WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "100");
                        Thread.Sleep(100);
                        pt1.DoDefaultAction();
                        Thread.Sleep(1000);
                    }
                }
            }
        }

        //普票全部红冲-只改变金额（不含税的地方）-- 测试成功
        public static void Method88()
        {
            //459660
            var tableBar = (IntPtr)394524;

            var childs = UiaHelper.GetUIAutomation().ElementFromHandle(tableBar)
                .FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());


            var subChilds = childs.GetElement(1).FindAll(TreeScope.TreeScope_Children,
                UiaHelper.GetUIAutomation().CreateTrueCondition());

            var pt1 = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(6)
                .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

            pt1.DoDefaultAction();
            var childinfos = WinApi.EnumChildWindowsCallback(tableBar);
            WinApi.SendMessage(childinfos[childinfos.Count - 1].hWnd, 12, IntPtr.Zero, "100");
            Thread.Sleep(100);
            pt1.DoDefaultAction();
        }


        /// <summary>
        /// 成功操作红字申请测试通过
        /// </summary>
        public static void Method7()
        {
            var tableBar = (IntPtr)6227812;

            var childs = UiaHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());
            for (var i = 1; i < childs.Length; i++)
            {

                var subChilds = childs.GetElement(i).FindAll(TreeScope.TreeScope_Children,
                    UiaHelper.GetUIAutomation().CreateTrueCondition());

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

            var childs = UiaHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());

            var subChilds = childs.GetElement(4).FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());

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

            var childs = UiaHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());

            var subChilds = childs.GetElement(3).FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());

           
            
            var pt = (IUIAutomationLegacyIAccessiblePattern)subChilds.GetElement(6)
                .GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
            pt.SetValue("100.99");
            

            

            //WinApi.SendMessage((IntPtr)subChilds.GetElement(3).CurrentNativeWindowHandle, 12, IntPtr.Zero, "88");
        }
        //操作表格控件
        public static void Method4()
        {
            var tableBar = (IntPtr)4852320;

            var childs = UiaHelper.GetUIAutomation().ElementFromHandle(tableBar).
                FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());

            var subChilds = childs.GetElement(3).FindAll(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreateTrueCondition());


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
                    var txtMation = UiaHelper.GetUIAutomation().ElementFromHandle(childinfos[i].hWnd);

                    var txtPt = (IUIAutomationValuePattern)txtMation.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
                    Console.WriteLine(txtPt.CurrentValue);
                }

                //Console.WriteLine($"No:{i},hwnd:{(int)childinfos[i].hWnd},szWindowName:{childinfos[i].szWindowName},szTextName{childinfos[i].szTextName}");
            }
        }

        public static void Method2()
        {
            var uiaInstance = UiaHelper.GetUIAutomation().ElementFromHandle((IntPtr)18615182);
            var allChilds = uiaInstance.FindAll(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreateTrueCondition());

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
            if (UiaHelper.IsAvailable == false) return;

            var uiaInstance = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);
            var allChilds = uiaInstance.FindAll(TreeScope.TreeScope_Descendants, UiaHelper.GetUIAutomation().CreateTrueCondition());
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

        public static bool ClickBtnUia(IntPtr toolBar, string name)
        {
            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(toolBar);
            var element = winBarUia.FindFirst(TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().CreatePropertyCondition(
                UIA_PropertyIds.UIA_NamePropertyId, name));
            var pattern = (IUIAutomationInvokePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
            pattern?.Invoke();
            return true;
        }

    }
}
