using System;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
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
        private readonly Randomizer randomizer = new();
        private readonly IContainer container = Program.ConfigureContainer();
        private readonly ConsoleArgsDecoder decoder;
        private readonly ColorPalette[] palettes;
        private readonly IOutputFormat[] outputFormats;
        private readonly ProgrammingLanguage[] programmingLanguages;
        

        public ConsoleArgsDecoder_Tests()
        {
            decoder = (ConsoleArgsDecoder) container.Resolve<IArgsDecoder>();
            palettes = container.Resolve<ColorPalette[]>();
            outputFormats = container.Resolve<IOutputFormat[]>();
            programmingLanguages = container.Resolve<ProgrammingLanguage[]>();
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
                Params = new[] { new[] { "-c", "Default" }, new[] { "-i", input }
                , new[] { "-f", "HTML" }, new[] { "-l", "Python" }, new[] { output } };
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
            var result = decoder.Decode(new[] { flag, "Default" });
            
            var expected = new DecodedArguments() { ColorPalette = palettes.First(x=> x.Name == "Default") };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_InputFilePath([Values("-i", "--input")] string flag)
        {
            var arg = randomizer.GetString();
            var result = decoder.Decode(new[] { flag, arg });
            var expected = new DecodedArguments() { InputFilePath = arg };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFilePath()
        {
            var arg = randomizer.GetString();
            var result = decoder.Decode(new[] { arg });
            var expected = new DecodedArguments() { OutputFilePath = arg };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFormat([Values("-f", "--format")] string flag)
        {
            var result = decoder.Decode(new[] { flag, "HTML" });
            var expected = new DecodedArguments() { OutputFormat = outputFormats.First(x=>x.Name == "HTML") };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ProgrammingLanguage([Values("-l", "--lang")] string flag)
        {
            var result = decoder.Decode(new[] { flag, "Python" });
            var expected = new DecodedArguments() { ProgrammingLanguage = programmingLanguages.First(x=> x.Name=="Python") };
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
            var result = decoder.Decode(new[] { "-c", "Default", "-i", input, "-f", "HTML", "-l", "Python", output });
            var expected = new DecodedArguments()
            {
                ColorPalette = palettes.First(x=>x.Name == "Default"),
                InputFilePath = input,
                OutputFilePath = output,
                OutputFormat = outputFormats.First(x=>x.Name == "HTML"),
                ProgrammingLanguage = programmingLanguages.First(x=>x.Name == "Python")
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
                ColorPalette = palettes.First(x=>x.Name == "Default"),
                InputFilePath = parameters.Input,
                OutputFilePath = parameters.Output,
                OutputFormat = outputFormats.First(x=>x.Name == "HTML"),
                ProgrammingLanguage = programmingLanguages.First(x=>x.Name == "Python")
            };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void IncorrectShortParameterArgumentExceptionWithMessage()
        {
            var flag = "-" + randomizer.GetString(3);
            var error = Assert.Throws<ArgumentException>(() => decoder.Decode(new[] { flag }));
            Assert.IsTrue(error.Message.Contains("Unknown argument"));
            Assert.IsTrue(error.Message.Contains(flag));
        }

        [Test]
        [Repeat(5)]
        public void IncorrectParameterArgumentExceptionWithMessage()
        {
            var flag = "--" + randomizer.GetString(5);
            var error = Assert.Throws<ArgumentException>(() => decoder.Decode(new[] { flag }));
            Assert.IsTrue(error.Message.Contains("Unknown argument"));
            Assert.IsTrue(error.Message.Contains(flag));
        }
    }
}