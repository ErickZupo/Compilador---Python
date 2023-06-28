using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    class Program
    {
        static void Main(string[] args)
        {
            Lexico lexico = new Lexico();

            if ( !lexico.errorEncontradoLexico )
            {
                Console.WriteLine("\n\tAnálisis Léxico Terminado Correctamente.");        
       
                Sintactico sintactico = new Sintactico(lexico.cabeza, lexico.p);

                if (!sintactico.errorEncontradoSintactico)
                {
                    Console.WriteLine("\n\tAnálisis Sintáctico Terminado Correctamente.");
                }

                if (!sintactico.errorEncontradoSemantico)
                {
                    Console.WriteLine("\n\tAnálisis Semántico Terminado Correctamente.");
                }
            }

            Console.ReadKey();
            
        }
    }
}
