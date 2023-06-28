using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    public class nodo
    {
        public string lexema;
        public int token;
        public int renglon;
        public nodo siguiente = null;

        public nodo(string lexema, int token, int renglon)
        {
            this.lexema = lexema;
            this.token = token;
            this.renglon = renglon;
        }
    }
}
