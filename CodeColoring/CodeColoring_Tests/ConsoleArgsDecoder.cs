using CodeColoring;

using System;
using System.Linq;
using CodeColoring.ArgsDecoder;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;
using NUnit.Framework;
using NUnit.Framework.Internal;

using Ninject;

namespace CodeColoring_Tests
{
    public class ConsoleArgsDecoder_Tests
    {
        private readonly ConsoleArgsDecoder decoder = new();
        private readonly Randomizer randomizer = new();
        private Parameters parameters;

        private class Parameters
        {
            public readonly string[][] Params;
            public readonly string Input;
            public readonly string Output;
            public Parameters(string input, string output)
            {
                Input = input;
                Output = output;
                Params = new[] { new[] { "-c", "DayTheme" }, new[] { "-i", input }
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
            var result = decoder.Decode(new[] { flag, "DayTheme" });
            var expected = new DecodedArguments() { ColorPalette = new DayTheme() };
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
            var expected = new DecodedArguments() { OutputFormat = Repository.Kernel.Get<HTML>() };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ProgrammingLanguage([Values("-l", "--lang")] string flag)
        {
            var result = decoder.Decode(new[] { flag, "Python" });
            var expected = new DecodedArguments() { ProgrammingLanguage = new Python() };
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
        public void AllParams()
        {
            var input = randomizer.GetString();
            var output = randomizer.GetString();
            var result = decoder.Decode(new[] { "-c", "DayTheme", "-i", input, "-f", "HTML", "-l", "Python", output });
            var expected = new DecodedArguments()
            {
                ColorPalette = new DayTheme(),
                InputFilePath = input,
                OutputFilePath = output,
                OutputFormat = Repository.Kernel.Get<HTML>(),
                ProgrammingLanguage = new Python()
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
                ColorPalette = new DayTheme(),
                InputFilePath = parameters.Input,
                OutputFilePath = parameters.Output,
                OutputFormat = Repository.Kernel.Get<HTML>(),
                ProgrammingLanguage = new Python()
            };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void IncorrectParameterArgumentExceptionWithMessage()
        {
            var flag = "--" + randomizer.GetString(5);
            Assert.IsTrue(
                Assert.Throws<ArgumentException>(() => decoder.Decode(new[] { flag }))
                .Message.Contains(flag));
        }
    }
}