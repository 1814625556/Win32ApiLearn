using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Xforceplus.Plugin.AdobePrinter
{
    internal class AdobeDialogueUI
    {
        public static void AdobeDefaultPDF_Dialogue()
        {
            Thread threadUI = new Thread(new ThreadStart(new Action(() =>
            {
                int whileCount = 0;
                while (true && whileCount++ < 20)
                {
                    var winBar = ApiUI.FindWindow(null, "Adobe Reader");
                    if (winBar != IntPtr.Zero)
                    {
                        var childs = ApiUI.EnumChildWindowsCallback(winBar);
                        LeftClickMsg(childs.Find(b => b.szWindowName.Contains("不再显示本消息")).hWnd);
                        Thread.Sleep(500);
                        LeftClickMsg(childs.Find(b => b.szWindowName == "确定").hWnd);

                        break;
                    }

                    Thread.Sleep(3000);
                }

                whileCount = 0;
                while (true && whileCount++ < 10)
                {
                    var winBar = ApiUI.FindWindow(null, "辅助工具设置助手");
                    if (winBar != IntPtr.Zero)
                    {
                        var childs = ApiUI.EnumChildWindowsCallback(winBar);
                        LeftClickMsg(childs.Find(b => b.szWindowName.Contains("取消")).hWnd);
                    }

                    Thread.Sleep(3000);
                }

            })));

            threadUI.IsBackground = true;
            threadUI.Start();
        }

        private static void LeftClickMsg(IntPtr intPtr)
        {
            ApiUI.PostMessage(intPtr, 245, 0, 0);
            Thread.Sleep(10);
        }
    }
}
