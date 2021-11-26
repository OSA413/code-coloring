using System.IO;
using CodeColoring.ArgsDecoder;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;
using Ninject;

namespace CodeColoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = ConfigureContainer();
            var dargs = container.Get<IArgsDecoder>().Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var outputText = dargs.OutputFormat.Format(parsingResult, dargs.ColorPalette);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }

        private static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();
            container.Bind<IArgsDecoder>().To<ConsoleArgsDecoder>();
            container.Bind<StreamReader>().ToSelf();
            container.Bind<StreamWriter>().ToSelf();
            container.Bind<Colorizer.Colorizer>().ToSelf().InSingletonScope();
            container.Bind<ColorPalette>().To<DayTheme>().Named("DayTheme");
            container.Bind<IOutputFormat>().To<HTML>().Named("HTML");
            container.Bind<IProgrammingLanguage>().To<Python>().Named("Python");

            return container;
        }
    }
}