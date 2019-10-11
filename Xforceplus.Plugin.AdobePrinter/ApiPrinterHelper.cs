using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Xforceplus.Plugin.AdobePrinter
{
    internal class ApiPrinterHelper
    {
        private ApiPrinterHelper()
        {
        }

        #region API声明

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct structPrinterDefaults
        {
            [MarshalAs(UnmanagedType.LPTStr)]
            public String pDatatype;

            public IntPtr pDevMode;

            [MarshalAs(UnmanagedType.I4)]
            public int DesiredAccess;
        }

        [DllImport("winspool.Drv",
             EntryPoint = "OpenPrinter",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPTStr)] string printerName,
            out IntPtr phPrinter,
            ref structPrinterDefaults pd);

        [DllImport("winspool.Drv",
             EntryPoint = "ClosePrinter",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool ClosePrinter(IntPtr phPrinter);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct structSize
        {
            public Int32 width;

            public Int32 height;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct structRect
        {
            public Int32 left;

            public Int32 top;

            public Int32 right;

            public Int32 bottom;
        }

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        internal struct FormInfo1
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.I4)]
            public uint Flags;

            [FieldOffset(4), MarshalAs(UnmanagedType.LPWStr)]
            public String pName;

            [FieldOffset(8)]
            public structSize Size;

            [FieldOffset(16)]
            public structRect ImageableArea;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct structDevMode
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String dmDeviceName; //打印机（显示设备）名称

            [MarshalAs(UnmanagedType.U2)]
            public short dmSpecVersion;

            [MarshalAs(UnmanagedType.U2)]
            public short dmDriverVersion; //驱动版本号

            [MarshalAs(UnmanagedType.U2)]
            public short dmSize; //结构体大小

            [MarshalAs(UnmanagedType.U2)]
            public short dmDriverExtra;

            [MarshalAs(UnmanagedType.U4)]
            public int dmFields;

            [MarshalAs(UnmanagedType.I2)]
            public short dmOrientation; //DMORIENT_PORTRAIT (1) 纵向 or DMORIENT_LANDSCAPE (2) 横向

            [MarshalAs(UnmanagedType.I2)]
            public short dmPaperSize; //打印纸张类型  常用 A3:8,A4,9 ,详见下图

            [MarshalAs(UnmanagedType.I2)]
            public short dmPaperLength; //只针对打印机， 覆盖dmPaperSize指定的纸张长度 ，单位0.1mm

            [MarshalAs(UnmanagedType.I2)]
            public short dmPaperWidth; //只针对打印机，覆盖dmPaperSize指定的纸张宽度,单位0.1mm

            [MarshalAs(UnmanagedType.I2)]
            public short dmScale; // 设置打印输出缩放因子，缩放比例为dmScale/100

            [MarshalAs(UnmanagedType.I2)]
            public short dmCopies; //设置打印份数（如果打印机支持的话）

            [MarshalAs(UnmanagedType.I2)]
            public short dmDefaultSource; // 指定打印机纸张来源，通过DC_BINS 标志调用<strong>DeviceCapabilities函数获取打印机可用纸张来源列表</strong>

            [MarshalAs(UnmanagedType.I2)]
            public short
                dmPrintQuality; //指定打印机质量（分辨率）DMRES_DRAFT(-1),DMRES_LOW(-2),DMRES_MEDIUM(-3),DMRES_HIGH(-4),指定正值，则表示设置的是DPI<br>};

            [MarshalAs(UnmanagedType.I2)]
            public short dmColor; //黑白，彩色设定 DMCOLOR_MONOCHROME（1）：黑白， DMCOLOR_COLOR（2）：彩色

            [MarshalAs(UnmanagedType.I2)]
            public short
                dmDuplex; //单面/双面设置，DMDUP_SIMPLEX（1），单面；DMDUP_VERTICAL（2），长边装订（左侧翻页)，DMDUP_HORIZONTAL（3），短边装订(顶端翻页）

            [MarshalAs(UnmanagedType.I2)]
            public short dmYResolution; //指定Y轴DPI，若初始化设定这个值，PrintQuality 设置值为X轴DPI

            [MarshalAs(UnmanagedType.I2)]
            public short dmTTOption;

            [MarshalAs(UnmanagedType.I2)]
            public short dmCollate; //设定打印多页时，是否整理。DMCOLLATE_TRUE（1），整理；DMCOLLATE_FALSE（0），不整理

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public String dmFormName; //表单名称，如"Letter" or "Legal"，完整表单列表通过，EnumForms 函数获取

            [MarshalAs(UnmanagedType.U2)]
            public short dmLogPixels;

            [MarshalAs(UnmanagedType.U4)]
            public int dmBitsPerPel;

            [MarshalAs(UnmanagedType.U4)]
            public int dmPelsWidth;

            [MarshalAs(UnmanagedType.U4)]
            public int dmPelsHeight;

            [MarshalAs(UnmanagedType.U4)]
            public int dmNup;

            [MarshalAs(UnmanagedType.U4)]
            public int dmDisplayFrequency;

            [MarshalAs(UnmanagedType.U4)]
            public int dmICMMethod;

            [MarshalAs(UnmanagedType.U4)]
            public int dmICMIntent;

            [MarshalAs(UnmanagedType.U4)]
            public int dmMediaType;

            [MarshalAs(UnmanagedType.U4)]
            public int dmDitherType;

            [MarshalAs(UnmanagedType.U4)]
            public int dmReserved1;

            [MarshalAs(UnmanagedType.U4)]
            public int dmReserved2;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct PRINTER_INFO_9
        {
            public IntPtr pDevMode;
        }

        [DllImport("winspool.Drv",
             EntryPoint = "AddFormW",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool AddForm(IntPtr phPrinter,
            [MarshalAs(UnmanagedType.I4)] int level,
            ref FormInfo1 form);

        [DllImport("winspool.Drv",
             EntryPoint = "DeleteForm",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall),
         SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool DeleteForm(IntPtr phPrinter, [MarshalAs(UnmanagedType.LPTStr)] string pName);

        [DllImport("winspool.Drv",
             EntryPoint = "SetForm",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool SetForm(IntPtr phPrinter,
            [MarshalAs(UnmanagedType.LPTStr)] string pName,
            [MarshalAs(UnmanagedType.I4)] int level,
            ref FormInfo1 form);

        [DllImport("kernel32.dll",
             EntryPoint = "GetLastError",
             SetLastError = false,
             ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern Int32 GetLastError();

        [DllImport("GDI32.dll",
             EntryPoint = "CreateDC",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern IntPtr CreateDC([MarshalAs(UnmanagedType.LPTStr)] string pDrive,
            [MarshalAs(UnmanagedType.LPTStr)] string pName,
            [MarshalAs(UnmanagedType.LPTStr)] string pOutput,
            ref structDevMode pDevMode);

        [DllImport("GDI32.dll",
             EntryPoint = "ResetDC",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern IntPtr ResetDC(IntPtr hDC, ref structDevMode pDevMode);

        [DllImport("GDI32.dll",
             EntryPoint = "DeleteDC",
             SetLastError = true,
             CharSet = CharSet.Unicode,
             ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool DeleteDC(IntPtr hDC);

        [DllImport("winspool.Drv",
             EntryPoint = "SetPrinterA",
             SetLastError = true,
             CharSet = CharSet.Auto,
             ExactSpelling = true,
             CallingConvention = CallingConvention.StdCall), SuppressUnmanagedCodeSecurityAttribute()]
        internal static extern bool SetPrinter(IntPtr hPrinter,
            [MarshalAs(UnmanagedType.I4)] int level,
            IntPtr pPrinter,
            [MarshalAs(UnmanagedType.I4)] int command);

        [DllImport("winspool.Drv",
            EntryPoint = "DocumentPropertiesA",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        internal static extern int DocumentProperties(IntPtr hwnd,
            IntPtr hPrinter,
            [MarshalAs(UnmanagedType.LPStr)] string pDeviceName,
            IntPtr pDevModeOutput,
            IntPtr pDevModeInput,
            int fMode);

        [DllImport("winspool.Drv",
            EntryPoint = "GetPrinterA",
            SetLastError = true,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        internal static extern bool GetPrinter(IntPtr hPrinter,
            int dwLevel,
            IntPtr pPrinter,
            int dwBuf,
            out int dwNeeded);

        [Flags]
        internal enum SendMessageTimeoutFlags : uint
        {
            SMTO_NORMAL = 0x0000,

            SMTO_BLOCK = 0x0001,

            SMTO_ABORTIFHUNG = 0x0002,

            SMTO_NOTIMEOUTIFNOTHUNG = 0x0008
        }

        private const int WM_SETTINGCHANGE = 0x001A;

        private const int HWND_BROADCAST = 0xffff;

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessageTimeout(IntPtr windowHandle,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam,
            SendMessageTimeoutFlags flags,
            uint timeout,
            out IntPtr result);

        //EnumPrinters用到的函数和结构体
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool EnumPrinters(PrinterEnumFlags Flags,
            string Name,
            uint Level,
            IntPtr pPrinterEnum,
            uint cbBuf,
            ref uint pcbNeeded,
            ref uint pcReturned);

        [StructLayout(LayoutKind.Sequential)]
        internal struct PRINTER_INFO_2
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pServerName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pPrinterName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pShareName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pPortName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pDriverName;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pComment;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pLocation;

            public IntPtr pDevMode;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pSepFile;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pPrintProcessor;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pDatatype;

            [MarshalAs(UnmanagedType.LPStr)]
            public string pParameters;

            public IntPtr pSecurityDescriptor;

            public uint Attributes;

            public uint Priority;

            public uint DefaultPriority;

            public uint StartTime;

            public uint UntilTime;

            public uint Status;

            public uint cJobs;

            public uint AveragePPM;
        }

        [FlagsAttribute]
        internal enum PrinterEnumFlags
        {
            PRINTER_ENUM_DEFAULT = 0x00000001,

            PRINTER_ENUM_LOCAL = 0x00000002,

            PRINTER_ENUM_CONNECTIONS = 0x00000004,

            PRINTER_ENUM_FAVORITE = 0x00000004,

            PRINTER_ENUM_NAME = 0x00000008,

            PRINTER_ENUM_REMOTE = 0x00000010,

            PRINTER_ENUM_SHARED = 0x00000020,

            PRINTER_ENUM_NETWORK = 0x00000040,

            PRINTER_ENUM_EXPAND = 0x00004000,

            PRINTER_ENUM_CONTAINER = 0x00008000,

            PRINTER_ENUM_ICONMASK = 0x00ff0000,

            PRINTER_ENUM_ICON1 = 0x00010000,

            PRINTER_ENUM_ICON2 = 0x00020000,

            PRINTER_ENUM_ICON3 = 0x00040000,

            PRINTER_ENUM_ICON4 = 0x00080000,

            PRINTER_ENUM_ICON5 = 0x00100000,

            PRINTER_ENUM_ICON6 = 0x00200000,

            PRINTER_ENUM_ICON7 = 0x00400000,

            PRINTER_ENUM_ICON8 = 0x00800000,

            PRINTER_ENUM_HIDE = 0x01000000
        }

        //打印机状态
        [FlagsAttribute]
        internal enum PrinterStatus
        {
            PRINTER_STATUS_BUSY = 0x00000200,

            PRINTER_STATUS_DOOR_OPEN = 0x00400000,

            PRINTER_STATUS_ERROR = 0x00000002,

            PRINTER_STATUS_INITIALIZING = 0x00008000,

            PRINTER_STATUS_IO_ACTIVE = 0x00000100,

            PRINTER_STATUS_MANUAL_FEED = 0x00000020,

            PRINTER_STATUS_NO_TONER = 0x00040000,

            PRINTER_STATUS_NOT_AVAILABLE = 0x00001000,

            PRINTER_STATUS_OFFLINE = 0x00000080,

            PRINTER_STATUS_OUT_OF_MEMORY = 0x00200000,

            PRINTER_STATUS_OUTPUT_BIN_FULL = 0x00000800,

            PRINTER_STATUS_PAGE_PUNT = 0x00080000,

            PRINTER_STATUS_PAPER_JAM = 0x00000008,

            PRINTER_STATUS_PAPER_OUT = 0x00000010,

            PRINTER_STATUS_PAPER_PROBLEM = 0x00000040,

            PRINTER_STATUS_PAUSED = 0x00000001,

            PRINTER_STATUS_PENDING_DELETION = 0x00000004,

            PRINTER_STATUS_PRINTING = 0x00000400,

            PRINTER_STATUS_PROCESSING = 0x00004000,

            PRINTER_STATUS_TONER_LOW = 0x00020000,

            PRINTER_STATUS_USER_INTERVENTION = 0x00100000,

            PRINTER_STATUS_WAITING = 0x20000000,

            PRINTER_STATUS_WARMING_UP = 0x00010000
        }

        //GetDefaultPrinter用到的API函数说明
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetDefaultPrinter(StringBuilder pszBuffer, ref int size);

        //SetDefaultPrinter用到的API函数声明
        [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetDefaultPrinter(string Name);

        //EnumFormsA用到的函数声明，应该和EnumPrinters类似
        [DllImport("winspool.drv", EntryPoint = "EnumForms")]
        internal static extern int EnumFormsA(IntPtr hPrinter,
            int Level,
            ref byte pForm,
            int cbBuf,
            ref int pcbNeeded,
            ref int pcReturned);

        #endregion API声明

        internal static int GetPrinterStatusInt(string PrinterName)
        {
            int intRet = 0;
            IntPtr hPrinter;
            structPrinterDefaults defaults = new structPrinterDefaults();
            if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
            {
                int cbNeeded = 0;
                bool bolRet = GetPrinter(hPrinter, 2, IntPtr.Zero, 0, out cbNeeded);
                if (cbNeeded > 0)
                {
                    IntPtr pAddr = Marshal.AllocHGlobal((int)cbNeeded);
                    bolRet = GetPrinter(hPrinter, 2, pAddr, cbNeeded, out cbNeeded);
                    if (bolRet)
                    {
                        PRINTER_INFO_2 Info2 = new PRINTER_INFO_2();

                        Info2 = (PRINTER_INFO_2)Marshal.PtrToStructure(pAddr, typeof(PRINTER_INFO_2));

                        intRet = System.Convert.ToInt32(Info2.Status);
                    }

                    Marshal.FreeHGlobal(pAddr);
                }

                ClosePrinter(hPrinter);
            }

            return intRet;
        }

        internal static PRINTER_INFO_2[] EnumPrintersByFlag(PrinterEnumFlags Flags)
        {
            uint cbNeeded = 0;
            uint cReturned = 0;
            bool ret = EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL,
                null,
                2,
                IntPtr.Zero,
                0,
                ref cbNeeded,
                ref cReturned);

            IntPtr pAddr = Marshal.AllocHGlobal((int)cbNeeded);
            ret = EnumPrinters(PrinterEnumFlags.PRINTER_ENUM_LOCAL,
                null,
                2,
                pAddr,
                cbNeeded,
                ref cbNeeded,
                ref cReturned);

            if (ret)
            {
                PRINTER_INFO_2[] Info2 = new PRINTER_INFO_2[cReturned];
                int offset = pAddr.ToInt32();
                for (int i = 0; i < cReturned; i++)
                {
                    Info2[i].pServerName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pPrinterName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pShareName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pPortName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pDriverName = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pComment = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pLocation = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pDevMode = Marshal.ReadIntPtr(new IntPtr(offset));
                    offset += 4;
                    Info2[i].pSepFile = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pPrintProcessor = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pDatatype = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pParameters = Marshal.PtrToStringAuto(Marshal.ReadIntPtr(new IntPtr(offset)));
                    offset += 4;
                    Info2[i].pSecurityDescriptor = Marshal.ReadIntPtr(new IntPtr(offset));
                    offset += 4;
                    Info2[i].Attributes = (uint)Marshal.ReadIntPtr(new IntPtr(offset));
                    offset += 4;
                    Info2[i].Priority = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].DefaultPriority = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].StartTime = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].UntilTime = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].Status = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].cJobs = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                    Info2[i].AveragePPM = (uint)Marshal.ReadInt32(new IntPtr(offset));
                    offset += 4;
                }

                Marshal.FreeHGlobal(pAddr);

                return Info2;
            }
            else
            {
                return new PRINTER_INFO_2[0];
            }
        }

        #region 获取当前指定打印机的状态

        /// </summary>
        /// 获取当前指定打印机的状态
        /// </summary>
        /// <param name="PrinterName">打印机名称</param>
        /// <returns>打印机状态描述</returns>
        public static string GetPrinterStatus(string PrinterName)
        {
            int intValue = GetPrinterStatusInt(PrinterName);
            string strRet = string.Empty;
            switch (intValue)
            {
                case 0:
                    strRet = "准备就绪（Ready）";

                    break;

                case 0x00000200:
                    strRet = "忙(Busy）";

                    break;

                case 0x00400000:
                    strRet = "门被打开（Printer Door Open）";

                    break;

                case 0x00000002:
                    strRet = "错误(Printer Error）";

                    break;

                case 0x0008000:
                    strRet = "正在初始化(Initializing）";

                    break;

                case 0x00000100:
                    strRet = "正在输入或输出（I/O Active）";

                    break;

                case 0x00000020:
                    strRet = "手工送纸（Manual Feed）";

                    break;

                case 0x00040000:
                    strRet = "无墨粉（No Toner）";

                    break;

                case 0x00001000:
                    strRet = "不可用（Not Available）";

                    break;

                case 0x00000080:
                    strRet = "脱机（Off Line）";

                    break;

                case 0x00200000:
                    strRet = "内存溢出（Out of Memory）";

                    break;

                case 0x00000800:
                    strRet = "输出口已满（Output Bin Full）";

                    break;

                case 0x00080000:
                    strRet = "当前页无法打印（Page Punt）";

                    break;

                case 0x00000008:
                    strRet = "塞纸（Paper Jam）";

                    break;

                case 0x00000010:
                    strRet = "打印纸用完（Paper Out）";

                    break;

                case 0x00000040:
                    strRet = "纸张问题（Page Problem）";

                    break;

                case 0x00000001:
                    strRet = "暂停（Paused）";

                    break;

                case 0x00000004:
                    strRet = "正在删除（Pending Deletion）";

                    break;

                case 0x00000400:
                    strRet = "正在打印（Printing）";

                    break;

                case 0x00004000:
                    strRet = "正在处理（Processing）";

                    break;

                case 0x00020000:
                    strRet = "墨粉不足（Toner Low）";

                    break;

                case 0x00100000:
                    strRet = "需要用户干预（User Intervention）";

                    break;

                case 0x20000000:
                    strRet = "等待（Waiting）";

                    break;

                case 0x00010000:
                    strRet = "正在准备（Warming Up）";

                    break;

                default:
                    strRet = "未知状态（Unknown Status）";

                    break;
            }

            return strRet;
        }

        #endregion 获取当前指定打印机的状态

        #region 删除已经存在的自定义纸张

        /**/

        /// <summary>
        /// 删除已经存在的自定义纸张
        /// </summary>
        /// <param name="PrinterName">打印机名称</param>
        /// <param name="PaperName">纸张名称</param>
        public static void DeleteCustomPaperSize(string PrinterName, string PaperName)
        {
            const int PRINTER_ACCESS_USE = 0x00000008;
            const int PRINTER_ACCESS_ADMINISTER = 0x00000004;

            structPrinterDefaults defaults = new structPrinterDefaults();
            defaults.pDatatype = null;
            defaults.pDevMode = IntPtr.Zero;
            defaults.DesiredAccess = PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;

            IntPtr hPrinter = IntPtr.Zero;

            //打开打印机
            if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
            {
                try
                {
                    DeleteForm(hPrinter, PaperName);
                }
                catch
                {
                }
                finally
                {
                    ClosePrinter(hPrinter);
                }
            }
        }

        #endregion 删除已经存在的自定义纸张

        #region 指定的打印机设置以mm为单位的自定义纸张(Form)

        /**/

        /// <summary>
        /// 指定的打印机设置以mm为单位的自定义纸张(Form)
        /// </summary>
        /// <param name="PrinterName">打印机名称</param>
        /// <param name="PaperName">Form名称</param>
        /// <param name="WidthInMm">以mm为单位的宽度</param>
        /// <param name="HeightInMm">以mm为单位的高度</param>
        public static void AddCustomPaperSize(string PrinterName, string PaperName, float WidthInMm, float HeightInMm)
        {
            if (PlatformID.Win32NT == Environment.OSVersion.Platform)
            {
                const int PRINTER_ACCESS_USE = 0x00000008;
                const int PRINTER_ACCESS_ADMINISTER = 0x00000004;
                structPrinterDefaults defaults = new structPrinterDefaults();
                defaults.pDatatype = null;
                defaults.pDevMode = IntPtr.Zero;
                defaults.DesiredAccess = PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;
                IntPtr hPrinter = IntPtr.Zero;

                //打开打印机
                if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
                {
                    try
                    {
                        //如果Form存在删除之
                        DeleteForm(hPrinter, PaperName);

                        //创建并初始化FORM_INFO_1
                        FormInfo1 formInfo = new FormInfo1();
                        formInfo.Flags = 0;
                        formInfo.pName = PaperName;

                        //所有尺寸大小都是毫米[millimeters] 1000倍
                        formInfo.Size.width = (int)(WidthInMm * 1000.0);
                        formInfo.Size.height = (int)(HeightInMm * 1000.0);
                        formInfo.ImageableArea.left = 0;
                        formInfo.ImageableArea.right = formInfo.Size.width;
                        formInfo.ImageableArea.top = 0;
                        formInfo.ImageableArea.bottom = formInfo.Size.height;
                        if (!AddForm(hPrinter, 1, ref formInfo))
                        {
                            StringBuilder strBuilder = new StringBuilder();
                            strBuilder.AppendFormat("向打印机 {1} 添加自定义纸张 {0} 失败！错误代号：{2}",
                                PaperName,
                                PrinterName,
                                GetLastError());

                            throw new ApplicationException(strBuilder.ToString());
                        }

                        //初始化设定打印机
                        const int DM_OUT_BUFFER = 2;
                        const int DM_IN_BUFFER = 8;
                        const int DM_ORIENTATION = 1;
                        const int DM_PAPERSIZE = 2;
                        structDevMode devMode = new structDevMode();
                        IntPtr hPrinterInfo, hDummy;
                        PRINTER_INFO_9 printerInfo;
                        printerInfo.pDevMode = IntPtr.Zero;
                        int iPrinterInfoSize, iDummyInt;

                        //获取DEV_MODE结构体缓冲大小
                        int iDevModeSize = DocumentProperties(IntPtr.Zero,
                            hPrinter,
                            PrinterName,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            0);

                        if (iDevModeSize < 0)
                            throw new ApplicationException("无法取得DEVMODE结构的大小！");

                        //分配缓冲
                        IntPtr hDevMode = Marshal.AllocCoTaskMem(iDevModeSize + 100);

                        //获取DEV_MODE指针
                        int iRet = DocumentProperties(IntPtr.Zero,
                            hPrinter,
                            PrinterName,
                            hDevMode,
                            IntPtr.Zero,
                            DM_OUT_BUFFER);

                        if (iRet < 0)
                            throw new ApplicationException("无法获得DEVMODE结构！");

                        //填充DEV_MODE
                        devMode = (structDevMode)Marshal.PtrToStructure(hDevMode, devMode.GetType());

                        devMode.dmFields = 0x10000;

                        //FORM名称
                        devMode.dmFormName = PaperName;

                        Marshal.StructureToPtr(devMode, hDevMode, true);

                        iRet = DocumentProperties(IntPtr.Zero,
                            hPrinter,
                            PrinterName,
                            printerInfo.pDevMode,
                            printerInfo.pDevMode,
                            DM_IN_BUFFER | DM_OUT_BUFFER);

                        if (iRet < 0)
                            throw new ApplicationException("无法为打印机设定打印纸张名称！");

                        GetPrinter(hPrinter, 9, IntPtr.Zero, 0, out iPrinterInfoSize);

                        if (iPrinterInfoSize == 0)
                            throw new ApplicationException("调用GetPrinter方法失败！");

                        hPrinterInfo = Marshal.AllocCoTaskMem(iPrinterInfoSize + 100);

                        bool bSuccess = GetPrinter(hPrinter, 9, hPrinterInfo, iPrinterInfoSize, out iDummyInt);

                        if (!bSuccess)
                            throw new ApplicationException("调用GetPrinter方法失败！");

                        printerInfo = (PRINTER_INFO_9)Marshal.PtrToStructure(hPrinterInfo, printerInfo.GetType());
                        printerInfo.pDevMode = hDevMode;

                        Marshal.StructureToPtr(printerInfo, hPrinterInfo, true);

                        bSuccess = SetPrinter(hPrinter, 9, hPrinterInfo, 0);

                        if (!bSuccess)
                            throw new Win32Exception(Marshal.GetLastWin32Error(), "调用SetPrinter方法失败，无法进行打印机设置！");

                        SendMessageTimeout(new IntPtr(HWND_BROADCAST),
                            WM_SETTINGCHANGE,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            ApiPrinterHelper.SendMessageTimeoutFlags.SMTO_NORMAL,
                            1000,
                            out hDummy);
                    }
                    finally
                    {
                        ClosePrinter(hPrinter);
                    }
                }
                else
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat("无法打开打印机{0}, 错误代号: {1}", PrinterName, GetLastError());

                    throw new ApplicationException(strBuilder.ToString());
                }
            }
            else
            {
                structDevMode pDevMode = new structDevMode();
                IntPtr hDC = CreateDC(null, PrinterName, null, ref pDevMode);
                if (hDC != IntPtr.Zero)
                {
                    const long DM_PAPERSIZE = 0x00000002L;
                    const long DM_PAPERLENGTH = 0x00000004L;
                    const long DM_PAPERWIDTH = 0x00000008L;
                    pDevMode.dmFields = (int)(DM_PAPERSIZE | DM_PAPERWIDTH | DM_PAPERLENGTH);
                    pDevMode.dmPaperSize = 256;
                    pDevMode.dmPaperWidth = (short)(WidthInMm * 1000.0);
                    pDevMode.dmPaperLength = (short)(HeightInMm * 1000.0);
                    ResetDC(hDC, ref pDevMode);
                    DeleteDC(hDC);
                }
            }
        }

        #endregion 指定的打印机设置以mm为单位的自定义纸张(Form)

        public static void SetCustomPaperSize(string PrinterName, string PaperName)
        {
            PaperSize ps = GetPrintForm(PrinterName, PaperName);
            float WidthInMm = 0f;
            float HeightInMm = 0f;
            if (ps == null)
            {
                throw new ApplicationException($"没有查询到[{PaperName}]纸张类型");
            }

            WidthInMm = ps.Width * 0.254f;
            HeightInMm = ps.Height * 0.254f;
            if (PlatformID.Win32NT == Environment.OSVersion.Platform)
            {
                const int PRINTER_ACCESS_USE = 0x00000008;
                const int PRINTER_ACCESS_ADMINISTER = 0x00000004;

                //设置打开打印机的方式
                structPrinterDefaults defaults = new structPrinterDefaults();
                defaults.pDatatype = null;
                defaults.pDevMode = IntPtr.Zero;
                defaults.DesiredAccess = PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;
                IntPtr hPrinter = IntPtr.Zero;

                //打开打印机
                if (OpenPrinter(PrinterName, out hPrinter, ref defaults))
                {
                    try
                    {
                        //初始化设定打印机
                        const int DM_OUT_BUFFER = 2;
                        const int DM_IN_BUFFER = 8;
                        structDevMode devMode = new structDevMode();
                        IntPtr hPrinterInfo;
                        IntPtr hDummy;
                        PRINTER_INFO_9 printerInfo = new PRINTER_INFO_9();
                        printerInfo.pDevMode = IntPtr.Zero;
                        int iPrinterInfoSize, iDummyInt;

                        int iDevModeSize = DocumentProperties(IntPtr.Zero,
                            hPrinter,
                            PrinterName,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            0);

                        if (iDevModeSize < 0)
                            throw new ApplicationException("无法取得DEVMODE结构的大小！");

                        //分配缓冲
                        IntPtr hDevMode = Marshal.AllocCoTaskMem(iDevModeSize + 100);

                        //获取DEV_MODE指针
                        int iRet = DocumentProperties(IntPtr.Zero,
                            hPrinter,
                            PrinterName,
                            hDevMode,
                            IntPtr.Zero,
                            DM_OUT_BUFFER);

                        if (iRet < 0)
                            throw new ApplicationException("无法获得DEVMODE结构！");

                        //填充DEV_MODE
                        devMode = (structDevMode)Marshal.PtrToStructure(hDevMode, devMode.GetType());

                        devMode.dmFields = 0x10000;

                        //FORM名称
                        devMode.dmFormName = PaperName;

                        Marshal.StructureToPtr(devMode, hDevMode, true);

                        iRet = DocumentProperties(IntPtr.Zero,
                            hPrinter,
                            PrinterName,
                            printerInfo.pDevMode,
                            printerInfo.pDevMode,
                            DM_IN_BUFFER | DM_OUT_BUFFER);

                        if (iRet < 0)
                            throw new ApplicationException("无法为打印机设定打印方向！");

                        GetPrinter(hPrinter, 9, IntPtr.Zero, 0, out iPrinterInfoSize);

                        if (iPrinterInfoSize == 0)
                            throw new ApplicationException("调用GetPrinter方法失败！");

                        hPrinterInfo = Marshal.AllocCoTaskMem(iPrinterInfoSize + 100);

                        bool bSuccess = GetPrinter(hPrinter, 9, hPrinterInfo, iPrinterInfoSize, out iDummyInt);

                        if (!bSuccess)
                            throw new ApplicationException("调用GetPrinter方法失败！");

                        printerInfo = (PRINTER_INFO_9)Marshal.PtrToStructure(hPrinterInfo, printerInfo.GetType());
                        printerInfo.pDevMode = hDevMode;

                        Marshal.StructureToPtr(printerInfo, hPrinterInfo, true);

                        bSuccess = SetPrinter(hPrinter, 9, hPrinterInfo, 0);

                        if (!bSuccess)
                            throw new Win32Exception(Marshal.GetLastWin32Error(), "调用SetPrinter方法失败，无法进行打印机设置！");

                        SendMessageTimeout(new IntPtr(HWND_BROADCAST),
                            WM_SETTINGCHANGE,
                            IntPtr.Zero,
                            IntPtr.Zero,
                            ApiPrinterHelper.SendMessageTimeoutFlags.SMTO_NORMAL,
                            1000,
                            out hDummy);
                    }
                    finally
                    {
                        ClosePrinter(hPrinter);
                    }
                }
                else
                {
                    StringBuilder strBuilder = new StringBuilder();
                    strBuilder.AppendFormat("无法打开打印机{0}, 错误代号: {1}", PrinterName, GetLastError());

                    throw new ApplicationException(strBuilder.ToString());
                }
            }
            else
            {
                structDevMode pDevMode = new structDevMode();
                IntPtr hDC = CreateDC(null, PrinterName, null, ref pDevMode);
                if (hDC != IntPtr.Zero)
                {
                    const long DM_PAPERSIZE = 0x00000002L;
                    const long DM_PAPERLENGTH = 0x00000004L;
                    const long DM_PAPERWIDTH = 0x00000008L;
                    pDevMode.dmFields = (int)(DM_PAPERSIZE | DM_PAPERWIDTH | DM_PAPERLENGTH);
                    pDevMode.dmPaperSize = 256;
                    pDevMode.dmPaperWidth = (short)(WidthInMm * 1000.0);
                    pDevMode.dmPaperLength = (short)(HeightInMm * 1000.0);
                    ResetDC(hDC, ref pDevMode);
                    DeleteDC(hDC);
                }
            }
        }

        /// <summary>
        /// 获取打印纸类型
        /// </summary>
        /// <param name="printerName">打印机名称</param>
        /// <param name="paperName">纸张类型名称</param>
        /// <returns>纸张类型</returns>
        public static PaperSize GetPrintForm(string printerName, string paperName)
        {
            PaperSize paper = null;
            PrinterSettings printer = new System.Drawing.Printing.PrinterSettings();
            printer.PrinterName = printerName;
            foreach (System.Drawing.Printing.PaperSize ps in printer.PaperSizes)
            {
                if (ps.PaperName.ToLower() == paperName.ToLower())
                {
                    paper = ps;

                    break;
                }
            }

            return paper;
        }

        ///
        /// 设置打印纸类型的尺寸，如果没有参数提供的类型则新建一个类型，
        ///
        /// 打印机名称
        /// 打印纸类型名称
        /// 宽度，单位毫米，需要在打印机允许的尺寸之内，否则按照默认打印纸类型打印
        /// 高度，单位毫米，需要在打印机允许的尺寸之内，否则按照默认打印纸类型打印
        public static void SetPrintForm(string printerName, string paperName, float width, float height)
        {
            if (PlatformID.Win32NT == Environment.OSVersion.Platform)
            {
                const int PRINTER_ACCESS_USE = 0x00000008;
                const int PRINTER_ACCESS_ADMINISTER = 0x00000004;
                structPrinterDefaults defaults = new structPrinterDefaults();
                defaults.pDatatype = null;
                defaults.pDevMode = IntPtr.Zero;
                defaults.DesiredAccess = PRINTER_ACCESS_ADMINISTER | PRINTER_ACCESS_USE;
                IntPtr hPrinter = IntPtr.Zero;
                if (OpenPrinter(printerName, out hPrinter, ref defaults))
                {
                    try
                    {
                        FormInfo1 formInfo = new FormInfo1();
                        formInfo.Flags = 0;
                        formInfo.pName = paperName;
                        formInfo.Size.width = (int)(width * 1000.0);
                        formInfo.Size.height = (int)(height * 1000.0);
                        formInfo.ImageableArea.left = 0;
                        formInfo.ImageableArea.right = formInfo.Size.width;
                        formInfo.ImageableArea.top = 0;
                        formInfo.ImageableArea.bottom = formInfo.Size.height;
                        bool rslt = false;
                        if (GetPrintForm(printerName, paperName) != null)
                        {
                            rslt = SetForm(hPrinter, paperName, 1, ref formInfo);
                        }
                        else
                        {
                            AddCustomPaperSize(printerName, paperName, width, height);
                            rslt = true;
                        }

                        if (!rslt)
                        {
                            StringBuilder strBuilder = new StringBuilder();
                            strBuilder.AppendFormat("添加纸张{0}时发生错误!, 系统错误号: {1}", paperName, GetLastError());

                            throw new ApplicationException(strBuilder.ToString());
                        }
                    }
                    finally
                    {
                        ClosePrinter(hPrinter);
                    }
                }
            }
        }

        #region 获取本地打印机列表

        /**/

        /// <summary>
        /// 获取本地打印机列表
        /// 可以通过制定参数获取网络打印机
        /// </summary>
        /// <returns>打印机列表</returns>
        public static System.Collections.ArrayList GetPrinterList()
        {
            System.Collections.ArrayList alRet = new System.Collections.ArrayList();
            PRINTER_INFO_2[] Info2 = EnumPrintersByFlag(PrinterEnumFlags.PRINTER_ENUM_LOCAL);
            for (int i = 0; i < Info2.Length; i++)
            {
                alRet.Add(Info2[i].pPrinterName);
            }

            return alRet;
        }

        #endregion 获取本地打印机列表

        #region 获取本机的默认打印机名称

        /// <summary>
        /// 获取本机的默认打印机名称
        /// </summary>
        /// <returns>默认打印机名称</returns>
        public static string GetDeaultPrinterName()
        {
            StringBuilder dp = new StringBuilder(256);
            int size = dp.Capacity;
            if (GetDefaultPrinter(dp, ref size))
            {
                return dp.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion 获取本机的默认打印机名称

        #region 设置默认打印机

        /**/

        /// <summary>
        /// 设置默认打印机
        /// </summary>
        /// <param name="PrinterName">可用的打印机名称</param>
        public static void SetPrinterToDefault(string PrinterName)
        {
            SetDefaultPrinter(PrinterName);
        }

        #endregion 设置默认打印机

        #region 判断打印机是否在系统可用的打印机列表中

        /**/

        ///// <summary>
        ///// 判断打印机是否在系统可用的打印机列表中
        ///// </summary>
        ///// <param name="PrinterName">打印机名称</param>
        ///// <returns>是：在；否：不在</returns>
        public static bool PrinterInList(string PrinterName)
        {
            bool bolRet = false;

            System.Collections.ArrayList alPrinters = GetPrinterList();

            for (int i = 0; i < alPrinters.Count; i++)
            {
                if (PrinterName == alPrinters[i].ToString())
                {
                    bolRet = true;

                    break;
                }
            }

            alPrinters.Clear();
            alPrinters = null;

            return bolRet;
        }

        #endregion 判断打印机是否在系统可用的打印机列表中

        #region 判断表单是否在指定的打印机所支持的纸张列表中

        /**/

        ///// <summary>
        ///// 判断表单是否在指定的打印机所支持的纸张列表中,表单就是我们平常所说的纸张
        ///// </summary>
        ///// <param name="PrinterName">打印机名称</param>
        ///// <param name="PaperName">纸张名称</param>
        ///// <returns>是：在；否：不在</returns>
        public static bool FormInPrinter(string PrinterName, string PaperName)
        {
            bool bolRet = false;

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

            pd.PrinterSettings.PrinterName = PrinterName;

            foreach (System.Drawing.Printing.PaperSize ps in pd.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName == PaperName)
                {
                    bolRet = true;

                    break;
                }
            }

            pd.Dispose();

            return bolRet;
        }

        #endregion 判断表单是否在指定的打印机所支持的纸张列表中

        #region 判断指定纸张的宽度和高度和与打印内容指定的宽度和高度是否匹配

        /**/

        /// <summary>
        /// 判断指定纸张的宽度和高度和与打印内容指定的宽度和高度是否匹配
        /// </summary>
        /// <param name="PrinterName">打印机名称</param>
        /// <param name="FormName">表单名称</param>
        /// <param name="Width">宽度</param>
        /// <param name="Height">高度</param>
        /// <returns></returns>
        public static bool FormSameSize(string PrinterName, string FormName, decimal Width, decimal Height)
        {
            bool bolRet = false;

            System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

            pd.PrinterSettings.PrinterName = PrinterName;

            foreach (System.Drawing.Printing.PaperSize ps in pd.PrinterSettings.PaperSizes)
            {
                if (ps.PaperName == FormName)
                {
                    decimal decWidth = FromInchToCM(System.Convert.ToDecimal(ps.Width));
                    decimal decHeight = FromInchToCM(System.Convert.ToDecimal(ps.Height));

                    //只要整数位相同即认为是同一纸张，毕竟inch到cm的转换并不能整除
                    if (Math.Round(decWidth, 0) == Math.Round(Width, 0) &&
                        Math.Round(decHeight, 0) == Math.Round(Height, 0))
                        bolRet = true;

                    break;
                }
            }

            pd.Dispose();

            return bolRet;
        }

        #endregion 判断指定纸张的宽度和高度和与打印内容指定的宽度和高度是否匹配

        #region 英寸到厘米的转换

        /**/

        /// <summary>
        /// 英寸到厘米的转换
        /// /* = = = = = = = = = = = = = = = = *\
        /// | 换算一下计量单位，将其换算成厘米  |
        /// |    厘米     像素     英寸         |
        /// |     1        38     0.395         |
        /// |   0.026       1      0.01         |
        /// |    2.54      96        1          |
        /// \* = = = = = = = = = = = = = = = = */
        /// </summary>
        /// <param name="inch">英寸数</param>
        /// <returns>厘米数，两位小数</returns>
        ///
        public static decimal FromInchToCM(decimal inch)
        {
            return Math.Round((System.Convert.ToDecimal((inch / 100)) * System.Convert.ToDecimal(2.5400)), 2);
        }

        #endregion 英寸到厘米的转换
    }
}
