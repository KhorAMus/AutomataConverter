using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class NondeterminedFiniteAutomata
    {
        HashSet<int> startStatesIndices;
        HashSet<int> terminalStates;
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
                return transitions[0].Count - 1;
            }
        }
        public int NumberOfStates
        {
            get
            {
                return transitions.Count;
            }
        }
        public void AddState()
        {
            List<HashSet<int>> transitionsFrom = new List<HashSet<int>>(NumberOfStates);
            for (int i = 0; i < NumberOfStates; i++)
            {
                transitionsFrom.Add(new HashSet<int>());
            }
            transitions.Add(transitionsFrom);
        }
        public void AddTransition(int existingSource,
            int existingDestination, int existingSymbol)
        {
            transitions[existingSource][existingSymbol].Add(existingDestination);
        }
        public void AddTransitionBySymbolsSequence(int existingSource,
            int existingDestination, List<int> symbolsSequence)
        {
            throw new NotImplementedException();
        }

        public void AddSymbol()
        {
            foreach (var transitionsFromOneState in transitions)
            {
                transitionsFromOneState.Add(new HashSet<int>());
            }
        }
        public void SetTerminalStates(HashSet<int> terminalStates)
        {
            if (terminalStates == null)
            {
                throw new ArgumentNullException("terminalStates");
            }
            this.terminalStates = terminalStates;
        }
        public void SetStartStates(HashSet<int> startStates)
        {
            if (startStates == null)
            {
                throw new ArgumentNullException("terminalStates");
            }
            this.startStatesIndices = startStates;
        }
        public void AddToTerminalStates(int existingState)
        {
            terminalStates.Add(existingState);
        }
        public void AddToStartStates(int existingState)
        {
            startStatesIndices.Add(existingState);
        }
        public HashSet<int> GetTransitionDestinations(int source, int symbol)
        {
            return transitions[source][symbol];
        }
        public NondeterminedFiniteAutomata(HashSet<int> startStates, int statesNumber)
        {
            startStatesIndices = startStates;
            transitions = new List<List<HashSet<int>>>();
            for (int i = 0; i < statesNumber; i++)
            {
                transitions.Add(new List<HashSet<int>>() { new HashSet<int>() });
            }
            terminalStates = new HashSet<int>();
        }
        public NondeterminedFiniteAutomata(): this(new HashSet<int>() { 0 }, 1)
        {

        } 
    }
}
