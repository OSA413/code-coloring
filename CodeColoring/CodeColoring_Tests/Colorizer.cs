using CodeColoring;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring_Tests
{
    class Colorizer_Tests
    {
        ParseUnit[] testParseUnits =
        {
            new ParseUnit(LanguageUnit.Function, "testFunc"),
            new ParseUnit(LanguageUnit.FunctionDefinition, "testDef"),
            new ParseUnit(LanguageUnit.Operator, "testOper"),
            new ParseUnit(LanguageUnit.Symbol, "testSymb"),
            new ParseUnit(LanguageUnit.Variable, "testVariable")
        };
        ColoringResult testResults;
        DayTheme dayTheme = new DayTheme();
        Dictionary<LanguageUnit, Color> dayThemeColorToUnitMap;

        [SetUp]
        public void SetUp()
        {
            testResults.Result = new List<ColorizedArgument>
            {
                new ColorizedArgument(dayTheme.FunctionColor, "testFunc"),
                new ColorizedArgument(dayTheme.FunctionDefinitionColor, "testDef"),
                new ColorizedArgument(dayTheme.FunctionDefinitionColor, "testOper"),
                new ColorizedArgument(dayTheme.FunctionDefinitionColor, "testSymb"),
                new ColorizedArgument(dayTheme.FunctionDefinitionColor, "testVariable")
            };
            dayThemeColorToUnitMap = new Dictionary<LanguageUnit, Color>
            {
                { LanguageUnit.Function, dayTheme.FunctionColor},
                { LanguageUnit.FunctionDefinition, dayTheme.FunctionDefinitionColor},
                { LanguageUnit.Operator, dayTheme.OperatorColor},
                { LanguageUnit.Symbol, dayTheme.SymbolColor},
                { LanguageUnit.Variable, dayTheme.VariableColor}
            };
        }

        [Test]
        public void OneArgColorization()
        {
            var oneArgArray = new ParseUnit[] { new ParseUnit(LanguageUnit.Function, "testFunc") };
            var actual = Colorizer.Colorize(oneArgArray, dayTheme);
            var expected = new ColoringResult();
            expected.Add(new ColorizedArgument(dayTheme.FunctionColor, "testFunc"));
            Assert.AreEqual(expected.Result[0].ArgumentColor, actual.Result[0].ArgumentColor);
        }

        [Test]
        public void AllArgsColorization()
        {
            var actual = Colorizer.Colorize(testParseUnits, dayTheme);
            Assert.IsTrue(ColoringResultsAreEqual(actual, testResults));
        }

        string RandomString(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        bool ColorizedArgumentsAreEqual(ColorizedArgument first, ColorizedArgument second)
        {
            return first.Argument == second.Argument && first.ArgumentColor == second.ArgumentColor;
        }

        bool ColoringResultsAreEqual(ColoringResult first, ColoringResult second)
        {
            if (first.Result.Count != second.Result.Count)
            {
                return false;
            }

            for (var i = 0; i < first.Result.Count; i++)
            {
                if (!ColorizedArgumentsAreEqual(first.Result[i], second.Result[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
