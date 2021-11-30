using System.Drawing;

namespace CodeColoring.Colorizer
{
    class DarkulaTheme : ColorPalette
    {
        public DarkulaTheme()
        {
            FunctionColor = Color.Turquoise;
            FunctionDefinitionColor = Color.DarkBlue;
            OperatorColor = Color.Olive;
            SymbolColor = Color.DarkOrange;
            VariableColor = Color.LightBlue;
            CommentColor = Color.DarkOliveGreen;
            ValueColor = Color.White;
            BackgroundColor = Color.DarkGray;
            Name = "DarkulaTheme";
        }
    }
}
