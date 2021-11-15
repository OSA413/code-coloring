using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public abstract class ProgrammingLanguage
    {
        public abstract ParseResult Parse(string text);
    }

    public class ParseResult
    {
        

    }
}
