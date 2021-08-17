using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using Tesseract;

namespace PClicker.Tools
{
    static class FindCommand
    {
        private static TesseractEngine engineRus = new TesseractEngine(System.IO.Directory.GetCurrentDirectory()+@"\Lang", "rus", EngineMode.Default);
        private static TesseractEngine engineEng = new TesseractEngine(System.IO.Directory.GetCurrentDirectory()+@"\Lang", "eng", EngineMode.Default);

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
            return Cmd;
        }

        public static string SwitchCommand(string str)
        {
            str = str.Trim().Replace(" ", "").ToLower();
            string[] Ignore = new string[] { "фолд/чек", "donp/yek" };
            foreach (var cmd in Ignore)
                if (str.Contains(cmd))
                    return "";

            string[] simpleCmds = new string[] {  "фолд", "allin", "колл", "чек" };
            foreach (var cmd in simpleCmds)
                if (str.Contains(cmd))
                    return cmd;

            (string, string)[] Crutches = new (string, string)[] { ("yek", "чек"),  };
            {
                foreach (var cmd in Crutches)
                    if (str.Contains(cmd.Item1))
                        return cmd.Item2;
            }

            if (str.Contains("бет"))
            {
                string[] BetCmds = new string[] { "1/2банк", "2/3банк", "2/збанк"  };
                foreach (var cmd in BetCmds)
                    if (str.Contains(cmd))
                        return "бет" + cmd.Replace("з", "3");
                return "бетбанк";
            }
            else if (str.Contains("рейз"))
            {
                string[] BetCmds = new string[] { "2х", "3х", "зх", "4х", "банк" };
                foreach (var cmd in BetCmds)
                    if (str.Contains(cmd))
                        return "рейз" + cmd.Replace("з", "3");
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
