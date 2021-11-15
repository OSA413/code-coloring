using System;
using System.Collections.Generic;

namespace CodeColoring
{
    public interface ILanguage
    {
        LanguageUnit GetUnit(string arg);
        string[] Extensions();
        Dictionary<string[], LanguageUnit> UnitCheck();

    }
    
    
}