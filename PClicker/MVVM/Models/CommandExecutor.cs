using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PClicker.Commands; 

namespace PClicker.MVVM.Models
{
    class CommandExecutor
    {
        private static readonly IPockerCmd[] Commands = new IPockerCmd[]
        {
            new FoldCmd(),
            new CheckOrCallCmd(),
            new RaiseOrBetCmd(),
            new AllInCmdType1(),
        };

        public void ExecCmd(string cmdStr, IntPtr WindowHandle)
        {
            if (WindowHandle == IntPtr.Zero)
                return;
            string execCmdStr = ToExec(cmdStr);
            if (string.IsNullOrEmpty(execCmdStr))
                return;
            WinAPI.ShowWindow(WindowHandle);
            WinAPI.SetForegroundWindow(WindowHandle);
            foreach (var cmd in Commands)
                if (cmd.TryExecute(WindowHandle, execCmdStr))
                    break;
        }

        public void CenterClick(IntPtr WindowHandle)
        {
            WinAPI.RECT wRect = WinAPI.GetWindowRect(WindowHandle);
            WinAPI.LeftClick(wRect.X + 260, wRect.Y + 950);
        }

        private string pastExecutedCmd;
        private string ToExec(string cmd)
        {
            if (string.IsNullOrEmpty(cmd))
                return "";
            if (cmd == "allin")
            {
                if (pastExecutedCmd == "allin")
                    return "чек";
                else
                    return "allin1";
            }
            pastExecutedCmd = cmd;
            return cmd;
        }
    }
}
