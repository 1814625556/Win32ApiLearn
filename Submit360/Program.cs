using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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
            //args = new string[]{"http://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/Xforceplus-client_ac_3.4.28.0828.exe" };
            //args = new string[]{ "https://imsc-prod-files.oss-cn-hangzhou.aliyuncs.com/file/client/un/xforceplus/发票助手_3.4.59.0925.c_ac.exe" };
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
                    var reg = new Regex("发票助手_.*exe");
                    version = reg.Match(downLoadLink).ToString();
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

                if (string.IsNullOrEmpty(version))
                {
                    version = "xxx";
                }

                if (!downLoadLink.Trim().StartsWith("http"))
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
                collection.Add("token", ConfigurationManager.AppSettings.Get("token"));
                collection.Add("stamp", ConfigurationManager.AppSettings.Get("stamp"));

                var jsonStr = HttpPostData("https://open.soft.360.cn/softpost.php?act=softadd", 3000, collection);

                ResponseM RM = JsonToObject<ResponseM>(jsonStr);
                var result = Uri.UnescapeDataString(RM.message);
                Console.WriteLine(result);
                if (result.Contains("失败"))
                {
                    Environment.Exit(1);
                }
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

            webRequest.Headers.Add("Cookie", ConfigurationManager.AppSettings.Get("cookie"));

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
            var scriptSerializer = new JavaScriptSerializer {MaxJsonLength = int.MaxValue};
            try
            {
                return scriptSerializer.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }

        public static string GetAppConfig(string strKey)
        {
            return ConfigurationManager.AppSettings.Get("cookie");
        }
    }
}
