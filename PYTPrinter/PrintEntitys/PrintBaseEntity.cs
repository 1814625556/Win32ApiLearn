using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using PYTPrinter.Helper;

namespace PYTPrinter.PrintEntitys
{
    public abstract class PrintBaseEntity
    {
        public int Width { get; set; }
        public int YIndex { get; set; }
        public Margins Margin { get; set; } = new Margins(0, 0, 0, 0);
        public abstract int Print(Graphics graphic);
    }
    public class TitlePrint : PrintBaseEntity
    {
        private Font font;
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        private StringFormat align = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        public StringFormat Align
        {
            get { return align; }
            set { align = value; }
        }

        public string Title { get; set; }

        public override int Print(Graphics graphic)
        {
            return graphic.DrawSingleString(Font, Title, new Rectangle(0, YIndex, Width, 1), Align);
        }
    }
    public class LinePrint : PrintBaseEntity
    {
        private DashStyle lineStyle = DashStyle.Dash;
        public DashStyle LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; }
        }

        public override int Print(Graphics graphic)
        {
            graphic.DrawLine(new Pen(Color.Black) { DashStyle = LineStyle }, 0, YIndex, Width, YIndex);
            return 2;
        }
    }
    public class SolidLinePrint : PrintBaseEntity
    {
        public DashStyle LineStyle { get; set; } = DashStyle.Solid;

        public override int Print(Graphics graphic)
        {
            graphic.DrawLine(new Pen(Color.Black) { DashStyle = LineStyle }, 0, YIndex, Width, YIndex);
            return 2;
        }
    }
    public class RowPrint : PrintBaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public double SeperatPercent { get; set; }

        private Font font;
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        public override int Print(Graphics graphic)
        {
            return graphic.DrawKeyValue(Font, Name, Value, Width, (int)(Width * (SeperatPercent - 0.05)), (int)(Width * SeperatPercent), YIndex);
        }
    }
}
