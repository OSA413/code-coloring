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
        Whitespace
    }

    public interface IProgrammingLanguage
    {
        public ParseUnit[] Parse(string text);
        public string[] Extensions(); //будем вообще это вызывать? Или другое определние языка
       
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
