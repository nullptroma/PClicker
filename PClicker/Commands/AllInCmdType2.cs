using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PClicker.Commands
{
    class AllInCmdType2 : IPockerCmd
    {
        private bool IsInValid(string str)
        {
            return str != "allin2";
        }

        public bool TryExecute(IntPtr wHandle, string str)
        {
            WinAPI.RECT wRect = WinAPI.GetWindowRect(wHandle);
            if (IsInValid(str))
                return false;
            WinAPI.LeftClick(wRect.X + 260, wRect.Y + 950);
            return true;
        }
    }
}
