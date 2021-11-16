using NUnit.Framework;
using CodeColoring;

namespace CodeColoring_Tests
{
    public class Tests
    {
        ConsoleArgsDecoder decoder = new ConsoleArgsDecoder();

        void AreEqual(DecodedArguments expected, DecodedArguments actual)
        {
            Assert.AreEqual(expected.ColorPalette, actual.ColorPalette);
            Assert.AreEqual(expected.InputFilePath, actual.InputFilePath);
            Assert.AreEqual(expected.OutputFilePath, actual.OutputFilePath);
            Assert.AreEqual(expected.OutputFormat, actual.OutputFormat);
            Assert.AreEqual(expected.ProgrammingLanguage, actual.ProgrammingLanguage);
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
            var arg = "D:\\nfrinfweio\\dsadasdas";
            DecodedArguments result = decoder.Decode(new string[] { "-i", arg });
            var expected = new DecodedArguments() { InputFilePath = arg };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFilePath()
        {
            var arg = "D:\\nfrinfweio\\dsadasdas";
            DecodedArguments result = decoder.Decode(new string[] { arg });
            var expected = new DecodedArguments() { OutputFilePath = arg };
            AreEqual(expected, result);
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_OutputFormat()
        {
            Assert.Fail();
            /*
            var arg = "D:\\nfrinfweio\\dsadasdas";
            DecodedArguments result = decoder.Decode(new string[] { "-f", arg });
            var expected = new DecodedArguments() { OutputFormat = arg };
            AreEqual(expected, result);
            */
        }

        [Test]
        [Repeat(5)]
        public void OnlyOneParam_ProgrammingLanguage()
        {
            Assert.Fail();
            /*
            var arg = "D:\\nfrinfweio\\dsadasdas";
            DecodedArguments result = decoder.Decode(new string[] { "-l", arg });
            var expected = new DecodedArguments() { ProgrammingLanguage = arg };
            AreEqual(expected, result);
            */
        }



        [Test]
        [Repeat(5)]
        public void AllParams()
        {
            Assert.Fail();
            /*
            var arg = "D:\\nfrinfweio\\dsadasdas";
            DecodedArguments result = decoder.Decode(new string[] { "-l", arg });
            var expected = new DecodedArguments()
            {
                ProgrammingLanguage = arg
            };
            AreEqual(expected, result);
            */
        }

        [Test]
        [Repeat(250)]
        public void AllParamsRandomOrder()
        {
            Assert.Fail();
        }
    }
}