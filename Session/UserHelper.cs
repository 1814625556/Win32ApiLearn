using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace Session
{
    public class UserHelper
    {
        private static readonly string _regWinLogonPath = 
            "SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Winlogon";
        /// <summary>
        /// 创建window当前用户账号
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="groupName"></param>
        /// <param name="canChangePwd"></param>
        /// <param name="pwdExpires"></param>
        static void CreateLocalWindowsAccount(string userName, string passWord, string displayName,
            string description, string groupName = "Administrators", 
            bool canChangePwd = false, bool pwdExpires = false)
        {
            var context = new PrincipalContext(ContextType.Machine);
            var user = new UserPrincipal(context);
            user.SetPassword(passWord);
            user.DisplayName = displayName;
            user.Name = userName;
            user.Description = description;
            user.UserCannotChangePassword = canChangePwd;
            user.PasswordNeverExpires = pwdExpires;
            user.Save();

            var group = GroupPrincipal.FindByIdentity(context, groupName);
            if (group == null)
                throw new ArgumentException($"Group:{groupName} not exist.");
            group.Members.Add(user);
            group.Save();
        }

        /// <summary>禁止Windows免密登录</summary>
        public static void DisableDefaultLogin()
        {
            RegistryKey subKey = Registry.LocalMachine.CreateSubKey(_regWinLogonPath);
            if (subKey == null)
                throw new ApplicationException("访问获取注册表:" + _regWinLogonPath + "失败。");
            using (subKey)
            {
                subKey.DeleteValue("DefaultUserName", false);
                subKey.DeleteValue("DefaultPassword", false);
                subKey.DeleteValue("AutoAdminLogon", false);
            }
        }

        /// <summary>设置Windows 免密登录</summary>
        /// <param name="userName">账户名称</param>
        /// <param name="password">账户密码</param>
        public static void EnableDefaultLogin(string userName, string password)
        {
            RegistryKey subKey = Registry.LocalMachine.CreateSubKey(_regWinLogonPath);
            if (subKey == null)
                throw new ApplicationException("访问获取注册表:" + _regWinLogonPath + "失败。");
            using (subKey)
            {
                subKey.SetValue("AutoAdminLogon", (object)"1");
                subKey.SetValue("DefaultUserName", (object)userName);
                subKey.SetValue("DefaultPassword", (object)password);
            }
        }

        /// <summary>设置程序开机启动_注册表形式</summary>
        /// <param name="path">需要开机启动的exe路径</param>
        /// <param name="keyName">注册表中键值名称</param>
        /// <param name="set">true设置开机启动，false取消开机启动</param>
        public static void StartupSet(string path, string keyName, bool set)
        {
            using (RegistryKey localMachine = Registry.LocalMachine)
            {
                RegistryKey subKey = localMachine.CreateSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                if (set)
                {
                    if (subKey == null)
                        return;
                    subKey.SetValue(keyName, (object)path);
                }
                else
                {
                    if ((subKey != null ? subKey.GetValue(keyName) : (object)null) == null)
                        return;
                    subKey.DeleteValue(keyName);
                }
            }
        }
    }
}
