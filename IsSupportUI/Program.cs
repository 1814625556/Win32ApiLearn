using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace IsSupportUI
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var uiElement = AutomationElement.FromHandle((IntPtr) (Convert.ToInt32(args[0])));
                var childs = uiElement.FindAll(System.Windows.Automation.TreeScope.Descendants,
                    Condition.TrueCondition);
                Console.WriteLine("Count:"+childs.Count);
                for (int i = 0; i < childs.Count; i++)
                {
                    Console.WriteLine($"Name:{childs[i].Current.Name},className:{childs[i].Current.ClassName}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("不支持UI");
                Console.WriteLine(e.Message+e.StackTrace);
            }
            Console.ReadKey();
        }
    }
}
