using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;

namespace CodeColoring
{
    public class Python : IProgrammingLanguage
    {
        public ParseUnit[] Parse(string text)
        {
            var result = new List<ParseUnit>();
            var strBuilder = new StringBuilder();
            foreach (var value in text)
            {
                if (UnitCheck()[LanguageUnit.Function].Contains(value.ToString()))
                {
                    result.Add(new ParseUnit(LanguageUnit.Symbol, value.ToString()));
                    result.Add(new ParseUnit(LanguageUnit.Function, strBuilder.ToString()));
                    strBuilder = new StringBuilder();
                }
                else if(value == ' ')
                {
                    result.Add(new ParseUnit(LanguageUnit.Whitespace, value.ToString()));
                    
                    if(UnitCheck()[LanguageUnit.FunctionDefinition].Contains(strBuilder.ToString()))
                    {
                        result.Add(new ParseUnit(LanguageUnit.FunctionDefinition, strBuilder.ToString()));
                    }
                    else if (UnitCheck()[LanguageUnit.Operator].Contains(strBuilder.ToString()))
                    {
                        result.Add(new ParseUnit(LanguageUnit.Operator, strBuilder.ToString()));
                    }
                    else
                    {
                        result.Add(new ParseUnit(LanguageUnit.Variable, strBuilder.ToString()));
                    }

                    strBuilder = new StringBuilder();
                }

                else if (UnitCheck()[LanguageUnit.Symbol].Contains(value.ToString()))
                {
                    result.Add(new ParseUnit(LanguageUnit.Symbol, value.ToString()));
                    result.Add(new ParseUnit(LanguageUnit.Variable, strBuilder.ToString()));
                    strBuilder = new StringBuilder();
                }
                else
                {
                    strBuilder.Append(value);
                }
            }

            return result.Where(unit => !unit.Symbol.IsNullOrEmpty()).ToArray();
        }

        public string[] Extensions() => new[] {".py", ".ipynb"};
        

        public Dictionary<LanguageUnit,string[]> UnitCheck() => new()
        {
            {
                LanguageUnit.FunctionDefinition,
                new[] {"def", "class"}//переименовать
            },
            {
                
                LanguageUnit.Operator,
                new[]
                {
                    "if", "else", "elif", "print", "for", "while", "pass", "break", "continue", "return", "yield",
                    "global", "nonlocal", "import", "from", "class", "try", "except", "finally", "raise", "assert",
                    "with", "as", "del"
                }
            },
            {
                LanguageUnit.Function,
                new []{"(", "."} 
                
            },
            {
                LanguageUnit.Symbol,
                new[] {"=", "+", "-", "<", ">", "!", "^", "%", "*", ")", "("}
                
            }
        };
    }
}