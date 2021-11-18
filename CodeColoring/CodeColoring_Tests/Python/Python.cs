using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NUnit.Framework;

using CodeColoring;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring_Tests
{
    partial class Python_Tests
    {
        public Python python = new();

        private void SameOutput(List<(string arg, LanguageUnit LanguageUnit)> expected, ParseUnit[] actual)
        {
            Assert.AreEqual(expected.Count, actual.Length, "Expected different lengh");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].arg, actual[i].Symbol, "Difference at index " + i);
                Assert.AreEqual(expected[i].LanguageUnit, actual[i].Unit, "Different unit for [" + expected[i].arg + "]");
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
        public void TestWithComment()
        {
            var input = "#hahah \nif x==5\n print(\"yes\")";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("#hahah ", LanguageUnit.Comment),
                ("\n", LanguageUnit.Whitespace),
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
    }
}
