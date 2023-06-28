using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    public class listaDeVariables
    {
        public string lexema;
        public int token;
        public listaDeVariables siguiente = null;

        public listaDeVariables(string lexema, int token)
        {
            this.lexema = lexema;
            this.token = token;
        }
    }
}
