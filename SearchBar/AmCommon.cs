using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UIAutomationClient;
using User32Test;

namespace SearchBar
{
    public class AmCommon
    {
        public static bool ClickBtnByNameWin32(IntPtr toolBar, string name)
        {
            if (toolBar == IntPtr.Zero)
                return false;
            
            try
            {
                var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(toolBar);
                var element = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().
                    CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, name));
                var toolBarRect = winBarUia.CurrentBoundingRectangle;
                var elementRect = element.CurrentBoundingRectangle;
                WinApi.ClickLocation(toolBar, elementRect.left - toolBarRect.left + 10,
                    (elementRect.bottom - elementRect.top) / 2);
                return true;
            }
            catch (Exception e)
            {
                
            }
            return false;
        }

    }
}
