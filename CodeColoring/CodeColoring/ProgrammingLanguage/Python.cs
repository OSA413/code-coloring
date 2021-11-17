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
            for (var index = 0; index < text.Length; index++)
            {
                var value = text[index];
                if (UnitCheck()[LanguageUnit.Function].Contains(value.ToString()))
                {
                    result.Add(new ParseUnit(LanguageUnit.Function, strBuilder.ToString()));
                    result.Add(new ParseUnit(LanguageUnit.Symbol, value.ToString()));
                    strBuilder = new StringBuilder();
                }
                else if (strBuilder.Length > 0 &&
                         (value == '\"' && strBuilder[0] == '\"' || value == '\'' && strBuilder[0] == '\''))
                {
                    strBuilder.Append(value);
                    result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                    strBuilder = new StringBuilder();
                }
                else if (UnitCheck()[LanguageUnit.Whitespace].Contains(value.ToString()))
                {
                    if (UnitCheck()[LanguageUnit.FunctionDefinition].Contains(strBuilder.ToString()))
                    {
                        result.Add(new ParseUnit(LanguageUnit.FunctionDefinition, strBuilder.ToString()));
                    }
                    else if (UnitCheck()[LanguageUnit.Operator].Contains(strBuilder.ToString()))
                    {
                        result.Add(new ParseUnit(LanguageUnit.Operator, strBuilder.ToString()));
                    }
                    else
                    {
                        if (strBuilder.Length > 0 && int.TryParse(strBuilder[0].ToString(), out _))
                        {
                            result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                        }
                        else
                        {
                            result.Add(new ParseUnit(LanguageUnit.Variable, strBuilder.ToString()));
                        }
                    }

                    result.Add(new ParseUnit(LanguageUnit.Whitespace, value.ToString()));
                    strBuilder = new StringBuilder();
                }

                else if (UnitCheck()[LanguageUnit.Symbol].Contains(value.ToString()))
                {
                    if (strBuilder.Length > 0 && int.TryParse(strBuilder[0].ToString(), out _))
                    {
                        result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                    }
                    else
                    {
                        result.Add(new ParseUnit(LanguageUnit.Variable, strBuilder.ToString()));
                    }

                    result.Add(new ParseUnit(LanguageUnit.Symbol, value.ToString()));
                    strBuilder = new StringBuilder();
                }
                
                else
                {
                    strBuilder.Append(value);
                    if (index != text.Length - 1) continue;
                    if (strBuilder.Length > 0 && int.TryParse(strBuilder[0].ToString(), out _))
                    {
                        result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                    }
                    else
                    {
                        result.Add(new ParseUnit(LanguageUnit.Variable, strBuilder.ToString()));
                    }

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
                new[] {"=", "+", "-", "<", ">", "!", "^", "%", "*", ")", "(", ";", "\\", "/", ":"}
                
            },
            {
                LanguageUnit.Whitespace,
                new []{" ", "\n", "\r", "\t"}
            }
        };
    }
}