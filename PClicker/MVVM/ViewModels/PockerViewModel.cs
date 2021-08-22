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
        public bool CheckBot
        { 
            get { return p.CheckBot; }
            set { p.CheckBot = value; OnPropertyChanged("CheckBot"); } 
        } 
        public string Action
        { 
            get { return p.Action; }
        }
        public int MaxPlayers
        { 
            get { return p.MaxPlayers; }
            set { p.MaxPlayers = value; OnPropertyChanged("MaxPlayers"); }
        } 
        
        public string TelegramID
        { 
            get { return p.TelegramID; }
            set { p.TelegramID = value; OnPropertyChanged("TelegramID"); }
        } 
        
        public string Note
        { 
            get { return p.Note; }
            set { p.Note = value; OnPropertyChanged("Note"); }
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
