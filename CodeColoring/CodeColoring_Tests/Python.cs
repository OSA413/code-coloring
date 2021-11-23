using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

using CodeColoring.ProgrammingLanguage;

namespace CodeColoring_Tests
{
    internal partial class Python_Tests
    {
        private readonly Python python = new();

        private static void SameOutput(List<(string arg, LanguageUnit LanguageUnit)> expected, ParsingResult actual)
        {
            Assert.AreEqual(expected.Count, actual.Result.Count, "Expected different lengh");
            for (var i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].arg, actual.Result[i].Symbol, "Difference at index " + i);
                Assert.AreEqual(expected[i].LanguageUnit, actual.Result[i].Unit, "Different unit for [" + expected[i].arg + "]");
            }
        }

        [Test]
        [Repeat(5)]
        public void SimpleAssignment()
        {
            var input = "x = 5";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("x", LanguageUnit.Variable),
                (" ", LanguageUnit.Whitespace),
                ("=", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("5", LanguageUnit.Value)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void SimpleAssignmentAndPrint()
        {
            var input = "x = 5\n print(x)";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("x", LanguageUnit.Variable),
                (" ", LanguageUnit.Whitespace),
                ("=", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("5", LanguageUnit.Value),
                ("\n", LanguageUnit.Whitespace),
                (" ", LanguageUnit.Whitespace),
                ("print", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("x", LanguageUnit.Variable),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void DefAPrint123()
        {
            var input = "def a(): print(\"123\")";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("def", LanguageUnit.FunctionDefinition),
                (" ", LanguageUnit.Whitespace),
                ("a", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                (")", LanguageUnit.Symbol),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("print", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("\"123\"", LanguageUnit.Value),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }
        [Test]
        [Repeat(5)]
        public void TestWithOperator()
        {
            var input = "if x==5\n print(\"yes\")";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("if", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("=", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
                ("\n", LanguageUnit.Whitespace),
                (" ", LanguageUnit.Whitespace),
                ("print", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("\"yes\"", LanguageUnit.Value),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }
        
        [Test]
        [Repeat(5)]
        public void SingleQuotes()
        {
            var input = "\'yes\'";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("'yes'", LanguageUnit.Value)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void TestWithComment0()
        {
            var input = "a=3#hahah \nx=5";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("a", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("3", LanguageUnit.Value),
                ("#hahah ", LanguageUnit.Comment),
                ("\n", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void TestWithComment1()
        {
            var input = "a=3 #hahah \nx=5";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("a", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("3", LanguageUnit.Value),
                (" ", LanguageUnit.Whitespace),
                ("#hahah ", LanguageUnit.Comment),
                ("\n", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void ClassExample()
        {
            var input = "class Employee:\n\t\"\"\"Базовый класс для всех сотрудников\"\"\"\n\temp_count=0\n\n"
                + "\tdef __init__(self, name):\n\t\tself.name=name";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("class", LanguageUnit.FunctionDefinition),
                (" ", LanguageUnit.Whitespace),
                ("Employee", LanguageUnit.Variable),
                (":", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("\"\"\"Базовый класс для всех сотрудников\"\"\"", LanguageUnit.Value),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("emp_count", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("0", LanguageUnit.Value),
                ("\n", LanguageUnit.Whitespace),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("def", LanguageUnit.FunctionDefinition),
                (" ", LanguageUnit.Whitespace),
                ("__init__", LanguageUnit.Variable),
                ("(", LanguageUnit.Symbol),
                ("self", LanguageUnit.Variable),
                (",", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("name", LanguageUnit.Variable),
                (")", LanguageUnit.Symbol),
                (":", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("self", LanguageUnit.Function),
                (".", LanguageUnit.Symbol),
                ("name", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("name", LanguageUnit.Variable)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void UnsupportedCharsAreUnknown()
        {
            var input = "\\";
            var expected = input.Select(x => (x.ToString(), LanguageUnit.Unknown)).ToList();
            SameOutput(expected, python.Parse(input));
        }
    }
}
