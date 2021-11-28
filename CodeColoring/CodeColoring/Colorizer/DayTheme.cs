using System.Drawing;

namespace CodeColoring.Colorizer
{
    public class DayTheme: ColorPalette
    {
        public DayTheme()
        {
            FunctionColor = Color.Blue;
            FunctionDefinitionColor = Color.Salmon;
            OperatorColor = Color.Olive;
            SymbolColor = Color.Orange;
            VariableColor = Color.Chocolate;
            CommentColor = Color.Green;
            ValueColor = Color.Gray;
            Name = "DayTheme";
        }
    }
}
