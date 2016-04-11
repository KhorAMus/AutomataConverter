using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            AutomataSample();
            AutomataSample2();
            AutomataSample3();
            AutomataSample4();
            Application app = new Application();
            app.AddCommand(new HelpCommand(app));
            app.AddCommand(new ExitCommand(app));
            app.Run();
        }
        /// <summary>
        /// Пример работы с автоматами
        /// </summary>
        static void AutomataSample()
        {
            DeterminedFiniteAutomaton dfa = new DeterminedFiniteAutomaton("q0");
            dfa.AddSymbol("a");
            dfa.AddToFinalStates("q0");
            dfa.AddTransition("q0", "q0", "a");
            dfa.AddSymbol("b");
            dfa.AddState("q1");
            dfa.AddState("q2");
            dfa.AddTransition("q0", "q2", "b");
            dfa.AddTransition("q2", "q1", "b");
            dfa.AddToFinalStates("q1");
            string fromQ0ByB = dfa.GetTransitionDestination("q0", "b");
            Console.WriteLine($"Переход по символу b из состояния q0 приведёт в состояние: {fromQ0ByB}");
        }
        static void AutomataSample2()
        {
            NondeterminedFiniteAutomaton nfa = new NondeterminedFiniteAutomaton();
            nfa.AddState("q0");
            nfa.AddToStartStates("q0");
            nfa.AddSymbol("a");
            nfa.AddToFinalStates("q0");
            nfa.AddTransition("q0", "q0", "a");
            nfa.AddSymbol("b");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddTransition("q0", "q2", "b");
            nfa.AddTransition("q2", "q1", "b");
            nfa.AddToFinalStates("q1");
            var fromQ0ByB = nfa.GetTransitionDestinations("q0", "b");
            if (fromQ0ByB.Count() != 0)
            {
                Console.WriteLine("Переход по символу b из состояния q0 приведёт в состояния: ");
                foreach (var state in fromQ0ByB)
                {
                    Console.Write(state + " ");
                }
            }
            else
            {
                Console.WriteLine("По символу b из q0 нет переходов.");
            }

        }
        static void AutomataSample3()
        {
            NondeterminedFiniteAutomaton nfa = new NondeterminedFiniteAutomaton();
            nfa.AddState("q0");
            nfa.AddState("q1");
            nfa.AddState("q2");
            nfa.AddToFinalStates("q2");
            nfa.AddToStartStates("q0");
            nfa.AddSymbol("А");
            nfa.AddSymbol("Б");
            nfa.AddTransition("q0", "q2", "А");
            nfa.AddTransitionBySymbolsSequence("q0", "q1", new List<string>() { "А", "Б", "А" });
            nfa.AddTransition("q1", "q2", "Б");
            var fromQ0ByA = nfa.GetTransitionDestinations("q0", "А");
            if (fromQ0ByA.Count() != 0)
            {
                Console.WriteLine("Переход по символу А из состояния q0 приведёт в состояния: ");
                foreach (var state in fromQ0ByA)
                {
                    Console.Write(state + " ");
                }
            }
            else
            {
                Console.WriteLine("По символу А из q0 нет переходов.");
            }
        }
        static void AutomataSample4()
        {
            NondeterminedFiniteAutomaton nfa = new NondeterminedFiniteAutomaton();
            nfa.AddStates(new string[] { "q0", "q1", "q2", "q3", "q4" });
            nfa.AddSymbols(new string[] { "a", "b" });
            nfa.SetFinalStates(new HashSet<string>() { "q3", "q2" });
            nfa.SetStartStates(new HashSet<string>() { "q0" });
            nfa.AddTransition("q0", "q3", "b");
            nfa.AddTransition("q0", "q2", "eps");
            nfa.AddTransition("q0", "q1", "a");
            nfa.AddTransition("q1", "q4", "b");
            nfa.AddTransition("q2", "q1", "eps");
            nfa.AddTransition("q3", "q2", "b");
            nfa.AddTransition("q4", "q1", "a");
            nfa.AddTransition("q4", "q2", "b");
            nfa.AddTransition("q4", "q0", "b");
            NondeterminedFiniteAutomaton NoEpsNfa = nfa.GetEquivalentDeletedEpsilons();
        }
    }
}
