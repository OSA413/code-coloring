using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;

namespace CodeColoring.ProgrammingLanguage
{
    public class Python : IProgrammingLanguage
    {
        public ParsingResult Parse(string text)
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
                else if (isComment)
                {
                    if (value == "\n")
                    {
                        isComment = false;
                        result.Add(new ParseUnit(LanguageUnit.Comment, strBuilder.ToString()));
                        result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                        strBuilder = new StringBuilder();
                    }
                    else
                    {
                        strBuilder.Append(value);
                    }
                }
                else if (units[LanguageUnit.Whitespace].Contains(value))
                {
                    result.Add(ChooseUnitWhereCurrentSymbolIsWhitSpace(strBuilder));
                    result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                    strBuilder = new StringBuilder();
                }
                else if (units[LanguageUnit.Function].Contains(value))
                {
                    result.Add(units[LanguageUnit.Operator].Contains(strBuilder.ToString())
                        ? new ParseUnit(LanguageUnit.Operator, strBuilder.ToString())
                        : new ParseUnit(LanguageUnit.Function, strBuilder.ToString()));
                    result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                    strBuilder = new StringBuilder();
                }

                else if (units[LanguageUnit.Symbol].Contains(value))
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

            var output = new ParsingResult();
            output.Result = result.Where(unit => !unit.Symbol.IsNullOrEmpty()).ToList();
            return output;
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
            if (units[LanguageUnit.FunctionDefinition].Contains(builder.ToString()))
            {
                return new ParseUnit(LanguageUnit.FunctionDefinition, builder.ToString());
            }

            if (units[LanguageUnit.Operator].Contains(builder.ToString()))
            {
                return new ParseUnit(LanguageUnit.Operator, builder.ToString());
            }

            return ChooseSymbolBetweenValueAndVariable(builder);
        }

        public string[] Extensions() => new[] {".py"};


        private readonly Dictionary<LanguageUnit, string[]> units = new()
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