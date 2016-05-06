using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace AutomataConverter
{
    class ReadAvtomateCommand : ICommand
    {
        private AutomataConverterApplication app;
        public string Help { get { return "Считывает автомат из файла"; } }
        public string Name { get { return "read"; } }
        public void Execute(params string[] parameters)
        {
            try
            {
                var name = parameters[0];
                var path = parameters[1];
                TableToAutomateConverter ttac = new TableToAutomateConverter();
                NamedAutomaton na = ttac.ReadAutomate(path);
                app.Storage.AddAutomate(name, na);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Слишком мало параметров команды");
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
        public string Description { get { return "read имя путь"; } }

        public string[] Synonyms { get { return new string[]{"get"}; } }

        public ReadAvtomateCommand(AutomataConverterApplication app)
        {
            this.app = app;
        }
    }
}
