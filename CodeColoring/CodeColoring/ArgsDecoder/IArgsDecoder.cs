﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    public interface IArgsDecoder
    {
        string Help { get; }
        bool ErrorOccured { get; }
        string ErrorMessage { get; }
        DecodedArguments Decode(string[] args);
    }

    public class DecodedArguments
    {
        public string InputFilePath;
        public ColorPalette ColorPalette;
        public ProgrammingLanguage ProgrammingLanguage;
        public string OutputFilePath;
        public OutputFormat OutputFormat;
    }
}