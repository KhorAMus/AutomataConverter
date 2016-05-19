using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class DeterminedFiniteAutomaton: NamedAutomaton
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
        public override bool IsStateExist(string stateName)
        {
            return namesStatesMap.Keys.Contains(stateName);
        }
        public override bool IsSymbolExist(string stateName)
        {
            return namesSymbolsMap.Keys.Contains(stateName);
        }
        public void AddStates(IEnumerable<string> newStates)
        {
            foreach (var state in newStates)
            {
                AddState(state);
            }
        }
        public void AddSymbols(IEnumerable<string> newSymbols)
        {
            foreach (var symbol in newSymbols)
            {
                AddSymbol(symbol);
            }
        }
        /// <summary>
        /// Добавить новое состояние.
        /// </summary>
        /// <param name="name">Имя состояния</param>
        /// <exception cref="AutomatonBuildException"></exception>
        public override void AddState(string name)
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
        /// <summary>
        /// Добавляет новый переход в автомат
        /// </summary>
        /// <param name="existingSource">Уже существующая в автомате вершина, из которой переход исходит.</param>
        /// <param name="existingDestination">Уже существующая в автомате вершина, в которую переход входит.</param>
        /// <param name="existingSymbol">Уже существующий в автомате символ по которому происходит переход.</param>
        /// <exception cref="AutomatonBuildException"></exception>
        public override void AddTransition(string existingSource, string existingDestination, string existingSymbol)
        {
            if (!IsStateExist(existingSource))
            {
                throw new AutomatonBuildException(string.Format("State {0} not exist.", existingSource));
            }
            if (!IsStateExist(existingDestination))
            {
                throw new AutomatonBuildException(string.Format("State {0} not exist.",existingDestination));
            }
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
        /// <summary>
        /// Добавляет новый символ в алфавит автомата.
        /// </summary>
        /// <param name="newSymbol">Имя нового  символа.</param>
        /// <exception cref="AutomatonBuildException"></exception>
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
                transitionsFromOneState.Add(null);
            }
        }
        public void SetFinalStates(HashSet<string> finalStates)
        {
            SetFinalStates(finalStates.Select((name) => (namesStatesMap[name])));
        }
        public void SetFinalStates(IEnumerable<int> finalStates)
        {
            this.finalStates =new HashSet<int> (finalStates);
        }
        /// <summary>
        /// Добавляет уже существующее состояние во мнеожество заключительных 
        /// </summary>
        /// <param name="finalState">Имя, уже существующего, состояния</param>
        public void AddToFinalStates(string finalState)
        {
            AddToFinalStates(namesStatesMap[finalState]);
        }
        public void AddToFinalStates(int terminalState)
        {
            finalStates.Add(terminalState);
        }
        /// <summary>
        /// Создаёт ДКА с одним начальным состоянием.
        /// </summary>
        /// <param name="startState">Имя начального состояния.</param>
        public DeterminedFiniteAutomaton(string startState)
        {
            startStateIndex = 0;
            transitions = new List<List<int?>>() { new List<int?>() };

            finalStates = new HashSet<int>();
            statesNamesMap = new List<string>() { startState };
            namesStatesMap = new Dictionary<string, int>();
            namesStatesMap.Add(startState, 0);
            symbolsNamesMap = new List<string>();
            namesSymbolsMap = new Dictionary<string, int>();
        } 

        public string GetTransitionDestination(string source, string symbol)
        {
            if (!IsSymbolExist(symbol))
            {
                throw new AutomatonBuildException(string.Format("Automaton's alphabet doesn't contain {0}", symbol));
            }
            if (!IsStateExist(source))
            {
                throw new AutomatonBuildException(string.Format("Symbol {0} already exist.", symbol));
            }
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

        public override List<string> GetAlphabet()
        {
            return new List<string>(namesSymbolsMap.Keys.ToList());
        }

        public override List<string> GetStates()
        {
            return new List<string>(namesStatesMap.Keys);
        }

        public string GetStartState()
        {
            return statesNamesMap[startStateIndex];
        }
        public override List<string> GetFinalStates()
        {
            return finalStates.Select((index) => (statesNamesMap[index])).ToList();
        }
    }
}
