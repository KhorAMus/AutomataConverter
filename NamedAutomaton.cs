using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    /// <summary>
    /// Класс представляющий автомат с проименованными состояниями и символами алфавита.
    /// </summary>
    abstract class NamedAutomaton
    {
        protected List<string> statesNamesMap;
        protected List<string> symbolsNamesMap;
        protected Dictionary<string, int> namesStatesMap;
        protected Dictionary<string, int> namesSymbolsMap;
        public abstract List<string> GetAlphabet();
        public abstract List<string> GetStates();

        public abstract List<string> GetFinalStates();
        /// <summary>
        /// Добавить новое состояние.
        /// </summary>
        /// <param name="name">Имя состояния</param>
        public abstract void AddState(string name);
        /// <summary>
        /// Проверить существует ли состояние уже в этом автомате.
        /// </summary>
        /// <param name="name">Имя состояния.</param>
        /// <returns>true, если состояние с таким именем присутствует в автомате, иначе false</returns>
        public abstract bool IsStateExist(string name);
        /// <summary>
        /// Добавляет новый переход в автомат.
        /// </summary>
        /// <param name="existingSource">Уже существующая в автомате вершина, из которой переход исходит.</param>
        /// <param name="existingDestination">Уже существующая в автомате вершина, в которую переход входит.</param>
        /// <param name="existingSymbol">Уже существующий в автомате символ по которому происходит переход.</param>
        public abstract void AddTransition(string existingSource, string existingDestination, string existingSymbol);
        /// <summary>
        /// Добавляет новый символ в алфавит автомата.
        /// </summary>
        /// <param name="newSymbol">Имя нового символа.</param>
        public abstract void AddSymbol(string newSymbol);
        /// <summary>
        /// Проверяет существует ли уже данный символ в алфавите этого автомата.
        /// </summary>
        /// <param name="name">Наименование символа</param>
        public abstract bool IsSymbolExist(string name);

        public int GetSymbolIndex(string name)
        {
            return namesSymbolsMap[name];
        }
        public string GetSymbolName(int index)
        {
            return symbolsNamesMap[index];
        }
        public int GetStateIndex(string name)
        {
            return namesStatesMap[name];
        }
        public string GetStateName(int index)
        {
            return statesNamesMap[index];
        }

    }
}
