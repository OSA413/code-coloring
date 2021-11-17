using System.Drawing;

namespace CodeColoring
{
    public abstract class ColorPalette
    {
        public Color SymbolColor { get; set; }
        public Color FunctionDefinitionColor { get; set; }
        public Color VariableColor { get; set; }
        public Color FunctionColor { get; set; }
        public Color OperatorColor { get; set; }
    }
}
