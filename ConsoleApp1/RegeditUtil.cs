using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ConsoleApp1
{
    /// <summary>
    /// 注册表枚举
    /// </summary>
    public enum RegeditEnums
    {
        CurrentUser,
        LocalMachine,
        ClassesRoot,
        Users,
        PerformanceData,
        CurrentConfig,
    }
    public static class RegeditUtil
    {
        /// <summary>
        /// 从注册表获取相关值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="path"></param>
        /// <param name="regeditEnum"></param>
        /// <returns></returns>
        public static string GetValueByRegeditKey(string key,
            string path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\App Paths\\fwkp.exe",
            RegeditEnums regeditEnum = RegeditEnums.LocalMachine)
        {

            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key值为空");
            }

            RegistryKey keyExpr = null;

            switch (regeditEnum)
            {
                case RegeditEnums.CurrentUser:
                    keyExpr = Registry.CurrentUser.OpenSubKey(path,
                        false);
                    break;
                case RegeditEnums.LocalMachine:
                    keyExpr = Registry.LocalMachine.OpenSubKey(path,
                        false);
                    break;
                case RegeditEnums.ClassesRoot:
                    keyExpr = Registry.ClassesRoot.OpenSubKey(path,
                        false);
                    break;
                case RegeditEnums.Users:
                    keyExpr = Registry.Users.OpenSubKey(path,
                        false);
                    break;
                case RegeditEnums.PerformanceData:
                    keyExpr = Registry.PerformanceData.OpenSubKey(path,
                        false);
                    break;
                case RegeditEnums.CurrentConfig:
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
