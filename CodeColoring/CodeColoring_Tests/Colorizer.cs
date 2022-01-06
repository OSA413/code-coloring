using System;
using System.Drawing;
using System.Linq;
using CodeColoring;
using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;
using NUnit.Framework;
using Autofac;


namespace CodeColoring_Tests
{
    internal class ColorPalette_Tests
    {
        private readonly ColorPalette defaultColorPalette;

        public ColorPalette_Tests()
        {
            defaultColorPalette =  ContainerSetting.ConfigureContainer().Resolve<ColorPalette[]>().First(x=> x.Name == "Default");
        }

        [Test]
        [Repeat(5)]
        public void OneArgColorization([Values] LanguageUnit unit)
        {
            var actual = Colorizer.Colorize(unit, defaultColorPalette);
            var expected = UnitToColorMap(unit, defaultColorPalette);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        [Repeat(5)]
        public void DefaultColorPaletteIsBlack([Values] LanguageUnit unit) =>
            Assert.AreEqual(Color.Black, Colorizer.Colorize(unit, defaultColorPalette));

        [Test]
        [Repeat(5)]
        public void DefaultBackgroundIsWhite() => Assert.AreEqual(Color.White, defaultColorPalette.BackgroundColor);

        [Test]
        [Repeat(5)]
        public void NonExistingColorThrowsArgumentOutOfRange() =>
            Assert.Throws<ArgumentOutOfRangeException>(() => Colorizer.Colorize((LanguageUnit) 99, defaultColorPalette));

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