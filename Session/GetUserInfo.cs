using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Session
{
    public class GetUserInfo
    {
        public enum WTSInfoClass
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType,
            WTSIdleTime,
            WTSLogonTime,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames,
            WTSClientInfo,
            WTSSessionInfo
        }

        [DllImport("Wtsapi32.dll")]
        protected static extern void WTSFreeMemory(IntPtr pointer);

        [DllImport("Wtsapi32.dll")]
        protected static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, WTSInfoClass wtsInfoClass, out IntPtr ppBuffer, out uint pBytesReturned);

        public static string GetCurrentUser()
        {
            IntPtr buffer;
            uint strLen;
            int cur_session = -1;
            var username = "SYSTEM"; // assume SYSTEM as this will return "\0" below
            if (WTSQuerySessionInformation(IntPtr.Zero, cur_session, WTSInfoClass.WTSUserName, out buffer, out strLen) && strLen > 1)
            {
                username = Marshal.PtrToStringAnsi(buffer); // don't need length as these are null terminated strings
                WTSFreeMemory(buffer);
                if (WTSQuerySessionInformation(IntPtr.Zero, cur_session, WTSInfoClass.WTSDomainName, out buffer, out strLen) && strLen > 1)
                {
                    username = Marshal.PtrToStringAnsi(buffer) + "\\" + username; // prepend domain name
                    WTSFreeMemory(buffer);
                }
            }
            return username;
        }

        //这里是测试当前激活用户的sessioid
        public static void Demo()
        {
            var ppSessionInfo = IntPtr.Zero;
            var sessionCount = 0;
            var hasSession = Win32Api.WTSEnumerateSessions(IntPtr.Zero, 0, 1, ref ppSessionInfo, ref sessionCount) != 0;//获取当前所有session

            for (var count = 0; count < sessionCount; count++)
            {
                var si = (WinApi.WTS_SESSION_INFO) Marshal.PtrToStructure(
                    ppSessionInfo + count * Marshal.SizeOf(typeof(WinApi.WTS_SESSION_INFO)),
                    typeof(WinApi.WTS_SESSION_INFO));

                //这里只有 服务可以跑
                if (si.State != WinApi.WTS_CONNECTSTATE_CLASS.WTSActive) Console.WriteLine();
            }
        }
    }
}
