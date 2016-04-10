using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class NamedDeterminedFiniteAutomaton
    {
        
        int startStateIndex;
        HashSet<int> terminalStatesIndexes;
        /// <summary>
        /// Matrix specify all transitions in automata.
        /// First index is a state index.
        /// Second one is a alphabet's symbol index.
        /// Value is a state index, too.
        /// int [5,4] == 3 means that 
        /// there is a transition from 5 to 3 by symbol with index 4.
        /// Value is null means that transition by this symbol doesn't exist.
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
        public bool IsStateExists(string stateName)
        {
            return namesStatesMap.Keys.Contains(stateName);
        }
        public void AddState(string name)
        {
            if (IsStateExists(name))
            {
                throw new AutomatonBuildException("State already exist.");
            }
            namesStatesMap.Add(name, statesNamesMap.Count);
            statesNamesMap.Add(name);
            List<int?> transitionsFrom = new List<int?>(NumberOfStates);
            for (int i = 0; i < NumberOfStates; i++)
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

        public bool IsTransitionExist(int sourceIndex, int destinationIndex)
        {
            return transitions[sourceIndex][destinationIndex] != null;
        }
        public void AddTransition(int existingSourceIndex, int existingDestinationIndex, int existingSymbolIndex)
        {
            if (IsTransitionExist(existingSourceIndex, existingDestinationIndex))
            {
                throw new AutomatonBuildException("Transition already exist.");
            }
            transitions[existingSourceIndex][existingDestinationIndex] = existingSymbolIndex;
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
            foreach (var transition in transitions)
            {
                transition.Add(null);
            }
        }
        public NamedDeterminedFiniteAutomaton(string startState)
        {
            startStateIndex = 0;
            transitions = new List<List<int?>> { new List<int?>() };
            terminalStatesIndexes = new HashSet<int>();
            statesNamesMap = new List<string>() {startState };
            namesStatesMap = new Dictionary<string, int>();
            namesStatesMap.Add(startState, 0);
            symbolsNamesMap = new List<string>();
            namesSymbolsMap = new Dictionary<string, int>();


        }
    }
}
