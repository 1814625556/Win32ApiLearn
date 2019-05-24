using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
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


            //SubmitSoftwareTest();

            //RequestDaYin();

            JieMa();
            Console.ReadKey();
        }

        /// <summary>
        /// 360软件体检解码返回值
        /// </summary>
        static void JieMa()
        {
            var str = "{\"error\":0,\"message\":\"\u63d0\u4ea4\u6210\u529f\"}";
            var entity = JsonConvert.DeserializeObject<ResponseData>(str);
            var message = Uri.UnescapeDataString(entity.message);
        }


        /// <summary>
        /// 请求打印报文成功
        /// </summary>
        static void RequestDaYin()
        {
            var url = "http://fat-taxware-output-service-api.phoenix-t.xforceplus.com/global/taxware/v1/output/invoices/custom/print";
            var str = File.ReadAllText("dayin.txt");
            var result = HttpHelper.HttpPost(url, str);
        }


        /// <summary>
        /// 文件下载测试
        /// </summary>
        static void FileDownLoad()
        {
            WebClient client = new WebClient();
            client.DownloadFile(new Uri("http://imsc-dvlp-files.oss-cn-hangzhou.aliyuncs.com/file/contract/pdf/print/20190522/20190522174941115/40c8d57d-5fc8-4b15-85e2-a37222cc6c31.pdf"),

                $"{DateTime.Now.Ticks}.pdf");
        }
        /// <summary>
        /// 360软件提交--功能搞定
        /// </summary>
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

            var downloadStr =
                "http://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com" +
                "/file/client/un/xforceplus/%E7%A5%A8%E6%98%93%E9%80%9A%E5%8F%91%E7%A5%A8%E5%8A%A9%E6%89%8B_setup_3.2.218.0524.exe";
            NameValueCollection collection = new NameValueCollection();
            collection.Add("version", "3.2.218.0524");
            collection.Add("download", downloadStr);
            collection.Add("name", "发票助手");
            collection.Add("intro", "5.24");
            collection.Add("token", "0253a3c3ae95d7b65b82050774fbe9e8");
            collection.Add("stamp", "1558402841");

            //正确返回值：{"error":0,"message":"\u63d0\u4ea4\u6210\u529f"}
            var jsonStr = HttpHelper.HttpPostData("https://open.soft.360.cn/softpost.php?act=softadd", 3000 ,collection);


        }

        static void UrlBianMa()
        {
            var str = System.Web.HttpUtility.UrlDecode("https://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/%E7%A5%A8%E6%98%93%E9%80%9A%E5%8F%91%E7%A5%A8%E5%8A%A9%E6%89%8B_setup_3.2.210.0519.exe");
        }


    }

    /// <summary>
    /// 360软件提交返回报文
    /// </summary>
    class ResponseData
    {
        public string error { get; set; }
        public string message { get; set; }
    }
}
