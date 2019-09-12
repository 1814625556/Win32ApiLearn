using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace UserOperator
{
    ///
    /// 计算机用户和组操作类
    ///

    public class UserAndGroupHelper
    {
        private static readonly string PATH = "WinNT://" + Environment.MachineName;
        ///

        /// 添加windows用户
        ///

        /// 用户名
        /// 密码
        /// 所属组
        /// 描述
        public static void AddUser(string username, string password, string group, string description)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry user = dir.Children.Add(username, "User")) //增加用户名
                {
                    user.Properties["FullName"].Add(username); //用户全称
                    user.Invoke("SetPassword", password); //用户密码
                    user.Invoke("Put", "Description", description);//用户详细描述
                    //user.Invoke("Put","PasswordExpired",1); //用户下次登录需更改密码
                    user.Invoke("Put", "UserFlags", 66049); //密码永不过期
                    //user.Invoke("Put", "UserFlags", 0x0040);//用户不能更改密码s
                    user.CommitChanges();//保存用户
                    using (DirectoryEntry grp = dir.Children.Find(group, "group"))
                    {
                        if (grp.Name != "")
                        {
                            grp.Invoke("Add", user.Path.ToString());//将用户添加到某组
                        }
                    }
                }
            }
        }
        ///

        /// 更改windows用户密码
        ///

        /// 用户名
        /// 新密码
        public static void UpdateUserPassword(string username, string newpassword)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry user = dir.Children.Find(username, "user"))
                {
                    user.Invoke("SetPassword", new object[] { newpassword });
                    user.CommitChanges();
                }
            }
        }
        ///

        /// 删除windows用户
        ///

        /// 用户名
        public static void RemoveUser(string username)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry user = dir.Children.Find(username, "User"))
                {
                    dir.Children.Remove(user);
                }
            }
        }
        ///

        /// 添加windows用户组
        ///

        /// 组名称
        /// 描述
        public static void AddGroup(string groupName, string description)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry group = dir.Children.Add(groupName, "group"))
                {
                    group.Invoke("Put", new object[] { "Description", description });
                    group.CommitChanges();
                }
            }
        }
        ///

        /// 删除windows用户组
        ///

        /// 组名称
        public static void RemoveGroup(string groupName)
        {
            using (DirectoryEntry dir = new DirectoryEntry(PATH))
            {
                using (DirectoryEntry group = dir.Children.Find(groupName, "Group"))
                {
                    dir.Children.Remove(group);
                }
            }
        }
    }
}
