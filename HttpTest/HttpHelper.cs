using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;

namespace HttpTest
{
    public class HttpHelper
    {
        public static string HttpPost(string url, string postDataStr = null)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 5000;
                request.ContentType = "application/json";
                request.Accept = "*/*";

                var data = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return "";

                    var responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return responseStr;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public static string HttpPostZip(string url, string postDataStr)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = 10000;
                request.ContentType = "application/gzip";
                //request.Headers.Add("Content-Encoding", "gzip");
                request.Accept = "*/*";

                var data = Encoding.UTF8.GetBytes(postDataStr);
                var zipData = Compress(data);
                //var zipStr = Encoding.UTF8.GetString(zipData);
                request.ContentLength = zipData.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(zipData,0,zipData.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return "";

                    var responseStr = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    return responseStr;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        /// <summary>
        /// gzip压缩
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytes)
        {
            using (MemoryStream cms = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(cms, CompressionMode.Compress))
                {
                    gzip.Write(bytes, 0, bytes.Length);
                }
                return cms.ToArray();
            }
        }

        /// <summary>
        /// gzip解压
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] cbytes)
        {
            using (MemoryStream dms = new MemoryStream())
            {
                using (MemoryStream cms = new MemoryStream(cbytes))
                {
                    using (GZipStream gzip = new GZipStream(cms, CompressionMode.Decompress))
                    {
                        byte[] bytes = new byte[1024];
                        int len = 0;

                        //读取压缩流，同时会被解压
                        while ((len = gzip.Read(bytes, 0, bytes.Length)) > 0)
                        {
                            dms.Write(bytes, 0, len);
                        }
                    }
                }

                return dms.ToArray();
            }
        }
    }
}
