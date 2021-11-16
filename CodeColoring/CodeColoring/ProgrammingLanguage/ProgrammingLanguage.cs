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
        public abstract LanguageUnit GetUnit(string arg);
        public abstract string[] Extensions();
        public abstract Dictionary<string[], LanguageUnit> UnitCheck();
    }

    public class ParseResult
    {
        

    }
}
