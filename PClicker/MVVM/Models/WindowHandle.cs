using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PClicker
{
    public struct WindowHandle
    {
        public string Name { get; }
        public IntPtr Handle { get; }

        public WindowHandle(IntPtr hwnd)
        {
            Handle = hwnd;
            Name = WinAPI.GetWindowText(hwnd);
        }

        public override string ToString()
        {
            return Name + " " + Handle;
        }
    }
}
