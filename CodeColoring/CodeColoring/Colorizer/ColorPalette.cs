using System.Drawing;

namespace CodeColoring.Colorizer
{
    public abstract class ColorPalette
    {
        public Color SymbolColor { get; set; }
        public Color FunctionDefinitionColor { get; set; }
        public Color VariableColor { get; set; }
        public Color FunctionColor { get; set; }
        public Color OperatorColor { get; set; }
        public Color CommentColor { get; set; }
        public Color ValueColor { get; set; }
        public Color WhitespaceColor { get; set; }
        public Color UnknownColor { get; set; }

        public ColorPalette()
        {
            WhitespaceColor = Color.Empty;
            UnknownColor = Color.Empty;
        }
    }
}
