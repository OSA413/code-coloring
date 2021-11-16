using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeColoring
{
    public class Python : ProgrammingLanguage //как по мне лучше интерфейс все же 
    {
        public override ParseResult Parse(string text);
        
        public override LanguageUnit GetUnit(string arg) //нужно предыдущее, текущее и следующее слово-знак
        {
            

            foreach (var (arr, languageUnit) in UnitCheck())
            {
                if (languageUnit == LanguageUnit.Function)
                {
                    foreach (var key in arr)
                    {
                        if(arg.Contains(key))
                            return languageUnit;
                    }
                }
                else
                {
                    if(arr.Contains(arg))
                        return languageUnit;
                }
            }

            return LanguageUnit.Variable;
        }

        public override string[] Extensions() => new[] {".py", ".ipynb"};
        

        public override Dictionary<string[], LanguageUnit> UnitCheck() => new()
        {
            {
                new[] {"def", "class"}, 
                LanguageUnit.FunctionDefinition //переименовать
            },
            {
                new[]
                {
                    "if", "else", "elif", "print", "for", "while", "pass", "break", "continue", "return", "yield",
                    "global", "nonlocal", "import", "from", "class", "try", "except", "finally", "raise", "assert",
                    "with", "as", "del"
                },
                LanguageUnit.Operator
            },
            {
                new []{"(", "."}, //TODO нужно отдавать следующий символ после последней буквы
                LanguageUnit.Function
            },
            {
                new[] {"=", "+", "-", "<", ">", "!", "^", "%", "*", ")", "("},
                LanguageUnit.Symbols
            }
        };
    }
}