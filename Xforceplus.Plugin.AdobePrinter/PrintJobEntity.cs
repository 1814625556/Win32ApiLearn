using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xforceplus.Plugin.AdobePrinter
{
    public class PrintJobEntity
    {
        /// <summary>
        /// 打印资源路径
        /// </summary>
        public string PrintFilePath { get; set; }

        /// <summary>
        /// 打印任务名称
        /// </summary>
        public string PrintJobName { get; set; }

        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; } = "";

        /// <summary>
        /// 打印纸张类型
        /// </summary>
        public SourceType PrintPageType { get; set; } = SourceType.Invoice;

        /// <summary>
        /// 自定义打印左边距
        /// </summary>
        public string CustomLeftMargin { get; set; }
        
        /// <summary>
        /// 自定义打印上边距
        /// </summary>
        public string CustomTopMargin { get; set; }

        /// <summary>
        /// 自定义是否显示打印弹窗
        /// </summary>
        public bool CustomShowPrintDialogue { get; set; }

        /// <summary>
        /// 选择打印模式
        /// </summary>
        public PrinterMethod PrintMethod { get; set; } = PrinterMethod.Default;

        /// <summary>
        /// 打印是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; } = false;
    }

    public enum PrinterMethod
    {
        /// <summary>
        /// 强制AdobeReader打印
        /// </summary>
        ForceAdobeReader,

        /// <summary>
        /// 优先AdobeReader打印
        /// </summary>
        PriorAdobeReader,

        /// <summary>
        /// 普通打印
        /// </summary>
        Default
    }

    public enum SourceType
    {
        /// <summary>
        /// 发票
        /// </summary>
        Invoice,

        /// <summary>
        /// 销货清单
        /// </summary>
        SalesList,

        /// <summary>
        /// 未知
        /// </summary>
        UnKnow
    }
}
