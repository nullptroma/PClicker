using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PClicker.Commands
{
    class RaiseOrBetCmd : IPockerCmd
    {
        private bool IsInValid(string str)
        {
            string[] bets = new string[] { "рейз2х", "рейз3х", "рейз4х", "бет1/2банк", "бет2/3банк", "бетбанк" };
            foreach (string b in bets)
                if (b == str)
                    return false;
            return true;
        }

        public bool TryExecute(IntPtr wHandle, string str)
        {
            WinAPI.RECT wRect = WinAPI.GetWindowRect(wHandle);
            if (IsInValid(str))
                return false;
            RightDownClick(wRect);
            Thread.Sleep(Settings.SleepTime);

            switch (str)
            {
                case "бет1/2банк":
                case "рейз2х":
                    WinAPI.LeftClick(wRect.X + 70, wRect.Y + 950);
                    break;
                case "бет2/3банк":
                case "рейз3х":
                    WinAPI.LeftClick(wRect.X + 200, wRect.Y + 950);
                    break;
                case "бетбанк":
                case "рейз4х":
                    WinAPI.LeftClick(wRect.X + 310, wRect.Y + 950);
                    break;
            }
            Thread.Sleep(Settings.SleepTime);

            RightDownClick(wRect);
            return true;
        }

        private void RightDownClick(WinAPI.RECT wRect)
        {
            WinAPI.LeftClick(wRect.X + 500, wRect.Y + 950);
        }
    }
}
