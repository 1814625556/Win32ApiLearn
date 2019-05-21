using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FileUploadDownLoad.HttpEntity;
using Newtonsoft.Json;
using System.Web;

namespace FileUploadDownLoad
{
    class Program
    {
        static void Main(string[] args)
        {
            //

            //string content = "\u63d0\u4ea4\u6210\u529f";
            //string result = Uri.UnescapeDataString(content);

            SubmitSoftwareTest();
            Console.ReadKey();
        }

        static void SubmitSoftwareTest()
        {
            //var postData = new Upload360Entity()
            //{
            //    Version = "3.2.210.0520",
            //    Download = "https://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/%E7%A5%A8%E6%98%93%E9%80%9A%E5%8F%91%E7%A5%A8%E5%8A%A9%E6%89%8B_setup_3.2.210.0519.exe",
            //    Name = "发票助手",
            //    Intro = "5.20",
            //    Token = "0253a3c3ae95d7b65b82050774fbe9e8",
            //    Stamp = "1558402841",
            //    Publi = "0",
            //    Softmgr_id = "0",
            //    Push2softmgr = "0"
            //};

            //var result = JsonConvert.SerializeObject(postData);
            //var jsonStr = HttpHelper.HttpPost("https://open.soft.360.cn/softpost.php?act=softadd", 
            //    JsonConvert.SerializeObject(postData));

            NameValueCollection collection = new NameValueCollection();
            collection.Add("version", "3.2.210.0521");
            collection.Add("download", "https://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/%E7%A5%A8%E6%98%93%E9%80%9A%E5%8F%91%E7%A5%A8%E5%8A%A9%E6%89%8B_setup_3.2.210.0519.exe");
            collection.Add("name", "发票助手");
            collection.Add("intro", "5.21");
            collection.Add("token", "0253a3c3ae95d7b65b82050774fbe9e8");
            collection.Add("stamp", "1558402841");
            var jsonStr = HttpHelper.HttpPostData("https://open.soft.360.cn/softpost.php?act=softadd", 3000 ,collection);
        }

        static void UrlBianMa()
        {
            var str = System.Web.HttpUtility.UrlDecode("https://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/%E7%A5%A8%E6%98%93%E9%80%9A%E5%8F%91%E7%A5%A8%E5%8A%A9%E6%89%8B_setup_3.2.210.0519.exe");
        }


    }
}
