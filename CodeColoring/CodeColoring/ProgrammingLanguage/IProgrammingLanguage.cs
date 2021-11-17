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
        Symbol,
        FunctionDefinition,
        Function,
        Operator,
        Value,
        Whitespace
    }

    public interface IProgrammingLanguage
    {
        public abstract ParsingResult Parse(string text);
        public abstract LanguageUnit GetUnit(string arg);
        public abstract string[] Extensions();
        public abstract Dictionary<string[], LanguageUnit> UnitCheck();
    }

    public class ParsingResult
    {
        public List<(string arg, LanguageUnit LanguageUnit)> Result;
    }
}
