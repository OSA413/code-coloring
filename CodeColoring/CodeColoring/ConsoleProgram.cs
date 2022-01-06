using System.IO;
using Autofac;
using CodeColoring.ArgsDecoder;

namespace CodeColoring
{
    public static class ConsoleProgram
    {
        public static void Main(string[] args)
        {
            var readOnlyKernel = ContainerSetting.ConfigureContainer();
            var dargs = readOnlyKernel.Resolve<IArgsDecoder>().Decode(args);
            var inputText = File.ReadAllText(dargs.InputFilePath);
            var parsingResult = dargs.ProgrammingLanguage.Parse(inputText);
            var outputText = dargs.OutputFormat.Format(parsingResult, dargs.ColorPalette);
            File.WriteAllText(dargs.OutputFilePath, outputText);
        }
    }
    
}