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
        private WindowHandle window;
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
        public int Id { get; }
        public string Command { get; private set; }
        private bool enable;
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
        private readonly Timer Timer = new Timer(1000);
        private CommandExecutor Ce = new CommandExecutor();
        private int CountClicks;
        public bool CheckBot { get; set; }

        public Pocker()
        {
            Id = PockersCount++;

            Timer.Elapsed += Process;
            Timer.AutoReset = false;
        }


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
                if (CheckBot && CountClicks % 4 == 0)
                    Ce.CenterClick();
                else
                {
                    Command = GetCommand();
                    OnPropertyChanged("Command");
                    Ce.ExecCmd(Command);
                }
            }
            finally
            {
                Timer.Enabled = Enable;
                CountClicks++;
            }
        }

        private string GetCommand()
        {
            var screen = Tools.WindowScreenshot.PrintWindow(Window.Handle, Settings.EndHeigth);
            Bitmap clue = Tools.WindowScreenshot.GetRect(screen, Settings.ClueRect);
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
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
