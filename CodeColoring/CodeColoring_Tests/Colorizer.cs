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

        ParseUnit[] testParseUnits =
        {
            new(LanguageUnit.Function, "testFunc"),
            new(LanguageUnit.FunctionDefinition, "testDef"),
            new(LanguageUnit.Operator, "testOper"),
            new(LanguageUnit.Symbol, "testSymb"),
            new(LanguageUnit.Variable, "testVariable"),
            new(LanguageUnit.Comment, "testComment")
        };
        ColoringResult testResults = new();
        DayTheme dayTheme = new();
        LanguageUnit[] allColorizedUnits;

        [OneTimeSetUp]
        public void SetUp()
        {
            testResults.Result = new List<ColorizedArgument>
            {
                new(dayTheme.FunctionColor, "testFunc"),
                new(dayTheme.FunctionDefinitionColor, "testDef"),
                new(dayTheme.OperatorColor, "testOper"),
                new(dayTheme.SymbolColor, "testSymb"),
                new(dayTheme.VariableColor, "testVariable"),
                new(dayTheme.CommentColor, "testComment")
            };
            allColorizedUnits = new LanguageUnit[]
            {
                LanguageUnit.Function, LanguageUnit.FunctionDefinition, LanguageUnit.Operator, LanguageUnit.Symbol, LanguageUnit.Variable, LanguageUnit.Comment
            };
        }

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
            var actual = Colorizer.Colorize(oneArgArray, dayTheme);
            var expected = new ColoringResult();
            expected.Add(new ColorizedArgument(UnitToColorMap(arg, dayTheme), argName));
            Assert.AreEqual(expected.Result[0].ArgumentColor, actual.Result[0].ArgumentColor);
        }

        [Test]
        [Repeat(5)]
        public void AllArgsColorization()
        {
            List<(string arg, LanguageUnit unit)> data = new();
            foreach (LanguageUnit unit in Enum.GetValues(typeof(LanguageUnit)))
                data.Add((randomizer.GetString(10), unit));

            var actual = Colorizer.Colorize(data.Select(x => new ParseUnit(x.unit, x.arg)).ToArray(), dayTheme);
            ColoringResultsAreEqual(data.Select(x => (x.arg, UnitToColorMap(x.unit, dayTheme))).ToList(), actual);
        }

        [Test]
        [Repeat(250)]
        public void RandomArgsColorization()
        {
            var randomizedArgs = allColorizedUnits.OrderBy(x => randomizer.Next());
            var randomizedParseUnits = new List<ParseUnit>();
            var expectedResult = new ColoringResult();
            foreach (var arg in randomizedArgs)
            {
                var randomString = randomizer.GetString(5);
                randomizedParseUnits.Add(new ParseUnit(arg, randomString));
                expectedResult.Add(new ColorizedArgument(UnitToColorMap(arg, dayTheme), randomString));
            }
            var actual = Colorizer.Colorize(randomizedParseUnits.ToArray(), dayTheme);
            ColoringResultsAreEqual(expectedResult, actual);
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
                case LanguageUnit.Variable:
                    return palette.VariableColor;
                case LanguageUnit.Comment:
                    return palette.CommentColor;
                case LanguageUnit.Function:
                    return palette.FunctionColor;
                case LanguageUnit.FunctionDefinition:
                    return palette.FunctionDefinitionColor;
                case LanguageUnit.Operator:
                    return palette.OperatorColor;
                case LanguageUnit.Symbol:
                    return palette.SymbolColor;
            }
            return Color.Empty;
        }
    }
}
