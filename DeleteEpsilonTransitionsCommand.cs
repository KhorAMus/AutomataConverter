using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class DeleteEpsilonTransitionsCommand : ICommand
    {
        private AutomataConverterApplication app;
        public string Help { get { return "Удаляет эпсилон-переходы из НКА";  } }
        public string Name { get { return "deps"; } }
        public void Execute(params string[] parameters)
        {
            try
            {
                NamedAutomaton na = app.Storage.Table[parameters[0]];
                if (na is NondeterminedFiniteAutomaton)
                {
                    string tmp = "_epsno";
                    if (parameters.Length > 2)
                    {
                        tmp = "";
                    }
                    app.Storage.AddAutomate(parameters[0] + tmp,
                      ((NondeterminedFiniteAutomaton)na).GetEquivalentDeletedEpsilons());
                }
                else
                {
                    Console.WriteLine("Автомат {0} является ДКА", parameters[0]);
                }

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine("Слишком мало параметров команды");
            }
            catch (KeyNotFoundException e)
            {
                Console.WriteLine("Автомат {0} не существует", parameters[0]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public string Description { get { return "deps имя [новое_имя]"; } }

        public string[] Synonyms { get { return new string[]{}; } }

        public DeleteEpsilonTransitionsCommand(AutomataConverterApplication app)
        {
            this.app = app;
        }
    }
}
