using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SimulationMouseKeyboard
{
    public class Win32Stuff
    {
        #region Class Variables

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        public const Int32 CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon; // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
            public Int32 xHotspot; // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
            public Int32 yHotspot; // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
            public IntPtr hbmMask; // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
            public IntPtr hbmColor; // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize; // Specifies the size, in bytes, of the structure. 
            public Int32 flags; // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr hCursor; // Handle to the cursor. 
            public POINT ptScreenPos; // A POINT structure that receives the screen coordinates of the cursor. 
        }

        #endregion


        #region Class Functions

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);


        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleDC", SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC([In] IntPtr hdc);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("gdi32.dll", EntryPoint = "CreateCompatibleBitmap")]
        public static extern IntPtr CreateCompatibleBitmap([In] IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "SelectObject")]
        public static extern IntPtr SelectObject([In] IntPtr hdc, [In] IntPtr hgdiobj);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
        public static extern bool DeleteDC([In] IntPtr hdc);

        #endregion

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }
    }
}
