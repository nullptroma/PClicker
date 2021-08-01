using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PClicker
{
    static partial class WinAPI
    {
        public static void LeftClick()
        {
            WinAPI.mouse_event(WinAPI.MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(125);
            WinAPI.mouse_event(WinAPI.MouseEventFlags.LEFTUP, 0, 0, 0, 0);
        }
    }
}
