using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class NondeterminedFiniteAutomaton: NamedAutomaton
    {
        HashSet<int> startStates;
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
        public override bool IsStateExist(string name)
        {
            return namesStatesMap.Keys.Contains(name);
        }
        public override bool IsSymbolExist(string name)
        {
            return namesSymbolsMap.Keys.Contains(name);
        }
        bool CheckIsStateNameReserved(string stateName)
        {
            return stateName.StartsWith("r");
        }

        public void AddStates(IEnumerable<string> newStates)
        {
            foreach (var state in newStates)
            {
                AddState(state);
            }
        }
        /// <summary>
        /// Добавить новое состояние.
        /// </summary>
        /// <param name="name">Имя состояния</param>
        public override void AddState(string name)
        {
            if (IsStateExist(name))
            {
                throw new AutomatonBuildException($"State {name} already exist.");
            }
            if (CheckIsStateNameReserved(name))
            {
                throw new AutomatonBuildException($"State {name} reserved. Please, choose other name.");
            }
            AddStateNoCheck(name);
        }

        void AddStateNoCheck(string name)
        {
            namesStatesMap.Add(name, statesNamesMap.Count);
            statesNamesMap.Add(name);
            List<HashSet<int>> transitionsFrom = new List<HashSet<int>>(NumberOfStates);
            for (int i = 0; i < NumberOfSymbols + 1; i++)
            {
                transitionsFrom.Add(new HashSet<int>());
            }
            transitions.Add(transitionsFrom);
        }
        public override void AddTransition(string existingSource,
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
                AddStateNoCheck(GetSpecialName());                                
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
        public void AddSymbols(IEnumerable<string> newSymbols)
        {
            foreach (var symbol in newSymbols)
            {
                AddSymbol(symbol);
            }
        }
        public override void AddSymbol(string newSymbol)
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
                => (namesStatesMap[name])));
        }
        public void SetFinalStates(IEnumerable<int> terminalStates)
        {
            this.finalStates = new HashSet<int>(terminalStates);
        }
        public void SetStartStates(HashSet<string> startStates)
        {
            SetStartStates(startStates.Select((name)
                => (namesStatesMap[name])));
        }
        public void SetStartStates(IEnumerable<int> startStates)
        {
            this.startStates = new HashSet<int>(startStates);
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
            startStates.Add(existingState);
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

        public override List<string> GetAlphabet()
        {
            var alphabetCopy = (namesSymbolsMap.Keys.ToList());
            alphabetCopy.Remove(emptyTransitionName);
            return alphabetCopy;
        }

        public override List<string> GetStates()
        {
            return new List<string>(namesStatesMap.Keys);
        }

        public List<string> GetStartStates()
        {
            return startStates.Select((index) => (statesNamesMap[index])).ToList();
        }

        public List<string> GetFinalStates()
        {
            return finalStates.Select((index) => (statesNamesMap[index])).ToList();
        }

        public NondeterminedFiniteAutomaton()
        {
            startStates = new HashSet<int>();
            finalStates = new HashSet<int>();
            namesStatesMap = new Dictionary<string, int>();
            statesNamesMap = new List<string>();
            namesSymbolsMap = new Dictionary<string, int>();
            namesSymbolsMap.Add(emptyTransitionName, 0);
            symbolsNamesMap = new List<string>() { emptyTransitionName };
            transitions = new List<List<HashSet<int>>>();

        } 
    }
}
