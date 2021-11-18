using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring
{
    public static class Repository
    {
        public static StandardKernel Kernel;
        //static Repository() => Kernel = new StandardKernel(); потом вернуть к этому виду

        static Repository()
        {
            Kernel = new StandardKernel();
            Kernel.Bind<ColorPalette>().To<DayTheme>().Named("DayTheme");
            Kernel.Bind<IOutputFormat>().To<HTML>().Named("HTML");
            Kernel.Bind<IProgrammingLanguage>().To<Python>().Named("Python");
        }
    }
}
