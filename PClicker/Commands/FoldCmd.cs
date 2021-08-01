using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PClicker.Commands
{
    class FoldCmd : IPockerCmd
    {
        public bool IsInValid(string str)
        {
            return str != "фолд";
        }

        public bool TryExecute(IntPtr wHandle, string str)
        {
            WinAPI.RECT wRect = WinAPI.GetWindowRect(wHandle);
            if (IsInValid(str))
                return false;
            WinAPI.SetCursorPos(wRect.X + 85, wRect.Y + 950);
            WinAPI.LeftClick();
            return true;
        }
    }
}
