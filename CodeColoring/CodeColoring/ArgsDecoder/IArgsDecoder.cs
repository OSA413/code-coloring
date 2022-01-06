using System.Collections.Generic;

namespace CodeColoring.ArgsDecoder
{
    public interface IArgsDecoder
    {
        string Help { get; }
        DecodedArguments Decode(IEnumerable<string> args);
    }
}
