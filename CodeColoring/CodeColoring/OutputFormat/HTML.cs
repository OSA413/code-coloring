using System;
using System.Text;
using System.Linq;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;
using System.Drawing;
using System.Collections.Generic;

namespace CodeColoring.OutputFormat
{
    public class HTML : IOutputFormat
    {
        public string Name => "HTML";
        HTMLPageSettings pageSettings = HTMLPageSettings.DefaultSettings();

        private readonly StringBuilder sb = new StringBuilder();
        private readonly HashSet<char> flattenCharExceptions = new HashSet<char>() { '<', '>', '/', '=', '{', '}', ',' };
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
            if (palette == null) palette = new DefaultColorPalette();
            pageSettings.Palette = palette;
            var resultBuilder = new StringBuilder();

            resultBuilder.Append("<!DOCTYPE HTML>\n");
            using (TagToken.Tag("html", resultBuilder, false, true, "", ""))
            {
                resultBuilder.Append(FormatHeader(pageSettings.Palette,
                    pageSettings.Title,
                    pageSettings.Font,
                    pageSettings.FontSize,
                    pageSettings.TabSize));
                if (parsed != null)
                    FormatBody(resultBuilder, parsed, pageSettings.TabSize);
            }

            return resultBuilder.ToString();
        }

        private void FormatBody(StringBuilder sb, ParsingResult parsed, int tab)
        {
            sb.Append(new String(' ', tab) + "<body>\n");
            sb.Append(new String(' ', tab * 2) + "<code>" + "<pre>\n");
            foreach(var unit in parsed.Result)
                sb.Append(FormatUnit(unit));
            sb.Append(new String(' ', tab * 2) + "\n</pre>" + "</code>\n");
            sb.Append(new String(' ', tab) + "</body>\n");
        }

        private string FormatHeader(ColorPalette palette, string pageTitle, string font, int size, int tab)
        {
            var header = new StringBuilder();
            using (TagToken.Tag("head", header, false, true, new String(' ', tab), new String(' ', tab))){
                header.Append(new String(' ', tab * 2) + $"<title>{pageTitle}</title>\n");
                header.Append(new String(' ', tab * 2) + "<meta charset=\"UTF-8\">\n");
                header.Append(new String(' ', tab * 2) + "<style>\n");
                    header.Append(new String(' ', tab * 3) + $"body {{background-color: rgb({palette.BackgroundColor.R},{palette.BackgroundColor.G},{palette.BackgroundColor.B}); font-family: {font}; font-size: {size}px; line-height: 1;}}\n");
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
            }
            return header.ToString();
        }

        private string FormatUnit(ParseUnit unit) => $"<span class=\"{unit.Unit}\">{unit.Symbol}</span>";
    }

    class TagToken : IDisposable
    {
        readonly StringBuilder builder;
        readonly string tag;
        bool newLineAtStart;
        bool newLineAtEnd;
        readonly string toAppendBeforeTag;
        readonly string toAppendAfterTag;

        public TagToken(string tag, StringBuilder builder, bool newLineAtStart, bool newLineAtEnd,
            string toAppendBeforeTag, string toAppendAfterTag)
        {
            this.builder = builder;
            this.tag = tag;
            this.newLineAtEnd = newLineAtEnd;
            this.newLineAtStart = newLineAtStart;
            this.toAppendAfterTag = toAppendAfterTag;
            this.toAppendBeforeTag = toAppendBeforeTag;
            if (newLineAtEnd)
                builder.Append(toAppendBeforeTag + String.Format("<{0}>\n", tag));
            else if (newLineAtStart)
                builder.Append(toAppendBeforeTag + String.Format("\n<{0}>", tag));
        }

        public void Dispose()
        {
            if (newLineAtEnd)
                builder.Append(toAppendAfterTag + String.Format("</{0}>\n", tag));
            else if (newLineAtStart)
                builder.Append(toAppendAfterTag + String.Format("\n</{0}>", tag));
        }

        public static TagToken Tag(string tag, StringBuilder builder, bool newLineAtStart, bool newLineAtEnd,
            string toAppendBeforeTag, string toAppendAfterTag)
        {
            return new TagToken(tag, builder, newLineAtStart, newLineAtEnd, toAppendBeforeTag, toAppendAfterTag);
        }
    }

    public class HTMLPageSettings
    {
        public string Title { get; }
        public string Font { get; }
        public int FontSize { get; }
        public int TabSize { get; }
        public ColorPalette Palette { get; set; }

        public static HTMLPageSettings DefaultSettings() => new HTMLPageSettings("Colored code", "serif", 14, 4, new DayTheme());

        public HTMLPageSettings(string title, string font, int fontSize, int tabSize, ColorPalette palette)
        {
            Title = title;
            Font = font;
            FontSize = fontSize;
            TabSize = tabSize;
            Palette = palette;
        }
    }
}
