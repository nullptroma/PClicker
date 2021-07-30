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
        private static Rectangle ClueRect = new Rectangle(1,603, 361, 51);
        private static int EndHeigth = 1000;
        private static int PockersCount = 0;
        public WindowHandle Window { get; set; }
        public int Id { get; }
        public string Command { get; private set; }
        private bool enable;
        public bool Enable 
        {
            get { return enable; }
            set
            {
                if (Window.Handle == IntPtr.Zero)
                {
                    MessageBox.Show("Выберите окно", "Ошибка");
                    return;
                }
                enable = value;
                Timer.Enabled = value;
            }
        }
        private readonly Timer Timer = new Timer(1000);

        public Pocker()
        {
            locker = "LockStr";
            Id = PockersCount++;

            Timer.Elapsed += Process;
            Timer.AutoReset = false;
        }


        private static object locker;
        private void Process(object sender, ElapsedEventArgs e)
        {
            try
            {
                var screen = Tools.WindowScreenshot.PrintWindow(Window.Handle, EndHeigth);
                Bitmap clue = new Bitmap(ClueRect.Width, ClueRect.Height);
                Graphics g = Graphics.FromImage(clue);
                g.DrawImage(screen, 0, 0, ClueRect, GraphicsUnit.Pixel);
                Tools.FindCommand.DeleteNonWhite(clue);
                Command = Tools.FindCommand.GetCommand(clue);
                OnPropertyChanged("Command");

                string command = Tools.FindCommand.TextRus(clue) + "|" + Tools.FindCommand.TextEng(clue);
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\text.txt", command);
                clue.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ffff.jpg");
            }
            finally
            {
                Timer.Enabled = Enable;
            }
        }

        public void SaveCluePos()
        {
            if(Window.Handle == IntPtr.Zero)
            {
                MessageBox.Show("Выберите окно", "Ошибка");
                return;
            }
            var bmp = Tools.WindowScreenshot.PrintWindow(Window.Handle, EndHeigth);
            Graphics g = Graphics.FromImage(bmp);
            g.DrawRectangle(new Pen(Brushes.Red, 3), ClueRect);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @$"\PlaceForClue{Id}.jpg";
            bmp.Save(path);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
