using System;
using CodeColoring.Colorizer;
using System.Text;
using System.Linq;

namespace CodeColoring.OutputFormat
{
    public class HTML : IOutputFormat
    {
        public string Format(ColoringResult cr)
        {
            var result = new StringBuilder();
            var title = "Colored code";
            var tab = 4;

            result.Append("<!DOCTYPE HTML>\n");
            result.Append("<html>\n");

            result.Append(FormatHeader(title, tab));
            result.Append(FormatBody(cr, tab));

            result.Append("</html>\n");
            return result.ToString();
        }

        private static string FormatBody(ColoringResult cr, int tab)
        {
            var body = new StringBuilder();
            ColorizedArgument unit;
            int space = 1;

            body.Append(new String(' ', tab) + "<body>\n");
            body.Append(new String(' ', tab * 2) + "<code>" + "<pre>\n");
            for(int i = 0; i < cr.Result.Count; i++)
            {
                unit = cr.Result[i];
                if (i < cr.Result.Count - 1)
                    space = unit.Argument.Equals("(") || cr.Result[i + 1].Argument.Any(c => !Char.IsLetterOrDigit(c)) ? 0 : 1;
                body.Append(FormatUnit(unit) + new String(' ', space));
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

        private static string FormatUnit(ColorizedArgument unit) => $"<span style=\"color:rgb({unit.ArgumentColor.R},{unit.ArgumentColor.G},{unit.ArgumentColor.B})\">{unit.Argument}</span>";
    }
}
