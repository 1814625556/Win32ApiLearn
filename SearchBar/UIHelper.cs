using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace SearchBar
{
    public class UIHelper
    {
        //设置税率
        public static void SetCombox(IntPtr comboxBar, string item)
        {
            if (comboxBar == IntPtr.Zero) return;

            var comBoxMation = AutomationElement.FromHandle(comboxBar);
            if (comBoxMation.Current.IsEnabled == false) return;

            var selectItem = comBoxMation.FindFirst(TreeScope.Subtree,
                new PropertyCondition(AutomationElement.NameProperty, item));

            if (selectItem == null) return;

            selectItem.TryGetCurrentPattern(SelectionItemPattern.Pattern, out var selectItemPattern);
            ((SelectionItemPattern)selectItemPattern)?.Select();
        }

        //UI
        public static void SetSelectedComboBoxItem(AutomationElement comboBox, string item)
        {
            AutomationPattern automationPatternFromElement = GetSpecifiedPattern(comboBox, "ExpandCollapsePatternIdentifiers.Pattern");

            ExpandCollapsePattern expandCollapsePattern = comboBox.GetCurrentPattern(automationPatternFromElement) as ExpandCollapsePattern;

            expandCollapsePattern.Expand();//复选框展开
            expandCollapsePattern.Collapse();//复选框折叠

            AutomationElement listItem = comboBox.FindFirst(TreeScope.Subtree, new PropertyCondition(AutomationElement.NameProperty, item));

            automationPatternFromElement = GetSpecifiedPattern(listItem, "SelectionItemPatternIdentifiers.Pattern");

            SelectionItemPattern selectionItemPattern = listItem.GetCurrentPattern(automationPatternFromElement) as SelectionItemPattern;

            selectionItemPattern.Select();
        }

        private static AutomationPattern GetSpecifiedPattern(AutomationElement element, string patternName)
        {
            AutomationPattern[] supportedPattern = element.GetSupportedPatterns();

            foreach (AutomationPattern pattern in supportedPattern)
            {
                if (pattern.ProgrammaticName == patternName)
                    return pattern;
            }

            return null;
        }

        public static void SetForeForm(IntPtr winBar)
        {
            var winMation = AutomationElement.FromHandle(winBar);
            if (winMation == null) return;

            winMation.TryGetCurrentPattern(WindowPattern.Pattern, out var winPattern);

            ((WindowPattern) winPattern)?.SetWindowVisualState(WindowVisualState.Normal);
        }
    }
}
