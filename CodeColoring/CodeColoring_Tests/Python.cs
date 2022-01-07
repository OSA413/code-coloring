using System;
using System.Linq;
using System.Collections.Generic;
using Autofac;
using CodeColoring;
using NUnit.Framework;
using CodeColoring.ProgrammingLanguage;
using CodeColoring.ProgrammingLanguage.Languages;

namespace CodeColoring_Tests
{
    internal class Python_Tests
    {
        private readonly Python python;

        public Python_Tests()
        {
            python = (Python) ContainerSetting.ConfigureContainer().Resolve<ProgrammingLanguage[]>().First(x=> x.Name == "Python");
        }

        private static void SameOutput(IReadOnlyList<(string arg, LanguageUnit LanguageUnit)> expected,
            ParsingResult actual)
        {
            for (var i = 0; i < Math.Min(expected.Count, actual.Result.Count); i++)
            {
                Assert.AreEqual(expected[i].arg, actual.Result[i].Symbol, "Difference at index " + i);
                Assert.AreEqual(expected[i].LanguageUnit, actual.Result[i].Unit,
                    "Different unit for [" + expected[i].arg + "], index " + i);
            }

            Assert.AreEqual(expected.Count, actual.Result.Count, "Expected different length");
        }

        [Test]
        [Repeat(5)]
        public void SimpleAssignment()
        {
            const string input = "x = 5";
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
            const string input = "x = 5\n print(x)";
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
            const string input = "def a(): print(\"123\")";
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
            const string input = "if x==5\n print(\"yes\")";
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
        public void TestWithHashInString()
        {
            const string input = "if x==5\n print(\"#yes\")";
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
                ("\"#yes\"", LanguageUnit.Value),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void SingleQuotes()
        {
            const string input = "\'yes\'";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("'yes'", LanguageUnit.Value)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void TestWithCommentAfterOperator()
        {
            const string input = "if x==5\n print(\"yes\")#ha-ha";
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
                (")", LanguageUnit.Symbol),
                ("#ha-ha", LanguageUnit.Comment)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void TestWithComment0()
        {
            const string input = "a=3#hahah \nx=5";
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
            const string input = "a=3 #hahah \nx=5";
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
            const string input = "class Employee:\n\temp_count=0\n\n"
                                 + "\tdef __init__(self, name):\n\t\tself.name=name";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("class", LanguageUnit.FunctionDefinition),
                (" ", LanguageUnit.Whitespace),
                ("Employee", LanguageUnit.Variable),
                (":", LanguageUnit.Symbol),
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
                ("__init__", LanguageUnit.Function),
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
            const string input = "\\";
            var expected = input.Select(x => (x.ToString(), LanguageUnit.Unknown)).ToList();
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void MultilineString()
        {
            const string input = "\"\"\"Multiline \n\tstring\t\n\"\"\"";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("\"\"\"Multiline \n\tstring\t\n\"\"\"", LanguageUnit.Comment)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void SeveralMultilineStrings()
        {
            const string input = "\"\"\"Multiline \n\tstring\t\n\"\"\"\n\n\"\"\"llamao\"\"\"";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("\"\"\"Multiline \n\tstring\t\n\"\"\"", LanguageUnit.Comment),
                ("\n", LanguageUnit.Whitespace),
                ("\n", LanguageUnit.Whitespace),
                ("\"\"\"llamao\"\"\"", LanguageUnit.Comment),
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void SomeMath()
        {
            const string input = "x=(int(((10+8)*2-9)/2%5)^6)+1";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("x", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("(", LanguageUnit.Symbol),
                ("int", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("(", LanguageUnit.Symbol),
                ("(", LanguageUnit.Symbol),
                ("10", LanguageUnit.Value),
                ("+", LanguageUnit.Symbol),
                ("8", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                ("*", LanguageUnit.Symbol),
                ("2", LanguageUnit.Value),
                ("-", LanguageUnit.Symbol),
                ("9", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                ("/", LanguageUnit.Symbol),
                ("2", LanguageUnit.Value),
                ("%", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                ("^", LanguageUnit.Symbol),
                ("6", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                ("+", LanguageUnit.Symbol),
                ("1", LanguageUnit.Value)
            };
            SameOutput(expected, python.Parse(input));
        }


        [Test]
        [Repeat(5)]
        public void ForLoop()
        {
            const string input = "[x for x in range(5)]";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("[", LanguageUnit.Symbol),
                ("x", LanguageUnit.Variable),
                (" ", LanguageUnit.Whitespace),
                ("for", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                (" ", LanguageUnit.Whitespace),
                ("in", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("range", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                ("]", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void Semicolon()
        {
            const string input = "x=5;print(x)";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("x", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
                (";", LanguageUnit.Symbol),
                ("print", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("x", LanguageUnit.Variable),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void Condition()
        {
            const string input = "if x>10: pass\nelif x<5: del x\nelse raise KeyError";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("if", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                (">", LanguageUnit.Symbol),
                ("10", LanguageUnit.Value),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("pass", LanguageUnit.Operator),
                ("\n", LanguageUnit.Whitespace),
                ("elif", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("<", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("del", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("\n", LanguageUnit.Whitespace),
                ("else", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("raise", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("KeyError", LanguageUnit.Variable)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void ReadFile()
        {
            const string input = "with open(\"file.txt\", \"r\") as f: f.read()";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("with", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("open", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("\"file.txt\"", LanguageUnit.Value),
                (",", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("\"r\"", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("as", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("f", LanguageUnit.Variable),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("f", LanguageUnit.Function),
                (".", LanguageUnit.Symbol),
                ("read", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void ImportFromRandom()
        {
            const string input = "from random import randint\r\nrandint(0, 10)";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("from", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("random", LanguageUnit.Variable),
                (" ", LanguageUnit.Whitespace),
                ("import", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("randint", LanguageUnit.Variable),
                ("\r", LanguageUnit.Whitespace),
                ("\n", LanguageUnit.Whitespace),
                ("randint", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("0", LanguageUnit.Value),
                (",", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("10", LanguageUnit.Value),
                (")", LanguageUnit.Symbol)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void While()
        {
            const string input = "while x!=10 or x>5 and x<15: x+=1";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("while", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("!", LanguageUnit.Symbol),
                ("=", LanguageUnit.Symbol),
                ("10", LanguageUnit.Value),
                (" ", LanguageUnit.Whitespace),
                ("or", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                (">", LanguageUnit.Symbol),
                ("5", LanguageUnit.Value),
                (" ", LanguageUnit.Whitespace),
                ("and", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("<", LanguageUnit.Symbol),
                ("15", LanguageUnit.Value),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("x", LanguageUnit.Variable),
                ("+", LanguageUnit.Symbol),
                ("=", LanguageUnit.Symbol),
                ("1", LanguageUnit.Value)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void TryExceptFinally()
        {
            const string input = "try:\n\t1/0\nexcept:\n\tprint(\"ooops!!!\")\nfinally:\n\treturn 5";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("try", LanguageUnit.Operator),
                (":", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("1", LanguageUnit.Value),
                ("/", LanguageUnit.Symbol),
                ("0", LanguageUnit.Value),
                ("\n", LanguageUnit.Whitespace),
                ("except", LanguageUnit.Operator),
                (":", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("print", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("\"ooops!!!\"", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("finally", LanguageUnit.Operator),
                (":", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("return", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("5", LanguageUnit.Value)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void YieldContinueBreak()
        {
            const string input = "for i in range(10):\n\tif i%4==0: continue\n\tif i==7: break\n\tyield i";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("for", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("i", LanguageUnit.Variable),
                (" ", LanguageUnit.Whitespace),
                ("in", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("range", LanguageUnit.Function),
                ("(", LanguageUnit.Symbol),
                ("10", LanguageUnit.Value),
                (")", LanguageUnit.Symbol),
                (":", LanguageUnit.Symbol),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("if", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("i", LanguageUnit.Variable),
                ("%", LanguageUnit.Symbol),
                ("4", LanguageUnit.Value),
                ("=", LanguageUnit.Symbol),
                ("=", LanguageUnit.Symbol),
                ("0", LanguageUnit.Value),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("continue", LanguageUnit.Operator),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("if", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("i", LanguageUnit.Variable),
                ("=", LanguageUnit.Symbol),
                ("=", LanguageUnit.Symbol),
                ("7", LanguageUnit.Value),
                (":", LanguageUnit.Symbol),
                (" ", LanguageUnit.Whitespace),
                ("break", LanguageUnit.Operator),
                ("\n", LanguageUnit.Whitespace),
                ("\t", LanguageUnit.Whitespace),
                ("yield", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("i", LanguageUnit.Variable)
            };
            SameOutput(expected, python.Parse(input));
        }

        [Test]
        [Repeat(5)]
        public void AssertCondition()
        {
            const string input = "assert condition";
            var expected = new List<(string arg, LanguageUnit LanguageUnit)>
            {
                ("assert", LanguageUnit.Operator),
                (" ", LanguageUnit.Whitespace),
                ("condition", LanguageUnit.Variable)
            };
            SameOutput(expected, python.Parse(input));
        }
    }
}