using System;
using System.Collections.Generic;
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
                decoded.ColorPalette = Repository.Kernel.Get<ColorPalette>(arg);
            private void HandleInputFilePath(string arg) =>
                decoded.InputFilePath = arg;
            private void HandleOutputFormat(string arg) =>
                decoded.OutputFormat = Repository.Kernel.Get<IOutputFormat>(arg);
            private void HandleProgrammingLanguage(string arg) =>
                decoded.ProgrammingLanguage = Repository.Kernel.Get<IProgrammingLanguage>(arg);
            private void HandleOutputFilePath(string arg) =>
                decoded.OutputFilePath = arg;

            private static bool IsKey(string arg)
            {
                if (arg.StartsWith("-") && arg.Length == 2)
                    return true;
                return arg.StartsWith("--") && !arg.Contains(" ");
            }

            private Action<string> GetAction(string arg)
            {
                switch (arg)
                {
                    case "-f":
                    case "--format":
                        return HandleOutputFormat;
                    case "-i":
                    case "--input":
                        return HandleInputFilePath;
                    case "-c":
                    case "--color":
                        return HandleColor;
                    case "-l":
                    case "--lang":
                        return HandleProgrammingLanguage;
                    default:
                        throw new ArgumentException("Unknown argument: " + arg);
                }
            }
        }

        public DecodedArguments Decode(IEnumerable<string> args)
        {
            var result = new DecodedArguments();
            var assigner = new ArgumentAssigner(result);
            foreach (var arg in args)
                assigner.Process(arg);

            return result;
        }
    }
}
