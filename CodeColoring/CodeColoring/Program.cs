using System;
using System.IO;
using CodeColoring.ArgsDecoder;
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
            var withColorsApplied = Colorizer.Colorizer.Colorize(parsingResult, dargs.ColorPalette);
            var outputText = dargs.OutputFormat.Format(withColorsApplied);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }
    }
}
