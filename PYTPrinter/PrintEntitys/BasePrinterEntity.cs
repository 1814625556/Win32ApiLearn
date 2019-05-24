using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;

namespace PYTPrinter.PrintEntitys
{
    /// <summary>
    /// 打印基类
    /// </summary>
    public class BasePrinter
    {
        public string Title { get; set; }
        public string DeskNo { get; set; }
        public DateTime BeginAt { get; set; }
        public decimal FinalMoney { get; set; }
        public string ExtraStr { get; set; }

        public static readonly Font LargerFont = new Font("宋体", 14);
        public static readonly Font DefaultFont = new Font("宋体", 8);
        public static readonly Font SmallFont = new Font("宋体", 5);
        public virtual PrintBaseEntity GenTitle(string title, Font font)
        {
            return new TitlePrint { Title = title, Font = font };
        }

        public virtual PrintBaseEntity GenDashLine()
        {
            return new LinePrint() { Margin = new Margins(0, 0, 3, 3) };
        }

        public virtual PrintBaseEntity GenRow(string name, string value, Font font = null, double splitPer = 0.45)
        {
            var rf = font ?? DefaultFont;
            var rowPrint = new RowPrint { Name = name, Value = value, SeperatPercent = splitPer, Font = rf };
            return rowPrint;
        }

        public virtual PrintBaseEntity GenSolidLine()
        {
            return new SolidLinePrint() { Margin = new Margins(0, 0, 3, 3) };
        }

        public virtual IList<PrintBaseEntity> GetPrintModel(BasePrinter pt)
        {
            return new List<PrintBaseEntity>()
            {
                GenTitle(pt.Title, LargerFont),
                GenDashLine(),
                GenRow("消费金额：", pt.FinalMoney.ToString("C")),
                GenRow("开台时间", pt.BeginAt.ToString()),
                GenRow("桌台号", pt.DeskNo),
                GenRow("ExtraStr", pt.ExtraStr)
            };
        }
    }
}
