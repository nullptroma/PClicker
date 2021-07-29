using System;
using System.Diagnostics;
using System.Drawing;
using Tesseract;

namespace PClicker.Tools
{
    static class FindText
    {
        private static TesseractEngine engine = new TesseractEngine(@"C:\lang", "eng", EngineMode.TesseractAndLstm);
        public static string Text(Bitmap bmp)
        {
            var page = TesseractDrawingExtensions.Process(engine, bmp);
            string txt = page.GetText();
            page.Dispose();
            return txt;
        }
    }
}
