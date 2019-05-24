using System;
using System.Collections.Generic;
using System.Drawing;

namespace PYTPrinter.PrintEntitys.PrinterBusiness
{
    public class PrintTest : BasePrinter
    {
        #region 派生类特有字段
        public string Row1 { get; set; }
        public string Row2 { get; set; }
        public string Row3 { get; set; }
        public string Row4 { get; set; }
        public string Row5 { get; set; }
        public string Row6 { get; set; }
        public string Row7 { get; set; }
        #endregion
        /// <summary>
        /// 每个派生类都要实现的打印方法
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public override IList<PrintBaseEntity> GetPrintModel(BasePrinter pt)
        {
            PrintTest entity = pt as PrintTest;
            return new List<PrintBaseEntity>()
            {
                GenTitle(entity.Title, BasePrinter.LargerFont),
                GenDashLine(),
                GenRow("Row1：", entity.Row1),
                GenRow("Row2：", entity.Row2),
                GenDashLine(),
                GenRow("Row3：", entity.Row3),
                GenRow("Row4：", entity.Row4),
                GenRow("Row5：", entity.Row5),
                GenRow("Row6：", entity.Row6),
                GenRow("Row7：",entity.Row7),
                GenRow("Row7：",entity.Row7),
                GenRow("DeskNo：",entity.DeskNo),
                GenRow("BeginAt：",entity.BeginAt.ToShortDateString()),
                GenRow("FinalMoney：",entity.FinalMoney.ToString())
            };
        }
        #region 继承父类
        public override PrintBaseEntity GenTitle(string title, Font font)
        {
            return base.GenTitle(title, font);
        }
        public override PrintBaseEntity GenRow(string name, string value, Font font = null, double splitPer = 0.45)
        {
            return base.GenRow(name, value, font, splitPer);
        }
        public override PrintBaseEntity GenDashLine()
        {
            return base.GenDashLine();
        }

      
        #endregion
    }
}
