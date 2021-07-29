using System;
using System.Diagnostics;
using System.Drawing;
using Tesseract;

namespace PClicker.Tools
{
    static class FindText
    {
        private static TesseractEngine engineRus = new TesseractEngine(@"C:\lang", "rus", EngineMode.TesseractAndLstm);
        private static TesseractEngine engineEng = new TesseractEngine(@"C:\lang", "eng", EngineMode.TesseractAndLstm);

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
    }
}
