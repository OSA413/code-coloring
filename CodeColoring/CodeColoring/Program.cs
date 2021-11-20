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
            var inputText = Repository.Kernel.Get<StreamReader>().ReadToEnd();
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var withColorsApplied = Colorizer.Colorizer.Colorize(parsingResult, dargs.ColorPalette);
            var outputText = dargs.OutputFormat.Format(withColorsApplied);
            Repository.Kernel.Get<StreamWriter>().Write(outputText);
        }
    }
}