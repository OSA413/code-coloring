﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeColoring
{
    class ConsoleArgsDecoder : IArgsDecoder
    {
        public bool ErrorOccured { get; private set; }
        public string ErrorMessage { get; private set; }
        public string Help => "Usage example goes here";

        public DecodedArguments Decode(string[] args)
        {
            throw new NotImplementedException();
            //массив ILanguage и смотрим по всем им расширения для поиска нужного по файлу. передаем же файл?
        }
    }
}
