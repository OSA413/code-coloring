using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Autofac;
using NUnit.Framework;
using NUnit.Framework.Internal;

using CodeColoring;
using CodeColoring.ArgsDecoder;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring_Tests.Steps
{
    [Binding]
    class CodeColoringSteps
    {
        private IArgsDecoder decoder;
        private DecodedArguments dargs;
        private StringWriter consoleWriter;
        private Exception exception = new Exception();

        #region given
        [Given(@"^the user uses Console$")]
        public void GivenConsole()
        {
            var container = Program.ConfigureContainer();
            decoder = container.Resolve<IArgsDecoder>() as ConsoleArgsDecoder;
            consoleWriter = new StringWriter();
            Console.SetOut(consoleWriter);
        }
        #endregion

        #region when
        [When(@"^the user types\s*(.*)?$")]
        public void WhenConsoleArgsFull(string args)
        {
            var split = args.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                dargs = decoder.Decode(split);
                Program.Main(split);
            }
            catch(Exception e)
            {
                exception = e;
            }
        }
        #endregion

        #region then

        [Then(@"^the concole should display output: <(.*)>")]
        public void ThenConsoleOutput(string expectedOutput)
        {
            var output = consoleWriter.ToString();
            Assert.AreEqual(expectedOutput, output);
            CleanUp();
        }

        [Then(@"^the console should display help command output$")]
        public void ThenConsoleHelp()
        {
            var output = consoleWriter.ToString();
            Assert.AreEqual(decoder.Help, output);
            CleanUp();
        }

        [Then(@"^the console should not display any output$")]
        public void ThenConsoleOutputEmpty()
        {
            var output = consoleWriter.ToString();
            Assert.AreEqual(String.Empty, output);
            CleanUp();

        }

        [Then(@"^the file at ([a-zA-Z0-9/._-]+) should be created$")]
        public void ThenFileCreated(string filePath)
        {
            Assert.True(File.Exists(filePath));
        }

        [Then(@"^the file at ([a-zA-Z0-9/._-]+) should not be created$")]
        public void ThenFileNotCreated(string filePath)
        {
            Assert.True(!File.Exists(filePath));
        }

        [Then(@"^([a-zA-Z0-9]+) theme is used$")]
        public void ThenThemeIsUsed(string themeName)
        {
            Assert.AreEqual(themeName, dargs.ColorPalette.Name);
        }

        [Then(@"^([a-zA-Z0-9]+) language is used$")]
        public void ThenLanguageIsUsed(string languageName)
        {
            Assert.AreEqual(languageName, dargs.ProgrammingLanguage.Name);
        }

        [Then(@"^output is in ([a-zA-Z0-9]+) format$")]
        public void ThenFormatIsUsed(string formatName)
        {
            Assert.AreEqual(formatName, dargs.OutputFormat.Name);
        }

        [Then(@"^throws exception with message ""(.[^: ]*): (.*)""$")]
        public void ThenConsoleNonExistentCommandOrException(string exceptionType, string exceptionMsg)
        {
            Assert.AreEqual(exceptionType, exception.GetType().Name);
            Assert.AreEqual(exceptionMsg, exception.Message);
        }
        #endregion

        private void CleanUp()
        {
            var directory = @"../../../DemoContent/";
            foreach (var file in Directory.GetFiles(directory))
                if (file.Contains(".html")) File.Delete(file);
        }
    }
}
