using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public enum LanguageUnit
    {
        Symbols ,
        FunctionDefinition,
        Variable = default,
        Function,
        Operator
        
    }
}
