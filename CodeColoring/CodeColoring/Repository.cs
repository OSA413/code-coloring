using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public static class Repository
    {
        public static StandardKernel Kernel;
        static Repository() => Kernel = new StandardKernel();

    }
}
