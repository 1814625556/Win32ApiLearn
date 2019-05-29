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
            var pdf = args[0];
            Console.WriteLine(pdf);
            PDFPrinter.pdfPrint(pdf, "");
        }
    }
}
