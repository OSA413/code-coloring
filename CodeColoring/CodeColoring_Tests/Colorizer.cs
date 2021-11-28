using System;
using System.Drawing;
using System.Linq;
using CodeColoring;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;
using Ninject;
using NUnit.Framework;
using NUnit.Framework.Internal;


namespace CodeColoring_Tests
{
    internal class ColorPalette_Tests
    { 
        private class NewColorPalette : ColorPalette { }

        private readonly ColorPalette palette;
        private readonly Colorizer colorizer;
        
        private readonly NewColorPalette newColorPalette = new();
      

        public ColorPalette_Tests()
        {
            var readOnlyKernel = Program.ConfigureContainer();
            palette = readOnlyKernel.Get<ColorPalette>();
            colorizer = readOnlyKernel.Get<Colorizer>();
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
        public void DefaultColorPaletteIsBlack([Values] LanguageUnit unit) =>
            Assert.AreEqual(Color.Black, colorizer.Colorize(unit, new NewColorPalette()));

        [Test]
        [Repeat(5)]
        public void NonExistingColorThrowsArgumentOutOfRange() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => colorizer.Colorize((LanguageUnit)99, palette));

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
