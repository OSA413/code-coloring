using System.IO;
using CodeColoring.ArgsDecoder;
using Ninject;

namespace CodeColoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dargs = Repository.Kernel.Get<IArgsDecoder>().Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var withColorsApplied = Colorizer.Colorizer.Colorize(parsingResult, dargs.ColorPalette);
            var outputText = dargs.OutputFormat.Format(withColorsApplied);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }
    }
}