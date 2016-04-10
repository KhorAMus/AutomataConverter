using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class DeterminedFiniteAutomaton
    {
        int startStateIndex;
        HashSet<int> finalStates;
        /// <summary>
        /// Matrix specify all transitions in automata.
        /// First index is a state index.
        /// Second one is a alphabet's symbol index.
        /// Value is a state index, too.
        /// int [5,4] == 3 means that 
        /// there is a transition from 5 to 3 by symbol with index 4.
        /// Value is null means that transition by this symbol doesn't exist.
        /// 
        /// Первый индекс это состояние из которого исходит переход
        /// Второй индекс указывает на символ алфавита по которому осуществляется переход
        /// Значение это состояние, в которое входит переход.
        /// int[5,4] == 3 означает, что 
        /// переход из состояния 5 в состояние 3 может осуществляться по символу 4.
        /// Если значение null, то это означает, что такого перехода не существует
        /// </summary>
        List<List<int?>> transitions;
        List<string> statesNamesMap;
        List<string> symbolsNamesMap;
        Dictionary<string, int> namesStatesMap;
        Dictionary<string, int> namesSymbolsMap;
        public int NumberOfSymbols
        {
            get
            {
                return symbolsNamesMap.Count;
            }
        }
        public int NumberOfStates
        {
            get
            {
                return statesNamesMap.Count;
            }
        }
        public bool IsStateExist(string stateName)
        {
            return namesStatesMap.Keys.Contains(stateName);
        }
        public void AddState(string name)
        {
            if (IsStateExist(name))
            {
                throw new AutomatonBuildException("State already exist.");
            }
            namesStatesMap.Add(name, statesNamesMap.Count);
            statesNamesMap.Add(name);
            List<int?> transitionsFrom = new List<int?>(NumberOfStates);
            for (int i = 0; i < NumberOfSymbols; i++)
            {
                transitionsFrom.Add(null);
            }
            transitions.Add(transitionsFrom);
        }
        public void AddTransition(string existingSource, string existingDestination, string existingSymbol)
        {
            AddTransition(namesStatesMap[existingSource],
                namesStatesMap[existingDestination], namesSymbolsMap[existingSymbol]);
        }
        public bool IsTransitionExist(int source, int symbol)
        {
            return transitions[source][symbol] != null;
        }
        public void AddTransition(int existingSource, int existingDestination, int existingSymbol)
        {
            if (IsTransitionExist(existingSource, existingSymbol))
            {
                throw new AutomatonBuildException("Transition already exist.");
            }
            transitions[existingSource][existingSymbol] = existingDestination;
        }
        public bool IsSymbolExist(string symbol)
        {
            return namesSymbolsMap.Keys.Contains(symbol);
        }
        public void AddSymbol(string newSymbol)
        {
            if (IsSymbolExist(newSymbol))
            {
                throw new AutomatonBuildException("Symbol already exist.");
            }
            namesSymbolsMap.Add(newSymbol, symbolsNamesMap.Count);
            symbolsNamesMap.Add(newSymbol);
            foreach (var transitionsFromOneState in transitions)
            {
                transitionsFromOneState.Add(null);
            }
        }
        public void SetFinalStates(HashSet<string> finalStates)
        {
            SetFinalStates(finalStates.Select((name) => (namesSymbolsMap[name])));
        }
        public void SetFinalStates(IEnumerable<int> finalStates)
        {
            this.finalStates =new HashSet<int> (finalStates);
        }
        public void AddToFinalStates(string finalState)
        {
            AddToFinalStates(namesStatesMap[finalState]);
        }
        public void AddToFinalStates(int terminalState)
        {
            finalStates.Add(terminalState);
        }
        
        public DeterminedFiniteAutomaton(string startState)
        {
            startStateIndex = 0;
            transitions = new List<List<int?>>() {new List<int?>() };
            
            finalStates = new HashSet<int> ();
            statesNamesMap = new List<string>() { startState };
            namesStatesMap = new Dictionary<string, int>();
            namesStatesMap.Add(startState, 0);
            symbolsNamesMap = new List<string>();
            namesSymbolsMap = new Dictionary<string, int>();
        }
        public string GetTransitionDestination(string source, string symbol)
        {
            int? destinationIndex = GetTransitionDestination(namesStatesMap[source],
                namesSymbolsMap[symbol]);
            if (destinationIndex == null)
            {
                return null;
            }
            return statesNamesMap[(int)destinationIndex];
        }
        public int? GetTransitionDestination(int source, int symbol)
        {
            return transitions[source][symbol];
        }
    }
}
