using PClicker.Tools;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace PClicker.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public ObservableCollection<WindowHandle> AllWindows { get; set; } = new ObservableCollection<WindowHandle>();
        public ObservableCollection<PockerViewModel> Pockers { get; set; } = new ObservableCollection<PockerViewModel>();
        private PockerViewModel selectedPocker;
        public PockerViewModel SelectedPocker
        {
            get { return selectedPocker; }
            set
            {
                selectedPocker = value;
                OnPropertyChanged("SelectedPocker");
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand(obj =>
                  {
                      var p = new PockerViewModel();
                      Pockers.Add(p);
                      SelectedPocker = p;
                  }));
            }
        }

        private RelayCommand removeCommand;
        public RelayCommand RemoveCommand
        {
            get
            {
                return removeCommand ??
                  (removeCommand = new RelayCommand(obj =>
                  {
                      if (SelectedPocker != null)
                      {
                          var index = Pockers.IndexOf(SelectedPocker);
                          Pockers.Remove(SelectedPocker);
                          if (index > 0)
                              SelectedPocker = Pockers.Count > 0 ? Pockers[index - 1] : null;
                      }
                  }));
            }
        }
        
        private RelayCommand updateWindowsCommand;
        public RelayCommand UpdateWindowsCommand
        {
            get
            {
                return updateWindowsCommand ??
                  (updateWindowsCommand = new RelayCommand(obj =>
                  {
                      WriteAllWindows();
                  }));
            }
        }
        
        private RelayCommand testCommand;
        public RelayCommand TestCommand
        {
            get
            {
                return testCommand ??
                  (testCommand = new RelayCommand(obj=> 
                  {
                      var r = AllWindows.Where(wh => wh.Name.Contains("LDPlayer")).FirstOrDefault();
                      var b = WindowScreenshot.PrintWindow(r.Handle);
                      b.Save(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+@"\aaa.png");
                  }));
            }
        }

        public MainViewModel()
        {
            WriteAllWindows();
        }

        private void WriteAllWindows()
        {
            AllWindows.Clear();
            WinAPI.EnumWindows((hwnd, l) =>
            {
                var wh = new WindowHandle(hwnd);
                if (!string.IsNullOrEmpty(wh.Name.Trim()) && wh.Name.Contains("LDPlayer"))
                    AllWindows.Add(wh);
                return true;
            }, IntPtr.Zero);
            AllWindows = new ObservableCollection<WindowHandle>(AllWindows.OrderBy(wh => wh.Name));
            OnPropertyChanged("AllWindows");
        }
    }
}
