using System;
using System.Text;
using System.Linq;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring.OutputFormat
{
    public class HTML : IOutputFormat
    {
        private Colorizer.Colorizer colorizer;
        public HTML(Colorizer.Colorizer colorizer) => this.colorizer = colorizer;
        public string Format(ParsingResult pu, ColorPalette palette)
        {
            var result = new StringBuilder();
            var title = "Colored code";
            var tab = 4;

            result.Append("<!DOCTYPE HTML>\n");
            result.Append("<html>\n");

            result.Append(FormatHeader(title, tab));
            result.Append(FormatBody(pu, palette, tab));

            result.Append("</html>\n");
            return result.ToString();
        }

        private string FormatBody(ParsingResult pu, ColorPalette palette, int tab)
        {
            var body = new StringBuilder();
            ParseUnit unit;
            int space = 1;

            body.Append(new String(' ', tab) + "<body>\n");
            body.Append(new String(' ', tab * 2) + "<code>" + "<pre>\n");
            for(int i = 0; i < pu.Result.Count; i++)
            {
                unit = pu.Result[i];
                if (i < pu.Result.Count - 1)
                    space = unit.Symbol.Equals("(") || pu.Result[i + 1].Symbol.Any(c => !Char.IsLetterOrDigit(c)) ? 0 : 1;
                body.Append(FormatUnit(unit, palette) + new String(' ', space));
            }
            body.Append(new String(' ', tab * 2) + "\n</code>" + "</pre>\n");
            body.Append(new String(' ', tab) + "</body>\n");
            return body.ToString();
        }

        private string FormatHeader(string pageTitle, int tab)
        {
            var header = new StringBuilder();
            header.Append(new String(' ', tab) + "<head>\n");
                header.Append(new String(' ', tab * 2) + $"<title>{pageTitle}</title>\n");
                header.Append(new String(' ', tab * 2) + "<meta charset=\"UTF-8\">\n");
            header.Append(new String(' ', tab) + "</head>\n");
            return header.ToString();
        }

        private string FormatUnit(ParseUnit unit, ColorPalette palette)
        {
            var color = colorizer.Colorize(unit.Unit, palette);
            return $"<span style=\"color:rgb({color.R},{color.G},{color.B})\">{unit.Symbol}</span>";
        }
    }
}
