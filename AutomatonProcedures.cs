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
                if (startStates.Contains(state))
                {
                    foreach (var s in statesReachByEpsilons)
                    {
                        startStates.Add(s);
                        noEpsilonsNfa.AddToStartStates(s);
                    }
                    
                }
                // Если это начальное состояние и из него достижимо заключительное по эпсилон
                // то сделаем его заключительным
                if (startStates.Contains(state) && statesReachByEpsilons.Any(finalStates.Contains))
                {
                    noEpsilonsNfa.AddToFinalStates(state);
                }

            }
            foreach (var state in finalStates)
            {
                noEpsilonsNfa.AddToFinalStates(state);
            }
            return noEpsilonsNfa;
        }
        private static int CheckIsListContains(List<HashSet<string>> list, HashSet<string> hashSet)
        {
            int i = 0;
            foreach (var set in list)
            {
                if (set.IsSupersetOf(hashSet) && hashSet.IsSupersetOf(set))
                {
                    return i;
                }
                i++;
            }
            return -1;
        }
        private static HashSet<string> GetAccessibleStates(NondeterminedFiniteAutomaton nfa, 
            IEnumerable<string> states, string symbol)
        {
            return new HashSet<string>(states.SelectMany(
                state => nfa.GetTransitionDestinations(state, symbol)).ToList());
        }
        private static List<string> SetNewNames(List<HashSet<string>> allSetsOfStates)
        {
            List<string> states = new List<string>();
            int i = 0;
            foreach (var set in allSetsOfStates)
            {
                states.Add(string.Format("q{0}", i));
                i++;
            }
            return states;
        }
        public static DeterminedFiniteAutomaton ConvertNFAToDFA(NondeterminedFiniteAutomaton nfa)
        {
            //DeterminedFiniteAutomaton dfa
            nfa = nfa.GetEquivalentDeletedEpsilons();
            HashSet<string> currentStates, accessibleStates;
            List<HashSet<string>> allSetsOfStates = new List<HashSet<string>>();
            allSetsOfStates.Add(new HashSet<string>(nfa.GetStartStates()));
            DeterminedFiniteAutomaton dfa = new DeterminedFiniteAutomaton("q0");

            List<string> names = new List<string>();
            dfa.AddSymbols(nfa.GetAlphabet());
            var finalStates = nfa.GetFinalStates();
            string from, to;
            bool f = true;
            for (int allSetsCounter = 0, currentSetNumber = 0, pos; currentSetNumber<=allSetsCounter; currentSetNumber++)
            {
                currentStates = allSetsOfStates[currentSetNumber];
                if (currentStates.Any(state => nfa.GetFinalStates().Contains(state)))
                {
                    dfa.AddToFinalStates(string.Format("q{0}", currentSetNumber));
                }
                foreach (var symbol in nfa.GetAlphabet())
                {                    
                    accessibleStates = GetAccessibleStates(nfa, currentStates, symbol);
                    if (accessibleStates.Count != 0)
                    {
                        pos = CheckIsListContains(allSetsOfStates, accessibleStates);
                        from = string.Format("q{0}", currentSetNumber);
                        to = string.Format("q{0}", pos);
                        if (pos == -1)
                        {
                            allSetsCounter++;
                            to = string.Format("q{0}", allSetsCounter);
                            dfa.AddState(to);
                            allSetsOfStates.Add(new HashSet<string>(accessibleStates));
                        }
                        dfa.AddTransition(from, to, symbol);
                    }
                }
            }
            return dfa;
        }
    }
}
