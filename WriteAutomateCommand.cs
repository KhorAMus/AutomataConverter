using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
namespace AutomataConverter
{
    class WriteAutomateCommand : ICommand
    {
        private AutomataConverterApplication app;
        public string Help { get { return "Представляет автомат в виде таблицы состояний и переходов"; } }
        public string Name { get { return "write"; } }
        public void Execute(params string[] parameters)
        {
            try
            {

                AutomateToTableConverter f = new AutomateToTableConverter();
                NamedAutomaton na = app.Storage.Table[parameters[0]];

                if (parameters.Length == 1)
                {
                    List<string> table = f.AutomateToTable(app.Storage.Table[parameters[0]]);
                    for (int j = 0; j < table.Count; j++)
                    {
                        Console.WriteLine(table[j]);
                    }
                    Console.WriteLine();
                }
                if (parameters.Length == 2)
                {
                    f.WriteAutomateToFile(na, parameters[0]);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Слишком мало параметров команды");
            }

            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Автомат '{0}' не существует", parameters[0]);
            }
            catch (FileNotFoundException e)
            {

                Console.WriteLine("Файл '{0}' не найден", parameters[1]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public string Description { get { return "write имя [путь]"; } }

        public string[] Synonyms { get { return new string[]{"save"}; } }

        public WriteAutomateCommand(AutomataConverterApplication app)
        {
            this.app = app;
        }
    }
}
