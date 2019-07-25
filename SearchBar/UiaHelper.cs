using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIAutomationClient;

namespace SearchBar
{
    public class UiaHelper
    {
        private static readonly CUIAutomation cUIAutomation;
        public static bool IsAvailable { get; private set; }



        static UiaHelper()
        {
            try
            {
                cUIAutomation = new CUIAutomation();
                IsAvailable = true;


                //var uiaTypes = typeof(UIA_ControlTypeIds);


            }
            catch (Exception ex)
            {
                //不支持Uia自动化
                IsAvailable = false;
            }
        }

        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static CUIAutomation GetUIAutomation()
        {
            return cUIAutomation;
        }

        public string GetAmId(IUIAutomationElement self)
        {
            return self.CurrentAutomationId;
        }

        public IntPtr GetHwnd(IUIAutomationElement self)
        {
            return self.CurrentNativeWindowHandle;
        }

        public string GetName(IUIAutomationElement self)
        {
            return self.CurrentName;
        }

    }
}
