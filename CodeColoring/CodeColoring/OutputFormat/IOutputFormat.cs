using CodeColoring.Colorizer;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring.OutputFormat
{
    public interface IOutputFormat
    {
        string Format(ParsingResult pr, ColorPalette palette);
        string Name { get; }
    }
}
