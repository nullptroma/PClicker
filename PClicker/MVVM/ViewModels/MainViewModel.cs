using PClicker.Tools;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading;

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
                          SelectedPocker.Enable = false;
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
                      Bitmap game = (Bitmap)Bitmap.FromFile(Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+ @"\рейзы.png");
                      Bitmap scroll = Tools.WindowScreenshot.GetRect(game, Settings.ScrollRect);
                      WinAPI.SetCursorPos(500, 1080);
                      //WinAPI.mouse_event(WinAPI.MouseEventFlags.LEFTDOWN, 0, 0, 0, 0);

                      for (int i = 0; i < 275; i += 1)
                          WinAPI.mouse_event(WinAPI.MouseEventFlags.MOVE, 0, -2, 0, 0);
                      Thread.Sleep(Settings.SleepTime);

                      WinAPI.mouse_event(WinAPI.MouseEventFlags.LEFTUP, 0, 0, 0, 0);
                      Thread.Sleep(Settings.SleepTime);
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
