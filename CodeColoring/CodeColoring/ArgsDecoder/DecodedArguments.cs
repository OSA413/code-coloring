using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring.ArgsDecoder
{
    public class DecodedArguments
    {
        public string InputFilePath;
        public ColorPalette ColorPalette;
        public IProgrammingLanguage ProgrammingLanguage;
        public string OutputFilePath;
        public IOutputFormat OutputFormat;
    }
}