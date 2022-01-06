using System.Text;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;
using System.Collections.Generic;

namespace CodeColoring.OutputFormat
{
    public class HTML : IOutputFormat
    {
        public string Name => "HTML";
        private readonly HTMLPageSettings pageSettings = HTMLPageSettings.DefaultSettings();

        private readonly StringBuilder sb = new();
        private readonly HashSet<char> flattenCharExceptions = new() { '<', '>', '/', '=', '{', '}', ',' };
        public string Flatten(string text)
        {
            var afterSpace = false;
            var lastMeaningChar = ' ';
            foreach (var c in text)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!flattenCharExceptions.Contains(lastMeaningChar))
                        afterSpace = true;
                    continue;
                }

                if (!flattenCharExceptions.Contains(c) && afterSpace)
                    sb.Append(' ');
                sb.Append(c);
                lastMeaningChar = c;
                afterSpace = false;
            }

            var result = sb.ToString();
            sb.Clear();
            return result;
        }

        public string Format(ParsingResult parsed, ColorPalette palette)
        {
            palette ??= new DefaultColorTheme();
            pageSettings.Palette = palette;
            var resultBuilder = new StringBuilder();

            resultBuilder.Append("<!DOCTYPE HTML>\n");
            using (TagToken.Tag("html", resultBuilder))
            {
                resultBuilder.Append(FormatHeader(pageSettings.Palette,
                    pageSettings.Title,
                    pageSettings.Font,
                    pageSettings.FontSize));
                if (parsed != null)
                    FormatBody(resultBuilder, parsed);
            }

            return resultBuilder.ToString();
        }

        private static void FormatBody(StringBuilder sb, ParsingResult parsed)
        {
            sb.Append("<body>\n");
            sb.Append("<code>" + "<pre>\n");
            foreach(var unit in parsed.Result)
                sb.Append(FormatUnit(unit));
            sb.Append("\n</pre>" + "</code>\n");
            sb.Append("</body>\n");
        }

        private static string FormatHeader(ColorPalette palette, string pageTitle, string font, int size)
        {
            var header = new StringBuilder();
            using (TagToken.Tag("head", header)){
                header.Append($"<title>{pageTitle}</title>\n");
                header.Append("<meta charset=\"UTF-8\">\n");
                header.Append("<style>\n");
                    header.Append($"body {{background-color: rgb({palette.BackgroundColor.R},{palette.BackgroundColor.G},{palette.BackgroundColor.B}); font-family: {font}; font-size: {size}px; line-height: 1;}}\n");
                    header.Append($".Function {{color: rgb({palette.FunctionColor.R},{palette.FunctionColor.G},{palette.FunctionColor.B});}}\n");
                    header.Append($".Comment {{color: rgb({palette.CommentColor.R},{palette.CommentColor.G},{palette.CommentColor.B});}}\n");
                    header.Append($".FunctionDefinition {{color: rgb({palette.FunctionDefinitionColor.R},{palette.FunctionDefinitionColor.G},{palette.FunctionDefinitionColor.B});}}\n");
                    header.Append($".Operator {{color: rgb({palette.OperatorColor.R},{palette.OperatorColor.G},{palette.OperatorColor.B});}}\n");
                    header.Append($".Symbol {{color: rgb({palette.SymbolColor.R},{palette.SymbolColor.G},{palette.SymbolColor.B});}}\n");
                    header.Append($".Variable {{color: rgb({palette.VariableColor.R},{palette.VariableColor.G},{palette.VariableColor.B});}}\n");
                    header.Append($".Value {{color: rgb({palette.ValueColor.R},{palette.ValueColor.G},{palette.ValueColor.B});}}\n");
                    header.Append($".Whitespace {{color: rgb({palette.WhitespaceColor.R},{palette.WhitespaceColor.G},{palette.WhitespaceColor.B});}}\n");
                    header.Append($".Unknown {{color: rgb({palette.UnknownColor.R},{palette.UnknownColor.G},{palette.UnknownColor.B});}}\n");
                header.Append("</style>\n");
            }
            return header.ToString();
        }

        private static string FormatUnit(ParseUnit unit) => $"<span class=\"{unit.Unit}\">{unit.Symbol}</span>";
    }
}
