using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomataConverterApplication: Application
    {
        public AutomateStorage Storage { get { return storage; } }
        AutomateStorage storage = new AutomateStorage();
        
    }
}
