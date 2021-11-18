using CodeColoring.Colorizer;

namespace CodeColoring.OutputFormat
{
    public interface IOutputFormat
    {
        string Format(ColoringResult cr);
    }
}
