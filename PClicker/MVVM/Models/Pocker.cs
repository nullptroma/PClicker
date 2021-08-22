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
        public int Id { get; }
        public WindowHandle Window
        {
            get { return window; }
            set
            {
                window = value;
                Ce.WindowHandle = window.Handle;
                OnPropertyChanged("Window");
            }
        }
        public bool Enable
        {
            get { return enable; }
            set
            {
                if (Window.Handle == IntPtr.Zero && value)
                {
                    MessageBox.Show("Выберите окно", "Ошибка");
                    return;
                }
                enable = value;
                Timer.Enabled = value;
                OnPropertyChanged("Enable");
            }
        }
        public bool CheckBot { get; set; }
        public int maxPlayers;
        public int MaxPlayers
        {
            get => maxPlayers;
            set
            {
                maxPlayers = value;
                Notifi.MaxPlayers = MaxPlayers;
            }
        }
        public string Action { get; private set; }
        public string TelegramID { get; set; }
        public string Note { get; set; }

        private readonly Timer Timer = new Timer(1000);
        private static int PockersCount = 0;
        private WindowHandle window;
        private CommandExecutor Ce = new CommandExecutor();
        private bool enable;
        private NotifiFinder Notifi = new NotifiFinder();
        private int CountTicks;
        private TelegramNotifi tn = new TelegramNotifi();

        public Pocker()
        {
            Id = PockersCount++;
            MaxPlayers = 6;
            Timer.Elapsed += Process;
            Timer.AutoReset = false;
        }

        private static object locker = "LockStr";
        private void Process(object sender, ElapsedEventArgs e)
        {
            if (!WinAPI.IsWindow(Window.Handle))
            {
                Enable = false;
                Window = new WindowHandle();
                return;
            }
            try
            {
                lock (locker)
                {
                    Bitmap pockerWindows = Tools.WindowScreenshot.PrintWindow(Window.Handle, Settings.EndHeigth);
                    Action = GetCommand(pockerWindows);

                    Bitmap pockerClone = Tools.WindowScreenshot.PrintWindow(Window.Handle, Settings.EndHeigth);
                    NotifiTypes notifi = Notifi.Find(pockerClone);
                    if (notifi == NotifiTypes.FoldButtonVisible)
                        if (string.IsNullOrEmpty(Action) == false)
                            notifi = notifi & (~NotifiTypes.FoldButtonVisible);
                    if (notifi != NotifiTypes.None)
                        tn.SendMessage($"Стол:\"{Note}\" {notifi}", TelegramID);

                    if (CheckBot && CountTicks % 4 == 0)
                        Ce.CenterClick();
                    else
                    {
                        OnPropertyChanged("Action");
                        Ce.ExecCmd(Action);
                    }
                }
            }
            finally
            {
                Timer.Enabled = Enable;
                CountTicks++;
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
