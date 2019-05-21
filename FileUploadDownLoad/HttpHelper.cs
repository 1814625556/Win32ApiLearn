using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileUploadDownLoad
{
    class HttpHelper
    {
        public static string HttpPost(string Url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(Url);

            request.Headers.Add("Cookie",
                "__huid=11+2mjzz/D2tfboY+/IbfgY0fkykrxf5oVorvA7Mf6zwg=; __guid=188751474.613110906250569984.1556438827000.9910; td_cookie=18446744073108727213; __sid=182495224.233534884503921280.1558400354538.9795; PHPSESSID=9ccget68ra6mspffqia6femek7; __DC_gid=182495224.43104162.1557454522883.1558402679693.16; quCapStyle=3; quCryptCode=D6TqMQxFspmFikzWCG%252FTQctXRAvW2kg%252FSlA%252FcnMm0DvRCWmFufVn%252BChpbi9hiwWZm5nJ2K6Fh2M%253D; Q=u%3D360H3081853709%26n%3D%26le%3Dq2ShM3W1nKScWGDjrTMipzAypTk1pl5wo20%3D%26m%3DZGt5WGWOWGWOWGWOWGWOWGWOZmHk%26qid%3D3081853709%26im%3D1_t0105d6cf9b508f72c8%26src%3Dpcw_renzheng%26t%3D1; T=s%3Dd4b0c334aa69377b5ca8f668f81c2b3c%26t%3D1558402833%26lm%3D%26lf%3D1%26sk%3D600cd470f6232a220c446c869f051d05%26mt%3D1558402833%26rc%3D%26v%3D2.0%26a%3D1; monitor_count=10; __gid=182495224.43104162.1557454522883.1558402886519.86");
            request.Method = "POST";
            request.ContentType = "text/html; charset=utf-8";
            byte[] data = Encoding.UTF8.GetBytes(postDataStr);
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            using (var response = (HttpWebResponse) request.GetResponse())
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            return "";
        }
        public static string HttpPostData(string url, int timeOut, NameValueCollection stringDict)
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
                //var str = HttpUtility.UrlEncode(responseContent);
                string result = Uri.UnescapeDataString(responseContent);//这里的解析只能对应单个 message里面的内容
            }

            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }

        static string HttpPostData(string url, int timeOut, string fileKeyName,
                                    string filePath, NameValueCollection stringDict)
        {
            string responseContent;
            var memStream = new MemoryStream();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            // 边界符  
            var boundary = "---------------" + DateTime.Now.Ticks.ToString("x");
            // 边界符  
            var beginBoundary = Encoding.ASCII.GetBytes("--" + boundary + "\r\n");
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // 最后的结束符  
            var endBoundary = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");

            // 设置属性  
            webRequest.Method = "POST";
            webRequest.Timeout = timeOut;
            webRequest.ContentType = "multipart/form-data; boundary=" + boundary;

            // 写入文件  
            const string filePartHeader =
                "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
                 "Content-Type: application/octet-stream\r\n\r\n";
            var header = string.Format(filePartHeader, fileKeyName, filePath);
            var headerbytes = Encoding.UTF8.GetBytes(header);

            memStream.Write(beginBoundary, 0, beginBoundary.Length);
            memStream.Write(headerbytes, 0, headerbytes.Length);

            var buffer = new byte[1024];
            int bytesRead; // =0  

            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                memStream.Write(buffer, 0, bytesRead);
            }
            var contentLine = Encoding.ASCII.GetBytes("\r\n"); memStream.Write(contentLine, 0, contentLine.Length);
            // 写入字符串的Key  
            var stringKeyHeader = "\r\n--" + boundary +
                                   "\r\nContent-Disposition: form-data; name=\"{0}\"" +
                                   "\r\n\r\n{1}\r\n";

            foreach (byte[] formitembytes in from string key in stringDict.Keys
                                             select string.Format(stringKeyHeader, key, stringDict[key])
                                                 into formitem
                                             select Encoding.UTF8.GetBytes(formitem))
            {
                memStream.Write(formitembytes, 0, formitembytes.Length);
            }

            // 写入最后的结束边界符  
            memStream.Write(endBoundary, 0, endBoundary.Length);

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
                                                            Encoding.GetEncoding("utf-8")))
            {
                responseContent = httpStreamReader.ReadToEnd();
            }

            fileStream.Close();
            httpWebResponse.Close();
            webRequest.Abort();

            return responseContent;
        }

    }
}
