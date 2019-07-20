using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Security.Cryptography.X509Certificates;

namespace SingCheckTool
{
    /// <summary>
    /// 检测程序帮助类
    /// </summary>
    public class CheckHelper
    {
        public static void CheckSingle(string path)
        {
            if(string.IsNullOrEmpty(path)) throw new Exception("打包程序路径为空");
            X509Certificate cert = X509Certificate.CreateFromSignedFile(path);
        }

        public static bool CheckSignture(string path)
        {
            try
            {
                X509Certificate cert = X509Certificate.CreateFromSignedFile(path);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}
