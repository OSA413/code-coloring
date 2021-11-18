using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography;
using NUnit.Framework;

using CodeColoring;
using CodeColoring.ProgrammingLanguage;

namespace CodeColoring_Tests
{
    partial class Python_Tests
    {
        SHA256CryptoServiceProvider sha256 = new();

        [Test]
        [Repeat(5)]
        public void Complex_ProcessKiller()
        {
            var input = CodeSamples.process_killer;
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
    }
}
