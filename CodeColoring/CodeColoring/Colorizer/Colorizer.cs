using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CodeColoring
{
    public class Colorizer : ColorPalette
    {
        private List<ColorizedArgument> result = new List<ColorizedArgument>();
        public IEnumerable<ColorizedArgument> Colorize(ParseResult parseResult)
        {
            //Предположу что в парс резалте будет словарь - аргумент:тип
            foreach (var arg in parseResult.results)
            {
                switch (arg.LanguageUnit)
                {
                    case LanguageUnit.Function:
                        result.Add(new ColorizedArgument(this.FunctionColor, arg.arg));
                        break;
                    case LanguageUnit.FunctionDefinition:
                        result.Add(new ColorizedArgument(this.FunctionDefinitionColor, arg.arg));
                        break;
                    case LanguageUnit.Operator:
                        result.Add(new ColorizedArgument(this.OperatorColor, arg.arg));
                        break;
                    case LanguageUnit.Symbols:
                        result.Add(new ColorizedArgument(this.SymbolColor, arg.arg));
                        break;
                    case LanguageUnit.Variable:
                        result.Add(new ColorizedArgument(this.VariableColor, arg.arg));
                }
            }
            return result;
        }
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
