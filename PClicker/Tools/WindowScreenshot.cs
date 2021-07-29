using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PClicker.Tools
{
    static class WindowScreenshot
    {
        public static Bitmap PrintWindow(IntPtr hwnd, int endHeigth = -1)
        {
            WinAPI.WINDOWPLACEMENT wp = new WinAPI.WINDOWPLACEMENT();
            WinAPI.GetWindowPlacement(hwnd, ref wp);
            switch (wp.ShowCmd)
            {
                case WinAPI.ShowCommands.SW_HIDE:
                case WinAPI.ShowCommands.SW_MINIMIZE:
                case WinAPI.ShowCommands.SW_SHOWMINIMIZED:
                case WinAPI.ShowCommands.SW_SHOWMINNOACTIVE:
                case WinAPI.ShowCommands.SW_SHOWNA:
                case WinAPI.ShowCommands.SW_SHOWNOACTIVATE:
                case WinAPI.ShowCommands.SW_FORCEMINIMIZE:
                    WinAPI.ShowWindow(hwnd, WinAPI.ShowCommands.SW_NORMAL);
                    break;
            }

            WinAPI.RECT rc;
            if (endHeigth != -1)
            {
                WinAPI.GetWindowRect(hwnd, out rc);
                double rate = rc.Height * 1.0 / rc.Width;
                int endWidth = (int)(endHeigth / rate);
                int newX = rc.X - (endWidth - rc.Width);
                int newY = rc.Y - (endHeigth - rc.Height);
                WinAPI.MoveWindow(hwnd, newX, newY, endWidth, endHeigth, true);
            }

            WinAPI.GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            WinAPI.PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }
    }
}
