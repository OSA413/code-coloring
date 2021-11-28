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
            var readOnlyKernel = ConfigureContainer();
            var dargs = readOnlyKernel.Get<IArgsDecoder>().Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var outputText = dargs.OutputFormat.Format(parsingResult, dargs.ColorPalette);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }

        public static IReadOnlyKernel ConfigureContainer()
        {
            var container = new KernelConfiguration();
            container.Bind<IArgsDecoder>().To<ConsoleArgsDecoder>().InSingletonScope();
            container.Bind<StreamReader>().ToSelf().InSingletonScope();
            container.Bind<StreamWriter>().ToSelf().InSingletonScope();
            container.Bind<Colorizer.Colorizer>().ToSelf().InSingletonScope();
            container.Bind<ColorPalette>().To<DayTheme>().InSingletonScope();
            container.Bind<IOutputFormat>().To<HTML>().InSingletonScope();
            container.Bind<IProgrammingLanguage>().To<Python>().InSingletonScope();
            
            return container.BuildReadonlyKernel();
        }
    }
}