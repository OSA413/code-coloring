using CodeColoring.Colorizer;

namespace CodeColoring.OutputFormat
{
    public class HTMLPageSettings
    {
        public string Title { get; }
        public string Font { get; }
        public int FontSize { get; }
        public ColorPalette Palette { get; set; }

        public static HTMLPageSettings DefaultSettings() => new("Colored code", "serif", 14, new DefaultColorTheme());

        private HTMLPageSettings(string title, string font, int fontSize, ColorPalette palette)
        {
            Title = title;
            Font = font;
            FontSize = fontSize;
            Palette = palette;
        }
    }
}