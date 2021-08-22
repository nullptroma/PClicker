using AForge.Imaging;

namespace PClicker.Tools.AForge
{
    public class FoundPlace
    {
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double Similarity { get; set; }

        public FoundPlace(TemplateMatch match)
        {
            Similarity = match.Similarity;
            Top = match.Rectangle.Top;
            Left = match.Rectangle.Left;
            Height = match.Rectangle.Height;
            Width = match.Rectangle.Width;
        }
    }
}
