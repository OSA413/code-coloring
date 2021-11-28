using System;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;
using System.Linq;
using Ninject;

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
        private readonly IProgrammingLanguage[] programmingLanguages;

        public ConsoleArgsDecoder(ColorPalette[] colorPalettes, IOutputFormat[] outputFormats, IProgrammingLanguage[] programmingLanguages)
        {
            this.colorPalettes = colorPalettes;
            this.outputFormats = outputFormats;
            this.programmingLanguages = programmingLanguages;
        }

        private class ArgumentAssigner
        {
            private Action<string> action;
            private readonly DecodedArguments decoded;

            private readonly ColorPalette[] colorPalettes;
            private readonly IOutputFormat[] outputFormats;
            private readonly IProgrammingLanguage[] programmingLanguages;

            public ArgumentAssigner(DecodedArguments result, ColorPalette[] colorPalettes, IOutputFormat[] outputFormats, IProgrammingLanguage[] programmingLanguages)
            {
                decoded = result;
                this.colorPalettes = colorPalettes;
                this.outputFormats = outputFormats;
                this.programmingLanguages = programmingLanguages;
            }
            
            public void Process(string arg)
            {
                if (IsKey(arg))
                    action = GetAction(arg);
                else if (action != null)
                {
                    action(arg);
                    action = null;
                }
                else
                    HandleOutputFilePath(arg);
            }

            private void HandleColor(string arg) =>
                decoded.ColorPalette = colorPalettes.Single(x => x.Name == arg);
            private void HandleInputFilePath(string arg) =>
                decoded.InputFilePath = arg;
            private void HandleOutputFormat(string arg) =>
                decoded.OutputFormat = outputFormats.Single(x => x.Name == arg);
            private void HandleProgrammingLanguage(string arg) =>
                decoded.ProgrammingLanguage = programmingLanguages.Single(x => x.Name == arg);
            private void HandleOutputFilePath(string arg) =>
                decoded.OutputFilePath = arg;

            private bool IsKey(string arg) =>
                arg.StartsWith("-") || arg.StartsWith("--");

            private Action<string> GetAction(string arg)
            {
                if (arg == "-f" || arg == "--format")
                    return HandleOutputFormat;
                if (arg == "-i" || arg == "--input")
                    return HandleInputFilePath;
                if (arg == "-c" || arg == "--color")
                    return HandleColor;
                if (arg == "-l" || arg == "--lang")
                    return HandleProgrammingLanguage;
                throw new ArgumentException("Unknown argument: " + arg);
            }
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
