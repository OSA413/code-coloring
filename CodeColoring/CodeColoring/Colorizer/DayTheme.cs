using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public class DayTheme: ColorPalette
    {
        public DayTheme()
        {
            this.FunctionColor = Color.Blue;
            this.FunctionDefinitionColor = Color.Aqua;
            this.OperatorColor = Color.White;
            this.SymbolColor = Color.Orange;
            this.VariableColor = Color.LightBlue;
        }
    }
}
