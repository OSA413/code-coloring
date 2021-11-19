using System.Drawing;

namespace CodeColoring.Colorizer
{
    public class DayTheme: ColorPalette
    {
        public DayTheme()
        {
            FunctionColor = Color.Blue;
            FunctionDefinitionColor = Color.Aqua;
            OperatorColor = Color.White;
            SymbolColor = Color.Orange;
            VariableColor = Color.LightBlue;
            CommentColor = Color.Green;
        }
    }
}
