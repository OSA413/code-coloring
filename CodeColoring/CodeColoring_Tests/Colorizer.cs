using CodeColoring;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        //Dictionary<LanguageUnit, Color> dayThemeColorToUnitMap;
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
            //dayThemeColorToUnitMap = new Dictionary<LanguageUnit, Color>
            //{
            //    { LanguageUnit.Function, dayTheme.FunctionColor},
            //    { LanguageUnit.FunctionDefinition, dayTheme.FunctionDefinitionColor},
            //    { LanguageUnit.Operator, dayTheme.OperatorColor},
            //    { LanguageUnit.Symbol, dayTheme.SymbolColor},
            //    { LanguageUnit.Variable, dayTheme.VariableColor},
            //    { LanguageUnit.Comment, dayTheme.CommentColor }
            //};
            allColorizedUnits = new LanguageUnit[]
            {
                LanguageUnit.Function, LanguageUnit.FunctionDefinition, LanguageUnit.Operator, LanguageUnit.Symbol, LanguageUnit.Variable, LanguageUnit.Comment
            };
        }

        [Test]
        [Repeat(5)]
        public void OneArgColorization()
        {
            var oneArgArray = new ParseUnit[] { new(LanguageUnit.Function, "testFunc") };
            var actual = Colorizer.Colorize(oneArgArray, dayTheme);
            var expected = new ColoringResult();
            expected.Add(new ColorizedArgument(dayTheme.FunctionColor, "testFunc"));
            Assert.AreEqual(expected.Result[0].ArgumentColor, actual.Result[0].ArgumentColor);
        }

        [Test]
        [Repeat(5)]
        public void AllArgsColorization()
        {
            var actual = Colorizer.Colorize(testParseUnits, dayTheme);
            Assert.IsTrue(ColoringResultsAreEqual(actual, testResults));
        }

        [Test]
        [Repeat(250)]
        public void DayThemeRandomArgsColorization()
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
            Assert.IsTrue(ColoringResultsAreEqual(expectedResult, actual));
        }

        bool ColorizedArgumentsAreEqual(ColorizedArgument first, ColorizedArgument second)
        {
            return first.Argument == second.Argument && first.ArgumentColor == second.ArgumentColor;
        }

        bool ColoringResultsAreEqual(ColoringResult first, ColoringResult second)
        {
            if (first.Result.Count != second.Result.Count)
                return false;

            for (var i = 0; i < first.Result.Count; i++)
                if (!ColorizedArgumentsAreEqual(first.Result[i], second.Result[i]))
                    return false;

            return true;
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
