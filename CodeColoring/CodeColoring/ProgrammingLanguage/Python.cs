using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;

namespace CodeColoring.ProgrammingLanguage
{
    public class Python : IProgrammingLanguage
    {
        public ParseUnit[] Parse(string text)
        {
            var result = new List<ParseUnit>();
            var strBuilder = new StringBuilder();
            var isComment = false;
            for (var index = 0; index < text.Length; index++)
            {
                var value = text[index].ToString(); 
                if (value == "#")
                {
                    strBuilder.Append(value);
                    isComment = true;
                }
                else if(isComment)
                {
                    strBuilder.Append(value);
                    if (value != "\n") continue;
                    isComment = false;
                    result.Add(new ParseUnit(LanguageUnit.Comment, strBuilder.ToString()));
                    strBuilder = new StringBuilder();

                }
                
                else if (unitCheck[LanguageUnit.Whitespace].Contains(value))
                {
                    
                    
                    result.Add(ChooseUnitWhereCurrentSymbolIsWhitSpace(strBuilder));
                    
                    result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                    strBuilder = new StringBuilder();
                }
                else if (unitCheck[LanguageUnit.Function].Contains(value))
                {
                    result.Add(unitCheck[LanguageUnit.Operator].Contains(strBuilder.ToString())
                        ? new ParseUnit(LanguageUnit.Operator, strBuilder.ToString())
                        : new ParseUnit(LanguageUnit.Function, strBuilder.ToString()));
                    result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                    strBuilder = new StringBuilder();
                }
                
                else if (unitCheck[LanguageUnit.Symbol].Contains(value))
                {
                    result.Add(ChooseSymbolBetweenValueAndVariable(strBuilder));
                    result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                    strBuilder = new StringBuilder();
                }
                else if (IsStringOrCharType(strBuilder, value))
                {
                    strBuilder.Append(value);
                    result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                    strBuilder = new StringBuilder();
                }
                else
                {
                    strBuilder.Append(value);
                    if (index != text.Length - 1) continue;
                    result.Add(ChooseSymbolBetweenValueAndVariable(strBuilder));
                }
            }

            return result.Where(unit => !unit.Symbol.IsNullOrEmpty()).ToArray();
        }

        private static ParseUnit ChooseSymbolBetweenValueAndVariable(StringBuilder builder)
        {
            if (builder.Length > 0 && int.TryParse(builder[0].ToString(), out _))
            {
                return new ParseUnit(LanguageUnit.Value, builder.ToString());
            }

            return new ParseUnit(LanguageUnit.Variable, builder.ToString());
        }

        private static bool IsStringOrCharType(StringBuilder builder, string currentValue)
        {
            return builder.Length > 0 &&
                   (currentValue == "\"" && builder[0] == '\"' || currentValue == "\'" && builder[0] == '\'');
        }

        private ParseUnit ChooseUnitWhereCurrentSymbolIsWhitSpace(StringBuilder builder)
        {
            if (unitCheck[LanguageUnit.FunctionDefinition].Contains(builder.ToString()))
            {
                return new ParseUnit(LanguageUnit.FunctionDefinition, builder.ToString());
            }
            if (unitCheck[LanguageUnit.Operator].Contains(builder.ToString()))
            {
                return new ParseUnit(LanguageUnit.Operator, builder.ToString());
            }
            return ChooseSymbolBetweenValueAndVariable(builder);
        }

        public string[] Extensions() => new[] {".py", ".ipynb"};


        private readonly Dictionary<LanguageUnit, string[]> unitCheck = new()
        {
            {
                LanguageUnit.FunctionDefinition,
                new[] {"def", "class"} //переименовать
            },
            {
                LanguageUnit.Operator,
                new[]
                {
                    "if", "else", "elif", "for", "while", "pass", "break", "continue", "return", "yield",
                    "global", "nonlocal", "import", "from", "class", "try", "except", "finally", "raise", "assert",
                    "with", "as", "del"
                }
            },
            {
                LanguageUnit.Function,
                new[] {"(", "."}
            },
            {
                LanguageUnit.Symbol,
                new[] {"=", "+", "-", "<", ">", "!", "^", "%", "*", ")", "(", ";", "\\", "/", ":"}
            },
            {
                LanguageUnit.Whitespace,
                new[] {" ", "\n", "\r", "\t"}
            }
        };
    }
}