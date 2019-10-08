using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using UIAutomationClient;
using User32Test;

namespace SearchBar
{
    public class SpecialRedRush
    {
        public static void Debug1()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税专用发票");

            //初始化控件句柄
            var addPhoBar = IntPtr.Zero;
            var accountBar = IntPtr.Zero;
            var remarkBar = IntPtr.Zero;

            var list = WinApi.EnumChilWindowsIntptr(winBar);
           

            var list2 = new List<IntPtr>();
            for (var i = 1; i < list.Count; i++)
            {
                list2 = WinApi.FindChildBar((IntPtr)list[i]);
                if (list2?.Count >= 22)
                {
                    break;
                }
            }

           
            remarkBar = list2[6];

            var accountBars = WinApi.FindChildInfo(list2[list2.Count - 4]);
            accountBar = accountBars.Find(bar => bar.szClassName.Contains("EDIT")).hWnd;

            var addPhoBars = WinApi.FindChildInfo(list2[list2.Count - 2]);
            addPhoBar = addPhoBars.Find(bar => bar.szClassName.Contains("EDIT")).hWnd;

            SetEditValueSpecialRed(accountBar, "宝山区 fffggg");
            SetEditValueSpecialRed(addPhoBar, "15721528ad020");

            var str1 = GetEditValueSpecialRed(accountBar)+1;
            var str2 = GetEditValueSpecialRed(addPhoBar)+1;
            if (str1 != "宝山区 fffggg")
            {
                throw new Exception("fuzhi fail..");
            }

            var accountMation = AutomationElement.FromHandle(accountBar);
            accountMation.TryGetCurrentPattern(ValuePattern.Pattern, out var pt);
            Console.WriteLine(((ValuePattern)pt).Current.Value);
        }

        public static void SetEditValueSpecialRed(IntPtr hwnd, string editValue)
        {
            try
            {
                var editMation = AutomationElement.FromHandle(hwnd);
                editMation.TryGetCurrentPattern(ValuePattern.Pattern, out var pt);
                if (pt == null)
                {
                    WinApi.SendMessage(hwnd, WinApi.BM_TEXT, IntPtr.Zero, editValue);
                }
                else
                {
                    ((ValuePattern)pt)?.SetValue(editValue);
                }
            }
            catch (Exception e)
            {
                
                WinApi.SendMessage(hwnd, WinApi.BM_TEXT, IntPtr.Zero, editValue);
            }
        }

        public static string GetEditValueSpecialRed(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
            {
                
                return "";
            }

            var length = WinApi.SendMessage(hwnd, 14, 0, 0);
            var sb = new StringBuilder(length + 1);
            WinApi.SendMessage(hwnd, 13, length, sb);
            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                return sb.ToString();
            }

            try
            {
                var editMation = AutomationElement.FromHandle(hwnd);
                editMation.TryGetCurrentPattern(ValuePattern.Pattern, out var pt);
                return ((ValuePattern)pt)?.Current.Value;
            }
            catch (Exception e)
            {
                
            }

            return "";
        }
    }
}
