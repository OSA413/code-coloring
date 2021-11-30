using System.Drawing;

namespace CodeColoring.Colorizer
{
    class OneMonokaiMockTheme : ColorPalette
    {
        public OneMonokaiMockTheme()
        {
            FunctionColor = Color.DarkSlateBlue;
            FunctionDefinitionColor = Color.IndianRed;
            OperatorColor = Color.DarkRed;
            SymbolColor = Color.OrangeRed;
            VariableColor = Color.Olive;
            CommentColor = Color.DarkSeaGreen;
            ValueColor = Color.White;
            BackgroundColor = Color.Black;
            Name = "OneMonokaiMockTheme";
        }
    }
}
