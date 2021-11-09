using System;

namespace CodeColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var conArgDec = new ConsoleArgsDecoder();
            var dargs = conArgDec.Decode(args);
        }
    }
}
