using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;

using NUnit.Framework;
using NUnit.Framework.Internal;

using CodeColoring;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring_Tests
{
    internal class HTML_Tests
    {
        private readonly Randomizer randomizer = new();
        private readonly IContainer container = Program.ConfigureContainer();
        private readonly HTML html;

        public HTML_Tests() => html = container.Resolve<HTML>();

        private string BuildHTML(string body = "")
            => "<!DOCTYPE HTML><html><head>" +
        "< title > Colored code</title>" +
"        <meta charset = \"UTF-8\" >" +
"        < style >" +
"            body {background-color: rgb(255,255,255); font-family: serif; font-size: 14px; line-height: 1;}" +
"            .Function {color: rgb(0,0,0);}" +
"            .Comment { color: rgb(0, 0, 0); }" +
"            .FunctionDefinition { color: rgb(0, 0, 0); }" +
"            .Operator { color: rgb(0, 0, 0); }" +
"            .Symbol { color: rgb(0, 0, 0); }" +
"            .Variable { color: rgb(0, 0, 0); }" +
"            .Value { color: rgb(0, 0, 0); }" +
"            .Whitespace { color: rgb(0, 0, 0); }" +
"            .Unknown { color: rgb(0, 0, 0); }" +
"        </ style > " +
"    </ head > " +
        body +
    "</html> ";

        public string BuildBody(string text = "") => "<body><code><pre>" + text + "</pre></code></body>";

        [Test]
        public void Internal_FlattenHTML() =>
            Assert.AreEqual("<><a b></><abc>", html.Flatten(" < >\n    <   a    \n    b >   </>\n<abc> "));

        [Test]
        public void AllNullsJustHead()
        {
            var actual = html.Format(null, null);
            Assert.AreEqual(html.Flatten(BuildHTML()), html.Flatten(actual));
        }

        [Test]
        public void EmptyParsingResultEmptyBody()
        {
            var actual = html.Format(new ParsingResult(), null);
            Assert.AreEqual(html.Flatten(BuildHTML(BuildBody())), html.Flatten(actual));
        }

        [Test]
        public void SingleParsingResult([Values] LanguageUnit unit)
        {
            var symbol = randomizer.GetString(7);
            var parsingResult = new ParsingResult();
            parsingResult.Result.Add(new ParseUnit(unit, symbol));
            var actual = html.Format(parsingResult, null);
            Assert.AreEqual(html.Flatten(BuildHTML(BuildBody(
                "<span class=\""+ Enum.GetName(typeof(LanguageUnit), unit) +"\">" + symbol + "</span>")))
                , html.Flatten(actual));
        }

        /*
        [Test]
        [Repeat(5)]
        public void OneArgColorization([Values] LanguageUnit arg)
        {
            var argName = randomizer.GetString(10);
            var oneArgArray = new ParseUnit[] { new(arg, argName) };
            var actual = Colorizer.Colorize(oneArgArray, palette);
            var expected = new ColoringResult();
            expected.Add(new ColorizedArgument(UnitToColorMap(arg, palette), argName));
            Assert.AreEqual(expected.Result[0].ArgumentColor, actual.Result[0].ArgumentColor);
        }

        [Test]
        [Repeat(5)]
        public void AllArgsColorization()
        {
            List<(string arg, LanguageUnit unit)> data = new();
            foreach (LanguageUnit unit in Enum.GetValues(typeof(LanguageUnit)))
                data.Add((randomizer.GetString(10), unit));

            var actual = Colorizer.Colorize(data.Select(x => new ParseUnit(x.unit, x.arg)).ToArray(), palette);
            ColoringResultsAreEqual(data.Select(x => (x.arg, UnitToColorMap(x.unit, palette))).ToList(), actual);
        }

        [Test]
        [Repeat(250)]
        public void RandomArgsColorization()
        {
            var range = randomizer.Next(50, 150);
            List<(string arg, LanguageUnit unit)> data = new();
            for (var i = 0; i < range; i++)
                data.Add((randomizer.GetString(15), randomizer.NextEnum<LanguageUnit>()));
            
            var actual = Colorizer.Colorize(data.Select(x => new ParseUnit(x.unit, x.arg)).ToArray(), palette);
            ColoringResultsAreEqual(data.Select(x => (x.arg, UnitToColorMap(x.unit, palette))).ToList(), actual);
        }

        private static void ColoringResultsAreEqual(List<(string arg, Color color)> expected, ColoringResult actual)
        {
            Assert.AreEqual(expected.Count, actual.Result.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].arg, actual.Result[i].Argument, "Difference at index " + i);
                Assert.AreEqual(expected[i].color, actual.Result[i].ArgumentColor, "Difference at index " + i);
            }
        }

        private static Color UnitToColorMap(LanguageUnit languageUnit, ColorPalette palette)
        {
            return languageUnit switch
            {
                LanguageUnit.Variable => palette.VariableColor,
                LanguageUnit.Comment => palette.CommentColor,
                LanguageUnit.Function => palette.FunctionColor,
                LanguageUnit.FunctionDefinition => palette.FunctionDefinitionColor,
                LanguageUnit.Operator => palette.OperatorColor,
                LanguageUnit.Symbol => palette.SymbolColor,
                LanguageUnit.Unknown => palette.UnknownColor,
                LanguageUnit.Value => palette.ValueColor,
                LanguageUnit.Whitespace => palette.WhitespaceColor,
                _ => throw new ArgumentOutOfRangeException(nameof(languageUnit), languageUnit, null)
            };
        }
    }
        */
    }
}
