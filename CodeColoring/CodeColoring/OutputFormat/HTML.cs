using System;
using System.Text;
using System.Linq;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;
using System.Drawing;

namespace CodeColoring.OutputFormat
{
    public class HTML : IOutputFormat
    {
        HTMLPageSettings pageSettings = HTMLPageSettings.DefaultSettings();
        public string Format(ParsingResult parsed, ColorPalette palette)
        {
            var result = new StringBuilder();
            pageSettings.Palette = palette;

            result.Append("<!DOCTYPE HTML>\n");
            result.Append("<html>\n");

            result.Append(FormatHeader(pageSettings.Palette,  
                pageSettings.Title, 
                pageSettings.BackgroundColor, 
                pageSettings.Font, 
                pageSettings.FontSize, 
                pageSettings.TabSize));
            result.Append(FormatBody(parsed, pageSettings.TabSize));

            result.Append("</html>\n");
            return result.ToString();
        }

        private string FormatBody(ParsingResult parsed, int tab)
        {
            var body = new StringBuilder();
            body.Append(new String(' ', tab) + "<body>\n");
            body.Append(new String(' ', tab * 2) + "<code>" + "<pre>\n");
            foreach(var unit in parsed.Result)
                body.Append(FormatUnit(unit));
            body.Append(new String(' ', tab * 2) + "\n</code>" + "</pre>\n");
            body.Append(new String(' ', tab) + "</body>\n");
            return body.ToString();
        }

        private string FormatHeader(ColorPalette palette, string pageTitle, Color backColor, string font, int size, int tab)
        {
            var header = new StringBuilder();
            header.Append(new String(' ', tab) + "<head>\n");
                header.Append(new String(' ', tab * 2) + $"<title>{pageTitle}</title>\n");
                header.Append(new String(' ', tab * 2) + "<meta charset=\"UTF-8\">\n");
                header.Append(new String(' ', tab * 2) + "<style>\n");
                    header.Append(new String(' ', tab * 3) + $"body {{background-color: rgb({backColor.R},{backColor.G},{backColor.B}); font-family: {font}; font-size: {size};}}\n");
                    header.Append(new String(' ', tab * 3) + $".Function {{color: rgb({palette.FunctionColor.R},{palette.FunctionColor.G},{palette.FunctionColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Comment {{color: rgb({palette.CommentColor.R},{palette.CommentColor.G},{palette.CommentColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".FunctionDefinition {{color: rgb({palette.FunctionDefinitionColor.R},{palette.FunctionDefinitionColor.G},{palette.FunctionDefinitionColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Operator {{color: rgb({palette.OperatorColor.R},{palette.OperatorColor.G},{palette.OperatorColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Symbol {{color: rgb({palette.SymbolColor.R},{palette.SymbolColor.G},{palette.SymbolColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Variable {{color: rgb({palette.VariableColor.R},{palette.VariableColor.G},{palette.VariableColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Value {{color: rgb({palette.ValueColor.R},{palette.ValueColor.G},{palette.ValueColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Whitespace {{color: rgb({palette.WhitespaceColor.R},{palette.WhitespaceColor.G},{palette.WhitespaceColor.B});}}\n");
                    header.Append(new String(' ', tab * 3) + $".Unknown {{color: rgb({palette.UnknownColor.R},{palette.UnknownColor.G},{palette.UnknownColor.B});}}\n");
                header.Append(new String(' ', tab * 2) + "</style>\n");
            header.Append(new String(' ', tab) + "</head>\n");
            return header.ToString();
        }

        private string FormatUnit(ParseUnit unit) => $"<span class=\"{unit.Unit}\">{unit.Symbol}</span>";
    }

    public class HTMLPageSettings
    {
        public string Title { get; }
        public string Font { get; }
        public int FontSize { get; }
        public Color BackgroundColor { get; }
        public int TabSize { get; }
        public ColorPalette Palette { get; set; }

        public static HTMLPageSettings DefaultSettings() => new HTMLPageSettings("Colored code", "serif", 14, Color.White, 4, new DayTheme());

        public HTMLPageSettings(string title, string font, int fontSize, Color backgroundColor, int tabSize, ColorPalette palette)
        {
            Title = title;
            Font = font;
            FontSize = fontSize;
            BackgroundColor = backgroundColor;
            TabSize = tabSize;
            Palette = palette;
        }
    }
}
