using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeColoring.ProgrammingLanguage
{
    public abstract class ProgrammingLanguage
    {
        public string Name { get; protected init; }

        protected string[] Extensions { get; init; } //будем вообще это вызывать? Или другое определние языка

        protected Dictionary<LanguageUnit, string[]> Units { get; init; }


        public ParsingResult Parse(string text)
        {
            var result = new List<ParseUnit>();
            var builder = new StringBuilder();
            var isInlineComment = false;
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
                        isMultilineComment = true;
                    else
                    {
                        result.Add(new ParseUnit(LanguageUnit.Comment, builder.ToString()));
                        builder.Clear();
                        isMultilineComment = false;
                    }
                }
                else if (isMultilineComment)
                    builder.Append(value);
                else if (value == "#")
                {
                    switch (builder.Length)
                    {
                        case > 0 when builder[0] == '\'' || builder[0] == '\"':
                            builder.Append(value);
                            isInlineComment = false;
                            continue;
                        case > 0:
                            ChooseUnit(result, "", builder);
                            break;
                    }

                    builder.Append(value);
                    isInlineComment = true;
                }
                else if (isInlineComment)
                {
                    if (index == text.Length - 1)
                    {
                        builder.Append(value);
                        result.Add(new ParseUnit(LanguageUnit.Comment, builder.ToString()));
                        builder.Clear();
                    }
                    else if (value == "\n")
                    {
                        isInlineComment = false;
                        result.Add(new ParseUnit(LanguageUnit.Comment, builder.ToString()));
                        result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                        builder.Clear();
                    }
                    else
                        builder.Append(value);
                }
                else
                    ChooseUnit(result, value, builder);


                if (index != text.Length - 1) continue;
                result.Add(ChooseUnit(builder));
            }

            return new ParsingResult {Result = result.Where(unit => !string.IsNullOrEmpty(unit.Symbol)).ToList()};
        }

        private void ChooseUnit(ICollection<ParseUnit> result, string value, StringBuilder strBuilder)
        {
            if (strBuilder.Length > 0 && (strBuilder[0] == '\"' || strBuilder[0] == '\''))
            {
                var firstChar = strBuilder[0];
                strBuilder.Append(value);
                if (value != firstChar.ToString()) return;
                result.Add(new ParseUnit(LanguageUnit.Value, strBuilder.ToString()));
                strBuilder.Clear();
            }
            else if (Units[LanguageUnit.Whitespace].Contains(value))
            {
                result.Add(ChooseUnit(strBuilder));
                result.Add(new ParseUnit(LanguageUnit.Whitespace, value));
                strBuilder.Clear();
            }
            else if (Units[LanguageUnit.Function].Contains(value))
            {
                result.Add(new ParseUnit(LanguageUnit.Function, strBuilder.ToString()));
                result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                strBuilder.Clear();
            }
            else if (Units[LanguageUnit.Symbol].Contains(value))
            {
                result.Add(ChooseUnit(strBuilder));
                result.Add(new ParseUnit(LanguageUnit.Symbol, value));
                strBuilder.Clear();
            }

            else
                strBuilder.Append(value);
        }

        private ParseUnit ChooseUnit(StringBuilder builder)
        {
            if (Units[LanguageUnit.FunctionDefinition].Contains(builder.ToString()))
                return new ParseUnit(LanguageUnit.FunctionDefinition, builder.ToString());

            if (Units[LanguageUnit.Operator].Contains(builder.ToString()))
                return new ParseUnit(LanguageUnit.Operator, builder.ToString());

            if (builder.Length > 0 && int.TryParse(builder[0].ToString(), out _))
                return new ParseUnit(LanguageUnit.Value, builder.ToString());

            if (builder.Length > 0 && char.IsLetter(builder[0]))
                return new ParseUnit(LanguageUnit.Variable, builder.ToString());

            return new ParseUnit(LanguageUnit.Unknown, builder.ToString());
        }
    }
}