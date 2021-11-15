using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeColoring
{
    public class Python : ILanguage
    {
        
        public LanguageUnit GetUnit(string[] args) //нужно предыдущее, текущее и следующее слово-знак
        {
            if (args.Length != 3)
            {
                //что-то
            }

            return (from unit in UnitCheck() where unit.Key(args) select unit.Value).FirstOrDefault();
        }

        public string[] Extensions() => new[] {".py", ".ipynb"};
        

        public Dictionary<Func<string[], bool>, LanguageUnit> UnitCheck() => new()
        {
            {
                s => new[] {"def"}.Contains(s[1]), 
                LanguageUnit.FunctionDefinition
            },
            {
                s => new[]
                {
                    "if", "else", "elif", "print", "for", "while", "pass", "break", "continue", "return", "yield",
                    "global", "nonlocal", "import", "from", "class", "try", "except", "finally", "raise", "assert",
                    "with",
                    "as", "del"
                }.Contains(s[1]),
                LanguageUnit.Operator
            },
            {
                s => s[1].EndsWith(")") || s[0] == "def",
                LanguageUnit.Function
            },
            {
                s =>
                {

                    var options = new[] {"=", "+", "-", "<", ">", "!", "^", "%", "*", ")", "("};
                    return options.Contains(s[2]) || options.Contains(s[0]);
                },
                LanguageUnit.Variable
            }
        };
    }
}