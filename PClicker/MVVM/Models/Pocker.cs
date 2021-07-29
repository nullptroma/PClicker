using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Windows;

namespace PClicker.MVVM.Models
{
    class Pocker
    {
        private static int PockersCount = 0;
        public WindowHandle Window { get; set; }
        public int Id { get; }
        private bool enable;
        public bool Enable 
        {
            get { return enable; }
            set
            {
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
            Timer.Elapsed += (sender, e)=> Timer.Enabled = Enable;
            Timer.AutoReset = false;
        }


        private static object locker;
        private void Process(object sender, ElapsedEventArgs e)
        {
            lock (locker)
            {
                var bmp = Tools.WindowScreenshot.PrintWindow(Window.Handle, 900);
                bmp.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@$"\screen{Id}.jpg");
            }
        }
    }
}
