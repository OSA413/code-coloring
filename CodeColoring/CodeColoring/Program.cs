using System;
using System.IO;

namespace CodeColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var conArgDec = new ConsoleArgsDecoder();
            var dargs = conArgDec.Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var withColorsApplied = dargs.ColorPalette.Colorize(parsingResult);
            var outputText = dargs.OutputFormat.Format(withColorsApplied);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }
    }
}
