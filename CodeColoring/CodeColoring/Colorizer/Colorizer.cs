using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CodeColoring
{
    public class Colorizer
    {
        public static ColoringResult Colorize(ParseUnit[] parseResult, ColorPalette palette)
        {
            var result = new ColoringResult();
            foreach (var arg in parseResult)
            {
                switch (arg.Unit)
                {
                    case LanguageUnit.Function:
                        result.Add(new ColorizedArgument(palette.FunctionColor, arg.Symbol));
                        break;
                    case LanguageUnit.FunctionDefinition:
                        result.Add(new ColorizedArgument(palette.FunctionDefinitionColor, arg.Symbol));
                        break;
                    case LanguageUnit.Operator:
                        result.Add(new ColorizedArgument(palette.OperatorColor, arg.Symbol));
                        break;
                    case LanguageUnit.Symbol:
                        result.Add(new ColorizedArgument(palette.SymbolColor, arg.Symbol));
                        break;
                    case LanguageUnit.Variable:
                        result.Add(new ColorizedArgument(palette.VariableColor, arg.Symbol));
                        break;
                }
            }
            return result;
        }
    }

    public class ColoringResult
    {
        public List<ColorizedArgument> Result = new List<ColorizedArgument>();
        public void Add(ColorizedArgument a) => Result.Add(a);
    }

    public class ColorizedArgument
    {
        public Color ArgumentColor { get; set; }
        public string Argument { get; set; }

        public ColorizedArgument(Color color, string argument)
        {
            ArgumentColor = color;
            Argument = argument;
        }
    }
}
