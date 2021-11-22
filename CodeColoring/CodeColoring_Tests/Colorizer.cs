using NUnit.Framework;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CodeColoring;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;

using Ninject;

namespace CodeColoring_Tests
{
    internal class ColorPalette_Tests
    {
        private class NewColorPalette : ColorPalette { }

        private readonly Randomizer randomizer = new();
        private readonly ColorPalette palette = new DayTheme();
        private readonly Colorizer colorizer = Repository.Kernel.Get<Colorizer>();
        private readonly NewColorPalette newColorPalette = new();

        [Test]
        [Repeat(5)]
        public void ColorPaletteHasTheSameAmountOfVariablesAsLanguageUnit()
        {
            Assert.AreEqual(Enum.GetValues(typeof(LanguageUnit)).Length, typeof(ColorPalette).GetProperties().Length);
        }

        [Test]
        [Repeat(5)]
        public void OneArgColorization([Values] LanguageUnit unit)
        {
            var actual = colorizer.Colorize(unit, palette);
            var expected = UnitToColorMap(unit, palette);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Repeat(5)]
        public void DefaultColorPaletteIsBlack()
        {
            Assert.True(typeof(NewColorPalette).GetProperties()
                .All(x => (Color)x.GetValue(newColorPalette) == Color.Black));
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
}
