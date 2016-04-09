using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class DeterminedFiniteAutomata
    {
        int startStateIndex;
        HashSet<int> terminalStates;
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
                return transitions[0].Count;
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
            List<int?> transitionsFrom = new List<int?>(NumberOfStates);
            for (int i = 0; i < NumberOfStates; i++)
            {
                transitionsFrom.Add(null);
            }
            transitions.Add(transitionsFrom);
        }
        public bool IsTransitionExists(int source, int symbol)
        {
            return transitions[source][symbol] != null;
        }
        public void AddTransition(int existingSource, int existingDestination, int existingSymbol)
        {
            if (IsTransitionExists(existingSource, existingSymbol))
            {
                throw new AutomataBuildException("Transition already exist.");
            }
            transitions[existingSource][existingSymbol] = existingDestination;
        }
        public void AddSymbol()
        {
            foreach (var transitionsFromOneState in transitions)
            {
                transitionsFromOneState.Add(null);
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
        public void AddToTerminalStates(int terminalState)
        {
            terminalStates.Add(terminalState);
        }
        public DeterminedFiniteAutomata() : this(0,1)
        {

        }
        public DeterminedFiniteAutomata(int startStateIndex, int statesNumber)
        {
            this.startStateIndex = startStateIndex;
            transitions = new List<List<int?>> (statesNumber);
            for (int i = 0; i < statesNumber; i++)
            {
                transitions.Add(new List<int?>());
            }
            terminalStates = new HashSet<int>();

        }
        public int? GetTransitionDestination(int source, int symbol)
        {
            return transitions[source][symbol];
        }
    }
}
