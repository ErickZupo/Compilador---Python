using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    class elementoPila
    {
        public string lexema;
        public int token;
        public elementoPila siguiente = null;
    }
}
