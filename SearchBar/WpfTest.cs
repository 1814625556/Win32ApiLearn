using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using User32Test;

namespace SearchBar
{
    public static class WpfTest
    {
        public static void CommonControl()
        {
            System.Diagnostics.Process.
                Start(@"C:\MyDatas\VsProjects\demo\SimulationMouseKeyboard\WpfSimulationMouseKeyboardForm\bin\Debug\WpfSimulationMouseKeyboardForm.exe");
            Thread.Sleep(1000);
            var bar = WinApi.FindWindow(null, "MainWindow");
            WinApi.ShowWindow(bar,3);
            Thread.Sleep(1000);
            var childs = AutomationElement.FromHandle(bar).FindAll(TreeScope.Descendants, Condition.TrueCondition);
            for (var i=0;i<childs.Count;i++)
            {
                Console.WriteLine(childs[i].Current.Name);
            }

            //精确查找并点击~
            var btn1 = AutomationElement.FromHandle(bar).FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, "btntwo"));

            

            btn1.TryGetCurrentPattern(InvokePattern.Pattern, out var btnPt);
            ((InvokePattern)btnPt).Invoke();
        }
    }
}
