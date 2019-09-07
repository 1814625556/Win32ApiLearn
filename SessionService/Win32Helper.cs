using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SessionService
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct WTS_SESSION_INFO
    {
        public readonly int SessionID;

        [MarshalAs(UnmanagedType.LPStr)] public readonly string pWinStationName;

        public readonly WTS_CONNECTSTATE_CLASS State;
    }

    public enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct STARTUPINFO
    {
        public int cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public int dwX;
        public int dwY;
        public int dwXSize;
        public int dwYSize;
        public int dwXCountChars;
        public int dwYCountChars;
        public int dwFillAttribute;
        public int dwFlags;
        public short wShowWindow;
        public short cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    public sealed class Win32ApiHelper
    {

        /// <summary>
        /// 查找窗体上控件句柄
        /// </summary>
        /// <param name="captionName">控件标题</param>
        /// <param name="bChild">设定是否在子窗体中查找</param>
        /// <returns></returns>
        public static IntPtr FindWindow(string captionName, bool bChild = false)
        {
            return FindWindow(IntPtr.Zero, captionName, bChild);
        }


        /// <summary>
        ///     查找窗体上控件句柄
        /// </summary>
        /// <param name="hwnd">父窗体句柄</param>
        /// <param name="captionName">控件标题(Text)</param>
        /// <param name="bChild">设定是否在子窗体中查找</param>
        /// <returns>控件句柄</returns>
        public static IntPtr FindWindow(IntPtr hwnd, string captionName, bool bChild = false)
        {
            var windowHandle = Win32Api.FindWindowEx(hwnd, IntPtr.Zero, null, null);
            if (windowHandle != IntPtr.Zero) return windowHandle;

            if (!bChild) return windowHandle;

            Win32Api.EnumChildWindows(
                hwnd,
                (h, l) =>
                {
                    var childWinHandle = Win32Api.FindWindowEx(h, IntPtr.Zero, null, captionName);
                    if (childWinHandle == IntPtr.Zero) return true;

                    windowHandle = childWinHandle;
                    return false;
                },
                0);
            return windowHandle;
        }
        /// <summary>
        ///     以当前登录系统的用户角色权限启动指定的进程
        /// </summary>
        /// <param name="processPath">指定的进程(全路径)</param>
        public static void CreateProcess(string processPath)
        {
            var ppSessionInfo = IntPtr.Zero;
            try
            {
                Win32Api.WTSQueryUserToken(2, out var hToken);
                File.AppendAllText("20190905.txt", hToken.ToString());
                var tStartUpInfo = new STARTUPINFO
                {
                    cb = Marshal.SizeOf(typeof(STARTUPINFO))
                };
                var childProcStarted = Win32Api.CreateProcessAsUser(
                    hToken,
                    processPath,
                    null,
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    0,
                    null,
                    null,
                    ref tStartUpInfo,
                    out var tProcessInfo
                );
                if (!childProcStarted) throw new Exception($"CreateProcessAsUser({processPath})");
                Win32Api.CloseHandle(tProcessInfo.hThread);
                Win32Api.CloseHandle(tProcessInfo.hProcess);
                Win32Api.CloseHandle(hToken);
            }
            finally
            {
                if (ppSessionInfo != IntPtr.Zero)
                    Win32Api.WTSFreeMemory(ppSessionInfo);
            }
        }
    }

    public sealed class Win32Api
    {
        [DllImport("wtsapi32.dll")]
        internal static extern int WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] int reserved,
            [MarshalAs(UnmanagedType.U4)] int version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref int pCount);

        [DllImport("WTSAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool WTSQueryUserToken(int sessionId, out IntPtr token);
        [DllImport("ADVAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, string lpEnvironment, string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        internal static extern bool CloseHandle(IntPtr handle);

        [DllImport("wtsapi32.dll")]
        internal static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
        internal static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childHandle, string className,
            string captionName);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hWndParent, ChildWindowsProc lpEnumFunc, int lParam);
        internal delegate bool ChildWindowsProc(IntPtr hwnd, int lParam);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int SendMessage(IntPtr HWnd, uint Msg, int WParam, int LParam);
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    }
}
