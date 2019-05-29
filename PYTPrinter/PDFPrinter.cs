using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using Spire.Pdf;

namespace PYTPrinter
{
    public static class PDFPrinter
    {
        /// <summary>
        /// 测试成功--可用方案
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="printName"></param>
        public static void pdfPrint(string filePath,string printName)
        {
            //创建PdfDocument对象
            PdfDocument doc = new PdfDocument();

            //加载一个现有文档
            doc.LoadFromFile(filePath);

            //选择打印机
            doc.PrintSettings.PrinterName = new PrintDocument().PrinterSettings.PrinterName;

            SettingPrinter(doc);

            //选择打印页码范围
            doc.PrintSettings.SelectPageRange(1, 10);

            //执行打印
            doc.Print();
        }
        /// <summary>
        /// 设置打印机默认配置
        /// </summary>
        /// <param name="doc"></param>
        private static void SettingPrinter(PdfDocument doc)
        {
            doc.PrintSettings.SelectPageRange(1, doc.Pages.Count);
            doc.PrintSettings.Copies = 1;
            doc.PageSettings.Orientation = PdfPageOrientation.Portrait;    //Portrait-纵向 LandScape-横向;
            doc.PrintSettings.SelectSinglePageLayout(Spire.Pdf.Print.PdfSinglePageScalingMode.ActualSize, false);  //Pdf文件的实际尺寸                                                                                                     //静默打印PDF文档
            doc.PrintSettings.PrintController = new StandardPrintController();
        }

        /// <summary>
        /// 测试成功，但是这个必须有打开pdf文件的程序 还必须打开一次才可以
        /// </summary>
        /// <param name="filePath"></param>
        public static void pdfPrint(string filePath)
        {
            PrintDocument pd = new PrintDocument();
            Process p = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.UseShellExecute = true;
            startInfo.FileName = filePath;
            startInfo.Verb = "print";
            startInfo.Arguments = @"/p /h \" + filePath + "\"\"" + pd.PrinterSettings.PrinterName + "\"";
            p.StartInfo = startInfo;
            p.Start();
            p.WaitForExit();


        }
    }
}
