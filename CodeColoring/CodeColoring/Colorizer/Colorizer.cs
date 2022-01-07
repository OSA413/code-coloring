using System;
using System.Drawing;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring.Colorizer
{
    public class Colorizer
    {
        public static Color Colorize(LanguageUnit unit, ColorPalette palette) =>
            unit switch
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
                _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
            };
    }
}