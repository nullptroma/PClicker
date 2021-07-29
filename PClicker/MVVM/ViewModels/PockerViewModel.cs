using PClicker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PClicker.ViewModels
{
    class PockerViewModel : BaseViewModel
    {
        private static int PockersCount = 0;
        public WindowHandle Window { get; set; }
        public int Id { get; }
        public bool Enable { get; set; }

        public PockerViewModel()
        {
            Id = PockersCount++;
        }

        private RelayCommand changeEnableCommand;
        public RelayCommand ChangeEnableCommand
        {
            get
            {
                return changeEnableCommand ??
                  (changeEnableCommand = new RelayCommand(obj =>
                  {
                      Enable = !Enable;
                      OnPropertyChanged("Enable");
                  }));
            }
        }
    }
}
