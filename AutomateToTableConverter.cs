using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomateToTableConverter
    {
        
        /// <summary>
        /// Запись автомата в файл
        /// </summary>
        /// <param name="na"></param>
        /// <param name="path"></param>
        public void WriteAutomateToFile(NamedAutomaton na, string path)
        {
            File.WriteAllLines(path, AutomateToTable(na));
        }
        /// <summary>
        /// Считывание автомата из файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        
        /// <summary>
        /// Получение таблицы по автомату
        /// </summary>
        /// <param name="na"></param>
        /// <returns></returns>
        public List<string> AutomateToTable(NamedAutomaton na)
        {
            if (na is NondeterminedFiniteAutomaton)
            {
                return Formating(NFAToTable((NondeterminedFiniteAutomaton)na));
            }
            if (na is DeterminedFiniteAutomaton)
            {
                return Formating(DFAToTable((DeterminedFiniteAutomaton)na));
            }
            return null;
        }

        /// <summary>
        /// Создает таблицу переходов автомата
        /// </summary>
        /// <param name="dfa"></param>
        /// <returns></returns>
        private string[,] DFAToTable(DeterminedFiniteAutomaton dfa)
        {
            List<string> startStates = new List<string>();
            startStates.Add(dfa.GetStartState());
            return StrangeFunc((NamedAutomaton)dfa, new Func<string, string, string>(
                (source, symbol) =>
                {
                    string tmp = dfa.GetTransitionDestination(source, symbol);
                    if (tmp == null)
                    {
                        return "-";
                    }
                    else
                    {
                        return tmp;
                    }
                }), dfa.GetAlphabet(), startStates);
        }

        private string[,] NFAToTable(NondeterminedFiniteAutomaton nfa)
        {
            List<string> alphabet = new List<string>(nfa.GetAlphabet());
            alphabet.Add(nfa.GetEpsilonName());
            return StrangeFunc((NamedAutomaton)nfa, new Func<string, string, string>(
                (source, symbol) =>
                {
                    List<string> tmp = nfa.GetTransitionDestinations(source, symbol).ToList();
                    if (tmp == null)
                    {
                        return "-";
                    }
                    else
                    {
                        return string.Join(", ", tmp);
                    }
                }), alphabet, nfa.GetStartStates());
        }
        /// <summary>
        /// Создает таблицу переходов автомата
        /// </summary>
        /// <param name="automate">Автомат</param>
        /// <param name="funcForNewStates">Функция, определяющая вид записи результата перехода</param>
        /// <param name="alphabet">Алфавит</param>
        /// <param name="startStates">Начальные состояния</param>
        /// <returns></returns>
        string[,] StrangeFunc(NamedAutomaton automate, Func<string, string, string> funcForNewStates, 
            List<string> alphabet, List<string> startStates)
        {            
            List<string> finalStates = new List<string>(automate.GetFinalStates()),
                    states = new List<string>(automate.GetStates());
            string[,] table = new string[states.Count + 1, alphabet.Count + 1];
            string tmp;
            table[0, 0] = "";
            for (int i = 0; i < alphabet.Count; i++)
            {
                table[0, i + 1] = alphabet[i];
            }
            for (int i = 0; i < states.Count; i++)
            {
                table[i + 1, 0] = states[i];
            }
            for (int i = 1; i < table.GetLength(0); i++)
            {
                for (int j = 1; j < table.GetLength(1); j++)
                {
                    table[i, j] = funcForNewStates(table[i, 0], table[0, j]);
                }
            }
            for (int j = 1; j < table.GetLength(0); j++)
            {
                tmp = "";
                if (finalStates.Contains(table[j, 0]))
                {
                    tmp += " (f)";
                }
                if (startStates.Contains(table[j, 0]))
                {
                    tmp += " (s)";
                }
                table[j, 0] += tmp;
            }
            return table;
        }
        /// <summary>
        /// Форматирование таблицы для вывода на экран или файл 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<string> Formating(string[,] table)
        {
            List<string> result = new List<string>(table.GetLength(0));
            int maxLength = 0;
            for (int i = 0; i < table.GetLength(0); i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (table[i, j].Length > maxLength)
                    {
                        maxLength = table[i, j].Length;
                    }
                }
            }
            for (int i = 0; i < table.GetLength(0); i++)
            {
                result.Add("");
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    result[i]+=table[i, j];
                    for (int d = maxLength/8, k = 0; k <= d - table[i,j].Length/8; k++)
                    {
                        result[i] += "\t ";
                    }
                }
            }
            return result;
        }

        
    }
}
