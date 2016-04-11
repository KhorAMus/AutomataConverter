using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomatonGetInformationException: AutomatonException
    {
        public AutomatonGetInformationException(string messageDetails): base(messageDetails)
        {

        }
    }
}
