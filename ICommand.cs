using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomataConverter
{
    public interface ICommand
    {
        string Name{ get; }
        string Help{ get; }
        string Description{ get; }
        void Execute(params string [] parameters);
        string[] Synonyms { get; }
    }
}
