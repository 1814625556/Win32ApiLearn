using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HttpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //testGzip();

            var url = "http://fat-taxware-output-service-api.phoenix-t.xforceplus.com/test/taxware/v1/output/invoices/custom-print";
            var str = File.ReadAllText(@"jsons/print.json");
            HttpHelper.HttpPostZip(
                url,
                str);
        }

        static void testGzip()
        {
            var str = File.ReadAllText(@"jsons/print.json");
            var data = Encoding.UTF8.GetBytes(str);
            var zipData = HttpHelper.Compress(data);
            var resultData = HttpHelper.Decompress(zipData);
            var resultStr = resultData.ToString();
        }

        /// <summary>
        /// 测试成功
        /// </summary>
        static void GzipTest()
        {
            byte[] cbytes = null;
            //压缩
            using (MemoryStream cms = new MemoryStream())
            {
                using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(cms, System.IO.Compression.CompressionMode.Compress))
                {
                    //将数据写入基础流，同时会被压缩
                    var str = File.ReadAllText(@"jsons/print.json");
                    byte[] bytes = Encoding.UTF8.GetBytes(str);
                    Console.WriteLine($"bytes.Length:{bytes.Length}");
                    gzip.Write(bytes, 0, bytes.Length);
                }
                cbytes = cms.ToArray();
                Console.WriteLine($"cbytes.Length:{cbytes.Length}");
            }
            //解压
            using (MemoryStream dms = new MemoryStream())
            {
                using (MemoryStream cms = new MemoryStream(cbytes))
                {
                    using (System.IO.Compression.GZipStream gzip = new System.IO.Compression.GZipStream(cms, System.IO.Compression.CompressionMode.Decompress))
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
                Console.WriteLine(Encoding.UTF8.GetString(dms.ToArray()));
            }
        }
    }
}
