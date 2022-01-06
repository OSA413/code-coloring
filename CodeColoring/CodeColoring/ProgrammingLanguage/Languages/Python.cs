using System.Collections.Generic;

namespace CodeColoring.ProgrammingLanguage.Languages
{
    public class Python : IProgrammingLanguage
    {
        public override string Name => "Python";
        
        public override string[] Extensions() => new[] {".py"};


        protected override Dictionary<LanguageUnit, string[]> Units() => new()
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