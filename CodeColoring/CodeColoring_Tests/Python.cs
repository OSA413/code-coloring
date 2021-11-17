﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

using CodeColoring;

namespace CodeColoring_Tests
{
    class Python_Tests
    {
        public Python python = new Python();

        private void SameOutput(List<(string arg, LanguageUnit LanguageUnit)> expected, ParsingResult actual)
        {
            Assert.AreEqual(expected.Count, actual.Result.Count, "Expected different lengh");
            for (int i = 0; i < expected.Count; i++)
            {
                Assert.AreEqual(expected[i].arg, actual.Result[i].arg, "Difference at index " + i);
                Assert.AreEqual(expected[i].LanguageUnit, actual.Result[i].LanguageUnit, "Different unit for [" + expected[i].arg + "]");
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
    }
}
