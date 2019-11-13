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
            var bar = WinApi.FindWindow(null, "MainWindow");
            if (bar != IntPtr.Zero)
            {
                WinApi.CloseWinForm(bar);
                Thread.Sleep(1000);
            }

            System.Diagnostics.Process.
                Start(@"C:\MyDatas\VsProjects\demo\SimulationMouseKeyboard\WpfSimulationMouseKeyboardForm\bin\Debug\WpfSimulationMouseKeyboardForm.exe");
            Thread.Sleep(1000);
            bar = WinApi.FindWindow(null, "MainWindow");
            WinApi.ShowWindow(bar,1);
            Thread.Sleep(1000);
            var childs = AutomationElement.FromHandle(bar).FindAll(TreeScope.Descendants, Condition.TrueCondition);
            //for (var i=0;i<childs.Count;i++)
            //{
            //    Console.WriteLine(childs[i].Current.Name);
            //}

            var editone = AutomationElement.FromHandle(bar).FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, "boxone"));
            editone.TryGetCurrentPattern(ValuePattern.Pattern, out var editPt);
            ((ValuePattern)editPt)?.SetValue("siemen cheer~");

            Thread.Sleep(2000);

            //精确查找并点击~
            var btn1 = AutomationElement.FromHandle(bar).FindFirst(TreeScope.Descendants,
                new PropertyCondition(AutomationElement.AutomationIdProperty, "btntwo"));

            btn1.TryGetCurrentPattern(InvokePattern.Pattern, out var btnPt);
            ((InvokePattern)btnPt).Invoke();

            Thread.Sleep(2000);

            var msgBar = WinApi.FindWindow(null, "SiomonMessageBox");
            if (msgBar != IntPtr.Zero)
            {
                Thread.Sleep(2000);
                WinApi.CloseWinForm(msgBar);
            }

            ((ValuePattern)editPt)?.SetValue("will be Closing~");
            Thread.Sleep(2000);
            WinApi.CloseWinForm(bar);
        }
    }
}
