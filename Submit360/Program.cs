using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Submit360
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("····请输入下载链接 (no download link)····");
                return;
            }

            var downLoadLink = args[0];
            Regex reg = new Regex("setup_.*exe");
            var version = reg.Match(downLoadLink);
            if (!downLoadLink.StartsWith("http") || string.IsNullOrEmpty(version.ToString()))
            {
                Console.WriteLine("····下载链接不合法 (download illegal)····");
                return;
            }

            NameValueCollection collection = new NameValueCollection();
            collection.Add("version", version.ToString());
            collection.Add("download", downLoadLink);
            collection.Add("name", "发票助手");
            collection.Add("intro", $"版本: {version.ToString()}");
            collection.Add("token", "0253a3c3ae95d7b65b82050774fbe9e8");
            collection.Add("stamp", "1558402841");

            var jsonStr = HttpPostData("https://open.soft.360.cn/softpost.php?act=softadd", 3000, collection);

            ResponseM RM = JsonToObject<ResponseM>(jsonStr);
            var result = Uri.UnescapeDataString(RM.message);
            Console.WriteLine(result);
        }

        static string HttpPostData(string url, int timeOut, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            webRequest.Accept = @"*/*";
            webRequest.Headers.Add("Origin", "https://open.soft.360.cn");
            webRequest.Referer = "https://open.soft.360.cn/softsubmit.php";

            //添加cookie
            webRequest.Headers.Add("Cookie",
                "__huid=11+2mjzz/D2tfboY+/IbfgY0fkykrxf5oVorvA7Mf6zwg=; __guid=188751474.613110906250569984.1556438827000.9910; td_cookie=18446744073108727213; __sid=182495224.233534884503921280.1558400354538.9795; PHPSESSID=9ccget68ra6mspffqia6femek7; __DC_gid=182495224.43104162.1557454522883.1558402679693.16; quCapStyle=3; quCryptCode=D6TqMQxFspmFikzWCG%252FTQctXRAvW2kg%252FSlA%252FcnMm0DvRCWmFufVn%252BChpbi9hiwWZm5nJ2K6Fh2M%253D; Q=u%3D360H3081853709%26n%3D%26le%3Dq2ShM3W1nKScWGDjrTMipzAypTk1pl5wo20%3D%26m%3DZGt5WGWOWGWOWGWOWGWOWGWOZmHk%26qid%3D3081853709%26im%3D1_t0105d6cf9b508f72c8%26src%3Dpcw_renzheng%26t%3D1; T=s%3Dd4b0c334aa69377b5ca8f668f81c2b3c%26t%3D1558402833%26lm%3D%26lf%3D1%26sk%3D600cd470f6232a220c446c869f051d05%26mt%3D1558402833%26rc%3D%26v%3D2.0%26a%3D1; monitor_count=10; __gid=182495224.43104162.1557454522883.1558402886519.86");

            // 边界符  
            var boundary = "----" + DateTime.Now.Ticks.ToString("x");
            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = $"multipart/form-data; boundary={boundary}";

            var contentLine = Encoding.ASCII.GetBytes("\r\n");

            memStream.Write(contentLine, 0, contentLine.Length);
            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            webRequest.ContentLength = memStream.Length;

            var requestStream = webRequest.GetRequestStream();

            memStream.Position = 0;
            var tempBuffer = new byte[memStream.Length];
            memStream.Read(tempBuffer, 0, tempBuffer.Length);
            memStream.Close();

            requestStream.Write(tempBuffer, 0, tempBuffer.Length);
            requestStream.Close();

            var httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            using (var httpStreamReader = new StreamReader(httpWebResponse.GetResponseStream(),
                                                            Encoding.GetEncoding("gb2312")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }

        /// <summary>
        /// 返回值
        /// </summary>
        class ResponseM
        {
            //{"error":0,"message":"\u63d0\u4ea4\u6210\u529f"}
            public string error { get; set; }

            public string message { get; set; }
        }

        public static string ObjectToJson(object obj)
        {
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            try
            {
                scriptSerializer.MaxJsonLength = int.MaxValue;
                return scriptSerializer.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }

        public static T JsonToObject<T>(string jsonText)
        {
            JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
            scriptSerializer.MaxJsonLength = int.MaxValue;
            try
            {
                return scriptSerializer.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }
    }
}
