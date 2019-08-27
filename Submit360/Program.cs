using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace Submit360
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[]{"http://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/Xforceplus-client_3.3.38.0722.exe"};
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.WriteLine("····请输入下载链接 (no download link)····");
                    Environment.Exit(1);
                    return;
                }

                //Console.WriteLine($"first:{args[0]}"+$"second:{args[1]}");

                var downLoadLink = args[0];
                var version = "";
                if (args.Length == 2)
                {
                    version = args[1];
                }
                if (string.IsNullOrEmpty(version))
                {
                    var reg = new Regex("setup_.*exe");
                    version = reg.Match(downLoadLink).ToString();
                }
                if (string.IsNullOrEmpty(version))
                {
                    var reg = new Regex("client_.*exe");
                    version = reg.Match(downLoadLink).ToString();
                }
                if (!downLoadLink.StartsWith("http") || string.IsNullOrEmpty(version))
                {
                    Console.WriteLine("····下载链接不合法 (download illegal)····");
                    Environment.Exit(1);
                    return;
                }

                var collection = new NameValueCollection();
                collection.Add("version", version);
                collection.Add("download", downLoadLink);
                collection.Add("name", "发票助手");
                collection.Add("intro", $"版本: {version}");
                collection.Add("token", "0253a3c3ae95d7b65b82050774fbe9e8");
                collection.Add("stamp", "1558402841");

                var jsonStr = HttpPostData("https://open.soft.360.cn/softpost.php?act=softadd", 3000, collection);

                ResponseM RM = JsonToObject<ResponseM>(jsonStr);
                var result = Uri.UnescapeDataString(RM.message);
                Console.WriteLine(result);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(1);
            }
           
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
            //webRequest.Headers.Add("Cookie",
            //    "Q=u%3D360H3081853709%26n%3D%26le%3Dq2ShM3W1nKScWGDjrTMipzAypTk1pl5wo20%3D%26m%3DZGt5WGWOWGWOWGWOWGWOWGWOZmHk%26qid%3D3081853709%26im%3D1_t0105d6cf9b508f72c8%26src%3Dpcw_renzheng%26t%3D1; " +
            //    "T=s%3D6c7235b27bc263e457362c59bec79aec%26t%3D1563851408%26lm%3D%26lf%3D1%26sk%3D00ad58aa49290f3012029d79ecaef110%26mt%3D1563851408%26rc%3D%26v%3D2.0%26a%3D1;");

            webRequest.Headers.Add("Cookie",
                "Q=u%3D360H3081853709%26n%3D%26le%3Dq2ShM3W1nKScWGDjrTMipzAypTk1pl5wo20%3D%26m%3DZGt5WGWOWGWOWGWOWGWOWGWOZmHk%26qid%3D3081853709%26im%3D1_t0105d6cf9b508f72c8%26src%3Dpcw_renzheng%26t%3D1; " +
                "T=s%3Dfdbd2d5596950061cda2e064a078405e%26t%3D1566452057%26lm%3D%26lf%3D1%26sk%3D7537a704535cf20114ffb885c43ec58c%26mt%3D1566452057%26rc%3D%26v%3D2.0%26a%3D1;");

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
