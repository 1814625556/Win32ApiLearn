using System;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace SearchBar
{
    public class DataGridTest
    {
        /// <summary>
        /// 暂时发现不行 -- 因为里面的单元是custom类型的
        /// </summary>
        public static void SetData()
        {
            var dataMation = AutomationElement.FromHandle((IntPtr)6555068);

            dataMation.TryGetCurrentPattern(GridPattern.Pattern, out var dp);
            //var tempElement = ((TableItemPattern) dp).;

            var childs = dataMation.FindAll(TreeScope.Children, Condition.TrueCondition);
            //foreach (AutomationElement child in childs)
            //{
            //    if (child.Current.Name == "行 0")
            //    {
            //        var items = child.FindAll(TreeScope.Children, Condition.TrueCondition);
            //        for (int i = 0; i < items.Count; i++)
            //        {
            //            Console.WriteLine($"type:{items[i].Current.ControlType == ControlType.Edit}," +
            //                              $"name:{items[i].Current.Name}");
            //            items[i].TryGetCurrentPattern(ValuePattern.Pattern, out var cc);
            //            ((ValuePattern)cc).SetValue("IIII");
            //        }
            //    }
            //}
            //Console.WriteLine($"{ControlType.Button.ToString()}");
            Console.ReadKey();
        }





    }
}
