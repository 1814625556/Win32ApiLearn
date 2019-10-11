using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace Xforceplus.Plugin.AdobePrinter
{
    public class PrinterHelper
    {
        /// <summary>
        /// 全局打印左边距
        /// </summary>
        private static float MarginLeft { get; set; } = 0f;

        /// <summary>
        /// 全局打印上边距
        /// </summary>
        private static float MarginTop { get; set; } = 0f;

        /// <summary>
        /// Adobe可执行文件路径
        /// </summary>
        private static string AdobeReaderPath { get; set; } = "";

        /// <summary>
        /// 打印机当前的纸张类型 - Adobe Reader 打印模式
        /// </summary>
        private static SourceType AdobeSourceType { get; set; } = SourceType.UnKnow;

        /// <summary>
        /// 设置偏移量
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <returns></returns>
        public static bool SetPrinterMargion(float left,float top)
        {
            MarginLeft = left;
            MarginTop = top;
            return true;
        }

        /// <summary>
        /// 打印接口
        /// </summary>
        /// <param name="jobs"></param>
        /// <returns></returns>
        public static bool Printer(List<PrintJobEntity> jobs)
        {
            var blResult = false;
            var strPdfFile = "";
            foreach (PrintJobEntity jobEntity in jobs)
            {
                try
                {
                    AdobeReaderPath = CheckeAdobeReader(jobEntity.PrintMethod);

                    strPdfFile = CheckPrinterFileSource(jobEntity.PrintFilePath,jobEntity.PrintJobName);

                    SizeF marginSize = CheckMarginSize(jobEntity);

                    //获取默认打印机
                    if (string.IsNullOrEmpty(jobEntity.PrinterName))
                    {
                        jobEntity.PrinterName = PdfHelper.DefaultPrinter();
                    }

                    //控制打印机打印
                    if (String.IsNullOrEmpty(strPdfFile) || !File.Exists(strPdfFile))
                    {
                        throw new Exception("下载Pdf文件资源失败！");
                    }
                    else
                    {
                        var isStartAdobeMode = false;

                        isStartAdobeMode = !String.IsNullOrEmpty(AdobeReaderPath) && File.Exists(AdobeReaderPath);

                        if (isStartAdobeMode)
                        {
                            SetPaperSize(jobEntity.PrinterName, jobEntity.PrintPageType);
                        }

                        blResult = PdfHelper.QuietPrinter(PdfHelper.LoadPdf(strPdfFile),
                            marginSize.Height,
                            marginSize.Width,
                            jobEntity.PrintJobName,
                            jobEntity.PrinterName,
                            AdobeReaderPath,
                            jobEntity.PrintPageType,
                            isStartAdobeMode);

                        if (!blResult)
                        {
                            throw new Exception("自定义打印失败！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return blResult;
        }

        private static void SetPaperSize(string printerName,SourceType paperType)
        {
            string paperName = "";
            float paperWidthmm = 0f;
            float paperHeightmm = 0f;

            switch (paperType)
            {
                case SourceType.Invoice:
                    paperName = "发票纸";
                    paperWidthmm = 215.9f;
                    paperHeightmm = 139.7f;

                    break;
                case SourceType.SalesList:
                    paperName = "A4";
                    paperWidthmm = 210.0f;
                    paperHeightmm = 297.0f;

                    break;
                default:
                    paperName = "A4";
                    paperWidthmm = 210.0f;
                    paperHeightmm = 297.0f;

                    break;
            }
            // 检查打印机设置

            PaperSize ps = ApiPrinterHelper.GetPrintForm(printerName, paperName);
            if (ps == null)
            {
                ApiPrinterHelper.AddCustomPaperSize(printerName, paperName, paperWidthmm, paperHeightmm);
            }
            else
            {
                if (AdobeSourceType == paperType)
                {
                    return;
                }

                AdobeSourceType = paperType;
                PageSettings pageSettings = new PageSettings();
                pageSettings.PrinterSettings.PrinterName = printerName;
                if (pageSettings.PrinterSettings.DefaultPageSettings.PaperSize.PaperName != paperName)
                {
                    ApiPrinterHelper.SetCustomPaperSize(printerName, ps.PaperName);
                }
            }
        }

        private static SizeF CheckMarginSize(PrintJobEntity jobEntity)
        {
            if (String.IsNullOrEmpty(jobEntity.CustomLeftMargin) || String.IsNullOrEmpty(jobEntity.CustomTopMargin))
            {
                return new SizeF(MarginLeft,MarginTop);
            }

            try
            {
                return new SizeF(Single.Parse(jobEntity.CustomLeftMargin), Single.Parse(jobEntity.CustomTopMargin));
            }
            catch (Exception e)
            {
                throw new Exception("自定义打印边距的参数转换错误！", e);
            }
        }

        private static string CheckeAdobeReader(PrinterMethod printerMethod)
        {
            string adobeReaderPath = "";
            if (printerMethod == PrinterMethod.ForceAdobeReader ||
                printerMethod == PrinterMethod.PriorAdobeReader)
            {
                if (String.IsNullOrEmpty(adobeReaderPath))
                {
                    adobeReaderPath = Path.Combine(AdobePrinterHelper.GetAdobePath(), "AcroRd32.exe");
                }
                if (!String.IsNullOrEmpty(adobeReaderPath) && !File.Exists(adobeReaderPath))
                {
                    if (printerMethod == PrinterMethod.ForceAdobeReader)
                    {
                        throw new Exception("客户端未安装Adobe Reader,终止打印!");
                    }
                    else
                    {
                        //Adobe Reader Not Found
                        adobeReaderPath = "";
                    }
                }
            }

            return adobeReaderPath;
        }

        private static string CheckPrinterFileSource(string printFilePath,string fileName = "")
        {
            string strPdfFile = "";
            //校验PrintJobEntity实体 PrintFilePath属性地址的合法性
            if (String.IsNullOrEmpty(printFilePath))
            {
                throw new Exception($"自定义打印文件地址校验不通过.错误原因:文件访问地址为空!");
            }
            else
            {
                if (!printFilePath.ToLower().StartsWith(@"http://") &&
                    !printFilePath.ToLower().StartsWith(@"https://"))
                {
                    if (!File.Exists(printFilePath))
                    {
                        throw new Exception($"自定义打印文件地址校验不通过.错误原因:文件资源不存在-{printFilePath}!");
                    }
                    else
                    {
                        strPdfFile = printFilePath;
                    }
                }
                else
                {
                    PdfHelper.PdfDownloadFromHttp(printFilePath,
                        out strPdfFile,
                        fileName);
                }
            }

            return strPdfFile;
        }
    }
}
