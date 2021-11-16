using NUnit.Framework;
using CodeColoring;

namespace CodeColoring_Tests
{
    public class Tests
    {
        ConsoleArgsDecoder decoder = new ConsoleArgsDecoder();

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