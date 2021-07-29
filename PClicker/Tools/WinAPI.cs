using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;

namespace PClicker.Tools
{
    static class WinAPI
    {
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        public static string GetWindowText(IntPtr hWnd)
        {
            StringBuilder sb = new StringBuilder(256);
            GetWindowText(hWnd, sb, 256);
            return sb.ToString();
        }

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rectangle lpRect);
        public static Rectangle GetWindowRect(IntPtr hwnd)
        {
            Rectangle rect;
            GetWindowRect(hwnd, out rect);
            rect.Width -= rect.X;
            rect.Height-= rect.Y;
            return rect;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }
}
