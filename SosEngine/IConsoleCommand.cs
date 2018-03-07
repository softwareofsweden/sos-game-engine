using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SosEngine
{
    public interface IConsoleCommand
    {
        void ConsoleExecute(string command, params string[] args);
    }
}
