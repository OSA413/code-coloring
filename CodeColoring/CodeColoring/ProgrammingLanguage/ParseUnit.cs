namespace CodeColoring.ProgrammingLanguage
{
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