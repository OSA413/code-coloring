using System;
using System.IO;
using Ninject;

namespace CodeColoring
{
    class Program
    {
        public static StandardKernel ConfigureContainer()
        {
            var container = new StandardKernel();

            container.Bind<IArgsDecoder>().To<ConsoleArgsDecoder>();
            container.Bind<TextReader>().To<ArgsReader>(); //??
            container.Bind<TextWriter>().To<FileWriter>(); //??
            container.Bind<ColorPalette>().ToSelf(); // возможно, создать какой-то интерфейс, но нужен ли он тут
            

            return container;
        }
        static void Main(string[] args)
        {
            // все что ниже, нужно выкинуть в классы, оставив только получение аргов(?) или работу с консолью
            var container = ConfigureContainer();
            var conArgDec = container.Get<IArgsDecoder>();
            var dargs = conArgDec.Decode(args);
            var textReader = container.Get<StreamReader>(); // или что-то другое (не TextReader),
                                                            // или дописать функцию на чтение всех строк,
                                                            // как принимает аргкменты???
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var withColorsApplied = dargs.ColorPalette.Colorize(parsingResult);
            var textWriter = container.Get<TextWriter>(); // вопросы те же 
            var outputText = dargs.OutputFormat.Format(withColorsApplied);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }
    }
}
