using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
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
            var dargs = readOnlyKernel.Resolve<IArgsDecoder>().Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var outputText = dargs.OutputFormat.Format(parsingResult, dargs.ColorPalette);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }

        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(currentAssembly).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StreamReader>().AsSelf().SingleInstance();
            builder.RegisterType<StreamWriter>().AsSelf().SingleInstance();
            builder.RegisterType<Colorizer.Colorizer>().AsSelf().SingleInstance();
            builder.RegisterAssemblyTypes(currentAssembly)
              .Where(x => x.Name.EndsWith("Theme"))
              .As<ColorPalette>().AsSelf().SingleInstance();

            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //  .Where(x => x is IOutputFormat)
            //  .AsSelf().AsImplementedInterfaces();


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
              .Where(x => x.Equals(typeof(HTML)))
              .AsSelf();

            builder.RegisterType<ConsoleArgsDecoder>().AsSelf();
            builder.RegisterType<Python>().AsSelf().SingleInstance();
            
            return builder.Build();
        }
    }
}