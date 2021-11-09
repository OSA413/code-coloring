using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    class ConsoleArgsDecoder: IArgsDecoder
    {
        public DecodedArguments Decode(string[] args)
        {
            throw new NotImplementedException();
        }

        public string Help()
        {
            return "Usage example goes here";
        }
    }
}
