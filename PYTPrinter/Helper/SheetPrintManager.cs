using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using PYTPrinter.PrintEntitys;

namespace PYTPrinter.Helper
{
    public class SheetPrintManager
    {
        public int YIndex { get; set; }
        public int Width { get; set; }
        public SheetPrintManager(int width, int yIndex)
        {
            YIndex = yIndex;
            Width = width;
        }

        public void Print(IList<PrintBaseEntity> prints, Graphics graphic)
        {
            foreach (var printItem in prints)
            {
                YIndex += printItem.Margin.Top;
                printItem.Width = Width;
                printItem.YIndex = YIndex;
                YIndex += printItem.Print(graphic);
                YIndex += printItem.Margin.Bottom;
            }
        }
    }
}
