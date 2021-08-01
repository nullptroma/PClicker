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
            WinAPI.ShowWindow(hwnd);

            if (endHeigth != -1)
                ResizeWindow(hwnd, endHeigth);

            WinAPI.RECT rc;
            WinAPI.GetWindowRect(hwnd, out rc);

            Bitmap bmp = new Bitmap(rc.Width, rc.Height, PixelFormat.Format32bppArgb);
            Graphics gfxBmp = Graphics.FromImage(bmp);
            IntPtr hdcBitmap = gfxBmp.GetHdc();

            WinAPI.PrintWindow(hwnd, hdcBitmap, 0);

            gfxBmp.ReleaseHdc(hdcBitmap);
            gfxBmp.Dispose();

            return bmp;
        }

        public static void ResizeWindow(IntPtr hWnd, int endHeigth)
        {
            WinAPI.RECT rc;
            WinAPI.GetWindowRect(hWnd, out rc);
            if (endHeigth == rc.Height)
                return;
            double rate = rc.Height * 1.0 / rc.Width;
            int endWidth = (int)(endHeigth / rate);
            int newX = rc.X - (endWidth - rc.Width);
            int newY = rc.Y - (endHeigth - rc.Height);
            WinAPI.MoveWindow(hWnd, newX, newY, endWidth, endHeigth, true);
        }

        public static Bitmap GetRect(Bitmap bmp, Rectangle rect)
        {
            Bitmap screenRect = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(screenRect);
            g.DrawImage(bmp, 0, 0, rect, GraphicsUnit.Pixel);
            return screenRect;
        }
    }
}
