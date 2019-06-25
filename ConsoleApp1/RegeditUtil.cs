using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ConsoleApp1
{
    public static class RegeditUtil
    {
        /// <summary>
        /// 注册表枚举
        /// </summary>
        public enum Enums
        {
            CurrentUser,
            LocalMachine,
            ClassesRoot,
            Users,
            PerformanceData,
            CurrentConfig,
        };
        /// <summary>
        /// 从注册表获取相关值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <param name="regeditEnum"></param>
        /// <returns></returns>
        public static string GetValueByRegeditKey(string key,
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\fwkp.exe",
            Enums regeditEnum = Enums.LocalMachine)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key值为空");
            }

            RegistryKey keyExpr = null;

            switch (regeditEnum)
            {
                case Enums.CurrentUser:
                    keyExpr = Registry.CurrentUser.OpenSubKey(path,
                        false);
                    break;
                case Enums.LocalMachine:
                    keyExpr = Registry.LocalMachine.OpenSubKey(path,
                        false);
                    break;
                case Enums.ClassesRoot:
                    keyExpr = Registry.ClassesRoot.OpenSubKey(path,
                        false);
                    break;
                case Enums.Users:
                    keyExpr = Registry.Users.OpenSubKey(path,
                        false);
                    break;
                case Enums.PerformanceData:
                    keyExpr = Registry.PerformanceData.OpenSubKey(path,
                        false);
                    break;
                case Enums.CurrentConfig:
                    keyExpr = Registry.CurrentConfig.OpenSubKey(path,
                        false);
                    break;
                default:
                    break;
            }
            if (keyExpr == null)
            {
                throw new Exception("路径打开失败");
            }
            return keyExpr.GetValue(key).ToString();
        }
    }
}
