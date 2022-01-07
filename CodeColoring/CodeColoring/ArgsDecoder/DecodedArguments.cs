using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;

namespace CodeColoring.ArgsDecoder
{
    public class DecodedArguments
    {
        public string InputFilePath;
        public ColorPalette ColorPalette;
        public ProgrammingLanguage.ProgrammingLanguage ProgrammingLanguage;
        public string OutputFilePath;
        public IOutputFormat OutputFormat;
    }
}