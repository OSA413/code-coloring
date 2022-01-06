namespace CodeColoring.ArgsDecoder
{
    public interface IArgsDecoder
    {
        string Help { get; }
        DecodedArguments Decode(string[] args);
        string Name { get; }
    }
}