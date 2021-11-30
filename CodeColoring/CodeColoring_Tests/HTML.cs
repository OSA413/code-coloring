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

        private string GetSpan(LanguageUnit unit, string symbol) =>
            "<span class=\"" + Enum.GetName(typeof(LanguageUnit), unit) + "\">" + symbol + "</span>";

        [Test]
        [Repeat(5)]
        public void Internal_FlattenHTML() =>
            Assert.AreEqual("<><a b></><abc>", html.Flatten(" < >\n    <   a    \n    b >   </>\n<abc> "));

        [Test]
        [Repeat(5)]
        public void AllNullsJustHead()
        {
            var actual = html.Format(null, null);
            Assert.AreEqual(html.Flatten(BuildHTML()), html.Flatten(actual));
        }

        [Test]
        [Repeat(5)]
        public void EmptyParsingResultEmptyBody()
        {
            var actual = html.Format(new ParsingResult(), null);
            Assert.AreEqual(html.Flatten(BuildHTML(BuildBody())), html.Flatten(actual));
        }

        [Test]
        [Repeat(5)]
        public void SingleParsingResult([Values] LanguageUnit unit)
        {
            var symbol = randomizer.GetString(7);
            var parsingResult = new ParsingResult();
            parsingResult.Result.Add(new ParseUnit(unit, symbol));
            var actual = html.Format(parsingResult, null);
            Assert.AreEqual(html.Flatten(BuildHTML(BuildBody(GetSpan(unit, symbol)))), html.Flatten(actual));
        }

        [Test]
        [Repeat(5)]
        public void AllArgsAtOnce()
        {
            var parsingResult = new ParsingResult();
            foreach (LanguageUnit unit in Enum.GetValues(typeof(LanguageUnit)))
                parsingResult.Result.Add(new ParseUnit(unit, randomizer.GetString(10)));

            var actual = html.Format(parsingResult, null);

            Assert.AreEqual(html.Flatten(BuildHTML(BuildBody(
                string.Join("", parsingResult.Result.Select(x => GetSpan(x.Unit, x.Symbol))))))
                , html.Flatten(actual));
        }

        [Test]
        [Repeat(250)]
        public void RandomArgs()
        {
            var range = randomizer.Next(50, 150);
            var parsingResult = new ParsingResult();
            for (var i = 0; i < range; i++)
                parsingResult.Result.Add(new ParseUnit(randomizer.NextEnum<LanguageUnit>(), randomizer.GetString(15)));

            var actual = html.Format(parsingResult, null);

            Assert.AreEqual(html.Flatten(BuildHTML(BuildBody(
                string.Join("", parsingResult.Result.Select(x => GetSpan(x.Unit, x.Symbol))))))
                , html.Flatten(actual));
        }
    }
}
