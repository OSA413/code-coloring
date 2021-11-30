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

namespace CodeColoring_Tests
{
    internal class HTML_Tests
    {
        private readonly Randomizer randomizer = new();
        private readonly IContainer container = Program.ConfigureContainer();
        private readonly HTML html;

        public HTML_Tests() => html = container.Resolve<HTML>();

        private readonly StringBuilder sb = new StringBuilder();
        public string FlattenHTML(string text)
        {
            var afterSpace = false;
            var lastMeaningChar = ' ';
            foreach (var c in text)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!(lastMeaningChar == '<' || lastMeaningChar == '>' || lastMeaningChar == '/'))
                        afterSpace = true;
                    continue;
                }

                if (!(c == '<' || c == '>' || c == '/') && afterSpace)
                    sb.Append(' ');
                sb.Append(c);
                lastMeaningChar = c;
                afterSpace = false;
            }

            var result = sb.ToString();
            sb.Clear();
            return result;
        }

        [Test]
        public void Internal_FlattenHTML() =>
            Assert.AreEqual("<><a b></><abc>", FlattenHTML(" < >\n    <   a    \n    b >   </>\n<abc> "));

        [Test]
        public void AllNulls()
        {
            var actual = html.Format(null, null);
        }
        
        /*
        private readonly ColorPalette palette = new DayTheme();

        [Test]
        [Repeat(5)]
        public void ColorPaletteHasTheSameAmountOfVariablesAsLanguageUnit()
        {
            Assert.AreEqual(Enum.GetValues(typeof(LanguageUnit)).Length, typeof(ColorPalette).GetProperties().Length);
        }

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
