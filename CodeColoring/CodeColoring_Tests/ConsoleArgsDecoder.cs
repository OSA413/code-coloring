using CodeColoring;

using System.Linq;
using CodeColoring.ArgsDecoder;
using CodeColoring.Colorizer;
using CodeColoring.OutputFormat;
using CodeColoring.ProgrammingLanguage;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace CodeColoring_Tests
{
    public class ConsoleArgsDecoder_Tests
    {
        ConsoleArgsDecoder decoder = new ConsoleArgsDecoder();
        Randomizer randomizer = new Randomizer();
        Parameters parameters;
        
        public class Parameters
        {
            public string[][] Params;
            public string Input;
            public string Output;
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

        void CheckType(object expected, object actual)
        {
            if (expected == null) Assert.IsNull(actual);
            else Assert.IsInstanceOf(expected.GetType(), actual);
        }

        void AreEqual(DecodedArguments expected, DecodedArguments actual)
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
            DecodedArguments result = decoder.Decode(new string[] { });
            var expected = new DecodedArguments();
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ColorPalette()
        {
            DecodedArguments result = decoder.Decode(new string[] { "-c", "DayTheme" });
            var expected = new DecodedArguments() { ColorPalette = new DayTheme() };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_InputFilePath()
        {
            var arg = randomizer.GetString();
            DecodedArguments result = decoder.Decode(new string[] { "-i", arg });
            var expected = new DecodedArguments() { InputFilePath = arg };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFilePath()
        {
            var arg = randomizer.GetString();
            DecodedArguments result = decoder.Decode(new string[] { arg });
            var expected = new DecodedArguments() { OutputFilePath = arg };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFormat()
        {
            DecodedArguments result = decoder.Decode(new string[] { "-f", "HTML" });
            var expected = new DecodedArguments() { OutputFormat = new HTML() };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ProgrammingLanguage()
        {
            DecodedArguments result = decoder.Decode(new string[] { "-l", "Python" });
            var expected = new DecodedArguments() { ProgrammingLanguage = new Python() };
            AreEqual(expected, result);
        }



        [Test]
        [Repeat(5)]
        public void AllParams()
        {
            var input = randomizer.GetString();
            var output = randomizer.GetString();
            DecodedArguments result = decoder.Decode(new string[] { "-c", "DayTheme", "-i", input, "-f", "HTML", "-l", "Python", output });
            var expected = new DecodedArguments()
            {
                ColorPalette = new DayTheme(),
                InputFilePath = input,
                OutputFilePath = output,
                OutputFormat = new HTML(),
                ProgrammingLanguage = new Python()
            };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(250)]
        public void AllParamsRandomOrder()
        {
            var args = parameters.Params.OrderBy(x => randomizer.Next()).SelectMany(x => x).ToArray();
            DecodedArguments result = decoder.Decode(args);
            var expected = new DecodedArguments()
            {
                ColorPalette = new DayTheme(),
                InputFilePath = parameters.Input,
                OutputFilePath = parameters.Output,
                OutputFormat = new HTML(),
                ProgrammingLanguage = new Python()
            };
            AreEqual(expected, result);
        }
    }
}