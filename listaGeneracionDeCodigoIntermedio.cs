using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    public class listaGeneracionDeCodigoIntermedio
    {
        public string apuntador;
        public string lexema;
        public listaGeneracionDeCodigoIntermedio siguiente = null;

        public listaGeneracionDeCodigoIntermedio(string lexema)
        {
            this.lexema = lexema;
        }
    }
}
