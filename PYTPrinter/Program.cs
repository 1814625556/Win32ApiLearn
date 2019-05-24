using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PYTPrinter
{
    class Program
    {
        //Microsoft XPS Document Writer
        static void Main(string[] args)
        {
            //var list = new Printer().GetPrinterList();
            PDFPrinter.pdfPrint(@"C:\Users\Admin\Desktop\cc.pdf");
        }
    }
}
