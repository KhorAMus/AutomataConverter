using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomatonBuildException: AutomatonException
    {
        
        public AutomatonBuildException(string messageDetails): base(messageDetails)
        {

        }
        
    }
}
