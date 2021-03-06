﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Windows.Automation;
using SearchBar;
using UIAutomationClient;
using TreeScope = System.Windows.Automation.TreeScope;

namespace User32Test
{
    class Program
    {
        class person
        {
            public string Name { get; set; }
        }
        /// <summary>
        /// 请输入检索关键字...
        /// </summary>
        /// <param name="args"></param>
        //[STAThread]
        static void Main(string[] args)
        {

            WpfTest.CommonControl();
            
            Console.ReadKey();
        }

        public static void TestGetEditValue()
        {
            int handle = 4851624;
            int txtLength = SendMessage(handle, WM_GETTEXTLENGTH, 0, 0);
            var sb = new StringBuilder(txtLength + 1);
            SendMessage(handle, WM_GETTEXT, sb.Capacity, sb);
            var result = sb.ToString();
        }

        // These are two Win32 constants that we'll need, they'll be explained later.
        const int WM_GETTEXT = 0x000D;

        const int WM_GETTEXTLENGTH = 0x000E;

        // The SendMessage function sends a Win32 message to the specified handle, it takes three
        // ints as parameters, the message to send, and to optional parameters (pass 0 if not required).
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int msg, int wParam, int lParam);

        // An overload of the SendMessage function, this time taking in a StringBuilder as the lParam.
        // Through the series we'll use a lot of different SendMessage overloads as SendMessage is one
        // of the most fundamental Win32 functions.
        [DllImport("User32.dll")]
        public static extern Int32 SendMessage(int hWnd, int msg, int wParam, StringBuilder lParam);

        /// <summary>
        /// 从剪切板获取截图--需要设置[STAThread]属性
        /// </summary>
        static void GetImagFromClipBoard()
        {
            try
            {
                Thread.Sleep(6000);
                WinApi.keybd_event(44, 0, 0, 0);
                var img = System.Windows.Forms.Clipboard.GetImage();

                //var text = Clipboard.GetText();
                if (img == null)
                {
                    Console.WriteLine("fail screen img is null..");
                    return;
                }
                img.Save($"{DateTime.Now:yyyyMMddhhmmss}.png");
                Console.WriteLine("success...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// 操作航信自定义报错弹框
        /// </summary>
        static void OperatorCustomer()
        {
            var custoBar = WinApi.FindWindow(null, "CusMessageBox");
            var editControl =
                UiaHelper.GetUIAutomation().ElementFromHandle(custoBar).FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants,
                    UiaHelper.GetUIAutomation().CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "rtbDescript"));

            var pt = (IUIAutomationValuePattern)editControl.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
            Console.WriteLine(pt.CurrentValue);
        }

        /// <summary>
        /// 获取显示器尺寸
        /// </summary>
        static void GetScreenSize()
        {
            var height = Screen.PrimaryScreen.Bounds.Height;
            var width = Screen.PrimaryScreen.Bounds.Width;
        }

        static void SearchInputFail()
        {
            WinApi.GetWindowRect(new HandleRef(null, (IntPtr)1901104), out var rectbefore);
            WinApi.MoveWindow((IntPtr)1901104, 10, 10, 3000, 2000, true);
            WinApi.GetWindowRect(new HandleRef(null, (IntPtr)1901104), out var rectbeafter);



            //Thread.Sleep(60000);
            //var formBar = WinApi.FindWindow(null, "票易通发票助手");
            var winBar = WinApi.FindWindow(null, "增值税发票税控开票软件（金税盘版） V2.2.34.190728");
            var child1s = WinApi.EnumChildWindowsCallback(winBar);
            var HmSearchBar = child1s.Find(b => b.szWindowName == "选择发票号码查询" || b.szTextName == "选择发票号码查询").hWnd;
            var childs = WinApi.FindChildInfo(HmSearchBar);
            var bar = childs.Find(b => b.szWindowName == "报送状态" || b.szTextName == "报送状态").hWnd;

            //WinApi.ClickLocation(bar, 5, 15);
            WinApi.ClickLocation(bar, 5, 15);

            Thread.Sleep(1000);

            //198352,
            var editControl = UiaHelper.GetUIAutomation().ElementFromHandle(winBar).FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants,
                UiaHelper.GetUIAutomation().CreatePropertyCondition(UIA_PropertyIds.UIA_AutomationIdPropertyId, "198352"));


            //var editControls = UiaHelper.GetUIAutomation().ElementFromHandle(winBar).FindAll(UIAutomationClient.TreeScope.TreeScope_Descendants,
            //    UiaHelper.GetUIAutomation().CreatePropertyCondition(
            //        UIA_PropertyIds.UIA_ControlTypePropertyId,UIA_ControlTypeIds.UIA_EditControlTypeId));

            //for (var i = 0; i < editControls.Length; i++)
            //{
            //    var element = editControls.GetElement(i);
            //    var pt = (IUIAutomationValuePattern)element.GetCurrentPattern(UIA_PatternIds.UIA_ValuePatternId);
            //    if (i == 0)
            //        pt.SetValue("chenchangchenchang");
            //    Console.WriteLine($"CurrentName:{element.CurrentName} ,currentValue:{pt.CurrentValue}");
            //}

            //var child = UiaHelper.GetUIAutomation().ElementFromHandle(winBar).FindAll(
            //    UIAutomationClient.TreeScope.TreeScope_Children,
            //    UiaHelper.GetUIAutomation().CreateTrueCondition());
            //for (var i = 0; i < child.Length; i++)
            //{
            //    Console.WriteLine(child.GetElement(i).CurrentName);
            //}


            //var elements = child.GetElement(1).FindAll(UIAutomationClient.TreeScope.TreeScope_Children,
            //    UiaHelper.GetUIAutomation().CreateTrueCondition());
            //for (var i = 0; i < elements.Length; i++)
            //{
            //    Console.WriteLine(elements.GetElement(i).CurrentName);
            //}

            //var elements = UiaHelper.GetUIAutomation().ElementFromHandle(bar).FindAll(
            //    UIAutomationClient.TreeScope.TreeScope_Descendants,
            //    UiaHelper.GetUIAutomation().CreateTrueCondition());
            //for (var i = 0; i < elements.Length; i++)
            //{
            //    var ele = elements.GetElement(i);
            //    Console.WriteLine(ele.CurrentName);
            //}


            //var element = UiaHelper.GetUIAutomation().ElementFromHandle(bar).FindFirst(
            //    UIAutomationClient.TreeScope.TreeScope_Children,
            //    UiaHelper.GetUIAutomation().CreateTrueCondition());
            //if (element != null)
            //{
            //    var pt = (IUIAutomationLegacyIAccessiblePattern) element.GetCurrentPattern(
            //        patternId: UIA_PatternIds.UIA_LegacyIAccessiblePatternId);
            //    pt.DoDefaultAction();
            //}
        }

        static void OperatorSys()
        {
            var winBar = WinApi.FindWindow(null, "SysMessageBox");
            var bars = WinApi.EnumChildWindowsCallback(winBar);
            for (var i = 0; i < bars.Count; i++)
            {
                Console.WriteLine($"bars[{i}].szWindowName:{bars[i].szWindowName},bars[i].szTextName:{bars[i].szTextName}");
            }
        }

        ///var img = CombinImage("20190918015322.png", "20190918020258.png");
        /// <summary>
        /// 调用此函数后使此两种图片合并，类似相册，有个
        /// 背景图，中间贴自己的目标图片
        /// </summary>
        /// <param name="sourceImg">粘贴的源图片</param>
        /// <param name="destImg">粘贴的目标图片</param>
        public static Image CombinImage(string sourceImg, string destImg)
        {
            Image imgBack = System.Drawing.Image.FromFile(sourceImg);     //相框图片  
            Image img = System.Drawing.Image.FromFile(destImg);        //照片图片

            //从指定的System.Drawing.Image创建新的System.Drawing.Graphics        
            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      // g.DrawImage(imgBack, 0, 0, 相框宽, 相框高); 
            //g.FillRectangle(System.Drawing.Brushes.Black, 16, 16, (int)112 + 2, ((int)73 + 2));//相片四周刷一层黑色边框
            
            //g.DrawImage(img, 照片与相框的左边距, 照片与相框的上边距, 照片宽, 照片高);
            g.DrawImage(img, 10, 10, img.Width, img.Width);
            GC.Collect();
            return imgBack;
        }
        

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="hwnd"></param>
        static void CapturePic(IntPtr hwnd)
        {
            var map = WinApi.GetWindowCapture(hwnd);
            map.Save($"{DateTime.Now:yyyyMMddhhmmss}.png");
        }

        static void Test()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税普通发票");
            var childs = WinApi.EnumChildWindowsCallback(winBar);
            var toolStrip = childs[childs.Count - 1].hWnd;
            ClickBtnByName(toolStrip, "红字");
        }

        /// <summary>
        /// 点击toolBar句柄下某个按钮--这个有可能点不中--需要优化
        /// </summary>
        /// <param name="toolBar"></param>
        /// <param name="name"></param>
        public static bool ClickBtnByName(IntPtr toolBar, string name)
        {
            if (toolBar == IntPtr.Zero)
                return false;
            //AmLogger.Info("ClickBtnByName", $"toolBar:{toolBar},name:{name}");
            try
            {
                var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(toolBar);
                var element = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Children, UiaHelper.GetUIAutomation().
                    CreatePropertyCondition(UIA_PropertyIds.UIA_NamePropertyId, name));
                Console.WriteLine($"{element.CurrentNativeWindowHandle}");
                var pattern = (IUIAutomationInvokePattern)element?.GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
                //var pattern = (IUIAutomationLegacyIAccessiblePattern)element?.GetCurrentPattern(UIA_PatternIds.UIA_LegacyIAccessiblePatternId);

                pattern?.Invoke();
                //pattern?.DoDefaultAction();
                Console.WriteLine("success....");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine($"error {e.Message}");
                //AmLogger.Error("ClickBtnUiaByName", $"{name},message:{e.Message},stacktrace:{e.StackTrace}");
            }
            return false;

        }

        public static void SetCombox()
        {
            try
            {
               var comBoxMation = UiaHelper.GetUIAutomation().ElementFromHandle((IntPtr)5571680);

                //WinApi.ClickLocation((IntPtr) 5571680, 50, 20);
                //Thread.Sleep(100);

                //var childs = comBoxMation.FindAll(UIAutomationClient.TreeScope.TreeScope_Descendants,
                //    UiaHelper.GetUIAutomation().CreateTrueCondition());
                //for (var i = 0; i < childs.Length; i++)
                //{
                //    Console.WriteLine(childs.GetElement(i).CurrentName);
                //}


                var selectItem = comBoxMation?.FindFirst(UIAutomationClient.TreeScope.TreeScope_Descendants,
                    UiaHelper.GetUIAutomation().CreatePropertyCondition(
                        UIA_PropertyIds.UIA_NamePropertyId, "9%"));

                var pattern = (IUIAutomationSelectionItemPattern)selectItem?.GetCurrentPattern(UIA_PatternIds.UIA_SelectionItemPatternId);
                if (pattern == null) return;

                pattern.Select();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static int GetHwByTitle(int loginHw, string title)
        {
            var listInfos = WinApi.EnumChildWindowsCallback((IntPtr)loginHw);
            try
            {
                for (var i = 0; i < listInfos.Count; i++)
                {

                    if (!string.IsNullOrEmpty(listInfos[i].szWindowName) && listInfos[i].szWindowName.Contains(title))
                        return (int)listInfos[i].hWnd;
                }
            }
            catch (Exception e)
            {
            }
            return 0;
        }

        static void ClickRedChar()
        {
            var winBar = WinApi.FindWindow(null, "开具增值税专用发票");
            var childs = WinApi.EnumChildWindowsCallback(winBar);
            var toolBar = childs[childs.Count - 1].hWnd;

            HxShengQing.ClickBtnByName(toolBar, "红字");
            Thread.Sleep(1000);

            var winBarUia = UiaHelper.GetUIAutomation().ElementFromHandle(winBar);
            var dropDown = winBarUia.FindFirst(UIAutomationClient.TreeScope.TreeScope_Children,
                UiaHelper.GetUIAutomation().CreatePropertyCondition(
                    UIA_PropertyIds.UIA_NamePropertyId, "DropDown"));

            Console.WriteLine(JsonConvert.SerializeObject(dropDown.CurrentBoundingRectangle));

            var dropChilds = dropDown.FindAll(UIAutomationClient.TreeScope.TreeScope_Children,
                UiaHelper.GetUIAutomation().CreateTrueCondition());

            Console.WriteLine(JsonConvert.SerializeObject(dropChilds.GetElement(2).CurrentBoundingRectangle));

            var pt = (IUIAutomationInvokePattern)dropChilds.GetElement(2).GetCurrentPattern(UIA_PatternIds.UIA_InvokePatternId);
            pt.Invoke();

            var toolChilds = AutomationElement.FromHandle(toolBar)
                .FindAll(TreeScope.Descendants, Condition.TrueCondition);
            for (var i = 0; i < toolChilds.Count; i++)
            {
                Console.WriteLine(toolChilds[i].Current.Name);
            }
        }

        public static void SetTab()
        {
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Tab, 0, 0, 0);
            Thread.Sleep(500);
        }
        /// <summary>
        /// 获取所有进程信息
        /// </summary>
        static void GetAllProcess()
        {
            //Get Processes
            Process[] processes = Process.GetProcesses();
            foreach (var pro in processes)
            {
                Console.WriteLine($"ProcessName:{pro.ProcessName}");
            }
        }

        /// <summary>
        /// 点击登录按钮--没有实现遮挡问题
        /// </summary>
        static void ClickLogin()
        {
            var loginWin = WinApi.FindWindow(null, "LoginForm");
            var list = WinApi.EnumChildWindowsCallback(loginWin);
            var loginBtn = list.Find(bar => bar.szWindowName == "登录");
            WinApi.LeftClick(loginBtn.hWnd);
        }

        static void GetAllDeskForm()
        {
            //var bar = WinApi.FindWindowEx(IntPtr.Zero,IntPtr.Zero, null,null);
            var list = WinApi.FindChildInfo((IntPtr)1442640);

            list.ForEach(i =>
            {
                if (i.szWindowName!=null)
                {
                    Console.WriteLine($"{i}:{i.szWindowName}");
                }
            });
        }

        //通过名称设置下拉框选项
        static void suilvwenti()
        {
            var bar = WinApi.FindWindow(null, "商品编码添加");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "*税率");
            var child1 = WinApi.FindWindowEx(bar, child, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1}");
            //通过索引设置下拉框选项
            WinApi.SetComboxItemValue(child1, "3%");
        }

        /// <summary>
        /// 专票红冲改动
        /// </summary>
        static void zhuanpiaohongchong()
        {
            //对专票备注进行赋值--貌似不能修改--需要确认
            var pageName = "开具增值税专用发票";
            var bar = WinApi.FindWindow(null, pageName);
            
            var list = WinApi.EnumChilWindowsIntptr(bar);

            List<IntPtr> list2 = new List<IntPtr>();
            for (var i = 2; i < list.Count; i++)
            {
                list2 = WinApi.FindChildBar((IntPtr)list[i]);
                if (list2?.Count >= 22)
                {
                    break;
                }
            }

            var temp = list2.Select(i => (int) i).ToList();

            WinApi.SendMessage(list2[6], 0x0C, IntPtr.Zero, "备注信息");

            var list3 = WinApi.FindChildBar(list2[19]);
            var list4 = WinApi.FindChildBar(list2[21]);

            foreach (IntPtr t in list3)
            {
                //银行名称账号
                WinApi.SendMessage(t, 12, IntPtr.Zero, $"银行账号9898989");
            }
            foreach (IntPtr t in list4)
            {
                //购方地址，电话
                var addressTel = $"购方地    址电话";
                WinApi.SendMessage(t, 12, IntPtr.Zero, addressTel);
            }
        }

        /// <summary>
        /// 最大最小化
        /// </summary>

        static void ShowWindow()
        {
            var winBar = WinApi.FindWindow(null, "Form1Text");
            var childs = WinApi.FindChildBar(winBar);
            WinApi.SendMessage(childs[childs.Count - 1], 0x0C, IntPtr.Zero, "chenchang");
            for (var i = 0; i < 1; i++)
            {
                WinApi.ShowWindow(winBar, 2); //最小
                Thread.Sleep(1000);
                WinApi.ShowWindow(winBar, 3); //最大
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 点击自动导入按钮--采用UI自动化方案--使用UI方案查找句柄--精确方法可以使用
        /// </summary>
        static void UIZiDongHua()
        {
            Thread.Sleep(2000);
            var bar = WinApi.FindWindow(null, "开具增值税专用发票");
            var list = WinApi.EnumChilWindowsIntptr(bar);

            var childBar = list[list.Count - 1];
            var parentAutomation = AutomationElement.FromHandle(childBar);
            var listAutomation = parentAutomation.
                FindAll(TreeScope.Children, Condition.TrueCondition);

            for (int i = 0; i < listAutomation.Count; i++)
            {
                if (listAutomation[i].Current.Name == "打印")
                {
                    object patternObject;
                    var pattern = listAutomation[i].TryGetCurrentPattern
                        (InvokePattern.Pattern, out patternObject);
                    ((InvokePattern)patternObject).Invoke();
                }
            }
            Thread.Sleep(10000);
        }



        /// <summary>
        /// 查找自动导入按钮
        /// </summary>
        static void ZiDongDaoRu()
        {
            var bar = WinApi.FindWindow(null, "开具增值税普通发票");
            var list = WinApi.EnumChilWindowsIntptr(bar);
            var childBar = list[list.Count - 1];
            RECT rect;
            var handler = new HandleRef(null, childBar);
            var flag = WinApi.GetWindowRect(handler, out rect);
            WinApi.ClickLocation(childBar, rect.right - rect.left - 690,25);
        }

        /// <summary>
        /// 专票地址信息填写
        /// </summary>
        static bool ZhuanPiaoInfo()
        {
            string pageName = "开具增值税专用发票";
            var bar = WinApi.FindWindow(null, pageName);
            if (bar == IntPtr.Zero)
            {
                return false;
            }

            var list = WinApi.EnumChilWindowsIntptr(bar).Select(ptr=>(int)ptr).ToList();


            //var grandparent = WinApi.GetParent((IntPtr) list[list.Count - 1]);
            //var parent = (int)WinApi.FindWindowEx(grandparent, IntPtr.Zero, null, null);

            //var parent2 = (int)WinApi.FindWindowEx((IntPtr)parent, IntPtr.Zero, null, null);

            var list2 = WinApi.FindChildBar((IntPtr) list[2]);

            var list3 = WinApi.FindChildBar(list2[19]);
            var list4 = WinApi.FindChildBar(list2[21]);

            for (var i = 0; i < list3.Count; i++)
            {
                WinApi.SendMessage(list3[i], 0x0C, IntPtr.Zero, "6217920170878354");

                StringBuilder sb = new StringBuilder();
                //获取文本
                WinApi.GetWindowTextW(list3[i], sb, 255);
                var str = sb.ToString();
            }
            for (var i = 0; i < list4.Count; i++)
            {
                WinApi.SendMessage(list4[i], 0x0C, IntPtr.Zero, "xinjiapo 15721527020");
            }

            var accountBar1 = WinApi.FindWindowEx((IntPtr) list2[list2.Count - 4], IntPtr.Zero, null, null);
            var accountBar2 = WinApi.FindWindowEx((IntPtr)list2[list2.Count - 4], accountBar1, null, null);
            //WinApi.SendMessage(accountBar, 0x0C, IntPtr.Zero, "6217720678878325");


            if (list == null || list.Count < 37)
            {
                return false;
            }

            WinApi.SendMessage((IntPtr)list[list.Count-5], 0x0C, IntPtr.Zero, "shanghai...");
            Thread.Sleep(500);
            //WinApi.SendMessage((IntPtr)list[37], 0x0C, IntPtr.Zero, "6217720678878325");
            WinApi.SendMessage((IntPtr)list[list.Count-9], 0x0C, IntPtr.Zero, "6217720678878325");
            return true;
        }

        /// <summary>
        /// 卷票折扣功能
        /// </summary>
        static void JuanPiaoZheKou()
        {
            Thread.Sleep(3000);

            var bar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "FPtiankai_new");
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);

            RECT rect;
            HandleRef ptrT = new HandleRef(null, child2);
            WinApi.GetWindowRect(ptrT, out rect);

            WinApi.ClickLocation(child2, rect.right - rect.left - 290, 25); //点击折扣


            Thread.Sleep(500);
            var bar1 = WinApi.FindWindow(null, "添加折扣行");
            var list = WinApi.EnumChilWindowsIntptr(bar1).Select(i => (int)i).ToList();
            if (list == null || list.Count < 7)
            {
                return;
            }



            WinApi.SendMessage((IntPtr)list[6], 0x0C, IntPtr.Zero, "2.5");

            //Thread.Sleep(1000);
            //WinApi.keybd_event(Keys.Tab, 0, 0, 0);
            //Thread.Sleep(1000);
            //WinApi.keybd_event(Keys.Tab, 0, 0, 0);
            //Thread.Sleep(1000);
            //WinApi.keybd_event(Keys.Tab, 0, 0, 0);
            //Thread.Sleep(1000);
            //WinApi.keybd_event(Keys.Enter, 0, 0, 0);

            Thread.Sleep(1000);


            WinApi.PostMessage((IntPtr)list[4], 0x00F5, 0, 0);

            //WinApi.LeftClick((IntPtr)list[4]);
            //WinApi.SendKey((IntPtr)list[4], KeySnap.VK_ENTER);
            //WinApi.ClickLocation((IntPtr) list[4],1,1);



            Console.ReadKey();
        }

        public static void leftClick(IntPtr hWnd1)
        {
            IntPtr hWnd = (IntPtr)hWnd1;
            if (hWnd != IntPtr.Zero)
            {
                WinApi.PostMessage(hWnd, 0x0201, 0, 0);
                WinApi.PostMessage(hWnd, 0x0202, 0, 0);
            }
        }

        /// <summary>
        /// 测试原生的sendKey方法
        /// </summary>
        static void TestYuanShengSendKey()
        {
            Thread.Sleep(5000);
            SendKeys.SendWait("{TAB}"); ;
            Thread.Sleep(100);

            SendKeys.SendWait(" ");
            SendKeys.SendWait("{BACKSPACE}");
            SendKeys.SendWait("陈昌测试状态");
            //JuanpiaoCeShi();
        }


        public static void Retry(Func<string,string> action, int count, int sleepMilliTimeout = 0)
        {
            if (count <= 0)
            {
                throw new Exception($"Retry: 参数count({count})必须为正数");
            }

            if (sleepMilliTimeout < 0)
            {
                throw new Exception($"Retry: 参数sleepMilliTimeout({sleepMilliTimeout})不可以为负数");
            }

            for (var i = 0; i < count; i++)
            {
                try
                {
                    Console.Write(action.Invoke("GOOD BEY"));
                    Thread.Sleep(sleepMilliTimeout);
                }
                catch (Exception e)
                {
                    if (sleepMilliTimeout > 0)
                    {
                        Thread.Sleep(sleepMilliTimeout);
                    }

                    if (i == count - 1)
                    {
                        throw;
                    }
                }
            }

            throw new Exception("Retry: 未正确执行");
        }

        /// <summary>
        /// 红冲专票-备注
        /// </summary>
        static void HongChongRemark()
        {
            var bar = WinApi.FindWindow(null, "开具增值税专用发票");
            var list = WinApi.EnumChilWindowsIntptr(bar);
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i] == (IntPtr)525010)
                    Console.WriteLine(i);
            }
            WinApi.SendMessage(list[11], 0x0C, IntPtr.Zero, "备注---chenchang---测试");
        }

        static void Change1(ref int age)
        {
            age = 18;
        }

        static void JuanPiaoCuoWuMingXi()
        {
            var bar = WinApi.FindWindow(null, "SysMessageBox");
            if (bar != IntPtr.Zero)
            {
                var list = WinApi.EnumChildWindowsCallback((IntPtr)bar);
                throw new Exception(list[4].szWindowName);
            }
        }

        /// <summary>
        /// 操作税收分类编码
        /// </summary>
        static void CaoZuoShuiShouFenLeiBianMa()
        {
            //int goodNoSettingHw = (int)WinApi.FindWindow(null, "税收分类编码设置");

            var bar = WinApi.FindWindow(null, "SysMessageBox");
            var child = (int)WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            for (var i = 0; i < 2; i++)
            {
                child = (int)WinApi.FindWindowEx((IntPtr)child, IntPtr.Zero, null, null);
            }
            var child1 = (int)WinApi.FindWindowEx((IntPtr)child, IntPtr.Zero, null, null);
            var child2 = (int)WinApi.FindWindowEx((IntPtr)child, (IntPtr)child1, null, null);

            var child3 = (int)WinApi.FindWindowEx((IntPtr)child2, IntPtr.Zero, null, null);

            WinApi.LeftClick((IntPtr)child3);

            //var stripHw = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "toolStrip1");
            //Thread.Sleep(200);
            //WinApi.ClickLocation(stripHw, 40, 15); //点击保存


            //修改实际传入的税率
            //var child = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "*使用税率");
            //var suilvBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, child, null, null);

            ////通过索引设置下拉框选项
            //var index = 9;
            //Thread.Sleep(1000);
            //WinApi.SendMessage(suilvBar, 0x014E, (IntPtr)index, ""); //调整税率为传入税率

            //

            //var child = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "享受优惠政策");
            //var yhzcBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, child, null, null);
            //WinApi.LeftClick(yhzcBar);
            //WinApi.SendKey(yhzcBar, 0x26);
            //Thread.Sleep(30);
            //WinApi.SendKey(yhzcBar, 0x0D);

            //Thread.Sleep(300);

            //int index = 2;
            ////设置优惠政策内容
            //var ssflbmBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, IntPtr.Zero, null, "税收分类编码");
            //var yhlxBar = WinApi.FindWindowEx((IntPtr)goodNoSettingHw, ssflbmBar, null, null);
            //WinApi.LeftClick(yhlxBar);
            //for (var i = 0; i < index; i++)
            //{
            //    WinApi.SendKey(yhlxBar, 0x28);
            //    Thread.Sleep(300);
            //}
            //WinApi.SendKey(yhlxBar, 0x0D);

            //ReadFile();
        }

        /// <summary>
        /// 获取卷票明细中的错误
        /// </summary>
        static void JuanPiaomingxi()
        {
            var bar = WinApi.FindWindow(null, "SysMessageBox");
            IntPtr child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            for (var i = 0; i < 4; i++)
            {
                child = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            }
            StringBuilder sb = new StringBuilder();
            WinApi.GetWindowText(child, sb, 1024);
            Console.WriteLine(sb.ToString());
        }

        static void JuanPiaoLuoJi()
        {
            var bar = WinApi.FindWindow(null, "开具增值税普通发票(卷票)");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "FPtiankai_new");
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);

            RECT rect;
            HandleRef ptrT = new HandleRef(null, child2);
            WinApi.GetWindowRect(ptrT, out rect);

            for (var i = 0; i < 5; i++)
            {
                WinApi.ClickLocation(child2, rect.right - rect.left - 230, 25); //增加行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 230, 25); //增加行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 230, 25); //增加行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 230, 25); //增加行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 170, 25); //减少行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 170, 25); //减少行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 170, 25); //减少行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 170, 25); //减少行
                Thread.Sleep(1000);
            }


            //收款员测试
            var cc1 = (int)WinApi.FindWindowEx(child1, IntPtr.Zero, null, null);
            //var cc2 = (int)WinApi.FindWindowEx((IntPtr)cc1, IntPtr.Zero, null, null);
            var fpdm = (int)WinApi.FindWindowEx((IntPtr)cc1, IntPtr.Zero, null, "发票代码：");
            var cc3 = (int)WinApi.FindWindowEx((IntPtr)cc1, (IntPtr)fpdm, null, null);
            var cc4 = (int)WinApi.FindWindowEx((IntPtr)cc1, (IntPtr)cc3, null, null);
            var cc5 = (int)WinApi.FindWindowEx((IntPtr)cc4, IntPtr.Zero, null, null);

            WinApi.SendMessage((IntPtr)cc4, 0x0C, (IntPtr)0, "管理员"); //调整税率为传入税率
        }

        //文件读取
        static void ReadFile()
        {
            string result = "";
            StreamReader sr = new StreamReader("cc.txt");
            while (!sr.EndOfStream)
            {
                string str = sr.ReadLine();
                result += "\"" + str + "\",";
            }
            Console.WriteLine(result);
        }

        //点击button按钮获取 文本框,title------------需要测试获取文本信息
        static void GetControlTest()
        {
            var bar = WinApi.FindWindow(null, "发票号码确认");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "确认");
            //WinApi.LeftClick(child);
            //WinApi.SendMessage(child, 0x00F5, IntPtr.Zero, "");//按钮点击事件--亲测可行
            WinApi.PostMessage(child, 0x00F5, 5,5);//按钮点击事件
        }

        /// <summary>
        /// 操作规格型号
        /// </summary>
        static void GuiGeXingHao()
        {
            var spflbmBar = WinApi.FindWindow(null, "商品编码添加");
            var guiGebar = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "规格型号");
            var guiGeSelectBar = WinApi.FindWindowEx(spflbmBar, guiGebar, null, null);
            WinApi.ClickLocation(guiGeSelectBar, 10, 10);
            SendKeys.SendWait("{UP}");
            Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
            //int selected = WinApi.SendMessage(guiGeSelectBar, 0x014e, (IntPtr)0, null);

            Thread.Sleep(2000);

            var jianma = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "简码");
            var temp = WinApi.FindWindowEx(spflbmBar, jianma, null, null);
            temp = WinApi.FindWindowEx(spflbmBar, temp, null, null);
            temp = WinApi.FindWindowEx(spflbmBar, temp, null, null);
            //var selected1 = WinApi.SendMessage(temp, 0x014e, (IntPtr)1, null);
            WinApi.ClickLocation(temp, 10, 10);
            SendKeys.SendWait("{UP}");
            Thread.Sleep(1000);
            SendKeys.SendWait("{ENTER}");
        }

        //填写税收分类编码--填写text文本
        static void TianXieShuiShouFenLeiBianMa()
        {
            var spflbmBar = WinApi.FindWindow(null, "商品编码添加");
            var ssflName = WinApi.FindWindowEx(spflbmBar, IntPtr.Zero, null, "税收分类名称");
            var temp1 = WinApi.FindWindowEx(spflbmBar, ssflName, null, null);
            var temp2 = WinApi.FindWindowEx(temp1, IntPtr.Zero, null, null);
            var ssflBar = WinApi.FindWindowEx(temp1, temp2, null, null);                //获取税收分类编码句柄
            //Console.WriteLine($"spflbmBar:{spflbmBar},child:{child},child1:{child1},child2:{child2},child3:child3");
            WinApi.SendMessage(ssflBar, 0x0C, IntPtr.Zero, "110");
        }


        /// <summary>
        /// 根据索引选中下拉框
        /// </summary>
        static void SelectTaxDemo()
        {
            var bar = WinApi.FindWindow(null, "商品编码添加");
            var child = WinApi.FindWindowEx(bar,IntPtr.Zero,null, "*税率");
            var child1 = WinApi.FindWindowEx(bar, child, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1}");
            //通过索引设置下拉框选项
            int selected = WinApi.SendMessage(child1, 0x014e, (IntPtr)6, "");

        }

        /// <summary>
        /// 截图测试
        /// </summary>
        static void JieTuCeShi()
        {
            var bar = WinApi.FindWindow(null, "开具增值税普通发票");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);
            //Console.WriteLine($"bar:{bar},child:{child},child1:{child1},child2:{child2}");
            var map = WinApi.GetWindowCapture(child2);

            Color destColor1 = Color.FromArgb(156, 175, 51);//
            Color destColor2 = Color.FromArgb(240, 132, 60);//
            bool isNoTax = false;
            var X = 0;
            for (var i = map.Width - 350; i < map.Width - 310; i++)//310,350,10,40
            {
                for (var j = 11; j < 39; j++)
                {
                    var color = map.GetPixel(i, j);
                    if (color.Equals(destColor1))
                    {
                        Console.WriteLine($"no tax success,(x:{i},y:{j})");
                        isNoTax = true;
                        X = i;
                        break;
                    }
                    if (color.Equals(destColor2))
                    {
                        Console.WriteLine($"tax success,(x:{i},y:{j})");
                        isNoTax = false;
                        X = i;
                        break;
                    }
                }
            }
            if (!isNoTax)
                WinApi.ClickLocation(child2, X + 20, 25);//点击价格，不含税

            Console.WriteLine("=========================================================");
            //WinApi.ClickLocation(child2, X + 20, 25);


            for (var i = 0; i < map.Width; i++)
            {
                var pixel = map.GetPixel(i, 25);

                var r3 = (pixel.R - destColor1.R) / 256.0;
                var g3 = (pixel.G - destColor1.G) / 256.0;
                var b3 = (pixel.B - destColor1.B) / 256.0;

                var diff = Math.Sqrt(r3 * r3 + g3 * g3 + b3 * b3);
                if (diff <= 0.05)
                {
                    Console.WriteLine($"success,(x:{i},y:25)");
                }
            }

            //map.GetPixel();
            //map.Save($"{DateTime.Now.Ticks}.png");
        }

        static void SendMessageTest()
        {

        }
        /// <summary>
        /// 电票 添加 减少按钮
        /// </summary>
        static void SearchAddLess()
        {
            var bar = WinApi.FindWindow(null, "开具增值税电子普通发票");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1},child2:{child2}");
            RECT rect;
            HandleRef ptrT = new HandleRef(null, child2);
            WinApi.GetWindowRect(ptrT,out rect);
            for (var i = 0; i < 4; i++)
            {
                WinApi.ClickLocation(child2, rect.right-rect.left-195, 25);//减少行
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 195, 25);//减少
                Thread.Sleep(1000);
                WinApi.ClickLocation(child2, rect.right - rect.left - 250, 25);//减少
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 查找红字句柄
        /// </summary>
        static void SearchRedChar()
        {
            var bar = WinApi.FindWindow(null, "开具增值税专用发票");
            var child = WinApi.FindWindowEx(bar, IntPtr.Zero, null, null);
            var child1 = WinApi.FindWindowEx(child, IntPtr.Zero, null, null);
            var child2 = WinApi.FindWindowEx(child, child1, null, null);
            Console.WriteLine($"bar:{bar},child:{child},child1:{child1},child2:{child2}");
        }

        /// <summary>
        /// 精度测试
        /// </summary>
        /// <param name="number1"></param>
        /// <param name="number2"></param>
        /// <param name="precision"></param>
        /// <returns></returns>
        private static string GetAmountWithTax(string number1, string number2, int precision = 2)
        {
            decimal num1 = 0;
            decimal num2 = 0;
            try
            {
                num1 = Convert.ToDecimal(number1);
                num2 = Convert.ToDecimal(number2);

                var sum = num1 + num2;
                sum = Math.Round(sum, precision, MidpointRounding.AwayFromZero);
                return sum.ToString();
            }
            catch (Exception e)
            {
               
            }
            return "0";
        }


        #region 2019年5月13日发送键盘值
        static void SendKey()
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var btnBar = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "button1");
            var txtBar = WinApi.FindWindowEx(bar, btnBar, null,null);

            //WinApi.SendKey(txtBar, KeySnap.VK_CAPITAL);
            //Thread.Sleep(500);
            WinApi.SendKey(txtBar, KeySnap.VK_A);
            Thread.Sleep(1000);
            WinApi.SendKey(txtBar, KeySnap.VK_B);
            Thread.Sleep(1000);
            WinApi.SendKey(txtBar, KeySnap.VK_C);
        }
        /// <summary>
        /// 模拟键盘操作
        /// </summary>
        static void SimulationKeyBoard()
        {
            WinApi.keybd_event(Keys.Shift, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.A, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.B, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.C, 0, 0, 0);
            Thread.Sleep(500);
            WinApi.keybd_event(Keys.Shift, 0, 2, 0);
        }

        /// <summary>
        /// 模拟键盘组合键
        /// </summary>
        static void SimulationKeyBoard2()
        {
            Thread.Sleep(2000);

            WinApi.keybd_event(VBKEY.vbKeyShift, 0, 0, 0);

            for (var i = 0; i < 10; i++)
            {
                #region 测试通过
                //WinApi.keybd_event(VBKEY.vbKeyA, 0, 0, 0);
                //WinApi.keybd_event(VBKEY.vbKeyA, 0, 2, 0);

                //WinApi.keybd_event(VBKEY.vbKeyB, 0, 0, 0);
                //WinApi.keybd_event(VBKEY.vbKeyB, 0, 2, 0);

                //WinApi.keybd_event(VBKEY.vbKeyTab, 0, 0, 0);
                //WinApi.keybd_event(VBKEY.vbKeyTab, 0, 2, 0);
                #endregion


                Thread.Sleep(1000);

            }

            WinApi.keybd_event(VBKEY.vbKeyShift, 0, 2, 0);
        }
        /// <summary>
        /// 改变文本文件的内容
        /// </summary>
        static void ChangeText()
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var btnBar = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "button1");
            var txtBar = WinApi.FindWindowEx(bar, btnBar, null, null);
            WinApi.SendMessage(txtBar, 0x0C, IntPtr.Zero, "AKKJJCCBBDDEE"); //对文本框进行赋值
        }

        /// <summary>
        /// 尚未成功
        /// </summary>
        static void GetText()
        {
            var bar = WinApi.FindWindow(null, "Form1Text");
            var btnBar = WinApi.FindWindowEx(bar, IntPtr.Zero, null, "button1");
            var txtBar = WinApi.FindWindowEx(bar, btnBar, null, null);

            StringBuilder sb = new StringBuilder();
            WinApi.GetDlgItemText(bar, (int)txtBar, sb, 255);
            Console.WriteLine(sb.ToString());

            WinApi.GetWindowText(txtBar, sb, 255);//获取标题
            Console.WriteLine(sb.ToString());
        }
        #endregion
    }
}
