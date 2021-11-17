using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public enum LanguageUnit
    {
        Unknown,
        Variable,
        Symbols,
        FunctionDefinition,
        Function,
        Operator,
        Whitespace
    }

    public interface IProgrammingLanguage
    {
        public abstract ParseUnit[] Parse(string text);
        public abstract string[] Extensions();
        public abstract Dictionary<LanguageUnit, string[] > UnitCheck();
    }

    
    public struct ParseUnit
    {
        public string Symbol;
        public LanguageUnit Unit;

        public ParseUnit(LanguageUnit unit, string symbol)
        {
            Unit = unit;
            Symbol = symbol;
        }
    }
}
