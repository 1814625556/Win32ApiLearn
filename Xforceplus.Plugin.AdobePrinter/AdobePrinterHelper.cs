using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Xforceplus.Plugin.AdobePrinter
{
    internal static class AdobePrinterHelper
    {
        private static IntPtr hWnd = new IntPtr(0);

        private static AdobePanel panelAdobe;

        private static bool IsAsyncPrint = true;

        public static bool AdobeReaderPrint(string adobePath, string filePath, string printerName)
        {
            try
            {
                if (string.IsNullOrEmpty(printerName))
                {
                    printerName = PdfHelper.DefaultPrinter();
                }

                var acroList = Process.GetProcessesByName("AcroRd32").ToList();
                if (acroList.Count <= 0)
                {
                    hWnd = new IntPtr(0);
                    if (panelAdobe != null)
                    {
                        panelAdobe.Close();
                        panelAdobe = null;
                    }
                }

                if (hWnd == new IntPtr(0))
                {
                    // 查杀AcroRd32进程
                    try
                    {
                        Process.GetProcessesByName("AcroRd32").ToList().ForEach(x => x.Kill());
                        Thread.Sleep(500);
                    }
                    catch
                    {
                    }
                }

                var p = new Process();
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = adobePath;
                var argumentone = "/h /t \"" + filePath + "\" \"" + printerName + "\"";
                if (hWnd != new IntPtr(0) && IsAsyncPrint == false)
                {
                    argumentone = "/n /t \"" + filePath + "\" \"" + printerName + "\"";
                }

                startInfo.Arguments = argumentone;
                startInfo.CreateNoWindow = true;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.UseShellExecute = false;
                p.StartInfo = startInfo;
                var isShellOk = p.Start();

                if (isShellOk == false)
                {
                    return false;
                }

                if (hWnd != new IntPtr(0))
                {
                    DateTime sPrintTime = System.DateTime.Now;
                    //智能检测一下打印机是否可以进入异步打印状态
                    isShellOk = p.WaitForExit(30000);
                    if (System.DateTime.Now.AddSeconds(-3) > sPrintTime)
                    {
                        IsAsyncPrint = false;
                    }
                    else
                    {
                        IsAsyncPrint = true;
                    }
                }

                if (isShellOk == false)
                {
                    throw new Exception("打印任务入Windows打印机等待队列超时!");
                }

                if (hWnd == new IntPtr(0))
                {
                    var thread = new Thread(() =>
                    {
                        var whileCount = 0;
                        while (true && whileCount++ < 20)
                        {
                            var prc = Process.GetProcessesByName("AcroRd32");
                            foreach (var item in prc)
                            {
                                if (item.MainWindowHandle != hWnd)
                                {
                                    hWnd = item.MainWindowHandle;

                                    break;
                                }
                            }

                            if (hWnd != new IntPtr(0))
                            {
                                break;
                            }

                            Thread.Sleep(300);
                        }
                    });

                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();

                    if (panelAdobe == null && hWnd != new IntPtr(0))
                    {
                        panelAdobe = new AdobePanel();
                        var insertwin = new FillAdobeWindow(panelAdobe.GetAdobePanel, hWnd);
                        panelAdobe.Hide();
                    }

                    // 检查Adobe Pdf 默认弹窗提示.
                    AdobeDialogueUI.AdobeDefaultPDF_Dialogue();
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("调用Adobe Reader组件出现异常", ex);
            }
        }

        public static string GetAdobePath()
        {
            RegistryKey key =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\AcroRd32.exe",
                    true);

            if (key == null)
            {
                return "";
            }

            string getKey = key.GetValue("Path", "").ToString();

            return getKey;
        }
    }
}
