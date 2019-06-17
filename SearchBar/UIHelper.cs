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
        //UI
        public static void SetSelectedComboBoxItem(AutomationElement comboBox, string item)
        {
            AutomationPattern automationPatternFromElement = GetSpecifiedPattern(comboBox, "ExpandCollapsePatternIdentifiers.Pattern");

            ExpandCollapsePattern expandCollapsePattern = comboBox.GetCurrentPattern(automationPatternFromElement) as ExpandCollapsePattern;

            expandCollapsePattern.Expand();
            expandCollapsePattern.Collapse();

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

        public static void InsertTextUsingUIAutomation(AutomationElement element,string value)
        {
            try
            {
                // Validate arguments / initial setup
                if (value == null)
                    throw new ArgumentNullException(
                        "String parameter must not be null.");

                if (element == null)
                    throw new ArgumentNullException(
                        "AutomationElement parameter must not be null");

                // A series of basic checks prior to attempting an insertion.
                //
                // Check #1: Is control enabled?
                // An alternative to testing for static or read-only controls 
                // is to filter using 
                // PropertyCondition(AutomationElement.IsEnabledProperty, true) 
                // and exclude all read-only text controls from the collection.
                if (!element.Current.IsEnabled)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + " is not enabled.\n\n");
                }

                // Check #2: Are there styles that prohibit us 
                //           from sending text to this control?
                if (!element.Current.IsKeyboardFocusable)
                {
                    throw new InvalidOperationException(
                        "The control with an AutomationID of "
                        + element.Current.AutomationId.ToString()
                        + "is read-only.\n\n");
                }


                // Once you have an instance of an AutomationElement,  
                // check if it supports the ValuePattern pattern.
                object valuePattern = null;

                // Control does not support the ValuePattern pattern 
                // so use keyboard input to insert content.
                //
                // NOTE: Elements that support TextPattern 
                //       do not support ValuePattern and TextPattern
                //       does not support setting the text of 
                //       multi-line edit or document controls.
                //       For this reason, text input must be simulated
                //       using one of the following methods.
                //       
                if (!element.TryGetCurrentPattern(
                    ValuePattern.Pattern, out valuePattern))
                {
                    //feedbackText.Append("The control with an AutomationID of ")
                    //    .Append(element.Current.AutomationId.ToString())
                    //    .Append(" does not support ValuePattern.")
                    //    .AppendLine(" Using keyboard input.\n");

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    // Pause before sending keyboard input.
                    Thread.Sleep(100);

                    // Delete existing content in the control and insert new content.
                    SendKeys.SendWait("^{HOME}");   // Move to start of control
                    SendKeys.SendWait("^+{END}");   // Select everything
                    SendKeys.SendWait("{DEL}");     // Delete selection
                    SendKeys.SendWait(value);
                }
                // Control supports the ValuePattern pattern so we can 
                // use the SetValue method to insert content.
                else
                {
                    //feedbackText.Append("The control with an AutomationID of ")
                    //    .Append(element.Current.AutomationId.ToString())
                    //    .Append((" supports ValuePattern."))
                    //    .AppendLine(" Using ValuePattern.SetValue().\n");

                    // Set focus for input functionality and begin.
                    element.SetFocus();

                    ((ValuePattern)valuePattern).SetValue(value);
                }
            }
            catch (ArgumentNullException exc)
            {
                //feedbackText.Append(exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                //feedbackText.Append(exc.Message);
            }
            finally
            {
                //Feedback(feedbackText.ToString());
            }
        }
    }
}
