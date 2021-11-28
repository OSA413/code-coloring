using System;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;
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

        private class ArgumentAssigner
        {
            private Action<string> action;
            private readonly DecodedArguments decoded;
            private readonly IReadOnlyKernel container = Program.ConfigureContainer();
            
            public ArgumentAssigner(DecodedArguments result) => decoded = result;
            
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
                decoded.ColorPalette = container.Get<ColorPalette>(arg);
            private void HandleInputFilePath(string arg) =>
                decoded.InputFilePath = arg;
            private void HandleOutputFormat(string arg) =>
                decoded.OutputFormat = container.Get<IOutputFormat>(arg);
            private void HandleProgrammingLanguage(string arg) =>
                decoded.ProgrammingLanguage = container.Get<IProgrammingLanguage>(arg);
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
            var assigner = new ArgumentAssigner(result);
            foreach (var arg in args)
                assigner.Process(arg);

            return result;
        }
    }
}
