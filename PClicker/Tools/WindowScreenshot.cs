using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PClicker.Tools
{
    static class WindowScreenshot
    {
        public static Bitmap GetWindowScreen(IntPtr hWnd, int endHeigth)
        {
            Rectangle rect = WinAPI.GetWindowRect(hWnd);
            double rate = rect.Height*1.0 / rect.Width;
            int newX = rect.X - ((int)(endHeigth / rate) - rect.Width);
            int newY = rect.Y - (endHeigth-rect.Height);
            WinAPI.MoveWindow(hWnd, newX, newY, (int)(endHeigth / rate), endHeigth,  true);

            rect = WinAPI.GetWindowRect(hWnd);
            WinAPI.SetForegroundWindow(hWnd);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);
            return bmp;
        }
    }
}
