using System;
using System.Diagnostics;
using System.Linq;
using CodeColoring;
using CodeColoring.ArgsDecoder;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Autofac;
using CodeColoring.ProgrammingLanguage.Languages;

namespace CodeColoring_Tests
{
    public class ConsoleArgsDecoder_Tests
    {
        private Parameters parameters;
        private readonly Randomizer randomizer;
        private readonly ConsoleArgsDecoder decoder;
        private readonly ColorPalette palette;
        private readonly IOutputFormat outputFormat;
        private readonly ProgrammingLanguage programmingLanguage;

        private const string DefaultProgrammingLanguageName = "Python";
        private const string DefaultColorPalletName = "Default";
        private const string DefaultOutputFormatName = "HTML";
        

        public ConsoleArgsDecoder_Tests()
        {
            randomizer = new Randomizer();
            var container = ContainerSetting.ConfigureContainer();
            var d = container.Resolve<Colorizer[]>();
            decoder = (ConsoleArgsDecoder) container.Resolve<IArgsDecoder[]>().First(x=> x.GetType() == typeof(ConsoleArgsDecoder));
            palette = ContainerSetting.ConfigureContainer().Resolve<ColorPalette[]>().First(x=> x.Name == "Default");
            outputFormat = ContainerSetting.ConfigureContainer().Resolve<IOutputFormat[]>().First(x=> x.Name == "HTML");
            programmingLanguage = ContainerSetting.ConfigureContainer().Resolve<ProgrammingLanguage[]>().First(x=> x.Name == "Python");
            

        }

        private class Parameters
        {
            public readonly string[][] Params;
            public readonly string Input;
            public readonly string Output;

            public Parameters(string input, string output)
            {
                Input = input;
                Output = output;
                Params = new[]
                {
                    new[] {"-c", DefaultColorPalletName}, new[] {"-i", input}, new[] {"-f", DefaultOutputFormatName},
                    new[] {"-l", DefaultProgrammingLanguageName}, new[] {output}
                };
            }
        }

        [SetUp]
        public void SetUp()
        {
            parameters = new Parameters(randomizer.GetString(), randomizer.GetString());
        }

        private static void CheckType(object expected, object actual)
        {
            if (expected == null) Assert.IsNull(actual);
            else Assert.IsInstanceOf(expected.GetType(), actual);
        }

        private void AreEqual(DecodedArguments expected, DecodedArguments actual)
        {
            CheckType(expected.ColorPalette, actual.ColorPalette);
            Assert.AreEqual(expected.InputFilePath, actual.InputFilePath);
            Assert.AreEqual(expected.OutputFilePath, actual.OutputFilePath);
            CheckType(expected.OutputFormat, actual.OutputFormat);
            CheckType(expected.ProgrammingLanguage, actual.ProgrammingLanguage);
        }

        [Test]
        [Repeat(5)]
        public void EmptyParams()
        {
            var result = decoder.Decode(new string[] { });
            var expected = new DecodedArguments();
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ColorPalette([Values("-c", "--color")] string flag)
        {
            var result = decoder.Decode(new[] {flag, DefaultColorPalletName});

            var expected = new DecodedArguments()
                {ColorPalette =palette};
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_InputFilePath([Values("-i", "--input")] string flag)
        {
            var arg = randomizer.GetString();
            var result = decoder.Decode(new[] {flag, arg});
            var expected = new DecodedArguments() {InputFilePath = arg};
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFilePath()
        {
            var arg = randomizer.GetString();
            var result = decoder.Decode(new[] {arg});
            var expected = new DecodedArguments() {OutputFilePath = arg};
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFormat([Values("-f", "--format")] string flag)
        {
            var result = decoder.Decode(new[] {flag, DefaultOutputFormatName});
            var expected = new DecodedArguments()
                {OutputFormat = outputFormat};
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ProgrammingLanguage([Values("-l", "--lang")] string flag)
        {
            var result = decoder.Decode(new[] {flag, DefaultProgrammingLanguageName});
            var expected = new DecodedArguments()
                {ProgrammingLanguage = programmingLanguage};
            AreEqual(expected, result);
        }


        [Test]
        [Repeat(5)]
        [TestCase("-f", "--format")]
        [TestCase("-l", "--lang")]
        [TestCase("-c", "--color")]
        [TestCase("-i", "--input")]
        public void HelpContainsParameters(string shrt, string full)
        {
            Assert.IsTrue(decoder.Help.Contains(shrt), shrt);
            Assert.IsTrue(decoder.Help.Contains(full), full);
        }

        [Test]
        [Repeat(5)]
        [TestCase("Code Coloring")]
        [TestCase("Usage Example")]
        public void HelpIsInformative(string text) =>
            Assert.IsTrue(decoder.Help.Contains(text));

        [Test]
        [Repeat(5)]
        public void AllParams()
        {
            var input = randomizer.GetString();
            var output = randomizer.GetString();
            var result = decoder.Decode(new[]
            {
                "-c", DefaultColorPalletName, "-i", input, "-f", DefaultOutputFormatName, "-l",
                DefaultProgrammingLanguageName, output
            });
            var expected = new DecodedArguments()
            {
                ColorPalette = palette,
                InputFilePath = input,
                OutputFilePath = output,
                OutputFormat = outputFormat,
                ProgrammingLanguage = programmingLanguage
            };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(250)]
        public void AllParamsRandomOrder()
        {
            var args = parameters.Params.OrderBy(_ => randomizer.Next()).SelectMany(x => x).ToArray();
            var result = decoder.Decode(args);
            var expected = new DecodedArguments
            {
                ColorPalette = palette,
                InputFilePath = parameters.Input,
                OutputFilePath = parameters.Output,
                OutputFormat = outputFormat,
                ProgrammingLanguage = programmingLanguage
            };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void IncorrectShortParameterArgumentExceptionWithMessage()
        {
            var flag = "-" + randomizer.GetString(3);
            var error = Assert.Throws<ArgumentException>(() => decoder.Decode(new[] {flag}));
            Debug.Assert(error != null, nameof(error) + " != null");
            Assert.IsTrue(error.Message.Contains("Unknown argument"));
            Assert.IsTrue(error.Message.Contains(flag));
        }

        [Test]
        [Repeat(5)]
        public void IncorrectParameterArgumentExceptionWithMessage()
        {
            var flag = "--" + randomizer.GetString(5);
            var error = Assert.Throws<ArgumentException>(() => decoder.Decode(new[] {flag}));
            Debug.Assert(error != null, nameof(error) + " != null");
            Assert.IsTrue(error.Message.Contains("Unknown argument"));
            Assert.IsTrue(error.Message.Contains(flag));
        }
    }
}