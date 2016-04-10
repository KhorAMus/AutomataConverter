using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class NondeterminedFiniteAutomaton
    {
        HashSet<int> startStatesIndices;
        HashSet<int> finalStates;
        /// <summary>
        /// Определяет все переходы в автомате
        /// Первый индекс это состояние из которого переходы исходят
        /// Второй индекс это символ алфавита, по которому происходит переход.
        /// Он равен 0, в том случае, если переход происходит по символу эпсилон.
        /// Значение это множество состояний, в которые можно
        /// попасть по данному символу. 
        /// </summary>
        List<List<HashSet<int>>> transitions;
        List<string> statesNamesMap;
        List<string> symbolsNamesMap;
        Dictionary<string, int> namesStatesMap;
        Dictionary<string, int> namesSymbolsMap;
        public int NumberOfSymbols
        {
            get
            {
                return symbolsNamesMap.Count - 1;
            }
        }
        public int NumberOfStates
        {
            get
            {
                return statesNamesMap.Count;
            }
        }
        /// <summary>
        /// Эта строка будет соответствовать символу Эпсилон
        /// </summary>
        const string emptyTransitionName = "eps";
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
            List<HashSet<int>> transitionsFrom = new List<HashSet<int>>(NumberOfStates);
            for (int i = 0; i < NumberOfSymbols+1; i++)
            {
                transitionsFrom.Add(new HashSet<int>());
            }
            transitions.Add(transitionsFrom);
        }
        public void AddTransition(string existingSource,
            string existingDestination, string existingSymbol)
        {
            AddTransition(namesStatesMap[existingSource],
                namesStatesMap[existingDestination], namesSymbolsMap[existingSymbol]);
        }


        public void AddTransition(int existingSource,
            int existingDestination, int existingSymbol)
        {
            transitions[existingSource][existingSymbol].Add(existingDestination);
        }
        public void AddTransitionBySymbolsSequence(string existingSource,
            string existingDestination, List<string> symbolsSequence)
        {
            var symbolsIndicesSequence = symbolsSequence.Select((name) => (namesSymbolsMap[name])).ToList();
            int sourceIndex = namesStatesMap[existingSource];
            int destinationIndex = namesStatesMap[existingDestination];
            AddTransitionBySymbolsSequence(sourceIndex, destinationIndex, symbolsIndicesSequence);
        }


        public void AddTransitionBySymbolsSequence(int existingSource,
            int existingDestination, List<int> symbolsSequence)
        {
            if (symbolsSequence.Count == 0)
            {
                AddTransition(existingSource, existingDestination, 0);
                return;
            }
            int sourceState = existingSource;
            for (int i = 0; i < symbolsSequence.Count - 1; i++)
            {
                AddState(GetSpecialName());                                
                AddTransition(sourceState, NumberOfStates - 1, symbolsSequence[i]);
                sourceState = NumberOfStates - 1;
            }
            AddTransition(sourceState, existingDestination, symbolsSequence.Last());
        }

        int specialStateNameCounter = 0;
        string GetSpecialName()
        {
            specialStateNameCounter++;
            return "r" + specialStateNameCounter;
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
                transitionsFromOneState.Add(new HashSet<int>());
            }
        }
        public void SetFinalStates(HashSet<string> finalStates)
        {
            SetFinalStates(finalStates.Select((name)
                => (namesSymbolsMap[name])));
        }
        public void SetFinalStates(IEnumerable<int> terminalStates)
        {
            this.finalStates = new HashSet<int>(terminalStates);
        }
        public void SetStartStates(HashSet<string> startStates)
        {
            SetFinalStates(startStates.Select((name)
                => (namesSymbolsMap[name])));
        }
        public void SetStartStates(IEnumerable<int> startStates)
        {
            this.startStatesIndices = new HashSet<int>(startStates);
        }
        public void AddToFinalStates(string existingState)
        {
            AddToFinalStates(namesStatesMap[existingState]);
        }
        public void AddToFinalStates(int existingState)
        {
            finalStates.Add(existingState);
        }
        public void AddToStartStates(string existingState)
        {
            AddToStartStates(namesStatesMap[existingState]);
        }
        public void AddToStartStates(int existingState)
        {
            startStatesIndices.Add(existingState);
        }
        public IEnumerable<string> GetTransitionDestinations(string source, string symbol)
        {
            int sourceIndex = namesStatesMap[source];
            int symbolIndex = namesSymbolsMap[symbol];
            return GetTransitionDestinations(sourceIndex, symbolIndex).Select(
                (index) => (statesNamesMap[index]));

        }
        public HashSet<int> GetTransitionDestinations(int source, int symbol)
        {
            return transitions[source][symbol];
        }
        /*public NondeterminedFiniteAutomata(HashSet<int> startStates, int statesNumber)
        {
            startStatesIndices = startStates;
            transitions = new List<List<HashSet<int>>>();
            for (int i = 0; i < statesNumber; i++)
            {
                transitions.Add(new List<HashSet<int>>() { new HashSet<int>() });
            }
            terminalStates = new HashSet<int>();
        }*/
        public NondeterminedFiniteAutomaton()
        {
            startStatesIndices = new HashSet<int>();
            finalStates = new HashSet<int>();
            namesStatesMap = new Dictionary<string, int>();
            statesNamesMap = new List<string>();
            namesSymbolsMap = new Dictionary<string, int>();
            namesSymbolsMap.Add("eps", 0);
            symbolsNamesMap = new List<string>() { "eps" };
            transitions = new List<List<HashSet<int>>>();

        } 
    }
}
