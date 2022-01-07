using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring.Colorizer
{
    class DarkulaTheme : ColorPalette
    {
        public DarkulaTheme()
        {
            FunctionColor = Color.WhiteSmoke;
            FunctionDefinitionColor = Color.DarkOrange;
            OperatorColor = Color.DarkOrange;
            SymbolColor = Color.DarkOrange;
            VariableColor = Color.White;
            CommentColor = Color.DarkGreen;
            ValueColor = Color.White;
            BackgroundColor = Color.FromArgb(35, 31, 32);
            UnknownColor = Color.WhiteSmoke;
            Name = "DarkulaTheme";
        }
    }
}
