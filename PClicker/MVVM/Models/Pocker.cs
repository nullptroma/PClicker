using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.ComponentModel;
using System.IO;

namespace PClicker.MVVM.Models
{
    class Pocker : INotifyPropertyChanged
    {
        private static int PockersCount = 0;

        public int Id { get; }
        public WindowHandle Window { get; set; }
        public bool Enable { get; private set; }
        public bool CheckBot { get; set; }
        public int MaxPlayers
        {
            get => FinderNotifi.MaxPlayers;
            set => FinderNotifi.MaxPlayers=value;
        }
        public string Action { get; private set; }
        public string TelegramID { get; set; }
        public string Note { get; set; }

        private readonly Timer Timer = new Timer(1000);
        private int CountTicks;
        private CommandExecutor Executor = new CommandExecutor();
        private NotifiFinder FinderNotifi = new NotifiFinder();
        private TelegramNotifi SenderNotifi = new TelegramNotifi();
        private ChipReplenisher Replenisher = new ChipReplenisher();

        public Pocker()
        {
            Id = PockersCount++;
            MaxPlayers = 6;
            Timer.Elapsed += Tick;
            Timer.AutoReset = false;
        }

        public void SetEnable(bool enable)
        {
            if (Window.Handle == IntPtr.Zero && enable)
            {
                MessageBox.Show("Выберите окно", "Ошибка");
                return;
            }
            Enable = enable;
            Timer.Enabled = enable;
            OnPropertyChanged("Enable");
        }

        private static object locker = "LockStr";
        private void Tick(object sender, ElapsedEventArgs e)
        {
            try
            {
                lock (locker)
                    Process();
            }
            finally
            {
                Timer.Enabled = Enable;
                CountTicks++;
            }
        }

        private void Process()
        {
            if (!WinAPI.IsWindow(Window.Handle))
            {
                Enable = false;
                Window = new WindowHandle();
                OnPropertyChanged("Window");
                return;
            }

            Bitmap pockerWindows = Tools.WindowScreenshot.PrintWindow(Window.Handle, Settings.EndHeigth);
            Action = GetCommand(pockerWindows);

            Bitmap pockerClone = Tools.WindowScreenshot.PrintWindow(Window.Handle, Settings.EndHeigth);
            NotifiTypes notifi = FinderNotifi.Find(pockerClone);
            if (notifi == NotifiTypes.FoldButtonVisible)
                if (string.IsNullOrEmpty(Action) == false)
                    notifi = notifi & (~NotifiTypes.FoldButtonVisible);
            if (notifi != NotifiTypes.None)
                SenderNotifi.SendMessage($"Стол:\"{Note}\" {notifi}", TelegramID);

            if (CheckBot && CountTicks % 4 == 0)
                Executor.CenterClick(Window.Handle);
            else
            {
                OnPropertyChanged("Action");
                Executor.ExecCmd(Action, Window.Handle);
            }
        }

        private string GetCommand(Bitmap pockerWindow)
        {
            Bitmap clue = Tools.WindowScreenshot.GetRect(pockerWindow, Settings.ClueRect);
            clue.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\pocker" + Id + ".png");
            Tools.FindCommand.DeleteNonWhite(clue);
            return Tools.FindCommand.GetCommand(clue);
        }

        public void SaveCluePos()
        {
            if(Window.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Выберите окно", "Ошибка");
                return;
            }
            try
            {
                var bmp = Tools.WindowScreenshot.PrintWindow(Window.Handle, Settings.EndHeigth);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawRectangle(new Pen(Brushes.Red, 3), Settings.ClueRect);
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\PlaceForClue{Id}.jpg";
                bmp.Save(path);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
