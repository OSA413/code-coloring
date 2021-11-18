using CodeColoring;
using NUnit.Framework;
using NUnit.Framework.Internal;
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
        Randomizer randomizer = new();

        ParseUnit[] testParseUnits =
        {
            new ParseUnit(LanguageUnit.Function, "testFunc"),
            new ParseUnit(LanguageUnit.FunctionDefinition, "testDef"),
            new ParseUnit(LanguageUnit.Operator, "testOper"),
            new ParseUnit(LanguageUnit.Symbol, "testSymb"),
            new ParseUnit(LanguageUnit.Variable, "testVariable")
        };
        ColoringResult testResults = new();
        DayTheme dayTheme = new();
        Dictionary<LanguageUnit, Color> dayThemeColorToUnitMap;
        LanguageUnit[] allColorizedUnits;

        [SetUp]
        public void SetUp()
        {
            testResults.Result = new List<ColorizedArgument>
            {
                new ColorizedArgument(dayTheme.FunctionColor, "testFunc"),
                new ColorizedArgument(dayTheme.FunctionDefinitionColor, "testDef"),
                new ColorizedArgument(dayTheme.OperatorColor, "testOper"),
                new ColorizedArgument(dayTheme.SymbolColor, "testSymb"),
                new ColorizedArgument(dayTheme.VariableColor, "testVariable")
            };
            dayThemeColorToUnitMap = new Dictionary<LanguageUnit, Color>
            {
                { LanguageUnit.Function, dayTheme.FunctionColor},
                { LanguageUnit.FunctionDefinition, dayTheme.FunctionDefinitionColor},
                { LanguageUnit.Operator, dayTheme.OperatorColor},
                { LanguageUnit.Symbol, dayTheme.SymbolColor},
                { LanguageUnit.Variable, dayTheme.VariableColor}
            };
            allColorizedUnits = new LanguageUnit[]
            {
                LanguageUnit.Function, LanguageUnit.FunctionDefinition, LanguageUnit.Operator, LanguageUnit.Symbol, LanguageUnit.Variable
            };
        }

        [Test]
        [Repeat(5)]
        public void OneArgColorization()
        {
            var oneArgArray = new ParseUnit[] { new ParseUnit(LanguageUnit.Function, "testFunc") };
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
        public void RandomArgsColorization()
        {
            var randomizedArgs = allColorizedUnits.OrderBy(x => randomizer.Next()).ToArray();
            var randomizedParseUnits = new List<ParseUnit>();
            var expectedResult = new ColoringResult();
            foreach (var arg in randomizedArgs)
            {
                var randomString = randomizer.GetString(5);
                randomizedParseUnits.Add(new ParseUnit(arg, randomString));
                expectedResult.Add(new ColorizedArgument(dayThemeColorToUnitMap[arg], randomString));
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
    }
}
