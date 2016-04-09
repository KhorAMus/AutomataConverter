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
            Application app = new Application();
            app.AddCommand(new HelpCommand(app));
            app.AddCommand(new ExitCommand(app));
            app.Run();
        }
    }
}
