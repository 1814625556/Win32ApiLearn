using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace PYTPrinter.Helper
{
    public static class GraphicHelper
    {

        /// <summary>
        /// print key value pair 
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="font"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="width">页面最大宽度</param>
        /// <param name="x_40_percent">分割线的横坐标</param>
        /// <param name="yIndex"></param>
        /// <returns>返回较大的行高</returns>
        public static int DrawKeyValue(this Graphics graphic, Font font, string key, string value, int width, int x_35_percent, int x_40_percent, int yIndex)
        {
            if (string.IsNullOrEmpty(value))
                return 0;
            var heightLeft = graphic.DrawStringWrap(font, key, new Rectangle(0, yIndex, x_35_percent, 1), new StringFormat() { Alignment = StringAlignment.Near });
            var heightRight = graphic.DrawStringWrap(font, value, new Rectangle(x_40_percent, yIndex, width - x_40_percent, 1), new StringFormat() { Alignment = StringAlignment.Far });
            return Math.Max(heightLeft, heightRight);
        }

        /// <summary>
        /// 画出单行的字符串/扩展方法
        /// </summary>
        /// <param name="graphic">画图graphic</param>
        /// <param name="font">字体</param>
        /// <param name="text">需打印的字符串</param>
        /// <param name="recangle">打印范围（left，top，width，height(不重要，因为会重新计算高度)）</param>
        /// <param name="sf">string format（align）</param>
        /// <returns>行高</returns>
        public static int DrawSingleString(this Graphics graphic, Font font, string text, Rectangle recangle, StringFormat sf)
        {
            var rowHeight = (int)(Math.Ceiling(graphic.MeasureString("测试", font).Height));
            var fontRectanle = new Rectangle(recangle.Left, recangle.Top, recangle.Width, rowHeight);
            graphic.DrawString(text, font, new SolidBrush(Color.Black), fontRectanle, sf);
            return rowHeight;
        }

        /// <summary>
        /// 绘制文本自动换行（超出截断）
        /// </summary>
        /// <param name=\"graphic\">绘图图面</param>
        /// <param name=\"font\">字体</param>
        /// <param name=\"text\">文本</param>
        /// <param name=\"recangle\">绘制范围</param>
        /// <param name="sf">string format（align）</param>
        /// <returns>行高</returns>
        public static int DrawStringWrap(this Graphics graphic, Font font, string text, Rectangle recangle, StringFormat sf)
        {
            var sfLeft = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near };
            if (string.IsNullOrEmpty(text))
                return 0;
            var textRows = GetStringRows(graphic, font, text, recangle.Width);
            var totalHeight = 0;
            var rowHeight = (int)(Math.Ceiling(graphic.MeasureString("测试", font).Height));
            for (var i = 0; i < textRows.Count; i++)
            {
                totalHeight += rowHeight;
                var fontRectanle = new Rectangle(recangle.Left, recangle.Top + rowHeight * i, recangle.Width, rowHeight);
                graphic.DrawString(textRows[i], font, new SolidBrush(Color.Black), fontRectanle, i == 0 ? sf : sfLeft);
            }
            return totalHeight;
        }

        private static List<string> GetStringRows(Graphics graphic, Font font, string text, int width)
        {
            var rowBeginIndex = 0;
            var textLength = text.Length;
            var textRows = new List<string>();
            for (var index = 0; index < textLength; index++)
            {
                var rowEndIndex = index;
                if (index == textLength - 1)
                    textRows.Add(text.Substring(rowBeginIndex));
                else if (rowEndIndex + 1 < text.Length && text.Substring(rowEndIndex, 2) == "\\r\\n")
                {
                    textRows.Add(text.Substring(rowBeginIndex, rowEndIndex - rowBeginIndex));
                    rowEndIndex = index += 2;
                    rowBeginIndex = rowEndIndex;
                }
                else if (graphic.MeasureString(text.Substring(rowBeginIndex, rowEndIndex - rowBeginIndex + 1), font).Width > width)
                {
                    textRows.Add(text.Substring(rowBeginIndex, rowEndIndex - rowBeginIndex));
                    rowBeginIndex = rowEndIndex;
                }
            }
            return textRows;
        }

    }
}
