using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomatonException: Exception
    {
        private string messageDetails;
        public AutomatonException(string messageDetails)
        {
            this.messageDetails = messageDetails;
        }
        public override string Message
        {
            get
            {
                return string.Format("Automata exception message: {0}", messageDetails);
            }
        }
    }
}
