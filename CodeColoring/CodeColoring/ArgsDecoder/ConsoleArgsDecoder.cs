using System;

namespace CodeColoring
{
    public class ConsoleArgsDecoder : IArgsDecoder
    {
        public string Help => "Usage example goes here";

        private class ArgumentAssigner
        {
            Action<string> action;
            DecodedArguments decoded;

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

            private void HandleColor(string arg) => throw new NotImplementedException();
            private void HandleInputFilePath(string arg) => decoded.InputFilePath = arg;
            private void HandleOutputFormat(string arg) => throw new NotImplementedException();
            private void HandleProgrammingLanguage(string arg) => throw new NotImplementedException();
            private void HandleOutputFilePath(string arg) => decoded.OutputFilePath = arg;

            private bool IsKey(string arg)
            {
                if (arg.StartsWith("-") && arg.Length == 2)
                    return true;
                if (arg.StartsWith("--") && !arg.Contains(" "))
                    return true;
                return false;
            }

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
            //массив ILanguage и смотрим по всем им расширения для поиска нужного по файлу. передаем же файл?
        }
    }
}
