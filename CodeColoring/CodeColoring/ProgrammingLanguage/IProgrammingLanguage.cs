using System.Collections.Generic;

namespace CodeColoring.ProgrammingLanguage
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
        Whitespace,
        Comment
    }

    public interface IProgrammingLanguage
    {
        public string Name { get; }
        public ParsingResult Parse(string text);
        public string[] Extensions(); //будем вообще это вызывать? Или другое определние языка
       
    }

    public class ParsingResult
    {
        public List<ParseUnit> Result = new();
    }
    
    public readonly struct ParseUnit
    {
        public readonly string Symbol;
        public readonly LanguageUnit Unit;

        public ParseUnit(LanguageUnit unit, string symbol)
        {
            Unit = unit;
            Symbol = symbol;
        }
    }
}
