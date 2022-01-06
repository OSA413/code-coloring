using System.IO;
using System.Reflection;
using Autofac;
using CodeColoring.ArgsDecoder;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;

namespace CodeColoring
{
    public static class ContainerSetting
    {
        public static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();
            var currentAssembly = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(currentAssembly).AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<StreamReader>().AsSelf().SingleInstance();
            builder.RegisterType<StreamWriter>().AsSelf().SingleInstance();
            builder.RegisterType<Colorizer.Colorizer>().AsSelf().SingleInstance();
            var interfaces = new[] {typeof(IOutputFormat), typeof(IArgsDecoder)};
            var abstractClasses = new[] {typeof(ProgrammingLanguage.ProgrammingLanguage), typeof(ColorPalette)};
            foreach (var abs in abstractClasses)
            {
                builder.RegisterAssemblyTypes(currentAssembly)
                    .Where(x => x.IsSubclassOf(abs))
                    .As(abs).SingleInstance();
            }
            foreach (var inf in interfaces)
            {
                builder.RegisterAssemblyTypes(currentAssembly)
                    .Where(x => inf.IsAssignableFrom(x))
                    .SingleInstance();
            }
            return builder.Build();
        }
    }
}