using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    public class listaDePostfijo
    {
        public string lexema;
        public int token;
        public listaDePostfijo siguiente = null;

        public listaDePostfijo(string lexema, int token)
        {
            this.lexema = lexema;
            this.token = token;
        }
    }
}
