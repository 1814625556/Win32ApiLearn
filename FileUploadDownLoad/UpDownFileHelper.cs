using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Linq;

namespace FileUploadDownLoad
{
    public class UpDownFileHelper
    {
        private static DateTime dt = DateTime.MinValue;
        public static string DownloadFile(string url,string localPath)
        {
            
            try
            {
                WebClient client = new WebClient();

                //Observable.FromEventPattern<System.Net.DownloadProgressChangedEventHandler, object, DownloadProgressChangedEventArgs>(
                //    h +=client.DownloadProgressChangedEventHandler,
                //    h -= client.DownloadProgressChangedEventHandler
                //    );

                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(OnDownloadProgressChanged);
                //绑定下载事件，以便于显示当前进度
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(OnDownloadFileCompleted);
                //绑定下载完成事件，以便于计算总进度
                client.DownloadFileAsync(new Uri(url), localPath);

                //client.DownloadFile(url, localPath);
                return localPath;
            }
            catch
            {
                return "";
            }
        }

        public static bool UploadFile(string url, string localPath)
        {
            try
            {
                WebClient client = new WebClient();
                var result = client.UploadFile(new Uri(url),localPath);
                return true;
            }
            catch (Exception e)
            {

            }

            return false;
        }

        private static void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //在网上看到有朋友这么来控制进度条，我觉得麻烦，毕竟有省事的为什么我要麻烦一番……
            //this.SetProcessBar(e.ProgressPercentage, (int)((nDownloadedTotal + e.BytesReceived) * 100 / total));
            var Text = "已下载" + e.BytesReceived + "字节/总计" + e.TotalBytesToReceive + "字节";//一个label框，用来显示当前下载的数据

            if ((DateTime.Now - dt).Seconds > 3)
            {
                dt = DateTime.Now;
                Console.WriteLine(Text);
            }
            
        }

        /// 
        /// 下载进程变更事件
        /// 
        ///

        ///

        private static void OnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("下载完成");
        }

        private static void test()
        {

        }
    }
}
