using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIAutomationClient;

namespace IsSupportcom
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var comElement = UiaHelper.GetUIAutomation().ElementFromHandle((IntPtr) (Convert.ToInt32(args[0])));
                var childs = comElement.FindAll(TreeScope.TreeScope_Descendants,
                    UiaHelper.GetUIAutomation().CreateTrueCondition());
                for (var i = 0; i < childs.Length; i++)
                {
                    Console.WriteLine($"Name:{childs.GetElement(i).CurrentName}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("不支持com");
                Console.WriteLine($"{e.Message},{e.StackTrace}");
            }
            Console.ReadKey();
        }
    }
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
