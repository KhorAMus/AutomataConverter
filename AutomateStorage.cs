using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomateStorage
    {
        public Dictionary<string, NamedAutomaton> Table { get; private set; }
        public AutomateStorage()
        {
            Table = new Dictionary<string, NamedAutomaton>();
        }

        public override string ToString()
        {
            string type, table = "name\ttype\n";
            foreach (string key in Table.Keys)
            {
                if (Table[key] is NondeterminedFiniteAutomaton)
                { 
                    type = "nfa";
                }
                else 
                {
                    type = "dfa";
                }
                table += string.Format("{0}\t{1}\n", key, type);
               
            }
            return table;
        }
        public void AddAutomate(string name, NamedAutomaton na)
        {
            if (Table.ContainsKey(name))
            {
                Table[name] = na;
            }
            else {
                Table.Add(name, na);
            }
        }
    }
}
