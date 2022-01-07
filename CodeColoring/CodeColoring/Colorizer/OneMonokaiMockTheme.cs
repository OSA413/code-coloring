using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring.Colorizer
{
    class OneMonokaiMockTheme : ColorPalette
    {
        public OneMonokaiMockTheme()
        {
            FunctionColor = Color.DarkCyan;
            FunctionDefinitionColor = Color.DarkRed;
            OperatorColor = Color.DarkRed;
            SymbolColor = Color.WhiteSmoke;
            VariableColor = Color.LightGray;
            CommentColor = Color.DarkSeaGreen;
            ValueColor = Color.WhiteSmoke;
            BackgroundColor = Color.FromArgb(32, 32, 32);
            UnknownColor = Color.LightGray;
            Name = "OneMonokaiMockTheme";
        }
    }
}
