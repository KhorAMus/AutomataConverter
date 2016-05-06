using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class ShowStorageCommand : ICommand
    {
        private AutomataConverterApplication app;
        public string Help { get { return "Выводит список хранимых автоматов"; } }
        public string Name { get { return "show"; } }
        public void Execute(params string[] parameters)
        {
            try
            {

                Console.WriteLine(app.Storage.ToString());

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Слишком мало параметров команды");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
        public string Description { get { return "show"; } }

        public string[] Synonyms { get { return new string[] { }; } }

        public ShowStorageCommand(AutomataConverterApplication app)
        {
            this.app = app;
        }
    }
}
