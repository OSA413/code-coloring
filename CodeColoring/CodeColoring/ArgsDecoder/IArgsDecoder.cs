using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public interface IArgsDecoder
    {
        DecodedArguments Decode(string[] args);
        string Help();
    }

    public class DecodedArguments
    {
        public ColorPalette ColorPalette;
        public ProgrammingLanguage ProgrammingLanguage;
        public OutputFormat OutputFormat;
    }
}
