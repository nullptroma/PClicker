using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using Tesseract;

namespace PClicker.Tools
{
    static class FindCommand
    {
        private static TesseractEngine engineRus = new TesseractEngine(@"C:\lang", "rus", EngineMode.Default);
        private static TesseractEngine engineEng = new TesseractEngine(@"C:\lang", "eng", EngineMode.Default);

        public static string TextRus(Bitmap bmp)
        {
            var page = TesseractDrawingExtensions.Process(engineRus, bmp);
            string txt = page.GetText();
            page.Dispose();
            return txt;
        }
        
        public static string TextEng(Bitmap bmp)
        {
            var page = TesseractDrawingExtensions.Process(engineEng, bmp);
            string txt = page.GetText();
            page.Dispose();
            return txt;
        }

        public static string GetCommand(Bitmap bmp)
        {
            string textRus = TextRus(bmp);
            string textEng = TextEng(bmp);
            string cmdRus = SwitchCommand(textRus);
            string cmdEng = SwitchCommand(textEng);
            string Cmd = string.IsNullOrEmpty(cmdRus) ? cmdEng : cmdRus;
            return textRus + "|" + textEng + "\n CMD: " + Cmd;
        }

        public static string SwitchCommand(string str)
        {
            str = str.Trim().Replace(" ", "").ToLower();
            string[] simpleCmds = new string[] { "фолд/чек", "фолд", "allin", "колл", "чек" };
            foreach (var cmd in simpleCmds)
                if (str.Contains(cmd))
                    return cmd;
            if (str.Contains("бет"))
            {
                string[] BetCmds = new string[] { "1/2банк", "2/3банк", "банк" };
                foreach (var cmd in BetCmds)
                    if (str.Contains(cmd))
                        return "бет" + cmd;
            }
            else if (str.Contains("рейз"))
            {
                string[] BetCmds = new string[] { "2х", "3х", "4х" };
                foreach (var cmd in BetCmds)
                    if (str.Contains(cmd))
                        return "рейз" + cmd;
            }
            return "";
        }

        public static void DeleteNonWhite(Bitmap bmp)
        {
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                {
                    var c = bmp.GetPixel(x, y);
                    if (c.A != 255 || c.R != 255 || c.G != 255 || c.B != 255)
                        bmp.SetPixel(x,y,Color.Black);
                }
        }
    }
}
