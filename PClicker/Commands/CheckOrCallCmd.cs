using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PClicker.Commands
{
    class CheckOrCallCmd : IPockerCmd
    {
        private bool IsInValid(string str)
        {
            return str != "чек" && str != "колл";
        }

        public bool TryExecute(IntPtr wHandle, string str)
        {
            WinAPI.RECT wRect = WinAPI.GetWindowRect(wHandle);
            if (IsInValid(str))
                return false;
            WinAPI.SetCursorPos(wRect.X + 260, wRect.Y + 950);
            WinAPI.LeftClick();
            return true;
        }
    }
}
