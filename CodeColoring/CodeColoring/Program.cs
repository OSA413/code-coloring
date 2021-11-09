using System;

namespace CodeColoring
{
    class Program
    {
        static void Main(string[] args)
        {
            var conArgDec = new ConsoleArgsDecoder();
            conArgDec.Decode(args);
        }
    }
}
