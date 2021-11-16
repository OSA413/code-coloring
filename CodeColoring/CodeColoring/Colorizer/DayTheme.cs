using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public class DayTheme: ColorPalette
    {
        static DayTheme()
        {
            Repository.Kernel.Bind<ColorPalette>().To<DayTheme>().Named("DayTheme");
        }
    }
}
