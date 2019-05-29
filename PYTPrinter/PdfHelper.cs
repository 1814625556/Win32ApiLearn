using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
//using Xforceplus.Log;

namespace Xforceplus.Mi.Core.Utils
{
    /// <summary>
    /// Spire 4.8.8.2020 版本 - 免费破解
    /// </summary>
    internal class PdfHelper
    {
        /// <summary>
        /// 操控打印机对象
        /// </summary>
        private static PrintDocument fPrintDocument = new PrintDocument();
        /// <summary>
        /// 获取本机默认打印机名称
        /// </summary>
        /// <returns></returns>
        public static String DefaultPrinter()
        {
            return fPrintDocument.PrinterSettings.PrinterName;
        }
        /// <summary>
        /// 获取所有本地打印机
        /// </summary>
        /// <returns></returns>
        public static List<String> GetLocalPrinters()
        {
            List<String> fPrinters = new List<String>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (String fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }
            return fPrinters;
        }
        /// <summary>
        /// 获取打印机的所有纸张类型
        /// </summary>
        /// <returns></returns>
        public static List<PaperSize> GetPrinterPapers()
        {
            PrintDocument printDoc = new PrintDocument();
            List<PaperSize> fPapers = new List<PaperSize>();
            for (int i = 0; i < printDoc.PrinterSettings.PaperSizes.Count; i++)
            {
                fPapers.Add(printDoc.PrinterSettings.PaperSizes[i]);
            }
            return fPapers;
        }

        public static PdfDocument LoadPdf(string pdfFile)
        {
            //加载PDF文档
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfFile);
            return doc;
        }

        public static PdfDocument LoadPdf(Stream pdfStream)
        {
            //加载PDF文档
            PdfDocument doc = new PdfDocument();
            doc.LoadFromStream(pdfStream);
            return doc;
        }
        /// <summary>
        /// 静默打印Pdf发票文档
        /// </summary>
        /// <param name="doc">PdfDocument 打印机文档对象</param>
        /// <param name="printTaskName">打印机任务名称</param>
        /// <param name="printerName">打印机名称</param>
        /// <returns></returns>
        public static bool QuietPrinter(PdfDocument doc, string printTaskName, string printerName)
        {
            try
            {
                //设置打印任务的名称
                if (!string.IsNullOrEmpty(printTaskName))
                    doc.PrintSettings.DocumentName = printTaskName;
                //选择打印机
                if (!string.IsNullOrEmpty(printerName))
                    doc.PrintSettings.PrinterName = printerName;
                SettingPrinter(doc);
                doc.Print();
                return true;
            }
            catch (Exception ex)
            {
                //MiLogger.Error("静默完美打印发票错误!", ex);
                return false;
            }
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
        /// 静默打印Pdf发票文档 带偏移量
        /// </summary>
        /// <param name="doc">PdfDocument对象 </param>
        /// <param name="top">上边距偏移量</param>
        /// <param name="left">左边距偏移量</param>
        /// <param name="printTaskName">打印机任务队列名称</param>
        /// <param name="printerName">打印机名称 不填表示选择系统默认打印机</param>
        /// <returns>打印机调用结果</returns>
        public static bool QuietPrinter(PdfDocument doc, int top = 0, int left = 0, string printTaskName = "", string printerName = "")
        {
            if (top == 0 && left == 0)
            {
                return QuietPrinter(doc, printTaskName, printerName);
            }
            else
            {
                return QuietPrinter(LoadPdf(ResetPageMargin(doc, top, left, printTaskName)));
            }
        }
        /// <summary>
        /// 设置页面打印Margin
        /// </summary>
        /// <param name="doc">PdfDocument打印机文档对象</param>
        /// <param name="top">上边距</param>
        /// <param name="left">左边距</param>
        /// <param name="invoiceName">偏移后PDF文件的名称</param>
        /// <returns>偏移后的pdf文档全路径</returns>
        private static string ResetPageMargin(PdfDocument doc, int top = 0, int left = 0, string invoiceName = "")
        {
            //实例化PdfDocument类，并加载测试文档
            PdfDocument pdfMargin = doc;
            //另新建一个PDF文档
            PdfDocument pdfResult = new PdfDocument();

            SizeF margin = MillimeterToPixel((float)left, (float)top);
            //遍历文档pdf1中的所有页面     
            foreach (PdfPageBase page in pdfMargin.Pages)
            {
                //指定A4大小的页面和页边距，并添加到文档pdf2
                SizeF size = page.Size;
                PdfPageBase newPage = pdfResult.Pages.Add(size, new PdfMargins(margin.Width, margin.Height));
                //将原pdfMargin中内容写入新页面     
                page.CreateTemplate().Draw(newPage.Canvas);
            }
            //保存新的PDF文档
            if (string.IsNullOrEmpty(invoiceName))
                invoiceName = System.Guid.NewGuid().ToString("N").ToLower();
            string newPdfPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfinvoicetemp"), invoiceName + string.Format("_margin_{0}&{1}.pdf", top, left));
            try
            {
                pdfResult.SaveToFile(newPdfPath, FileFormat.PDF);
                return newPdfPath;
            }
            catch (Exception ex)
            {
                //MiLogger.Error("发票打印边距偏移量设置失败!", ex);
                return "";
            }
        }

        /// <summary>
        /// 毫米转换为像素点数量
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private static SizeF MillimeterToPixel(float left = 0f, float top = 0f)
        {
            if (left == 0 && top == 0)
                return new SizeF(0f, 0f);
            float dpiX = 0f;
            float dpiY = 0f;
            float pixelX = 0;
            float pixelY = 0;
            const float inch = 25.4f;   //一英寸等于25.4mm
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpiX = graphics.DpiX;
                dpiY = graphics.DpiY;
            }
            //象素数 / DPI = 英寸数
            //英寸数 * 25.4 = 毫米数
            pixelX = (left / inch) * dpiX;
            pixelY = (top / inch) * dpiY;
            return new SizeF(pixelX, pixelY);
        }

        /// <summary>
        /// Http下载Pdf文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pdfSavePath">下载文件的存储路径</param>
        /// <param name="invoiceName">可自定义发票Pdf文件名称</param>
        /// <returns>返回PDF文件流</returns>
        public static Stream PdfDownloadFromHttp(string url, out string pdfSavePath, string invoiceName = "")
        {
            MemoryStream memoryStream = new MemoryStream();
            pdfSavePath = "";
            if (string.IsNullOrEmpty(invoiceName))
                invoiceName = url.Substring(url.LastIndexOf(@"/") + 1);
            string tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfinvoicetemp", invoiceName);
            string tempDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfinvoicetemp");
            if (!Directory.Exists(tempDirectory))
            {
                System.IO.Directory.CreateDirectory(tempDirectory);  //创建pdf发票文件目录
            }
            try
            {
                if (System.IO.File.Exists(tempPath))
                {
                    System.IO.File.Delete(tempPath);    //存在相同文件则删除
                }
                FileStream fs = null;
                Stream responseStream = null;
                try
                {
                    // 设置参数
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //发送请求并获取相应回应数据
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //直到request.GetResponse()程序才开始向目标网页发送Post请求

                    responseStream = response.GetResponseStream();
                    //创建PDF文件
                    fs = new FileStream(tempPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    //创建本地文件写入流
                    byte[] bArr = new byte[1024];
                    int actual = responseStream.Read(bArr, 0, (int)bArr.Length);
                    while (actual > 0)
                    {
                        fs.Write(bArr, 0, actual);
                        memoryStream.Write(bArr, 0, actual);
                        actual = responseStream.Read(bArr, 0, (int)bArr.Length);
                    }
                    pdfSavePath = tempPath;
                    return memoryStream;
                }
                catch (Exception)
                {
                    return memoryStream;
                }
                finally
                {
                    if (fs != null)
                        fs.Close();
                    if (responseStream != null)
                        responseStream.Close();
                }
            }
            catch { return memoryStream; }
        }
    }
}

