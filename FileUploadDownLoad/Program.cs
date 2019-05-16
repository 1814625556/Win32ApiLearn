using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileUploadDownLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            //var url = "http://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/票易通发票助手_setup_3.2.195.0514.exe";
            //UpDownFileHelper.DownloadFile(url, "fpzs.exe");

            UpDownFileHelper.UploadFile("http://10.0.2.15/636934310189905255.png", "636934310189905255.png");
            Console.ReadKey();
        }
    }
}
