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

            WinAPI.LeftClick(wRect.X + 435, wRect.Y + 485);
            Thread.Sleep(Settings.SleepTime);

            for (int i = 0; i < 6; i++)
            {
                WinAPI.LeftClick(wRect.X + 360, wRect.Y + 700);
                Thread.Sleep(75);
            }

            RightDownClick(wRect);
            Thread.Sleep(Settings.SleepTime);
            return true;
        }

        private static void RightDownClick(WinAPI.RECT wRect)
        {
            WinAPI.LeftClick(wRect.X + 500, wRect.Y + 950);
        }
    }
}
