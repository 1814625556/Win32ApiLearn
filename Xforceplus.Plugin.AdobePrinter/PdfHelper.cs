using Spire.Pdf;
using Spire.Pdf.Graphics;
using Spire.Pdf.Print;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Xforceplus.Plugin.AdobePrinter
{
    /// <summary>
    ///     Spire 4.8.8.2020 版本 - 免费破解
    /// </summary>
    internal class PdfHelper
    {
        /// <summary>
        ///     操控打印机对象
        /// </summary>
        private static readonly PrintDocument fPrintDocument = new PrintDocument();

        /// <summary>
        ///     获取本机默认打印机名称
        /// </summary>
        /// <returns></returns>
        public static string DefaultPrinter()
        {
            return fPrintDocument.PrinterSettings.PrinterName;
        }

        /// <summary>
        ///     获取所有本地打印机
        /// </summary>
        /// <returns></returns>
        public static List<string> GetLocalPrinters()
        {
            var fPrinters = new List<string>();
            fPrinters.Add(DefaultPrinter()); //默认打印机始终出现在列表的第一项
            foreach (string fPrinterName in PrinterSettings.InstalledPrinters)
            {
                if (!fPrinters.Contains(fPrinterName))
                {
                    fPrinters.Add(fPrinterName);
                }
            }

            return fPrinters;
        }

        /// <summary>
        ///     获取打印机的所有纸张类型
        /// </summary>
        /// <returns></returns>
        public static List<PaperSize> GetPrinterPapers()
        {
            var printDoc = new PrintDocument();
            var fPapers = new List<PaperSize>();
            for (var i = 0; i < printDoc.PrinterSettings.PaperSizes.Count; i++)
            {
                fPapers.Add(printDoc.PrinterSettings.PaperSizes[i]);
            }

            return fPapers;
        }

        public static PdfDocument LoadPdf(string pdfFile)
        {
            //加载PDF文档
            var doc = new PdfDocument();
            doc.LoadFromFile(pdfFile);

            return doc;
        }

        public static PdfDocument LoadPdf(Stream pdfStream)
        {
            //加载PDF文档
            var doc = new PdfDocument();
            doc.LoadFromStream(pdfStream);

            return doc;
        }

        /// <summary>
        ///     静默打印Pdf发票文档
        /// </summary>
        /// <param name="doc">PdfDocument 打印机文档对象</param>
        /// <param name="printTaskName">打印机任务名称</param>
        /// <param name="printerName">打印机名称</param>
        /// <returns></returns>
        public static bool QuietPrinter(PdfDocument doc,
            string printTaskName,
            string printerName)
        {
            try
            {
                //设置打印任务的名称
                if (!string.IsNullOrEmpty(printTaskName))
                {
                    doc.PrintSettings.DocumentName = printTaskName;
                }

                //选择打印机
                if (!string.IsNullOrEmpty(printerName))
                {
                    doc.PrintSettings.PrinterName = printerName;
                }

                SettingPrinter(doc);
                doc.Print();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("静默完美打印发票错误!", ex);
            }
        }

        /// <summary>
        ///     设置打印机默认配置
        /// </summary>
        /// <param name="doc"></param>
        private static void SettingPrinter(PdfDocument doc)
        {
            doc.PrintSettings.SelectPageRange(1, doc.Pages.Count);
            doc.PrintSettings.Copies = 1;
            doc.PageSettings.Orientation = PdfPageOrientation.Portrait; //Portrait-纵向 LandScape-横向;
            doc.PrintSettings.SelectSinglePageLayout(PdfSinglePageScalingMode.ActualSize,
                false); //Pdf文件的实际尺寸                                                                                                     //静默打印PDF文档

            doc.PrintSettings.PrintController = new StandardPrintController();
        }

        /// <summary>
        ///     静默打印Pdf发票文档 带偏移量
        /// </summary>
        /// <param name="doc">PdfDocument对象 </param>
        /// <param name="top">上边距偏移量</param>
        /// <param name="left">左边距偏移量</param>
        /// <param name="printTaskName">打印机任务队列名称</param>
        /// <param name="printerName">打印机名称 不填表示选择系统默认打印机</param>
        /// <param name="adobePath">Adobe Reader Pdf 可执行程序路径</param>
        /// <param name="paperType">纸张类型</param>
        /// <param name="startAdobePrinter">启动Adobe模式打印</param>
        /// <returns>打印机调用结果</returns>
        public static bool QuietPrinter(PdfDocument doc,
            float top = 0,
            float left = 0,
            string printTaskName = "",
            string printerName = "",
            string adobePath = "",
            SourceType paperType = SourceType.Invoice,
            bool startAdobePrinter = false)
        {
            string pdfPath = ResetPageMargin(doc, top, left, paperType, printTaskName);
            if (startAdobePrinter)
            {
                try
                {
                    return AdobePrinterHelper.AdobeReaderPrint(adobePath, pdfPath, printerName);
                }
                catch (Exception ex)
                {
                    throw new Exception("调用Adobe Reader执行打印任务失败！",ex);
                }
            }

            return QuietPrinter(LoadPdf(pdfPath), printTaskName, printerName);
        }

        /// <summary>
        ///     设置页面打印Margin
        /// </summary>
        /// <param name="doc">PdfDocument打印机文档对象</param>
        /// <param name="top">上边距</param>
        /// <param name="left">左边距</param>
        /// <param name="paperType">纸张类型</param>
        /// <param name="invoiceName">偏移后PDF文件的名称</param>
        /// <returns>偏移后的pdf文档全路径</returns>
        private static string ResetPageMargin(PdfDocument doc,
            float top = 0,
            float left = 0,
            SourceType paperType = SourceType.Invoice,
            string invoiceName = "")
        {
            //实例化PdfDocument类，并加载测试文档
            var pdfMargin = doc;

            //另新建一个PDF文档
            var pdfResult = new PdfDocument();

            var margin = MillimeterToPixel(left, top);

            //遍历文档pdf1中的所有页面
            foreach (PdfPageBase page in pdfMargin.Pages)
            {
                Size paperSize = TransferPaperSize(paperType);

                //打印机单位尺寸 每百分之一英寸(0.254mm)
                var pargeSize = new PaperSize("发票纸张", paperSize.Width, paperSize.Height);

                //转mm长宽尺寸
                var sizemm = new SizeF(pargeSize.Width * 0.254f, pargeSize.Height * 0.254f);

                //纸张大小改为像素
                var size = MillimeterToPixel(sizemm.Width, sizemm.Height); //固定每英寸 72DPI

                var newPage = pdfResult.Pages.Add(size, new PdfMargins(margin.Width, margin.Height));

                //将原pdfMargin中内容写入新页面
                var pdfTemplate = page.CreateTemplate();

                pdfTemplate.Draw(newPage.Canvas);
            }

            //保存新的PDF文档
            if (string.IsNullOrEmpty(invoiceName))
            {
                invoiceName = Guid.NewGuid().ToString("N").ToLower();
            }

            var strDirectoryName = DateTime.Now.ToString("yyyyMMdd");
            var pdfDirectoryPath =
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfinvoicetemp", strDirectoryName);

            var newPdfPath = Path.Combine(pdfDirectoryPath,
                invoiceName + (paperType==SourceType.SalesList?"_sales":"") + string.Format("_margin_tp{0}&lf{1}.pdf", top, left));

            try
            {
                if (!Directory.Exists(pdfDirectoryPath))
                {
                    CreateDirectory(pdfDirectoryPath); //检测pdf发票文件目录
                }

                pdfResult.SaveToFile(newPdfPath, FileFormat.PDF);

                return newPdfPath;
            }
            catch (Exception ex)
            {
                throw new Exception("发票打印边距偏移量设置失败!", ex);
            }
        }

        /// <summary>
        /// 转百分之一英寸
        /// 等于0.254mm
        /// </summary>
        /// <param name="paperType"></param>
        /// <returns></returns>
        private static Size TransferPaperSize(SourceType paperType)
        {
            switch (paperType)
            {
                case SourceType.Invoice: //21.59*13.97 cm
                    return new Size(850,550);
                case SourceType.SalesList:  //21.00 * 29.7 cm
                    return new Size(827,1169);
                default:
                    return new Size(827, 1169);
            }
        }

        /// <summary>
        ///     毫米转换为像素点数量
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        private static SizeF MillimeterToPixel(float left = 0f,
            float top = 0f)
        {
            if (left == 0 && top == 0)
            {
                return new SizeF(0f, 0f);
            }

            var dpiX = 0f;
            var dpiY = 0f;
            float pixelX = 0;
            float pixelY = 0;
            const float inch = 25.4f; //一英寸等于25.4mm

            //using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            //{
            //    dpiX = graphics.DpiX;
            //    dpiY = graphics.DpiY;
            //}

            dpiX = 72f;
            dpiY = 72f;

            //象素数 / DPI = 英寸数
            //英寸数 * 25.4 = 毫米数
            pixelX = left / inch * dpiX;
            pixelY = top / inch * dpiY;

            return new SizeF(pixelX, pixelY);
        }

        /// <summary>
        ///     Http下载Pdf文件
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pdfSavePath">下载文件的存储路径</param>
        /// <param name="invoiceName">可自定义发票Pdf文件名称</param>
        /// <returns>返回PDF文件流</returns>
        public static Stream PdfDownloadFromHttp(string url,
            out string pdfSavePath,
            string invoiceName = "")
        {
            var memoryStream = new MemoryStream();
            pdfSavePath = "";
            if (string.IsNullOrEmpty(invoiceName))
            {
                invoiceName = url.Substring(url.LastIndexOf(@"/") + 1);
            }

            if (!invoiceName.ToLower().EndsWith(".pdf"))
            {
                invoiceName = invoiceName + ".pdf";
            }

            var strDirectoryName = DateTime.Now.ToString("yyyyMMdd");
            var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "pdfinvoicetemp",
                strDirectoryName,
                invoiceName);

            var tempDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pdfinvoicetemp", strDirectoryName);
            try
            {
                if (!Directory.Exists(tempDirectory))
                {
                    CreateDirectory(tempDirectory); //创建pdf发票文件目录
                }

                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath); //存在相同文件则删除
                }

                FileStream fs = null;
                Stream responseStream = null;
                try
                {
                    // 设置参数
                    var request = WebRequest.Create(url) as HttpWebRequest;

                    // 设置请求超时时间为5s
                    request.Timeout = 5000;

                    // 发送请求并获取相应回应数据
                    var response = request.GetResponse() as HttpWebResponse;

                    // 直到request.GetResponse()程序才开始向目标网页发送Post请求
                    responseStream = response.GetResponseStream();

                    // 创建PDF文件
                    fs = new FileStream(tempPath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);

                    // 创建本地文件写入流
                    var bArr = new byte[1024];
                    var actual = responseStream.Read(bArr, 0, bArr.Length);
                    while (actual > 0)
                    {
                        fs.Write(bArr, 0, actual);
                        memoryStream.Write(bArr, 0, actual);
                        actual = responseStream.Read(bArr, 0, bArr.Length);
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
                    {
                        fs.Close();
                    }

                    if (responseStream != null)
                    {
                        responseStream.Close();
                    }
                }
            }
            catch
            {
                return memoryStream;
            }
        }

        /// <summary>
        ///     递归创建文件夹
        /// </summary>
        /// <param name="fileFullPath">文件夹全路径</param>
        /// <returns></returns>
        public static bool CreateDirectory(string fileFullPath)
        {
            if (Directory.Exists(fileFullPath))
            {
                return true;
            }

            var tmpPath = fileFullPath.Substring(0, fileFullPath.LastIndexOf('\\'));
            if (CreateDirectory(tmpPath))
            {
                Directory.CreateDirectory(fileFullPath);

                return true;
            }

            return false;
        }
    }
}
