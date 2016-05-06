using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class ConvertAutomateCommand : ICommand
    {
        private AutomataConverterApplication app;
        public string Help { get { return "Преобразует НКА в ДКА";  } }
        public string Name { get { return "conv"; } }
        public void Execute(params string[] parameters)
        {
            try
            {
                NamedAutomaton na = app.Storage.Table[parameters[0]];
                if (na is NondeterminedFiniteAutomaton)
                {
                    string tmp = "_dfa";
                    if (parameters.Length > 2)
                    {
                        tmp = "";
                    }
                    app.Storage.AddAutomate(parameters[0] + tmp,
                        AutomatonProcedures.ConvertNFAToDFA((NondeterminedFiniteAutomaton)na));
                }
                else
                {
                    Console.WriteLine("Автомат {0} уже является ДКА", parameters[0]);
                }
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Слишком мало параметров команды");
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Автомата с таким именем не существует");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public string Description { get { return "conv имя [новое_имя]"; } }

        public string[] Synonyms { get { return new string[]{}; } }

        public ConvertAutomateCommand(AutomataConverterApplication app)
        {
            this.app = app;
        }
    }
}
