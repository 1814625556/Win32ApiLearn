using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Session
{
    public class SessionHelper
    {
        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentUser()
        {
            IntPtr buffer;
            uint strLen;
            int cur_session = -1;
            var username = "SYSTEM"; // assume SYSTEM as this will return "\0" below
            if (WinApi.WTSQuerySessionInformation(IntPtr.Zero, cur_session, WinApi.WTSInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer); // don't need length as these are null terminated strings
                WinApi.WTSFreeMemory(buffer);
                if (WinApi.WTSQuerySessionInformation(IntPtr.Zero, cur_session, WinApi.WTSInfoClass.WTSDomainName, out buffer, out strLen) && strLen > 1)
                {
                    username = Marshal.PtrToStringAnsi(buffer) + "\\" + username; // prepend domain name
                    WinApi.WTSFreeMemory(buffer);
                }
            }
            return username;
        }

        /// <summary>
        ///     以当前登录系统的用户角色权限启动指定的进程
        /// </summary>
        /// <param name="processPath">指定的进程(全路径)</param>
        public static void CreateProcess(string processPath)
        {
            var ppSessionInfo = IntPtr.Zero;
            var sessionCount = 0;
            var hasSession = WinApi.WTSEnumerateSessions(IntPtr.Zero, 0, 1, ref ppSessionInfo, ref sessionCount) != 0;

            try
            {
                if (!hasSession)
                    throw new Exception("WTSEnumerateSessions==0");
                for (var count = 0; count < sessionCount; count++)
                {
                    var si = (WinApi.WTS_SESSION_INFO)Marshal.PtrToStructure(
                        ppSessionInfo + count * Marshal.SizeOf(typeof(WinApi.WTS_SESSION_INFO)), typeof(WinApi.WTS_SESSION_INFO));

                    if (si.State != WinApi.WTS_CONNECTSTATE_CLASS.WTSActive) continue;

                    IntPtr hToken = IntPtr.Zero;

                    if (!WinApi.WTSQueryUserToken(si.SessionID, out hToken)) continue;

                    var tStartUpInfo = new WinApi.STARTUPINFO
                    {
                        cb = Marshal.SizeOf(typeof(WinApi.STARTUPINFO))
                    };
                    var childProcStarted = WinApi.CreateProcessAsUser(
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
                    WinApi.CloseHandle(tProcessInfo.hThread);
                    WinApi.CloseHandle(tProcessInfo.hProcess);

                    WinApi.CloseHandle(hToken);
                    break;
                }
            }
            finally
            {
                if (ppSessionInfo != IntPtr.Zero)
                    WinApi.WTSFreeMemory(ppSessionInfo);
            }
        }
    }
}
