using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;


namespace CodeColoring.ArgsDecoder
{
    public class ConsoleArgsDecoder : IArgsDecoder
    {
        public string Help => "Code Coloring\n\nAvailable parameters:"
                              + "\n\t-i, --input\tInput file path"
                              + "\n\t-l, --lang\tInput programming language"
                              + "\n\t-f, --format\tOutput format"
                              + "\n\t-c, --color\tOutput color palette"
                              + "\n\nUsage Example:\nCodeColoring -i D:\\main.py -c DayTheme -l Python -f HTML D:\\main.html";


        private readonly ColorPalette[] colorPalettes;
        private readonly IOutputFormat[] outputFormats;
        private readonly ProgrammingLanguage.ProgrammingLanguage[] programmingLanguages;

        public ConsoleArgsDecoder(ColorPalette[] colorPalettes, IOutputFormat[] outputFormats,
            ProgrammingLanguage.ProgrammingLanguage[] programmingLanguages)
        {
            this.colorPalettes = colorPalettes;
            this.outputFormats = outputFormats;
            this.programmingLanguages = programmingLanguages;
        }


        public DecodedArguments Decode(string[] args)
        {
            var result = new DecodedArguments();
            var assigner = new ArgumentAssigner(result, colorPalettes, outputFormats, programmingLanguages);
            foreach (var arg in args)
                assigner.Process(arg);

            return result;
        }

    }
}