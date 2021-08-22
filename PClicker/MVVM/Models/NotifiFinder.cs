using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PClicker.MVVM.Models
{
    class NotifiFinder
    {
        public int MaxPlayers { get; set; }

        private Test[] Tests;

        public NotifiFinder()
        {
            Tests = new Test[]
            {
                new Test() { Checker = this.LessThan3Players, Notifi=NotifiTypes.LessThan3Players},
                new Test() { Checker = this.NeedCaptcha, Notifi=NotifiTypes.Captcha},
                new Test() { Checker = this.FoldButtonVisible, Notifi=NotifiTypes.FoldButtonVisible},
                new Test() { Checker = this.FiveCoins, Notifi=NotifiTypes.FiveCoins},
            };
        }

        private bool WasFold = false;
        private int ChecksСount = 0;
        public NotifiTypes Find(Bitmap pockerScreen)
        {
            NotifiTypes Notifi = NotifiTypes.None;
            foreach (var t in Tests)
                if (t.Checker(pockerScreen))
                {
                    Notifi |= t.Notifi;
                    Notifi = Notifi & (~NotifiTypes.None);
                }
            ChecksСount++;
            if (Notifi.HasFlag(NotifiTypes.FoldButtonVisible))
            {

                if (WasFold)
                    return Notifi;
                else
                {
                    WasFold = true;
                    return Notifi & (~NotifiTypes.FoldButtonVisible);
                }
            }
            WasFold = false;
            return Notifi;
        }

        private static Bitmap Empty = new Bitmap("Resources/Empty.png");
        private bool LessThan3Players(Bitmap pockerScreen)
        {
            if (ChecksСount % 10 != 0)
                return false;
            Bitmap clone = new Bitmap(pockerScreen.Width, pockerScreen.Height,
                PixelFormat.Format24bppRgb);
            using (Graphics gr = Graphics.FromImage(clone))
                gr.DrawImage(pockerScreen, new Rectangle(0, 0, clone.Width, clone.Height));
            try
            {
                Tools.AForge.AforgeService a = new Tools.AForge.AforgeService();
                var places = a.GetPlaces(clone, Empty).Result;
                return (MaxPlayers - places.Count) < 3;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            return false;
        }
        
        private bool NeedCaptcha(Bitmap pockerScreen)
        {
            if (pockerScreen.GetPixel(260, 500) == Color.FromArgb(255, 33, 81, 57))//цвет ободка для любой капчи
                return true;
            return false;
        }
        
        private bool FoldButtonVisible(Bitmap pockerScreen)
        {
            if (pockerScreen.GetPixel(55, 955) == Color.FromArgb(255, 156, 38, 8))
                return true;
            return false;
        }
        
        private bool FiveCoins(Bitmap pockerScreen)
        {
            Bitmap bal = Tools.WindowScreenshot.GetRect(pockerScreen, new Rectangle(451, 124, 82, 28));

            if(double.TryParse(Tools.FindCommand.TextEngFast(bal).Trim().Replace(".", ","), out double d))
                if (d <= 5)
                    return true;
            return false;
        }

        private struct Test
        {
            public Func<Bitmap, bool> Checker;
            public NotifiTypes Notifi;
        }
    }

    [Flags]
    public enum NotifiTypes
    {
        None=0,
        LessThan3Players=1,
        Captcha=2,
        FoldButtonVisible=4,
        FiveCoins=8,
    }
}
