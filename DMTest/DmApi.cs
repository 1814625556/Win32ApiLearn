using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Dm;

namespace DMTest
{
    public class DmApi
    {
        /// <summary>
        /// 查找窗体
        /// </summary>
        /// <param name="hwclass"></param>
        /// <param name="hwtitle"></param>
        /// <returns></returns>
        public static int FindWindow(string hwclass, string hwtitle)
        {
            dmsoft dm = new dmsoft();
            return dm.FindWindow(hwclass, hwtitle);
        }

        /// <summary>
        /// 寻找窗体句柄，通过循环多次查找
        /// </summary>
        /// <param name="hwclass"></param>
        /// <param name="hwtitle"></param>
        /// <param name="cirNum"></param>
        /// <param name="sleepTime"></param>
        /// <returns></returns>
        public static int FindWindow(string hwclass, string hwtitle, int cirNum = 1, int sleepTime = 200)
        {
            int hw = 0;
            dmsoft dm = new dmsoft();
            for (int i = 0; i < cirNum; i++)
            {
                hw = dm.FindWindow(hwclass, hwtitle);

                if (hw != 0)
                {
                    break;
                }

                Thread.Sleep(sleepTime);
            }

            return hw;
        }

        public static int FindWindowEx(int parent, string hwclass, string hwtitle)
        {
            dmsoft dm = new dmsoft();
            return dm.FindWindowEx(parent, hwclass, hwtitle);
        }

        public static int FindWindowLike(int parents, string hwTitle)
        {
            dmsoft dm = new dmsoft();

            string[] strs = dm.EnumWindow(parents, hwTitle, null, 1).Split(',');
            int ptr = 0;
            try
            {
                ptr = int.Parse(strs[0]);
            }
            catch (Exception e)
            {
            }

            return ptr;
        }

        /// <summary>
        /// 根据标题取句柄
        /// </summary>
        /// <param name="loginHw"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static int GetHwByTitle(int loginHw, string title)
        {
            dmsoft dm = new dmsoft();
            string zsklHw = dm.EnumWindow(loginHw, "", "", 3);
            foreach (string i in zsklHw.Split(','))
            {
                string title1 = dm.GetWindowTitle((int.Parse(i)));
                if (title.Equals(title1))
                {
                    return int.Parse(i);
                }
            }

            return 0;
        }

        /// <summary>
        /// 获取窗口句柄的类名
        /// </summary>
        /// <param name="loginHw"></param>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static int GetHwByClass(int loginHw, string classname)
        {
            dmsoft dm = new dmsoft();
            string zsklHw = dm.EnumWindow(loginHw, "", "", 3);
            foreach (string i in zsklHw.Split(','))
            {
                string title1 = dm.GetWindowClass((int.Parse(i)));
                if (title1.Equals(classname))
                {
                    return int.Parse(i);
                }
            }

            return 0;
        }

        /// <summary>
        /// 设置窗体状态
        /// </summary>
        /// <param name="hw"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int SetWindowStatus(int hw, int flag)
        {
            dmsoft dm = new dmsoft();
            return dm.SetWindowState(hw, flag);
        }

        /// <summary>
        /// 根据下标取句柄
        /// </summary>
        /// <param name="loginHw"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetHwByIndex(int loginHw, int index)
        {
            dmsoft dm = new dmsoft();
            string zsklHw = dm.EnumWindow(loginHw, "", "", 3);
            for (int i = 0; i < zsklHw.Split(',').Length; i++)
            {
                string item = zsklHw.Split(',')[i];
                if (i == index)
                {
                    return int.Parse(item);
                }
            }

            return 0;
        }

        public static void UserLeftClick(int windowHw, int xleft, int yleft)
        {
            if (windowHw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            UserBindWindow(dm, windowHw);
            object x11 = 0;
            object x22 = 0;
            object y21 = 0;
            object y22 = 0;
            dm.GetWindowRect(windowHw, out x11, out y21, out x22, out y22);
            
            int width1 = (int)x22 - (int)x11; //窗口的宽度
            int height1 = (int)y22 - (int)y21; //窗口的高度
            //int res1 = dm.FindPic(0, 0, width1, height1, path + "\\img\\普票选择.bmp", "000000", 0.9, 0, out intX, out intY);
            //if (res1 == -1) {
            //    return "普票选择按钮定位失败";
            //}
            dm.MoveToEx((int)width1 + xleft, (int)yleft, 0, 0);
            dm.LeftClick();
            dm.ForceUnBindWindow(windowHw);

            
        }

        public static void LocationClickLeft(int windowHw, int xLeft, int yTop, int clickNum = 1)
        {
            if (windowHw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            object x11 = 0;
            object x22 = 0;
            object y21 = 0;
            object y22 = 0;
            int gwr = dm.GetWindowRect(windowHw, out x11, out y21, out x22, out y22);

            
            if (gwr == 0)
            {
                return;
            }

           
            int x = (int)x11 - xLeft;
            int y = (int)y21 + yTop;
            dm.MoveToEx(x, y, 0, 0);

            for (int i = 0; i < clickNum; i++)
            {
                
                dm.LeftClick();
            }
        }

        public static void LocationClickRight(int windowHw, int xRight, int yTop, int clickNum = 1)
        {
            if (windowHw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            object x11 = 0;
            object x22 = 0;
            object y21 = 0;
            object y22 = 0;
            int gwr = dm.GetWindowRect(windowHw, out x11, out y21, out x22, out y22);

            
            if (gwr == 0)
            {
                return;
            }

            
            int x = (int)x22 + xRight;
            int y = (int)y21 + yTop;
            dm.MoveToEx(x, y, 0, 0);
            for (int i = 0; i < clickNum; i++)
            {
                
                dm.LeftClick();
                Thread.Sleep(200);
            }
        }

        /// <summary>
        /// 获取句柄相对位置
        /// </summary>
        /// <param name="windowHw"></param>
        /// <param name="x11"></param>
        /// <param name="y21"></param>
        /// <param name="x22"></param>
        /// <param name="y22"></param>
        public static void GetWindowRect(int windowHw, out int ox11, out int oy21, out int ox22, out int oy22)
        {
            dmsoft dm = new dmsoft();
            object x11 = 0;
            object x22 = 0;
            object y21 = 0;
            object y22 = 0;

            dm.GetWindowRect(windowHw, out x11, out y21, out x22, out y22);

            ox11 = (int)x11;
            oy21 = (int)y21;
            ox22 = (int)x22;
            oy22 = (int)y22;
        }

        /// <summary>
        /// 按下键盘
        /// 40-down  38-up  37-left  39-right  17-ctrl  13-enter
        /// </summary>
        /// <param name="vk_code"></param>
        public static void KeyPress(int vk_code)
        {
            dmsoft dm = new dmsoft();
            int t = dm.KeyPress(vk_code);
        }

        public static void KeyDown(int vk_code)
        {
            dmsoft dm = new dmsoft();
            int t = dm.KeyDown(vk_code);
        }

        public static void SetWindowText(int hwnd, string text)
        {
            dmsoft dm = new dmsoft();
            dm.SetWindowText(hwnd, text);
        }

        public static void SendMessage(int windowHw, string msg)
        {
            if (windowHw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            int t = dm.SendString(windowHw, msg);
            
        }

        public static void UserLeftPress(int hw, int key)
        {
            if (hw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            UserBindWindow(dm, hw);
            dm.KeyPress(key);
            dm.ForceUnBindWindow(hw);
        }

        /// <summary>
        /// 按下鼠标左键
        /// </summary>
        /// <param name="hw"></param>
        public static void UserLeftClick(int hw)
        {
            if (hw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            UserBindWindow(dm, hw);
            int a = dm.LeftClick();
            dm.ForceUnBindWindow(hw);

            
        }

        /// <summary>
        /// 双击鼠标左键
        /// </summary>
        /// <param name="hw"></param>
        public static void UserDoubleLeftClick(int hw)
        {
            if (hw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            UserBindWindow(dm, hw);
            int a = dm.LeftDoubleClick();
            dm.ForceUnBindWindow(hw);

            
        }

        /// <summary>
        /// 获取窗体
        /// </summary>
        /// <param name="hw"></param>
        /// <param name="flag">0 : 获取父窗口  1 : 获取第一个儿子窗口 2 : 获取First 窗口 3 : 获取Last窗口 4 : 获取下一个窗口 5 : 获取上一个窗口 6 : 获取拥有者窗口  7 : 获取顶层窗口 </param>
        /// <returns></returns>
        public static int GetWindow(int hw, int flag)
        {
            if (hw == 0)
            {
                return 0;
            }

            dmsoft dm = new dmsoft();
            return dm.GetWindow(hw, flag);
        }

        /// <summary>
        /// 获取下一个窗体
        /// </summary>
        /// <param name="hw">窗体句柄</param>
        /// <param name="num">向下次数</param>
        /// <returns></returns>
        public static int GetNextWindow(int hw, int num)
        {
            if (hw == 0)
            {
                return 0;
            }

            dmsoft dm = new dmsoft();
            int hw2 = hw;
            for (int i = 0; i < num; i++)
            {
                hw2 = dm.GetWindow(hw2, 4);
            }

            return hw2;
        }

        /// <summary>
        /// 获取上一个窗体
        /// </summary>
        /// <param name="hw">窗体句柄</param>
        /// <param name="num">向上次数</param>
        /// <returns></returns>
        public static int GetLastWindow(int hw, int num)
        {
            if (hw == 0)
            {
                return 0;
            }

            dmsoft dm = new dmsoft();
            int hw2 = hw;
            for (int i = 0; i < num; i++)
            {
                hw2 = dm.GetWindow(hw2, 5);
            }

            return hw2;
        }

        public static string GetWindowTitle(int hw)
        {
            if (hw == 0)
            {
                return "";
            }

            dmsoft dm = new dmsoft();
            return dm.GetWindowTitle(hw);
        }

        public static string UserFindWordClick(int hw, string iconImage)
        {
            dmsoft dm = new dmsoft();
            UserBindWindow(dm, hw);
            object x1 = 0;
            object x2 = 0;
            object y1 = 0;
            object y2 = 0;
            dm.GetWindowRect(hw, out x1, out y1, out x2, out y2);
            int width = (int)x2 - (int)x1; //窗口的宽度
            int height = (int)y2 - (int)y1; //窗口的高度
            object intX = 0;
            object intY = 0;

           
            dm.SetDict(0, iconImage + ".txt");

            char[] iconImage1 = iconImage.ToCharArray();

            string s = "";
            foreach (char c in iconImage1)
            {
                s += c.ToString() + "|";
            }

            int res = dm.FindStrFast(0, 0, 1024, 768, s, "000000-000000|da8f39-da8f39", 0.8, out intX, out intY);
            if (res == -1)
            {
                dm.ForceUnBindWindow(hw);
                return "识别失败:" + iconImage;
            }

            dm.MoveTo((int)intX, (int)intY);
            //dm.LeftClick();
            dm.RightClick();
            dm.ForceUnBindWindow(hw);
            //dm.UnBindWindow();
            return "0000";
        }

        public static void UserBindWindow(dmsoft dm, int hw)
        {
            dm.BindWindowEx(hw, "dx2", "windows3", "windows", "dx.mouse.input.lock.api3", 0);
            //dm.BindWindow(hw, "dx2", "windows3", "windows", 0);
            //dm.BindWindow(hw, "dx2", "dx2", "windows", 0);
            //dm.BindWindowEx(hw, "dx2", "dx2", "windows", "", 0);
        }

        public static bool CopyFile(string sourFile, string destFile, int over)
        {
            dmsoft dm = new dmsoft();
            int result = dm.CopyFile(sourFile, destFile, over);

            return result == 1;
        }

        /// <summary>
        /// 移动鼠标并点击左键
        /// </summary>
        /// <param name="windowHw"></param>
        /// <param name="xLeft"></param>
        /// <param name="yTop"></param>
        /// <param name="screenShot"></param>
        public static void MoveAndClickLeft(int windowHw, int xLeft, int yTop, bool screenShot = false)
        {
            if (windowHw == 0)
            {
                return;
            }

            dmsoft dm = new dmsoft();
            object x11 = 0;
            object x22 = 0;
            object y21 = 0;
            object y22 = 0;
            int gwr = dm.GetWindowRect(windowHw, out x11, out y21, out x22, out y22);

            if (gwr == 0)
            {
                return;
            }

            int x = (int)x11 + xLeft;
            int y = (int)y21 + yTop;
            dm.MoveTo(x, y);

            if (screenShot)
            {
                
            }

            dm.LeftClick();
        }
    }
}
