using System;
using System.IO;
using Ninject;
using Ninject.Extensions.Conventions;

namespace CodeColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var dargs = new ConsoleArgsDecoder().Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var withColorsApplied = Colorizer.Colorize(parsingResult, dargs.ColorPalette);
            var outputText = dargs.OutputFormat.Format(withColorsApplied);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }
    }
}
