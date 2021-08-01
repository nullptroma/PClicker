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
        private static IPockerCmd[] Commands = new IPockerCmd[]
        {
            new FoldCmd(),
            new CheckOrCallCmd(),
            new RaiseOrBetCmd(),
            new AllInCmdType1(),
            new AllInCmdType2(),
        };
        public IntPtr WindowHandle;

        private static object locker = "LockStr";
        public void ExecCmd(string cmdStr)
        {
            if (WindowHandle == IntPtr.Zero)
                return;
            lock (locker)
            {
                string execCmdStr = ToExec(cmdStr);
                if (string.IsNullOrEmpty(execCmdStr))
                    return;
                WinAPI.ShowWindow(WindowHandle);
                WinAPI.SetForegroundWindow(WindowHandle);
                WinAPI.RECT wRect = WinAPI.GetWindowRect(WindowHandle);
                WinAPI.LeftClick(wRect.X + 268, wRect.Y + 282);
                Thread.Sleep(Settings.SleepTime);
                foreach (var cmd in Commands)
                    if (cmd.TryExecute(WindowHandle, execCmdStr))
                        break;
            }
        }

        private string pastCmd;
        private string pastExecutedCmd;
        private string ToExec(string cmd)
        {
            if (cmd == pastCmd)
            {
                if (cmd != pastExecutedCmd)
                {
                    pastExecutedCmd = cmd;
                    if (cmd == "allin")
                        return cmd + "1";
                    return cmd;
                }
                else if(cmd == "allin")
                {
                    return "allin2";
                }
            }
            if (string.IsNullOrEmpty(cmd))
                pastExecutedCmd = cmd;
            pastCmd = cmd;
            return "";
        }
    }
}
