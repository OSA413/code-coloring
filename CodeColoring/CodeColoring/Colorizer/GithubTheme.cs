using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring.Colorizer
{
    class GithubTheme : ColorPalette
    {
        public GithubTheme()
        {
            FunctionColor = Color.BlueViolet;
            FunctionDefinitionColor = Color.Red;
            OperatorColor = Color.Red;
            SymbolColor = Color.Red;
            VariableColor = Color.Black;
            CommentColor = Color.Gray;
            ValueColor = Color.Black;
            BackgroundColor = Color.White;
            UnknownColor = Color.Black;
            Name = "GithubTheme";
        }
    }
}
