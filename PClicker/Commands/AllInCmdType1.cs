using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace PClicker.Commands
{
    class AllInCmdType1 : IPockerCmd
    {
        public bool IsInValid(string str)
        {
            return str != "allin1";
        }

        public bool TryExecute(IntPtr wHandle, string str)
        {
            WinAPI.RECT wRect = WinAPI.GetWindowRect(wHandle);
            if (IsInValid(str))
                return false;
            RightDownClick(wRect);
            Thread.Sleep(Settings.SleepTime);

            Point scrollerPos = ScrollPos(wHandle, wRect);
            WinAPI.SetCursorPos(scrollerPos.X, scrollerPos.Y);
            WinAPI.mouse_event(WinAPI.MouseEventFlags.LEFTDOWN,0,0,0,0);

            for (int i = 0; i < 275; i += 1)
                WinAPI.mouse_event(WinAPI.MouseEventFlags.MOVE, 0, -2, 0, 0);
            Thread.Sleep(Settings.SleepTime);

            WinAPI.mouse_event(WinAPI.MouseEventFlags.LEFTUP,0,0,0,0);
            Thread.Sleep(Settings.SleepTime);


            RightDownClick(wRect);
            Thread.Sleep(Settings.SleepTime);
            return true;
        }

        private static void RightDownClick(WinAPI.RECT wRect)
        {
            WinAPI.SetCursorPos(wRect.X + 500, wRect.Y + 950);
            WinAPI.LeftClick();
        }

        private static Point ScrollPos(IntPtr wHandle, WinAPI.RECT wRect)
        {
            Bitmap screen = Tools.WindowScreenshot.PrintWindow(wHandle, Settings.EndHeigth);
            Bitmap scroll = Tools.WindowScreenshot.GetRect(screen, Settings.ScrollRect);
            Color needC = Color.FromArgb(189, 199, 206);
            Point answer = new Point();
            for (int y = 0; y < scroll.Height; y++)
            {
                Color c = scroll.GetPixel(0, y);
                if (c == needC)
                    answer = new Point(wRect.X+ Settings.ScrollRect.X, wRect.Y+ Settings.ScrollRect.Y+y);
            }
            return answer;
        }
    }
}
