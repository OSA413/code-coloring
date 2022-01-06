using System;
using System.Linq;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;

namespace CodeColoring.ArgsDecoder
{
    internal class ArgumentAssigner
    {
        private Action<string> action;
        private readonly DecodedArguments decoded;

        private readonly ColorPalette[] colorPalettes;
        private readonly IOutputFormat[] outputFormats;
        private readonly ProgrammingLanguage.ProgrammingLanguage[] programmingLanguages;

        public ArgumentAssigner(DecodedArguments result, ColorPalette[] colorPalettes, IOutputFormat[] outputFormats,
            ProgrammingLanguage.ProgrammingLanguage[] programmingLanguages)
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

        private static bool IsKey(string arg) =>
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
}