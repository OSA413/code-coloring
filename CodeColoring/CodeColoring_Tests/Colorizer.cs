using CodeColoring;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring_Tests
{
    class Colorizer_Tests
    {
        Randomizer randomizer = new();
        ColorPalette palette = new DayTheme();

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
            for (int i = 0; i < range; i++)
                data.Add((randomizer.GetString(15), randomizer.NextEnum<LanguageUnit>()));
            
            var actual = Colorizer.Colorize(data.Select(x => new ParseUnit(x.unit, x.arg)).ToArray(), palette);
            ColoringResultsAreEqual(data.Select(x => (x.arg, UnitToColorMap(x.unit, palette))).ToList(), actual);
        }

        void ColoringResultsAreEqual(List<(string arg, Color color)> expected, ColoringResult actual)
        {
            Assert.AreEqual(expected.Count, actual.Result.Count);
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].arg, actual.Result[i].Argument, "Difference at index " + i);
                Assert.AreEqual(expected[i].color, actual.Result[i].ArgumentColor, "Difference at index " + i);
            }
        }

        Color UnitToColorMap(LanguageUnit languageUnit, ColorPalette palette)
        {
            switch (languageUnit)
            {
                case LanguageUnit.Variable: return palette.VariableColor;
                case LanguageUnit.Comment: return palette.CommentColor;
                case LanguageUnit.Function: return palette.FunctionColor;
                case LanguageUnit.FunctionDefinition: return palette.FunctionDefinitionColor;
                case LanguageUnit.Operator: return palette.OperatorColor;
                case LanguageUnit.Symbol: return palette.SymbolColor;
                case LanguageUnit.Unknown: return palette.UnknownColor;
                case LanguageUnit.Value: return palette.ValueColor;
                case LanguageUnit.Whitespace: return palette.WhitespaceColor;
            }
            throw new ArgumentException("Color not found");
        }
    }
}
