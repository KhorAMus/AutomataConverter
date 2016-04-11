using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    static class AutomatonProcedures
    {
        public static IEnumerable<string> GetAllDestinationsByEpsilons(NondeterminedFiniteAutomaton epsilonNfa, string startState)
        {
            Queue<int> front = new Queue<int>();
            int[] number = new int[epsilonNfa.NumberOfStates];
            int[] father = new int[epsilonNfa.NumberOfStates];
            for (int i = 0; i < number.Length;i++)
            {
                number[i] = 0;
                father[i] = 0;
            }
            int startStateIndex = epsilonNfa.GetStateIndex(startState);
            number[startStateIndex] = 1;
            front.Enqueue(startStateIndex);
            int counterNum = 1;
            while (front.Count != 0)
            {
                int frontHead = front.Dequeue();
                var destinationsFrom = epsilonNfa.GetTransitionDestinations(frontHead, 0);
                foreach (var state in destinationsFrom)
                {
                    if (number[state] == 0)
                    {
                        front.Enqueue(state);
                        number[state] = ++counterNum;
                        father[state] = frontHead;
                    }                    
                }
            }
            return epsilonNfa.GetStates().Where((name) => (number[epsilonNfa.GetStateIndex(name)] > 0));

        }
        public static NondeterminedFiniteAutomaton GetEquivalentDeletedEpsilons(this
            NondeterminedFiniteAutomaton epsilonNfa)
        {
            NondeterminedFiniteAutomaton noEpsilonsNfa = new NondeterminedFiniteAutomaton();
            // копируем состояния и алфавит
            var alphabet = epsilonNfa.GetAlphabet();
            var states = epsilonNfa.GetStates();
            var finalStates = new HashSet<string>(epsilonNfa.GetFinalStates());
            var startStates = new HashSet<string>( epsilonNfa.GetStartStates());
            noEpsilonsNfa.AddSymbols(alphabet);
            noEpsilonsNfa.AddStates(states);
            noEpsilonsNfa.SetFinalStates(finalStates);
            noEpsilonsNfa.SetStartStates(startStates);
            // добавляем необходимые переходы
            foreach (var state in states)
            {
                // состояния в которые мы можем перейти из состояния state, используя эпсилон переходы.
                var statesReachByEpsilons = GetAllDestinationsByEpsilons(epsilonNfa, state);
                foreach (var symbol in alphabet)
                {
                    // состояния в которые мы можем попасть используя сперва эпсилон переходы, 
                    // а затем символ symbol единажды
                    var statesReachByEpsilonsAndSymbol = statesReachByEpsilons.SelectMany((reachedByEps)
                        => (epsilonNfa.GetTransitionDestinations(reachedByEps, symbol))).Distinct();
                    // состояния в которые мы можем перейти из состояния state, используя символ symbol единажды
                    // и эпсилон переходы в любом порядке
                    var statesOneSymbolAndEpsilons = statesReachByEpsilonsAndSymbol.SelectMany((stateFrom
                        => GetAllDestinationsByEpsilons(epsilonNfa, stateFrom))).Distinct();
                    foreach (var reachedState in statesOneSymbolAndEpsilons)
                    {
                        noEpsilonsNfa.AddTransition(state, reachedState, symbol);
                    }
                }
                // Если это начальное состояние и из него достижимо заключительное по эпсилон
                // то сделаем его заключительным
                if (startStates.Contains(state) && statesReachByEpsilons.Any(finalStates.Contains))
                {
                    noEpsilonsNfa.AddToFinalStates(state);
                }
            }
            return noEpsilonsNfa;
        }
    }
}
