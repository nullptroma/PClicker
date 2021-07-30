using PClicker.MVVM.Models;
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
        private Pocker p = new Pocker();
        public WindowHandle Window
        {
            get { return p.Window; }
            set { p.Window = value; OnPropertyChanged("Window"); }
        }
        public int Id 
        { 
            get { return p.Id; }
        }
        public bool Enable
        { 
            get { return p.Enable; }
            set { p.Enable = value; OnPropertyChanged("Enable"); } 
        } 
        public string Command
        { 
            get { return p.Command; }
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
                  }));
            }
        }
        
        private RelayCommand saveCluePosCommand;
        public RelayCommand SaveCluePosCommand
        {
            get
            {
                return saveCluePosCommand ??
                  (saveCluePosCommand = new RelayCommand(obj =>
                  {
                      p.SaveCluePos();
                  }));
            }
        }

        public PockerViewModel()
        {
            p.PropertyChanged += (sender, e) => { OnPropertyChanged(e.PropertyName); };
        }
    }
}
