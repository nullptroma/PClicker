using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace PClicker.MVVM.Models
{
    class Pocker
    {
        private static int PockersCount = 0;
        public WindowHandle Window { get; set; }
        public int Id { get; }
        public Timer T { get; } = new Timer(1000);

        public Pocker()
        {
            Id = PockersCount++;

            T.Elapsed += Process;
            T.AutoReset = true;
        }

        private void Process(object sender, ElapsedEventArgs e)
        {

        }
    }
}
