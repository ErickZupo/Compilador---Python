using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    class pila
    {
        public elementoPila tope;

        //Apilar
        public void Push(elementoPila elemento)
        {
            if (tope == null)
            {
                tope = elemento;
            }
            else
            {
                elementoPila nuevoElemento = tope;
                tope = elemento;
                tope.siguiente = nuevoElemento;
            }
        
        }

        //Desapilar
        public void Pop()
        {
            if (tope != null)
            {
                tope = tope.siguiente;
            }
        }
    }
}
