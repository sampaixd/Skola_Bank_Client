using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skola_Bank_Client
{
    internal class ReturnToMenu  : Exception    
    {
        public ReturnToMenu()   // used to return to main menu rather than an actual exception, hence missing "Exception" in the name
        { }
    }
}
