using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class TableToAutomateConverter
    {
        private List<string> startStates;
        private List<string> finalStates;

        private List<List<string>> symbolsSequence;
        private List<string> states;
        private Type automateType;
        private List<TransitionBySequence> transitionsBySequence;
        private List<Transition> transitions;

        /// <summary>
        /// Считывание автомата из файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public NamedAutomaton ReadAutomate(string path)
        {
            startStates = new List<string>();
            finalStates = new List<string>();
            symbolsSequence = new List<List<string>>();
            states = new List<string>();
            transitionsBySequence = new List<TransitionBySequence>();
            transitions = new List<Transition>();
            List<string> text = File.ReadAllLines(path).ToList();
            List<List<string>> table = new List<List<string>>();
            foreach (var line in text)
            {
                table.Add(new List<string>(line.Split('\t').Where(item => item != "\t" && item != "")));
            }
            ParseTable(table);
            EvalType();
            if (automateType == typeof(NondeterminedFiniteAutomaton))
            {
                return TableToNFA(table);
            }
            else
            {
                return TableToDFA(table);
            }
        }
        /// <summary>
        /// Определение типа автомата, таблица которого считана из файла 
        /// </summary>
        private void EvalType()
        {
            if (startStates.Count > 1 ||
                symbolsSequence.Count(i=>i.Contains("eps"))!=0 ||
                transitions.Count(item => item.Destinations.Count > 1) != 0)
            {
                automateType = typeof(NondeterminedFiniteAutomaton);
            }
            else
            {
                automateType = typeof(DeterminedFiniteAutomaton);
            }
        }
        /// <summary>
        /// Разбор таблицы
        /// </summary>
        /// <param name="table">Таблица</param>
        private void ParseTable(List<List<string>> table)
        {
            string s;
            string[] symbolSequence;
            List<string> splited;

            for (int i = 0; i < table[0].Count; i++)
            {
                symbolSequence = table[0][i].Split(
                    new char[] { ','}, StringSplitOptions.RemoveEmptyEntries);
               symbolsSequence.Add(new List<string>(symbolSequence));         
            }
            for (int i = 1; i < table.Count; i++)
            {
                s = table[i][0];
                splited = s.Split(' ').Where(item => item != " " && item != "").ToList();
                states.Add(splited[0]);
                if (splited.Contains("(f)"))
                {
                    finalStates.Add(splited[0]);
                }
                if (splited.Contains("(s)"))
                {
                    startStates.Add(splited[0]);
                }
            }
            for (int i = 1; i < table.Count; i++)
            {
                for (int j = 1; j < table[i].Count; j++)
                {
                    splited = table[i][j].Split(new char[] { '-', ',' }, StringSplitOptions.RemoveEmptyEntries).
                        Where(item => item != " " && item != ",").ToList();
                    if (splited.Count == 0)
                    {
                        continue;
                    }
                    if (symbolsSequence[j - 1].Count > 1)
                    {
                        transitionsBySequence.Add(
                            new TransitionBySequence(states[i - 1], symbolsSequence[j - 1], splited));
                    }
                    else
                    {
                        transitions.Add(new Transition(states[i - 1], symbolsSequence[j - 1][0], splited));
                    }
                }
            }
        }
        /// <summary>
        /// Создание автомата по таблице
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private NondeterminedFiniteAutomaton TableToNFA(List<List<string>> table)
        {
            NondeterminedFiniteAutomaton nfa = new NondeterminedFiniteAutomaton();
            foreach(var sequence in symbolsSequence)
            { 
                foreach (var symbol in sequence)
                {
                    if (!nfa.IsSymbolExist(symbol))
                    {
                        nfa.AddSymbol(symbol);
                    }
                }
            }

            for (int i = 0; i < states.Count; i++)
            {
                nfa.AddState(states[i]);
            }
            for (int j = 0; j < transitionsBySequence.Count; j++)
            {
                for (int i = 0; i < transitionsBySequence[j].Destinations.Count; i++)
                {
                    nfa.AddTransitionBySymbolsSequence(
                        transitionsBySequence[j].Source, transitionsBySequence[j].Destinations[i],
                        transitionsBySequence[j].Symbols);
                }
            }
            for (int j = 0; j < transitions.Count; j++)
            {
                for (int i = 0; i < transitions[j].Destinations.Count; i++)
                {
                    nfa.AddTransition(transitions[j].Source, transitions[j].Destinations[i],
                        transitions[j].Symbol);
                }
            }
            nfa.SetStartStates(new HashSet<string>(startStates));
            nfa.SetFinalStates(new HashSet<string>(finalStates));
            return nfa;
        }

        /// <summary>
        /// Создание автомата по таблице
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private DeterminedFiniteAutomaton TableToDFA(List<List<string>> table)
        {
            DeterminedFiniteAutomaton dfa = new DeterminedFiniteAutomaton(startStates[0]);
            states.Remove(startStates[0]);
            foreach (var sequence in symbolsSequence)
            {
                foreach (var symbol in sequence)
                {
                    if (!dfa.IsSymbolExist(symbol))
                    {
                        dfa.AddSymbol(symbol);
                    }
                }
            }
            for (int i = 0; i < states.Count; i++)
            {
                dfa.AddState(states[i]);
            }
            dfa.SetFinalStates(new HashSet<string>(finalStates));
            for (int j = 0; j < transitions.Count; j++)
            {
                dfa.AddTransition(transitions[j].Source, transitions[j].Destinations[0], transitions[j].Symbol);
            }
            return dfa;
        }

        private class TransitionBySequence
        {
            public string Source { get; set; }
            public List<string> Symbols { get; set; }
            public List<string> Destinations { get; set; }

            public TransitionBySequence()
            {
                Destinations = new List<string>();
            }
            public TransitionBySequence(string source, List<string> symbols, List<string> destinations)
            {
                Source = source;
                Symbols = symbols;
                Destinations = destinations;
            }
        }
        /// <summary>
        /// Переход
        /// </summary>
        private class Transition
        {
            public string Source { get; set; }
            public string Symbol { get; set; }
            public List<string> Destinations { get; set; }

            public Transition()
            {
                Destinations = new List<string>();
            }
            public Transition(string source, string symbol, List<string> destinations)
            {
                Source = source;
                Symbol = symbol;
                Destinations = destinations;
            }
        }
    }
}
