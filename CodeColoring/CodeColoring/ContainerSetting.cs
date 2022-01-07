using System.IO;
using System.Linq;
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
            builder.RegisterType<Colorizer.Colorizer>().SingleInstance();
            var abstractClasses = currentAssembly.GetTypes().Where(x=>x.IsAbstract);
            foreach (var abs in abstractClasses)
            {
                builder.RegisterAssemblyTypes(currentAssembly)
                    .Where(x => x.IsSubclassOf(abs))
                    .As(abs).SingleInstance();
            }
            return builder.Build();
        }
    }
}