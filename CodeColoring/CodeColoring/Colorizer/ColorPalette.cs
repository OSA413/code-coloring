using System.Drawing;

namespace CodeColoring.Colorizer
{
    public abstract class ColorPalette
    {
        public Color SymbolColor { get; protected set; }
        public Color FunctionDefinitionColor { get; protected set; }
        public Color VariableColor { get; protected set; }
        public Color FunctionColor { get; protected set; }
        public Color OperatorColor { get; protected set; }
        public Color CommentColor { get; protected set; }
        public Color ValueColor { get; protected set; }
        public Color WhitespaceColor { get; protected set; }
        public Color UnknownColor { get; protected set; }
        public Color BackgroundColor { get; protected set; }
        public string Name { get; protected set; }

        protected ColorPalette()
        {
            SymbolColor = Color.Black;
            FunctionDefinitionColor = Color.Black;
            VariableColor = Color.Black;
            FunctionColor = Color.Black;
            OperatorColor = Color.Black;
            CommentColor = Color.Black;
            ValueColor = Color.Black;
            WhitespaceColor = Color.Black;
            UnknownColor = Color.Black;
            BackgroundColor = Color.White;
            Name = "Default";
        }
    }
}
