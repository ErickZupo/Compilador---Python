using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilador_Python
{
    class Sintactico
    {
        //Variables del analizador sintáctico
        nodo cabeza = null, p;
        public bool errorEncontradoSintactico = false;

        //Variables del analizador semántico
        listaDeVariables cabezaListaVariables = null, pListaVariables, tmpVariable;
        listaDePostfijo cabezaListaPostfijo = null, pListaPostfijo;
        listaGeneracionDeCodigoIntermedio cabezaCodigoIntermedio = null, pCodigoIntermedio;
        pila topeEstructuraPila = null, pEstructuraPila;
        pila TopePilaApuntadorB = null, tmpPilaApuntadorB;
        public bool errorEncontradoSemantico = false;
        string tmpNombreFuncion, tmpNombreVariable, tmpLexema, tmpApuntadorA, tmpApuntadorB, tmpApuntadorC, tmpApuntadorD;
        int tmpNivelJerarquicoOperadorUno, tmpNivelJerarquicoOperadorDos, tmpToken, tmpTokenTipoAlmacenado, columna, fila, 
            instanciaDeA = 1, instanciaDeB = 1, instanciaDeC = 1, instanciaDeD = 1;
        bool AgregarApuntadoresB = false;
        bool FinDeArchivo = false;
        bool AgregarApuntadores = true;

        string[,] erroresSemanticos = {
            /*                       0                                                                  1
            /*0*/{ "Error, el identificador ya se encuentra establecido como nombre de la función." , "504" },
            /*1*/{ "Error, variable ya declarada."                                                  , "505" },
            /*2*/{ "Error, variable sin declarar."                                                  , "506" },
            /*3*/{ "Error, incompatibilidad de tipos."                                              , "507" }
       };

        // Sistema de tipos - Aritméticos

        int[,] sistemaDeTiposSuma =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  101  ,  102  ,   0   ,    0    },
            /* Decimal */{  102  ,  102  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,  122  ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTiposResta =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  101  ,  102  ,   0   ,    0    },
            /* Decimal */{  102  ,  102  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTiposMultiplicacion =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  101  ,  102  ,   0   ,    0    },
            /* Decimal */{  102  ,  102  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTiposDivision =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  102  ,  102  ,   0   ,    0    },
            /* Decimal */{  102  ,  102  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTiposModulo =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  101  ,  102  ,   0   ,    0    },
            /* Decimal */{  102  ,  102  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        // Sistema de tipos - Relacionales

        int[,] sistemaDeTipos_MayorQueTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  213  ,  213  ,   0   ,    0    },
            /* Decimal */{  213  ,  213  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MayorQueFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  214  ,  214  ,   0   ,    0    },
            /* Decimal */{  214  ,  214  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MenorQueTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  213  ,  213  ,   0   ,    0    },
            /* Decimal */{  213  ,  213  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MenorQueFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  214  ,  214  ,   0   ,    0    },
            /* Decimal */{  214  ,  214  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MayorOIgualQueTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  213  ,  213  ,   0   ,    0    },
            /* Decimal */{  213  ,  213  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MayorOIgualQueFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  214  ,  214  ,   0   ,    0    },
            /* Decimal */{  214  ,  214  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MenorOIgualQueTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  213  ,  213  ,   0   ,    0    },
            /* Decimal */{  213  ,  213  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_MenorOIgualQueFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  214  ,  214  ,   0   ,    0    },
            /* Decimal */{  214  ,  214  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_IgualdadTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  213  ,  213  ,   0   ,    0    },
            /* Decimal */{  213  ,  213  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,  213  ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };
        
        int[,] sistemaDeTipos_IgualdadFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  214  ,  214  ,   0   ,    0    },
            /* Decimal */{  214  ,  214  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,  214  ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_DesigualTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  213  ,  213  ,   0   ,    0    },
            /* Decimal */{  213  ,  213  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,  213  ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        int[,] sistemaDeTipos_DesigualFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{  214  ,  214  ,   0   ,    0    },
            /* Decimal */{  214  ,  214  ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,  214  ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,    0    }
        };

        // Sistema de tipos - Lógicos

        int[,] sistemaDeTipos_AndTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{   0   ,   0   ,   0   ,    0    },
            /* Decimal */{   0   ,   0   ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,   213   }
        };

        int[,] sistemaDeTipos_AndFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{   0   ,   0   ,   0   ,    0    },
            /* Decimal */{   0   ,   0   ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,   214   }
        };

        int[,] sistemaDeTipos_OrTrue =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{   0   ,   0   ,   0   ,    0    },
            /* Decimal */{   0   ,   0   ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,   213   }
        };

        int[,] sistemaDeTipos_OrFalse =
        {
            /*            Entero  Decimal  Cadena  Lógico */
            /* Entero  */{   0   ,   0   ,   0   ,    0    },
            /* Decimal */{   0   ,   0   ,   0   ,    0    },
            /* Cadena  */{   0   ,   0   ,   0   ,    0    },
            /* Lógico  */{   0   ,   0   ,   0   ,   214   }
        };

        int[,] sistemaDeTipos_NotTrue =
        {            
            /* Entero  */{   0   },
            /* Decimal */{   0   },
            /* Cadena  */{   0   },
            /* Lógico  */{  213  }
        };

        int[,] sistemaDeTipos_NotFalse =
        {            
            /* Entero  */{   0   },
            /* Decimal */{   0   },
            /* Cadena  */{   0   },
            /* Lógico  */{  214  }
        };

        public Sintactico(nodo cabeza, nodo p)
        {
            this.cabeza = cabeza;
            this.p = p;

            sintaxis();
        }

        private void sintaxis()
        {
            p = cabeza;

            while (p != null && !errorEncontradoSintactico && !errorEncontradoSemantico && (!FinDeArchivo))
            {
                funcdef();
            }
        }

        private void funcdef()
        {
            if (p.token == 211) // Palabra reservada | def
            {
                p = p.siguiente;

                if (p.token == 100) // Identificador
                {
                    /*24/02/2022 - Semántico
                     * Agregue una instrucción para almacenar temporalmente el lexema de p
                     */
                    tmpNombreFuncion = p.lexema;
                    p = p.siguiente;

                    if (p.token == 113) // Op. Agrupación | ( 
                    {
                        p = p.siguiente;
                        if (p.token == 114) // Op. Agrupación | )
                        {
                            p = p.siguiente;
                            if (p.token == 121) // Símbolo puntuación | :
                            {
                                p = p.siguiente;

                                do
                                {
                                    statement();

                                } while (p != null && !errorEncontradoSintactico && !errorEncontradoSemantico && !FinDeArchivo);
                            }
                            else
                            {
                                errorEncontradoSintactico = true;
                                Console.WriteLine("\nSe esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
                                return;
                            }
                        }
                        else
                        {
                            errorEncontradoSintactico = true;
                            Console.WriteLine("\nSe esperaba el símbolo ')' en el renglón '" + p.renglon + "'.");
                            return;
                        }
                    }
                    else
                    {
                        errorEncontradoSintactico = true;
                        Console.WriteLine("\nSe esperaba el símbolo '(' en el renglón '" + p.renglon + "'.");
                        return;
                    }
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("\nSe esperaba un identificador en el renglón '" + p.renglon + "'.");
                    return;
                }
            }
            else
            {
                errorEncontradoSintactico = true;
                Console.WriteLine("\nSe esperaba la palabra reservada 'def' en el renglón '" + p.renglon + "'.");
                return;
            }
        }

        private void statement()
        {
            if (errorEncontradoSintactico || p == null)
            {
                return;
            }

            assignment_expression();

            if_stmt();
            while_stmt();
            for_stmt();

            if (p != null && p.token == 218) // Palabra reservada | break
            {
                p = p.siguiente;
            }

            print_stmt();
            input_stmt();

            if (p != null && p.token == 219) // Palabra reservada | continue
            {
                p = p.siguiente;
            }

            if (FinDeArchivo) imprimirListaCodigoIntermedio();
        }

        private void print_stmt()
        {
            if (errorEncontradoSintactico || p == null || errorEncontradoSemantico) return;

            if (p.token == 209) // Palabra reservada | print
            {
                p = p.siguiente;
                if (p.token == 113) // Op. Agrupación | ( 
                {
                    p = p.siguiente;
                    /* ((Identificador)  || (Cadena)) */
                    if ((p.token == 100) || (p.token == 122))
                    {
                        if (p.token == 100)
                        {
                            esVariableDeclarada();
                            if (errorEncontradoSemantico) return;
                        }

                        /* Implementación de la estructura */ 
                        tmpLexema = p.lexema;
                        insertarListaCodigoIntermedio();
                        tmpLexema = "print";
                        insertarListaCodigoIntermedio();

                        p = p.siguiente;
                        if (p.token == 114) // Op. Agrupación | )
                        {
                            p = p.siguiente;
                        }
                        if (p.lexema == "endfile")
                        {
                            FinDeArchivo = true;
                            insertarListaCodigoIntermedio();
                        }
                    }
                }
            }

        }

        private void input_stmt()
        {
            if (errorEncontradoSintactico || p == null || errorEncontradoSemantico)
            {
                return;
            }

            if (p.token == 210) // Palabra reservada | input
            {
                p = p.siguiente;

                if (p.token == 113) // Op. Agrupación | (
                {
                    p = p.siguiente;

                    /* (Cadena) */
                    if (p.token == 122) p = p.siguiente;

                    if (p.token == 114) // Op. Agrupación | )
                    {
                        p = p.siguiente;
                    }
                    else
                    {
                        errorEncontradoSintactico = true;
                        Console.WriteLine("Se esperaba el símbolo ')' en el renglón '" + p.renglon + "'.");
                        return;
                    }
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("Se esperaba el símbolo '(' en el renglón '" + p.renglon + "'.");
                    return;
                }
            }
        }

        private void assignment_expression()
        {
            if ((errorEncontradoSintactico) ||
                (errorEncontradoSemantico) ||
                (p == null)) return;

            do
            {
                if (p.token == 119) p = p.siguiente; // Símbolo puntuación | ,

                if (p.token == 100) // Identificador
                {
                    esIgualNombreFuncion();
                    esIdentificadorRepetido();
                    tmpLexema = p.lexema;
                    preparacionDeCodigoIntermedio();

                    if (errorEncontradoSemantico) return;

                    p = p.siguiente;

                    if (p.token == 123) // Op. Asignación | =
                    {
                        if (p.siguiente.token != 210) // Palabra reservada | input
                        {
                            preparacionDeCodigoIntermedio();
                        }
                        p = p.siguiente;

                        if (p.token == 101 || // Número Entero
                            p.token == 102 || // Número Decimal
                            p.token == 122 || // Cadena
                            p.token == 213 || // true
                            p.token == 214)   // false
                        {
                            insertarVariable();
                            tmpLexema = p.lexema;
                            preparacionDeCodigoIntermedio();
                            p = p.siguiente;
                        }
                        else if (p.token == 100) // Identificador
                        {
                            tmpLexema = p.lexema;
                            preparacionDeCodigoIntermedio();
                            while (p.renglon == p.siguiente.renglon)
                            {
                                p = p.siguiente;
                                tmpLexema = p.lexema;
                                preparacionDeCodigoIntermedio();
                            }
                            p = p.siguiente;
                        }
                        else if (p.token == 210) // Palabra reservada | input
                        {
                            insertarVariable();
                            tmpLexema = p.lexema;
                            insertarListaCodigoIntermedio();
                            input_stmt();
                        }
                    }
                    else
                    {
                        errorEncontradoSintactico = true;
                        Console.WriteLine("Se esperaba el símbolo '=' en el renglón '" + p.renglon + "'.");
                        return;
                    }
                }
            } while ((p.token == 119) && // Símbolo puntuación | ,
                     (!errorEncontradoSintactico));
        }

        private void if_stmt()
        {
            if ((p == null) || (errorEncontradoSintactico) || (errorEncontradoSemantico)) return;

            if (p.token == 203) // Palabra reservada | if
            {
                p = p.siguiente;
                conditional_expression();
                if (p.token == 121) // Símbolo puntuación | :
                {
                    tmpLexema = "BRF-A" + instanciaDeA;
                    insertarListaCodigoIntermedio();
                    p = p.siguiente;
                    statement();
                    if (p == null) return;
                    tmpLexema = "BRI-B" + instanciaDeB;
                    insertarListaCodigoIntermedio();
                    tmpApuntadorB = "B" + instanciaDeB;
                    instanciaDeB = instanciaDeB + 1;
                    pila ApuntadoresB = new pila();
                    elementoPila ApuntadorB = new elementoPila();
                    ApuntadorB.lexema = tmpApuntadorB;
                    ApuntadoresB.Push(ApuntadorB);
                    tmpApuntadorB = null;
                    TopePilaApuntadorB = ApuntadoresB;
                    tmpPilaApuntadorB = TopePilaApuntadorB;
                    tmpApuntadorA = "A" + instanciaDeA;
                    instanciaDeA = instanciaDeA + 1;  
                    
                    if (p.token == 205) // Palabra reservada | elif
                    {
                        while (p.token == 205) // Palabra reservada | elif
                        {
                            p = p.siguiente;
                            conditional_expression();
                            
                            if (p.token == 121) // Símbolo puntuación | :
                            {
                                tmpLexema = "BRF-A" + instanciaDeA;
                                insertarListaCodigoIntermedio();
                                p = p.siguiente;
                                statement();
                                tmpLexema = "BRI-B" + instanciaDeB;
                                insertarListaCodigoIntermedio();
                                tmpApuntadorB = "B" + instanciaDeB;
                                instanciaDeB = instanciaDeB + 1;
                                elementoPila OtroApuntadorB = new elementoPila();
                                OtroApuntadorB.lexema = tmpApuntadorB;
                                tmpPilaApuntadorB.Push(OtroApuntadorB);
                                tmpApuntadorB = null;
                                tmpApuntadorA = "A" + instanciaDeA;
                                instanciaDeA = instanciaDeA + 1;
                            }
                            else
                            {
                                Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
                                return;
                            }
                        }
                    }
                    if (p == null) return;
                    if (p.token == 204) // Palabra reservada | else
                    {
                        p = p.siguiente;
                        if (p.token == 121) // Símbolo puntuación | :
                        {
                            p = p.siguiente;
                            statement();
                            if (p.lexema == "endif")
                            {
                                p = p.siguiente;
                                AgregarApuntadoresB = true;
                            }
                            if (p.lexema == "endfile")
                            {
                                FinDeArchivo = true;
                                insertarListaCodigoIntermedio();
                            }
                        }
                        else
                        {
                            errorEncontradoSintactico = true;
                            Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
                            return;
                        }

                    }
                    if (p.lexema == "endif")
                    {
                        p = p.siguiente;
                        AgregarApuntadoresB = true;
                    }
                    if (p.lexema == "endfile")
                    {
                        FinDeArchivo = true;
                        insertarListaCodigoIntermedio();
                    }
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
                    return;
                }
            }
        }

        private void while_stmt()
        {
            if ((p == null) || (errorEncontradoSintactico) || (errorEncontradoSemantico)) return;     
            
            if (p.token == 208) // Palabra reservada | while
            {
                tmpApuntadorD = "D" + instanciaDeD;             
                p = p.siguiente;
                conditional_expression();
                if (p.token == 121) // Símbolo puntuación | :
                {
                    tmpLexema = "BRF-C" + instanciaDeC;
                    insertarListaCodigoIntermedio();
                    p = p.siguiente;
                    statement();
                    tmpLexema = "BRI-D" + instanciaDeD;
                    instanciaDeD = instanciaDeD + 1;
                    AgregarApuntadores = false;
                    insertarListaCodigoIntermedio();
                    AgregarApuntadores = true;
                    tmpApuntadorC = "C" + instanciaDeC;
                    instanciaDeC = instanciaDeC + 1;
                    if (p.lexema == "endwhile") p = p.siguiente;
                    if (p.lexema == "endfile") 
                    {
                        FinDeArchivo = true;
                        insertarListaCodigoIntermedio();
                    }
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
                    return;
                }
            }
        }

        private void compatibilidadDeTipos()
        {
            if (p.token == 100 || // Identificador
                p.token == 101 || // Número entero
                p.token == 102 || // Número decimal
                p.token == 122 || // Cadena
                p.token == 213 || // true
                p.token == 214)   // false
            {

                if (p.token == 100) // Identificador
                {
                    tmpVariable = cabezaListaVariables;

                    while (tmpVariable != null)
                    {
                        if (p.lexema == tmpVariable.lexema)
                        {
                            tmpLexema = p.lexema;
                            tmpToken = tmpVariable.token;
                            break;
                        }

                        tmpVariable = tmpVariable.siguiente;
                    }
                }
                insertarElementoListaPostfijo();
                insertarListaCodigoIntermedio();
            }
            else if (p.token == 103 || // Op. Aritmético | +
                     p.token == 104 || // Op. Aritmético | -
                     p.token == 105 || // Op. Aritmético | *
                     p.token == 106 || // Op. Aritmético | /
                     p.token == 127 || // Op. Aritmético | %
                     p.token == 107 || // Op. Relacional | >
                     p.token == 108 || // Op. Relacional | <
                     p.token == 109 || // Op. Relacional | <=
                     p.token == 110 || // Op. Relacional | >=
                     p.token == 111 || // Op. Relacional | ==
                     p.token == 112 || // Op. Relacional | !=
                     p.token == 113 || // Op. Agrupación | (
                     p.token == 114 || // Op. Agrupación | )
                     p.token == 115 || // Op. Agrupación | [
                     p.token == 116 || // Op. Agrupación | ]
                     p.token == 117 || // Op. Agrupación | {
                     p.token == 118 || // Op. Agrupación | }
                     p.token == 200 || // Op. Lógico     | and 
                     p.token == 201 || // Op. Lógico     | or 
                     p.token == 202)   // Op. Lógico     | not 
            {
                pila estructuraPila = new pila();
                elementoPila tmpElementoPila = new elementoPila();
                tmpElementoPila.lexema = p.lexema;
                tmpElementoPila.token = p.token;

                if ((topeEstructuraPila == null) ||
                    (topeEstructuraPila.tope == null))
                {
                    estructuraPila.Push(tmpElementoPila);
                    tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(p.token);
                    topeEstructuraPila = estructuraPila;
                    pEstructuraPila = topeEstructuraPila;
                }
                else
                {
                    tmpNivelJerarquicoOperadorDos = establecerJerarquiaDelOperador(p.token);

                    if (pEstructuraPila.tope.token == 113 || // Op. Agrupación | (
                        pEstructuraPila.tope.token == 115 || // Op. Agrupación | [
                        pEstructuraPila.tope.token == 117)   // Op. Agrupación | {
                    {
                        pEstructuraPila.Push(tmpElementoPila);
                        tmpNivelJerarquicoOperadorUno = tmpNivelJerarquicoOperadorDos;
                    }
                    else if ((p.token == 114) || // Op. Agrupación | )
                             (p.token == 116) || // Op. Agrupación | ]
                             (p.token == 118))   // Op. Agrupación | }
                    {
                        while ((topeEstructuraPila.tope.token != 113) && // Op. Agrupación | (
                               (topeEstructuraPila.tope.token != 115) && // Op. Agrupación | [
                               (topeEstructuraPila.tope.token != 117))   // Op. Agrupación | {
                        {
                            tmpLexema = topeEstructuraPila.tope.lexema;
                            tmpToken = topeEstructuraPila.tope.token;
                            insertarElementoListaPostfijo();
                            insertarListaCodigoIntermedio();
                            pEstructuraPila.Pop();

                            if (topeEstructuraPila.tope != null)
                            {
                                tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(topeEstructuraPila.tope.token);
                            }
                            else
                            {
                                tmpNivelJerarquicoOperadorUno = 0;
                            }
                        }

                        if ((pEstructuraPila.tope == null) ||
                            (pEstructuraPila.tope.token == 113) || // Op. Agrupación | (
                            (pEstructuraPila.tope.token == 115) || // Op. Agrupación | [
                            (pEstructuraPila.tope.token == 117))   // Op. Agrupación | {
                        {
                            pEstructuraPila.Pop();
                            if (pEstructuraPila.tope == null) return;
                            tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(topeEstructuraPila.tope.token);
                        }
                    }
                    else
                    {
                        if (tmpNivelJerarquicoOperadorUno < tmpNivelJerarquicoOperadorDos)
                        {
                            pEstructuraPila.Push(tmpElementoPila);
                            tmpNivelJerarquicoOperadorUno = tmpNivelJerarquicoOperadorDos;
                        }
                        else if ((tmpNivelJerarquicoOperadorUno > tmpNivelJerarquicoOperadorDos) ||
                                 (tmpNivelJerarquicoOperadorUno == tmpNivelJerarquicoOperadorDos))
                        {
                            if (topeEstructuraPila.tope != null)
                            {

                                while (((tmpNivelJerarquicoOperadorUno > tmpNivelJerarquicoOperadorDos) ||
                                        (tmpNivelJerarquicoOperadorUno == tmpNivelJerarquicoOperadorDos)) &&
                                         topeEstructuraPila.tope.token != 115 &&
                                         topeEstructuraPila.tope.token != 113)
                                {
                                    tmpLexema = topeEstructuraPila.tope.lexema;
                                    tmpToken = topeEstructuraPila.tope.token;
                                    insertarElementoListaPostfijo();
                                    insertarListaCodigoIntermedio();
                                    pEstructuraPila.Pop();

                                    if (topeEstructuraPila.tope != null)
                                    {
                                        tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(topeEstructuraPila.tope.token);
                                    }
                                    else
                                    {
                                        tmpNivelJerarquicoOperadorUno = 0;
                                    }
                                }

                                pEstructuraPila.Push(tmpElementoPila);
                                tmpNivelJerarquicoOperadorUno = tmpNivelJerarquicoOperadorDos;
                            }
                        }
                    }
                }
            }

            if (p.siguiente.token == 121) // Símbolo puntuación | :
            {
                while (topeEstructuraPila.tope != null)
                {
                    tmpLexema = topeEstructuraPila.tope.lexema.ToString();
                    tmpToken = topeEstructuraPila.tope.token;
                    insertarElementoListaPostfijo();
                    insertarListaCodigoIntermedio();
                    topeEstructuraPila.Pop();
                }
                evaluarListaDePolish();
            }
        }

        private void insertarListaCodigoIntermedio()
        {
            listaGeneracionDeCodigoIntermedio elemento = new listaGeneracionDeCodigoIntermedio(tmpLexema);
            tmpLexema = null;

            if (AgregarApuntadores)
            {
                if (tmpApuntadorA != null)
                {
                    elemento.apuntador = tmpApuntadorA;
                    tmpApuntadorA = null;
                }

                if (AgregarApuntadoresB)
                {
                    AgregarApuntadoresB = false;
                    while (TopePilaApuntadorB.tope != null)
                    {
                        if (elemento.apuntador == null)
                        {
                            elemento.apuntador = TopePilaApuntadorB.tope.lexema.ToString();
                            TopePilaApuntadorB.Pop();
                        }
                        else
                        {
                            elemento.apuntador = elemento.apuntador + " - " + TopePilaApuntadorB.tope.lexema.ToString();
                            TopePilaApuntadorB.Pop();
                        }
                    }
                }

                if (tmpApuntadorC != null)
                {
                    if (elemento.apuntador == null)
                    {
                        elemento.apuntador = tmpApuntadorC;
                        tmpApuntadorC = null;
                    }
                    else
                    {
                        elemento.apuntador = elemento.apuntador + " - " + tmpApuntadorC;
                        tmpApuntadorC = null;
                    }
                }

                if (tmpApuntadorD != null)
                {
                    if (elemento.apuntador == null)
                    {
                        elemento.apuntador = tmpApuntadorD;
                        tmpApuntadorD = null;
                    }
                    else
                    {
                        elemento.apuntador = elemento.apuntador + " - " + tmpApuntadorD;
                        tmpApuntadorD = null;
                    }
                }
            }

            if (cabezaCodigoIntermedio == null)
            {
                cabezaCodigoIntermedio = elemento;
                pCodigoIntermedio = cabezaCodigoIntermedio;
            }
            else
            {
                pCodigoIntermedio.siguiente = elemento;
                pCodigoIntermedio = elemento;
            }
        }

        private void preparacionDeCodigoIntermedio()
        {
            if (p.token == 100 || // Identificador
                p.token == 101 || // Número entero
                p.token == 102 || // Número decimal
                p.token == 122 || // Cadena
                p.token == 213 || // true
                p.token == 214)   // false
            {
                insertarListaCodigoIntermedio();
            }
            else if (p.token == 123 || // Op. Asignación | =
                     p.token == 103 || // Op. Aritmético | +
                     p.token == 104 || // Op. Aritmético | -
                     p.token == 105 || // Op. Aritmético | *
                     p.token == 106 || // Op. Aritmético | /
                     p.token == 127 || // Op. Aritmético | %
                     p.token == 107 || // Op. Relacional | >
                     p.token == 108 || // Op. Relacional | <
                     p.token == 109 || // Op. Relacional | <=
                     p.token == 110 || // Op. Relacional | >=
                     p.token == 111 || // Op. Relacional | ==
                     p.token == 112 || // Op. Relacional | !=
                     p.token == 113 || // Op. Agrupación | (
                     p.token == 114 || // Op. Agrupación | )
                     p.token == 115 || // Op. Agrupación | [
                     p.token == 116 || // Op. Agrupación | ]
                     p.token == 117 || // Op. Agrupación | {
                     p.token == 118 || // Op. Agrupación | }
                     p.token == 200 || // Op. Lógico     | and 
                     p.token == 201 || // Op. Lógico     | or 
                     p.token == 202)   // Op. Lógico     | not 
            {
                pila estructuraPila = new pila();
                elementoPila tmpElementoPila = new elementoPila();
                tmpElementoPila.lexema = p.lexema;
                tmpElementoPila.token = p.token;

                if ((topeEstructuraPila == null) ||
                    (topeEstructuraPila.tope == null))
                {
                    estructuraPila.Push(tmpElementoPila);
                    tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(p.token);
                    topeEstructuraPila = estructuraPila;
                    pEstructuraPila = topeEstructuraPila;
                }
                else
                {
                    tmpNivelJerarquicoOperadorDos = establecerJerarquiaDelOperador(p.token);

                    if (pEstructuraPila.tope.token == 113 || // Op. Agrupación | (
                        pEstructuraPila.tope.token == 115 || // Op. Agrupación | [
                        pEstructuraPila.tope.token == 117)   // Op. Agrupación | {
                    {
                        pEstructuraPila.Push(tmpElementoPila);
                        tmpNivelJerarquicoOperadorUno = tmpNivelJerarquicoOperadorDos;
                    }
                    else if ((p.token == 114) || // Op. Agrupación | )
                             (p.token == 116) || // Op. Agrupación | ]
                             (p.token == 118))   // Op. Agrupación | }
                    {
                        while ((topeEstructuraPila.tope.token != 113) && // Op. Agrupación | (
                               (topeEstructuraPila.tope.token != 115) && // Op. Agrupación | [
                               (topeEstructuraPila.tope.token != 117))   // Op. Agrupación | {
                        {
                            tmpLexema = topeEstructuraPila.tope.lexema;
                            tmpToken = topeEstructuraPila.tope.token;
                            insertarElementoListaPostfijo();
                            pEstructuraPila.Pop();

                            if (topeEstructuraPila.tope != null)
                            {
                                tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(topeEstructuraPila.tope.token);
                            }
                            else
                            {
                                tmpNivelJerarquicoOperadorUno = 0;
                            }
                        }

                        if ((pEstructuraPila.tope == null) ||
                            (pEstructuraPila.tope.token == 113) || // Op. Agrupación | (
                            (pEstructuraPila.tope.token == 115) || // Op. Agrupación | [
                            (pEstructuraPila.tope.token == 117))   // Op. Agrupación | {
                        {
                            pEstructuraPila.Pop();
                            if (pEstructuraPila.tope == null) return;
                            tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(topeEstructuraPila.tope.token);
                        }
                    }
                    else
                    {
                        if (tmpNivelJerarquicoOperadorUno < tmpNivelJerarquicoOperadorDos)
                        {
                            pEstructuraPila.Push(tmpElementoPila);
                            tmpNivelJerarquicoOperadorUno = tmpNivelJerarquicoOperadorDos;
                        }
                        else if ((tmpNivelJerarquicoOperadorUno > tmpNivelJerarquicoOperadorDos) ||
                                 (tmpNivelJerarquicoOperadorUno == tmpNivelJerarquicoOperadorDos))
                        {
                            if (topeEstructuraPila.tope != null)
                            {

                                while (((tmpNivelJerarquicoOperadorUno > tmpNivelJerarquicoOperadorDos) ||
                                        (tmpNivelJerarquicoOperadorUno == tmpNivelJerarquicoOperadorDos)) &&
                                         topeEstructuraPila.tope.token != 115 &&
                                         topeEstructuraPila.tope.token != 113)
                                {
                                    tmpLexema = topeEstructuraPila.tope.lexema;
                                    tmpToken = topeEstructuraPila.tope.token;
                                    insertarElementoListaPostfijo();
                                    pEstructuraPila.Pop();

                                    if (topeEstructuraPila.tope != null)
                                    {
                                        tmpNivelJerarquicoOperadorUno = establecerJerarquiaDelOperador(topeEstructuraPila.tope.token);
                                    }
                                    else
                                    {
                                        tmpNivelJerarquicoOperadorUno = 0;
                                    }
                                }

                                pEstructuraPila.Push(tmpElementoPila);
                                tmpNivelJerarquicoOperadorUno = tmpNivelJerarquicoOperadorDos;
                            }
                        }
                    }
                }
            }

            if ((p.siguiente.token == 119) || // Símbolo puntuación | ,
                (p.renglon != p.siguiente.renglon))
            {
                while (topeEstructuraPila.tope != null)
                {
                    tmpLexema = topeEstructuraPila.tope.lexema.ToString();
                    insertarListaCodigoIntermedio();
                    topeEstructuraPila.Pop();
                }
            }
        }

        private void for_stmt()
        {
            if ((p == null) || (errorEncontradoSintactico) || (errorEncontradoSemantico)) return;

            if (p.token == 206) // Palabra reservada | for
            {
                p = p.siguiente;
                esIgualNombreFuncion();
                esIdentificadorRepetido();
                p = p.siguiente;

                if (p.token == 207) // Palabra reservada | in
                {
                    p = p.siguiente;
                    esVariableDeclarada();
                    p = p.siguiente;

                    if (p.token == 121) // Símbolo puntuación | :
                    {
                        p = p.siguiente;
                        statement();
                    }
                    else
                    {
                        errorEncontradoSintactico = true;
                        Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
                        return;
                    }
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("Se esperaba la palabra reservada 'in' en el renglón '" + p.renglon + "'.");
                    return;
                }
            }
        }
 
        private void target()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;

            if (p.token == 100) // Identificador
            {
                esVariableDeclarada();
                if (errorEncontradoSemantico) return;
                compatibilidadDeTipos();
                p = p.siguiente;
                a_expr();
                target();
            }
            else if (p.token == 113) // Op. Agrupación | ( 
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                target();
                a_expr();
                target();
                if (errorEncontradoSemantico) return;
            }
            else if (p.token == 114) // Op. Agrupación | )
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                a_expr();
                target();
            }
            else if (p.token == 115) // Op. Agrupación | [
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                target();
                a_expr();
                target();
                if (errorEncontradoSemantico) return;
            }
            else if (p.token == 116) // Op. Agrupación | ]
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                a_expr();
                target();
            }
            /*       (Cadena        ) || (Número Entero ) || (Número Decimal) */
            else if ((p.token == 122) || (p.token == 101) || (p.token == 102)) 
            {
                tmpLexema = p.lexema;
                tmpToken = p.token;
                compatibilidadDeTipos();
                p = p.siguiente;
            }
            
            if (errorEncontradoSemantico) return;
        }
                
        private void conditional_expression()
        {
            if (errorEncontradoSintactico || p == null) return;

            target();
            if (errorEncontradoSemantico) return;
        }

        private void or_test()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;

            if (p.token == 201) // Palabra reservada | or
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            and_test();
            if (errorEncontradoSemantico) return;
        }

        private void and_test()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;

            if (p.token == 200) // Palabra reservada | and
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            not_test();
            if (errorEncontradoSemantico) return;
        }

        private void not_test()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;

            if (p.token == 202) // Palabra reservada | not
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }
        }

        private void comparison()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;
            do
            {
                comp_operator();
                or_expr();
                if (errorEncontradoSemantico) break;

            } while (p.token == 108 || // Op. Relacional | <
                     p.token == 107 || // Op. Relacional | >
                     p.token == 111 || // Op. Relacional | ==
                     p.token == 110 || // Op. Relacional | >=
                     p.token == 109 || // Op. Relacional | <=
                     p.token == 112 || // Op. Relacional | !=
                     p.token == 220 || // Palabra reservada | is
                     p.token == 202);  // Palabra reservada | not

            or_expr();
            if (errorEncontradoSemantico) return;
        }

        private void or_expr()
        {
            if (errorEncontradoSintactico ||
                p == null ||
                errorEncontradoSemantico) return;

            xor_expr();
            if (errorEncontradoSemantico) return;

            if (p.token == 124) // Op. a Nivel de Bits | |
            {
                or_expr();
                p = p.siguiente;
                xor_expr();
            }
        }

        private void xor_expr()
        {
            if (errorEncontradoSintactico || p == null)
            {
                return;
            }

            and_expr();
            if (errorEncontradoSemantico) return;

            if (p.token == 125) // Op. a Nivel de Bits | ^
            {
                xor_expr();
                p = p.siguiente;
                and_expr();
            }

        }

        private void comp_operator()
        {
            if (errorEncontradoSintactico ||
                p == null ||
                errorEncontradoSemantico)
            {
                return;
            }

            if (p.token == 108 || // Op. Relacional | <
                p.token == 107 || // Op. Relacional | >
                p.token == 111 || // Op. Relacional | ==
                p.token == 110 || // Op. Relacional | >=
                p.token == 109 || // Op. Relacional | <=
                p.token == 112)   // Op. Relacional | !=
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            else if (p.token == 220) // Palabra reservada | is
            {
                p = p.siguiente;

                if (p.token == 202) // Palabra reservada | not
                {
                    compatibilidadDeTipos();
                    p = p.siguiente;
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("Se esperaba la palabra reservada 'not' en el renglón '" + p.renglon + "'.");
                    return;
                }
            }

            else if (p.token == 202) // Palabra reservada | not
            {
                compatibilidadDeTipos();
                p = p.siguiente;

                if (p.token == 207) // Palabra reservada | in
                {
                    p = p.siguiente;
                }
                else
                {
                    errorEncontradoSintactico = true;
                    Console.WriteLine("Se esperaba la palabra reservada 'in' en el renglón '" + p.renglon + "'.");
                    return;
                }
            }

            or_test();
            if (errorEncontradoSemantico) return;
        }

        private void and_expr()
        {
            if (errorEncontradoSintactico || p == null) return;

            a_expr();
            if (errorEncontradoSemantico) return;

            if (p.token == 126) // Op. a Nivel de Bits | &
            {
                and_expr();
                p = p.siguiente;
                a_expr();
            }
        }

        private void a_expr()
        {
            if (errorEncontradoSintactico || p == null) return;

            if (p.token == 103) // Op. Aritmético | + 
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            if (p.token == 104) // Op. Aritmético | -
            {
                if (p.siguiente.siguiente.token == 114) p.lexema = "@";
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            m_expr();
            if (errorEncontradoSemantico) return;
        }

        private void m_expr()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;

            if (p.token == 105) // Op. Aritmético | *
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            if (p.token == 106) // Op. Aritmético | /
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            if (p.token == 127) // Op. Aritmético | %
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                return;
            }

            comp_operator();
            if (errorEncontradoSemantico) return;
        }

        private void u_expr()
        {
            if ((errorEncontradoSintactico) ||
                (p == null)) return;

            power();
            if (errorEncontradoSemantico) return;

            if (p.token == 104) // Op. Aritmético | -
            {
                if (p.siguiente.siguiente.token == 114) p.lexema = "@";
                compatibilidadDeTipos();
                p = p.siguiente;
                u_expr();
            }

            if (p.token == 103) // Op. Aritmético | +
            {
                compatibilidadDeTipos();
                p = p.siguiente;
                u_expr();
                if (errorEncontradoSemantico) return;
            }
        }

        private void power()
        {
            if (errorEncontradoSintactico || p == null) return;

            atom();
            if (errorEncontradoSemantico) return;

            if (p.token == 105) // Op. Aritmético | *
            {
                compatibilidadDeTipos();
                p = p.siguiente;

                if (p.token == 105) // Op. Aritmético | *
                {
                    compatibilidadDeTipos();
                    p = p.siguiente;
                    u_expr();
                }
            }
        }        

        private void atom()
        {
            if (errorEncontradoSintactico || p == null) return;

            if (p.token == 100 || // Identificador
                p.token == 122 || // Cadena
                p.token == 101 || // Número Entero
                p.token == 102)   // Número Decimal
            {
                esVariableDeclarada();
                if (errorEncontradoSemantico) return;
                compatibilidadDeTipos();
                if (errorEncontradoSemantico) return;
                p = p.siguiente;
            }

            //target();
            if (errorEncontradoSemantico) return;
        }

        private void insertarVariable()
        {
            tmpTokenTipoAlmacenado = p.token;

            listaDeVariables variable = new listaDeVariables(tmpNombreVariable, tmpTokenTipoAlmacenado);

            if (cabezaListaVariables == null)
            {
                cabezaListaVariables = variable;
                pListaVariables = cabezaListaVariables;
            }
            else
            {
                pListaVariables.siguiente = variable;
                pListaVariables = variable;
            }
        }

        private void imprimirListaCodigoIntermedio()
        {
            pCodigoIntermedio = cabezaCodigoIntermedio;

            Console.WriteLine("\n");
            while (pCodigoIntermedio != null)
            {
                Console.WriteLine("\t" + pCodigoIntermedio.lexema + "\t" + pCodigoIntermedio.apuntador);
                pCodigoIntermedio = pCodigoIntermedio.siguiente;
            }
        }

        private void esIgualNombreFuncion()
        {

            if (p.lexema == tmpNombreFuncion)
            {
                errorEncontradoSemantico = true;
                Console.WriteLine("\n\t" + erroresSemanticos[0, 0] + " Renglón " + p.renglon);
                return;
            }
            else
            {
                tmpNombreVariable = p.lexema;
            }
        }


        private void esIdentificadorRepetido()
        {
            if (p.lexema == p.siguiente.siguiente.lexema) return;

            if (!(cabezaListaVariables == null))
            {
                tmpVariable = cabezaListaVariables;

                while (tmpVariable != null)
                {
                    if (tmpNombreVariable == tmpVariable.lexema)
                    {
                        if (p.siguiente.siguiente.lexema == "input")
                        {
                            break;
                        }
                        else if (p.siguiente.siguiente.token == 100)
                        {
                            break;
                        }
                        else
                        {
                            errorEncontradoSemantico = true;
                            Console.WriteLine("\n\t" + erroresSemanticos[1, 0] + " Renglón " + p.renglon);
                            return;
                        }
                        //errorEncontradoSemantico = true;
                        //Console.WriteLine("\n\t" + erroresSemanticos[1, 0] + " Renglón " + p.renglon);
                        //return;
                    }

                    tmpVariable = tmpVariable.siguiente;
                }
            }
        }

        private void esVariableDeclarada()
        {
            esIgualNombreFuncion();

            if (errorEncontradoSemantico) return;

            tmpNombreVariable = p.lexema;
            bool variableDeclarada = false;
            tmpVariable = cabezaListaVariables;

            while (tmpVariable != null)
            {
                if (tmpNombreVariable == tmpVariable.lexema)
                {
                    variableDeclarada = true;
                    return;
                }

                tmpVariable = tmpVariable.siguiente;
            }

            if (!variableDeclarada)
            {
                errorEncontradoSemantico = true;
                Console.WriteLine("\n\t" + erroresSemanticos[2, 0] + " Renglón " + p.renglon + " Variable no declarada '" + p.lexema + "'");
                return;
            }
        }

        private void insertarElementoListaPostfijo()
        {
            listaDePostfijo elementoListaPostfijo = new listaDePostfijo(tmpLexema, tmpToken);

            if (cabezaListaPostfijo == null)
            {
                cabezaListaPostfijo = elementoListaPostfijo;
                pListaPostfijo = cabezaListaPostfijo;
            }
            else
            {
                pListaPostfijo.siguiente = elementoListaPostfijo;
                pListaPostfijo = elementoListaPostfijo;
            }
        }

        public int establecerJerarquiaDelOperador(int token)
        {
            if (token == 103 || //Op. Aritmético | +
                token == 104)   //Op. Aritmético | -
            {
                return 6;
            }
            else if (token == 105 || // Op. Aritmético | *
                     token == 106)   // Op. Aritmético | /
            {
                return 7;
            }
            else if (token == 127) // Op. Aritmético | %
            {
                return 5;
            }
            else if (token == 123 || // Op. Asignación | =
                     token == 107 || // Op. Relacional | >
                     token == 108 || // Op. Relacional | <
                     token == 109 || // Op. Relacional | <=
                     token == 110 || // Op. Relacional | >=
                     token == 111 || // Op. Relacional | ==
                     token == 112)   // Op. Relacional | !=
            {
                return 4;
            }
            else if (token == 202) // Op. Lógico | not 
            {
                return 3;
            }
            else if (token == 200) // Op. Lógico | and 
            {
                return 2;
            }
            else if (token == 201) // Op. Lógico | or 
            {
                return 1;
            }
            else if (token == 113 || // Op. Agrupación | (
                     token == 114 || // Op. Agrupación | )
                     token == 115 || // Op. Agrupación | [
                     token == 116 || // Op. Agrupación | ]
                     token == 117 || // Op. Agrupación | {
                     token == 118)   // Op. Agrupación | }
            {
                return 9;
            }
            return 0;
        }

        private void evaluarListaDePolish()
        {
            int operadorUnoToken, operadorDosToken;
            string operadorUnoLexema, operadorDosLexema;
            
            pListaPostfijo = cabezaListaPostfijo;

            while (pListaPostfijo != null)
            {
                if (pListaPostfijo.lexema == "@" || // Op. A.Unitario | - 
                    pListaPostfijo.token == 103  || // Op. Aritmético | +
                    pListaPostfijo.token == 104  || // Op. Aritmético | -
                    pListaPostfijo.token == 105  || // Op. Aritmético | *
                    pListaPostfijo.token == 106  || // Op. Aritmético | /
                    pListaPostfijo.token == 127  || // Op. Aritmético | %
                    pListaPostfijo.token == 107  || // Op. Relacional | >
                    pListaPostfijo.token == 108  || // Op. Relacional | <
                    pListaPostfijo.token == 109  || // Op. Relacional | <=
                    pListaPostfijo.token == 110  || // Op. Relacional | >=
                    pListaPostfijo.token == 111  || // Op. Relacional | ==
                    pListaPostfijo.token == 112  || // Op. Relacional | !=
                    pListaPostfijo.token == 200  || // Op. Lógico     | and 
                    pListaPostfijo.token == 201  || // Op. Lógico     | or 
                    pListaPostfijo.token == 202)    // Op. Lógico     | not 
                {
                    if (pListaPostfijo.lexema == "@")
                    {
                        operadorDosLexema = pEstructuraPila.tope.lexema;
                        operadorDosToken = pEstructuraPila.tope.token;
                        pEstructuraPila.Pop();

                        elementoPila tmpElementoPila = new elementoPila();
                        tmpElementoPila.lexema = "(" + pListaPostfijo.lexema + operadorDosLexema + ")";
                        pEstructuraPila.Push(tmpElementoPila);
                    }
                    else
                    {
                        elementoPila tmpElementoPila = new elementoPila();
                        operadorDosLexema = pEstructuraPila.tope.lexema;
                        operadorDosToken = pEstructuraPila.tope.token;
                        pEstructuraPila.Pop();
                        operadorUnoLexema = pEstructuraPila.tope.lexema;
                        operadorUnoToken = pEstructuraPila.tope.token;
                        pEstructuraPila.Pop();

                        if (operadorDosToken == 101) // Número Entero
                        {
                            columna = 0;
                        }
                        else if (operadorDosToken == 102) // Número Decimal
                        {
                            columna = 1;
                        }
                        else if (operadorDosToken == 122) // Cadena
                        {
                            columna = 2;
                        }
                        else if (operadorDosToken == 213) // true
                        {
                            columna = 3;
                        }
                        else if (operadorDosToken == 214) // false
                        {
                            columna = 3;
                        }

                        if (operadorUnoToken == 101) // Número Entero
                        {
                            fila = 0;
                        }
                        else if (operadorUnoToken == 102) // Número Decimal
                        {
                            fila = 1;
                        }
                        else if (operadorUnoToken == 122) // Cadena
                        {
                            fila = 2;
                        }
                        else if (operadorUnoToken == 213) // true
                        {
                            fila = 3;
                        }
                        else if (operadorUnoToken == 214) // false
                        {
                            fila = 3;
                        }

                        if (pListaPostfijo.token == 103) // Op. Aritmético | +
                        {
                            if (sistemaDeTiposSuma[fila,columna] == 0)
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTiposSuma[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }

                        }
                        else if (pListaPostfijo.token == 104) // Op. Aritmético | -
                        {
                            if (sistemaDeTiposResta[fila, columna] == 0)
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTiposResta[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 105) // Op. Aritmético | *
                        {
                            if (sistemaDeTiposMultiplicacion[fila, columna] == 0)
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" +operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                break;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTiposMultiplicacion[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 106) // Op. Aritmético | /
                        {
                            if (sistemaDeTiposDivision[fila, columna] == 0)
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTiposDivision[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 127) // Op. Aritmético | %
                        {
                            if (sistemaDeTiposModulo[fila, columna] == 0)
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTiposModulo[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 107) // Op. Relacional | >
                        {
                            if ((sistemaDeTipos_MayorQueTrue[fila, columna] == 0) && (sistemaDeTipos_MayorQueFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_MayorQueTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 108) // Op. Relacional | <
                        {
                            if ((sistemaDeTipos_MenorQueTrue[fila, columna] == 0) && (sistemaDeTipos_MenorQueFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_MenorQueTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 109) // Op. Relacional | <=
                        {
                            if ((sistemaDeTipos_MenorOIgualQueTrue[fila, columna] == 0) && (sistemaDeTipos_MenorOIgualQueFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_MenorOIgualQueTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 110) // Op. Relacional | >=
                        {
                            if ((sistemaDeTipos_MayorOIgualQueTrue[fila, columna] == 0) && (sistemaDeTipos_MayorOIgualQueFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_MayorOIgualQueTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 111) // Op. Relacional | ==
                        {
                            if ((sistemaDeTipos_IgualdadTrue[fila, columna] == 0) && (sistemaDeTipos_IgualdadFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_IgualdadTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 112) // Op. Relacional | !=
                        {
                            if ((sistemaDeTipos_DesigualTrue[fila, columna] == 0) && (sistemaDeTipos_DesigualFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_DesigualTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 200) // Op. Lógico     | and 
                        {
                            if ((sistemaDeTipos_AndTrue[fila, columna] == 0) && (sistemaDeTipos_AndFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_AndTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 201) // Op. Lógico     | or 
                        {
                            if ((sistemaDeTipos_OrTrue[fila, columna] == 0) && (sistemaDeTipos_OrFalse[fila, columna] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_OrTrue[fila, columna];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                        else if (pListaPostfijo.token == 202) // Op. Lógico     | not 
                        {
                            if ((sistemaDeTipos_NotTrue[fila, 0] == 0) && (sistemaDeTipos_NotFalse[fila, 0] == 0))
                            {
                                errorEncontradoSemantico = true;
                                Console.WriteLine("\n\t" + erroresSemanticos[3, 0] + " '" + operadorUnoLexema + "' y '" + operadorDosLexema + "' no son tipos compatibles." +
                                                  " " + " Renglón " + p.renglon);
                                return;
                            }
                            else
                            {
                                tmpElementoPila.lexema = "(" + operadorUnoLexema + pListaPostfijo.lexema + operadorDosLexema + ")";
                                tmpElementoPila.token = sistemaDeTipos_NotTrue[fila, 0];
                                pEstructuraPila.Push(tmpElementoPila);
                            }
                        }
                    }
                }
                else
                {
                    pila estructuraPila = new pila();
                    elementoPila tmpElementoPila = new elementoPila();
                    tmpElementoPila.lexema = pListaPostfijo.lexema;
                    tmpElementoPila.token = pListaPostfijo.token;

                    if ((topeEstructuraPila == null)    ||
                        (topeEstructuraPila.tope == null))
                    {
                        estructuraPila.Push(tmpElementoPila);
                        topeEstructuraPila = estructuraPila;
                        pEstructuraPila = topeEstructuraPila;
                    }
                    else
                    {
                        pEstructuraPila.Push(tmpElementoPila);
                    }
                }

                pListaPostfijo = pListaPostfijo.siguiente;       
            }
            cabezaListaPostfijo = null;
            topeEstructuraPila = null;
        }
    }
}

