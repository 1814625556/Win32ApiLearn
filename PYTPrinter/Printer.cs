using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Management;
using PYTPrinter.Helper;
using PYTPrinter.PrintEntitys;

namespace PYTPrinter
{
    public interface IPrinter
    {
        /// <summary>
        /// 打印方法
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="entity"></param>
        /// <param name="horiModel"></param>
        /// <returns></returns>
        Tuple<bool,Exception> Print(string printerName, BasePrinter entity, MarginModel horiModel=null);
        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <returns></returns>
        Tuple<IList<string>,Exception> GetPrinterList();
        /// <summary>
        /// 打印测试案例
        /// </summary>
        /// <param name="printName"></param>
        /// <returns></returns>
        Tuple<bool, Exception> PrintTest(string printName);
    }

    public class Printer : IPrinter , IDisposable
    {
        public Tuple<bool, Exception> Print(string printerName, BasePrinter entity, MarginModel horiModel = null)
        {
            var tuple = new Tuple<bool,Exception>(true,null);
            try
            {
                if (horiModel == null)
                    horiModel = new MarginModel() { LeftMargin = 0, RightMargin = 0 };
                PrintController pc = new StandardPrintController();
                var pd = new PrintDocument
                {
                    PrintController = pc,
                    DefaultPageSettings =
                    {
                        Margins = new Margins((int) (horiModel.LeftMargin / 25.4 * 100),
                            (int) (horiModel.RightMargin / 25.4 * 100), 0, 0)
                    },
                    OriginAtMargins = true
                };
                pd.PrintPage += (s, e) => Pdoc_PrintPage(e, entity);
                pd.PrinterSettings.PrinterName = printerName;
                pd.Print();
            }
            catch (Exception ex)
            {
                return new Tuple<bool, Exception>(false,ex);
            }
            return tuple;
        }

        public Tuple<IList<string>, Exception> GetPrinterList()
        {
            var lt = new List<string>();
            try
            {
                var scope = new ManagementScope(@"\root\cimv2");
                scope.Connect();
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                var allPrinterManagement = searcher.Get();

                foreach (var printer in allPrinterManagement)
                {
                    if (printer["WorkOffline"].ToString().ToLower().Equals("false"))
                        lt.Add(printer["Name"]?.ToString());
                }
            }
            catch (Exception ex)
            {
                return new Tuple<IList<string>, Exception>(null, new Exception("获取打印机列表出错", ex));
            }
            return new Tuple<IList<string>, Exception>(lt,null);
        }

        public Tuple<bool, Exception> PrintTest(string printerName)
        {
            var tuple = new Tuple<bool, Exception>(true, null);

            #region 昌总写的
            try
            {
                PrintController pc = new StandardPrintController();
                var pd = new PrintDocument
                {
                    PrintController = pc,
                    DefaultPageSettings =
                                {
                                    Margins = new Margins(0, 0, 0, 0)
                                },
                    OriginAtMargins = true
                };
                BasePrinter entity = new BasePrinter()
                {
                    Title = "测试",
                    DeskNo = "666",
                    BeginAt = DateTime.Now,
                    FinalMoney = Decimal.MaxValue,
                    ExtraStr = "这是打印测试"
                };
                pd.PrintPage += (s, e) => Pdoc_PrintPage(e, entity);
                pd.PrinterSettings.PrinterName = printerName;
                pd.Print();
            }
            catch (Exception ex)
            {
                return new Tuple<bool, Exception>(false, ex);
            }


            #endregion
            return tuple;
        }

        private void Pdoc_PrintPage(PrintPageEventArgs e, BasePrinter pt)
        {
            //e.PageBounds 纸张大小
            //e.PageSettings.PrintableArea 可打印边距、物理边距
            //e.MarginBounds 软件边距 
            var graphics = e.Graphics;
            var width = e.MarginBounds.Width;    //整个页面的大小 189 (1/100inch)
            var sheetPrintManager = new SheetPrintManager(width, 0);
            //_log.Trace($"visit id: {pt.Id} begin printing");
            sheetPrintManager.Print(pt.GetPrintModel(pt), graphics);
        }
        public void Dispose()
        {
        }
    }
}
