using System.Collections.Generic;

namespace CodeColoring.ProgrammingLanguage.Languages
{
    public class Python : ProgrammingLanguage
    {
        public Python()
        {
            Name = "Python";
            Extensions = new[] {".py"};
            base.Units = Units;
        }
        
        private new static readonly Dictionary<LanguageUnit, string[]> Units = new()
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
            },
            {
                LanguageUnit.Comment,
                new []{"#"}
            }
        };
    }
}