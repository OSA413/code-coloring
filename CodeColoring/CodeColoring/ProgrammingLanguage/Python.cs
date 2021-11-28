using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Internal;

namespace CodeColoring.ProgrammingLanguage
{
    public class Python : IProgrammingLanguage
    {
        public string Name => "Python";
        public ParsingResult Parse(string text)
        {
            var result = new List<ParseUnit>();
            var builder = new StringBuilder();
            var isComment = false;
            var isMultilineComment = false;
            for (var index = 0; index < text.Length; index++)
            {
                var value = text[index].ToString();

                if (index != text.Length - 1 && value == "\"" && text[index + 1].ToString() == "\"")
                {
                    builder.Append(value);
                    continue;
                }

                if (builder.Length > 1 && builder[^1].Equals('\"') && builder[^2].Equals('\"') && value == "\"")
                {
                    builder.Append(value);
                    if (!isMultilineComment)
                    {
                        isMultilineComment = true;
                    }
                    else
                    {
                        result.Add(new ParseUnit(LanguageUnit.Comment, builder.ToString()));
                        builder.Clear();
                        isMultilineComment = false;
                    }
                }
                else if (isMultilineComment)
                {
                    builder.Append(value);
                }
                else if (value == "#")
                {
                    if (builder.Length > 0)
                    {
                        ChooseUnit(result, "", builder);
                        builder.Clear();
                    }

                    builder.Append(value);
                    isComment = true;
                }
                else if (isComment)
                {
                    if (value == "\n")
                    {
                        isComment = false;
                        result.Add(new ParseUnit(LanguageUnit.Comment, builder.ToString()));
                        result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                        builder = new StringBuilder();
                    }
                    else
                    {
                        builder.Append(value);
                    }
                }
                else
                {
                    ChooseUnit(result, value, builder);
                }


                if (index != text.Length - 1) continue;
                result.Add(ChooseUnit(builder));
            }

            return new ParsingResult {Result = result.Where(unit => !unit.Symbol.IsNullOrEmpty()).ToList()};
        }

        private void ChooseUnit(ICollection<ParseUnit> result, string value, StringBuilder strBuilder)
        {
            if (strBuilder.Length > 0 &&
                (strBuilder[0] == '\"' && value != "\"" || strBuilder[0] == '\'' && value != "\'"))
            {
                strBuilder.Append(value);
                return;
            }

            if (IsStringOrCharType(strBuilder, value))
            {
                strBuilder.Append(value);
                result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                strBuilder.Clear();
            }
            else if (units[LanguageUnit.Whitespace].Contains(value))
            {
                result.Add(ChooseUnit(strBuilder));
                result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                strBuilder.Clear();
            }
            else if (units[LanguageUnit.Function].Contains(value))
            {
                result.Add(new ParseUnit(LanguageUnit.Function, strBuilder.ToString()));
                result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                strBuilder.Clear();
            }
            else if (units[LanguageUnit.Symbol].Contains(value))
            {
                result.Add(ChooseUnit(strBuilder));
                result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                strBuilder.Clear();
            }

            else
            {
                strBuilder.Append(value);
            }
        }

        private ParseUnit ChooseUnit(StringBuilder builder)
        {
            if (units[LanguageUnit.FunctionDefinition].Contains(builder.ToString()))
            {
                return new ParseUnit(LanguageUnit.FunctionDefinition, builder.ToString());
            }

            if (units[LanguageUnit.Operator].Contains(builder.ToString()))
            {
                return new ParseUnit(LanguageUnit.Operator, builder.ToString());
            }

            if (builder.Length > 0 && int.TryParse(builder[0].ToString(), out _))
            {
                return new ParseUnit(LanguageUnit.Value, builder.ToString());
            }

            if (builder.Length > 0 && char.IsLetter(builder[0]))
            {
                return new ParseUnit(LanguageUnit.Variable, builder.ToString());
            }

            return new ParseUnit(LanguageUnit.Unknown, builder.ToString());
        }

        private static bool IsStringOrCharType(StringBuilder builder, string currentValue)
        {
            return builder.Length > 0 && (currentValue == "\"" && builder[0] == '\"'
                                          || currentValue == "\'" && builder[0] == '\'') && builder.Length > 1;
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
                    "global", "nonlocal", "import", "from", "try", "except", "finally", "raise", "assert",
                    "with", "as", "del", "in", "and", "or"
                }
            },
            {
                LanguageUnit.Function,
                new[] {"(", "."}
            },
            {
                LanguageUnit.Symbol,
                new[] {"=", "+", "-", "<", ">", "!", "^", "%", "*", ")", ";", "/", ":", ",", "[", "]"}
            },
            {
                LanguageUnit.Whitespace,
                new[] {" ", "\n", "\r", "\t", ""}
            }
        };
    }
}