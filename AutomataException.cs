using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomataConverter
{
    class AutomataException: Exception
    {
        private string messageDetails;
        public AutomataException(string messageDetails)
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
