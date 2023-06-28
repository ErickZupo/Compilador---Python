using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Compilador_Python
{
    public class Lexico
    {
        public nodo cabeza = null, p;
        int estado = 0, columna, valorMatrizTransicion, numeroRenglon = 1;
        int caracter = 0;
        string lexema = "";                        
        public bool errorEncontradoLexico = false; 
        string entradaLexico;
                
        string archivo = "C:\\Users\\eeric\\OneDrive\\Escritorio\\Hola.txt";
        
        int[,] matrizTransicion = {
             //   L    D    _    .    +    -    *    /    <    >    =    !    (    )    [    ]    {    }    ,    ;    :    "   nl   eb   cr   oc   |    ^    &    %
             //   0    1    2    3    4    5    6    7    8    9   10   11   12   13   14   15   16   17   18   19   20   21   22   23   24   25   26   27   28   29
        /*0*/{    1,   2, 502, 502, 103, 104, 105,   5,  11,  10,  13,  12, 113, 114, 115, 116, 117, 118, 119, 120, 121,   7,   0,   0,   0, 502, 124, 125, 126, 127},
        /*1*/{    1,   1,   1, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100},
        /*2*/{  101,   2, 101,   3, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101, 101},
        /*3*/{  500,   4, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500, 500},
        /*4*/{  102,   4, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102, 102},
        /*5*/{  106, 106, 106, 106, 106, 106, 106,   6, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106, 106},
        /*6*/{    6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   6,   0,   6,   0,   6,   6,   6,   6,   6},
        /*7*/{    9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9, 122, 503,   9, 503,   9,   9,   9,   9,   9},
        /*8*/{  122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122, 122},
        /*9*/{    9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9,   9, 122, 503,   9, 503,   9,   9,   9,   9,   9},
       /*10*/{  107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 110, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107, 107},
       /*11*/{  108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 109, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108, 108},
       /*12*/{  501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 112, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501, 501},
       /*13*/{  123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 111, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123, 123}
       };

        string[,] palabrasReservadas = {
            //       0            1
            /*0*/{ "and"      , "200" },
            /*1*/{ "or"       , "201" },
            /*2*/{ "not"      , "202" },
            /*3*/{ "if"       , "203" },
            /*4*/{ "else"     , "204" },
            /*5*/{ "elif"     , "205" },
            /*6*/{ "for"      , "206" },
            /*7*/{ "in"       , "207" },
            /*8*/{ "while"    , "208" },
            /*9*/{ "print"    , "209" },
           /*10*/{ "input"    , "210" },
           /*11*/{ "def"      , "211" },
           /*12*/{ "main"     , "212" },
           /*13*/{ "true"     , "213" },
           /*14*/{ "false"    , "214" },
           /*15*/{ "int"      , "215" },
           /*16*/{ "float"    , "216" },
           /*17*/{ "string"   , "217" },
           /*18*/{ "break"    , "218" },
           /*19*/{ "continue" , "219" },
           /*20*/{ "is"       , "220" }
       };

        string[,] errores = {
            //                       0                       1
            /*0*/{ "Error, se esperaba un dígito"        , "500" },
            /*1*/{ "Error, se esperaba un signo"         , "501" },
            /*2*/{ "Error, carácter no identificado"     , "502" },
            /*3*/{ "Error, se esperaban comillas dobles" , "503" }
       };

        public Lexico()
        {
            TextReader programaFuente = new StreamReader(archivo);
            entradaLexico = programaFuente.ReadToEnd();

            try
            {
                for (int cont = 0; cont < entradaLexico.ToCharArray().Length; cont++)
                {
                    caracter = siguienteCaracter(cont);

                    if (char.IsLetter((char)caracter))
                    {
                        columna = 0;
                    }
                    else if (char.IsDigit((char)caracter))
                    {
                        columna = 1;
                    }
                    else
                    {
                        switch ((char)caracter)
                        {
                            case '_':
                                columna = 2;
                                break;
                            case '.':
                                columna = 3;
                                break;
                            case '+':
                                columna = 4;
                                break;
                            case '-':
                                columna = 5;
                                break;
                            case '*':
                                columna = 6;
                                break;
                            case '/':
                                columna = 7;
                                break;
                            case '<':
                                columna = 8;
                                break;
                            case '>':
                                columna = 9;
                                break;
                            case '=':
                                columna = 10;
                                break;
                            case '!':
                                columna = 11;
                                break;
                            case '(':
                                columna = 12;
                                break;
                            case ')':
                                columna = 13;
                                break;
                            case '[':
                                columna = 14;
                                break;
                            case ']':
                                columna = 15;
                                break;
                            case '{':
                                columna = 16;
                                break;
                            case '}':
                                columna = 17;
                                break;
                            case ',':
                                columna = 18;
                                break;
                            case ';':
                                columna = 19;
                                break;
                            case ':':
                                columna = 20;
                                break;
                            case '"':
                                columna = 21;
                                break;
                            case (char)10:
                                {
                                    columna = 22;
                                    numeroRenglon = numeroRenglon + 1;
                                }
                                break;
                            case (char)9:
                                columna = 23;
                                break;
                            case (char)32:
                                columna = 23;
                                break;
                            case (char)13:
                                columna = 24;
                                break;
                            case '|':
                                columna = 26;
                                break;
                            case '^':
                                columna = 27;
                                break;
                            case '&':
                                columna = 28;
                                break;
                            case '%':
                                columna = 29;
                                break;
                            default:
                                columna = 25;
                                break;
                        }
                    }

                    valorMatrizTransicion = matrizTransicion[estado, columna];

                    if (valorMatrizTransicion < 100)
                    {
                        estado = valorMatrizTransicion;

                        if (estado == 0)
                        {
                            lexema = "";
                        }
                        else
                        {
                            lexema = lexema + (char)caracter;
                        }
                    }
                    else if (valorMatrizTransicion >= 100 && valorMatrizTransicion < 500)
                    {
                        if (valorMatrizTransicion == 100)
                        {
                            validarSiEsPalabraReservada();
                        }
                        if ( valorMatrizTransicion == 100 || valorMatrizTransicion == 101 || valorMatrizTransicion == 102 ||
                             valorMatrizTransicion == 106 || valorMatrizTransicion == 107 || valorMatrizTransicion == 108 ||
                             valorMatrizTransicion == 123 || valorMatrizTransicion >= 200)
                        {
                            cont--;
                        }
                        else
                        {
                            lexema = lexema + (char)caracter;
                        }
                        insertarNodo();
                        estado = 0;
                        lexema = "";

                    }
                    else
                    {
                        mensajeError();
                        break;
                    }
                }
                //imprimirNodos();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                try
                {
                    if (programaFuente != null)
                    {
                        programaFuente.Close();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private void imprimirNodos()
        {
            p = cabeza;

            Console.WriteLine("\n");
            while (p != null)
            {
                Console.WriteLine("\t" + p.lexema + "\t" + p.token + "\t" + p.renglon);
                p = p.siguiente;
            }
        }

        private void validarSiEsPalabraReservada()
        {

            for (int i = 0; i < palabrasReservadas.GetLength(0); i++)
            {
                if (lexema.Equals(palabrasReservadas[i, 0]))
                {
                    valorMatrizTransicion = int.Parse(palabrasReservadas[i, 1]);
                }

            }

        }

        private void mensajeError()
        {
            if (caracter != -1 && valorMatrizTransicion >= 500)
            {
                for (int i = 0; i < errores.GetLength(0); i++)
                {
                    if (valorMatrizTransicion == int.Parse(errores[i, 1]))
                    {
                        Console.WriteLine(" El error encontrado es: '" + errores[i, 0] + "'. Error " + valorMatrizTransicion + " Caracter '" + caracter + "' en el renglón " + numeroRenglon + ".\n");
                    }

                }
                errorEncontradoLexico = true;
            }
        }

        private void insertarNodo()
        {
            nodo nodo = new nodo(lexema, valorMatrizTransicion, numeroRenglon);

            if (cabeza == null)
            {
                cabeza = nodo;
                p = cabeza;
            }
            else
            {
                p.siguiente = nodo;
                p = nodo;
            }
        } 

        private char siguienteCaracter(int puntero)
        {
            return Convert.ToChar(entradaLexico.Substring(puntero, 1));
        }

        //private void sintaxis()
        //{
        //    p = cabeza;

        //    Console.WriteLine("\n");
        //    while (p != null)
        //    {
        //        funcdef();
        //        break;
        //    }

        //    if (errorEncontradoSintactico == false)
        //    {
        //        Console.WriteLine(" Análisis Sintáctico Terminado !!");
        //    }
            
        //}

        //private void funcdef()
        //{
        //    if (p.token == 211)
        //    {
        //        p = p.siguiente;
        //        if (p.token == 100)
        //        {
        //            p = p.siguiente;
        //            if (p.token == 113)
        //            {
        //                p = p.siguiente;
        //                if (p.token == 114)
        //                {
        //                    p = p.siguiente;
        //                    if (p.token == 121)
        //                    {
        //                        p = p.siguiente;

        //                        do
        //                        {

        //                            statement();

        //                        } while (p != null && errorEncontradoSintactico != true);
        //                    }
        //                    else
        //                    {
        //                        errorEncontradoSintactico = true;
        //                        Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    errorEncontradoSintactico = true;
        //                    Console.WriteLine("Se esperaba el símbolo ')' en el renglón '" + p.renglon + "'.");
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                errorEncontradoSintactico = true;
        //                Console.WriteLine("Se esperaba el símbolo '(' en el renglón '" + p.renglon + "'.");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba un identificador en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //    else
        //    {
        //        errorEncontradoSintactico = true;
        //        Console.WriteLine("Se esperaba la palabra reservada 'def' en el renglón '" + p.renglon + "'.");
        //        return;
        //    }
        //}

        //private void statement()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if_stmt();
        //    while_stmt();
        //    for_stmt();

        //    expression_list();
        //    assignment_stmt();

        //    if (p != null && p.token == 218)
        //    {
        //        p = p.siguiente;
        //    }

        //    print_stmt();
        //    input_stmt();

        //    if (p != null && p.token == 219)
        //    {
        //        p = p.siguiente;
        //    }
        //}

        //private void expression_list()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    conditional_expression();

        //    do
        //    {
        //        if (p.token == 119)
        //        {
        //            p = p.siguiente;

        //            if (p.token == 100)
        //            {
        //                conditional_expression();
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }

        //    } while (p.token != 100);  
        //}

        //private void if_stmt()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if ( p.token == 203)
        //    {
        //        p = p.siguiente;
        //        conditional_expression();
        //        if ( p.token == 121 )
        //        {
        //            p = p.siguiente;
        //            statement();
                    
        //            if ( p.token == 205)
        //            {
        //                while (p.token == 205)
        //                {
        //                    p = p.siguiente;
        //                    conditional_expression();
        //                    if (p.token == 121)
        //                    {
        //                        p = p.siguiente;
        //                        statement();
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //                        return;
        //                    }
        //                }
        //            }

        //            if (p.token == 204)
        //            {
        //                p = p.siguiente;
        //                if (p.token == 121)
        //                {
        //                    p = p.siguiente;
        //                    statement();
        //                }
        //                else
        //                {
        //                    errorEncontradoSintactico = true;
        //                    Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //                    return;
        //                }

        //            }
        //            else
        //            {
        //                errorEncontradoSintactico = true;
        //                Console.WriteLine("Se esperaba la palabra reservada 'else' en el renglón '" + p.renglon + "'.");
        //                return;
        //            }

        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }

        //}

        //private void while_stmt()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 208)
        //    {
        //        p = p.siguiente;
        //        conditional_expression();
        //        if (p.token == 121)
        //        {
        //            p = p.siguiente;
        //            statement();

        //            if (p.token == 204)
        //            {
        //                p = p.siguiente;
        //                if (p.token == 121)
        //                {
        //                    p = p.siguiente;
        //                    statement();
        //                }
        //                else
        //                {
        //                    errorEncontradoSintactico = true;
        //                    Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                errorEncontradoSintactico = true;
        //                Console.WriteLine("Se esperaba la palabra reservada 'else' en el renglón '" + p.renglon + "'.");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //}

        //private void for_stmt()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 206)
        //    {
        //        p = p.siguiente;
        //        target_list();

        //        if (p.token == 207)
        //        {
        //            p = p.siguiente;
        //            expression_list();

        //            if (p.token == 121)
        //            {
        //                p = p.siguiente;
        //                statement();

        //                if (p.token == 204)
        //                {
        //                    p = p.siguiente;

        //                    if (p.token == 121)
        //                    {
        //                        p = p.siguiente;
        //                        statement();
        //                    }
        //                    else
        //                    {
        //                        errorEncontradoSintactico = true;
        //                        Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //                        return;
        //                    }
        //                }
        //                else
        //                {
        //                    errorEncontradoSintactico = true;
        //                    Console.WriteLine("Se esperaba la palabra reservada 'else' en el renglón '" + p.renglon + "'.");
        //                    return;
        //                }
        //            }
        //            else
        //            {
        //                errorEncontradoSintactico = true;
        //                Console.WriteLine("Se esperaba el símbolo ':' en el renglón '" + p.renglon + "'.");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba la palabra reservada 'in' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //}

        //private void assignment_stmt()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    do
        //    {

        //        target_list();

        //        if (p != null && p.token == 123)
        //        {
        //            p = p.siguiente;
        //        }
        //        else
        //        {
        //            break;
        //        }

        //    } while (p.token != 100);

        //    expression_list();

        //}

        //private void target_list()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    do
        //    {
        //        target();

        //        if (p != null && p.token == 119)
        //        {
        //            p = p.siguiente;
        //        }
        //        else
        //        {
        //            break;
        //        }

        //    } while (p.token != 100);            
        //}

        //private void target()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 100)
        //    {
        //        p = p.siguiente;
        //    }
        //    else if (p.token == 113)
        //    {
        //        p = p.siguiente;
        //        target_list();

        //        if (p.token == 114)
        //        {
        //            p = p.siguiente;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //    else if (p.token == 115)
        //    {
        //        p = p.siguiente;
        //        target_list();

        //        if (p.token == 116)
        //        {
        //            p = p.siguiente;
        //        }
        //        else
        //        {
        //            return;
        //        }
        //    }
        //}

        //private void print_stmt()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 209)
        //    {
        //        p = p.siguiente;
        //        do
        //        {
        //            conditional_expression();

        //            if (p.token == 119)
        //            {
        //                p = p.siguiente;
        //            }
        //            else
        //            {
        //                break;
        //            }

        //        } while (p.token != 100);
        //    }

        //}

        //private void conditional_expression()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    assignment_expression();

        //    if (p.token == 203)
        //    {
        //        p = p.siguiente;
        //        or_test();
        //    }
        //    if (p.token == 204)
        //    {
        //        p = p.siguiente;
        //        conditional_expression();
        //    }
        //}

        //private void or_test()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    and_test();

        //    if (p.token == 201)
        //    {
        //        or_test();
        //        p = p.siguiente;
        //        and_test();
        //    }
        //}

        //private void and_test()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    not_test();

        //    if (p.token == 200)
        //    {
        //        and_test();
        //        p = p.siguiente;
        //        not_test();
        //    }
        //}

        //private void not_test()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    comparison();

        //    if (p.token == 202)
        //    {
        //        p = p.siguiente;
        //        not_test();
        //    }
        //}

        //private void comparison()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    or_expr();

        //    do
        //    {
        //        comp_operator();
        //        or_expr();

        //    } while (p.token == 108 || p.token == 107 || p.token == 111 || 
        //             p.token == 110 || p.token == 109 || p.token == 112 ||
        //             p.token == 220 || p.token == 202);
        //}

        //private void or_expr()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    xor_expr();

        //    if (p.token == 124)
        //    {
        //        or_expr();
        //        p = p.siguiente;
        //        xor_expr();
        //    }
        //}

        //private void xor_expr()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    and_expr();

        //    if (p.token == 125)
        //    {
        //        xor_expr();
        //        p = p.siguiente;
        //        and_expr();
        //    }

        //}

        //private void comp_operator()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 108 || p.token == 107 || p.token == 111 || p.token == 110 || p.token == 109 || p.token == 112)
        //    {
        //        p = p.siguiente;
        //    }

        //    if (p.token == 220)
        //    {
        //        p = p.siguiente;
                
        //        if (p.token == 202)
        //        {
        //            p = p.siguiente;
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba la palabra reservada 'not' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }

        //    if (p.token == 202)
        //    {
        //        p = p.siguiente;

        //        if (p.token == 207)
        //        {
        //            p = p.siguiente;
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba la palabra reservada 'in' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //}

        //private void and_expr()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    a_expr();

        //    if (p.token == 126)
        //    {
        //        and_expr();
        //        p = p.siguiente;
        //        a_expr();
        //    }
        //}

        //private void a_expr()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    m_expr();

        //    if (p.token == 103)
        //    {
        //        a_expr();
        //        p = p.siguiente;
        //        m_expr();
        //    }

        //    if (p.token == 104)
        //    {
        //        a_expr();
        //        p = p.siguiente;
        //        m_expr();
        //    }
        //}

        //private void m_expr()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    u_expr();

        //    if (p.token == 105)
        //    {
        //        m_expr();
        //        p = p.siguiente;
        //        u_expr();
        //    }

        //    if (p.token == 106)
        //    {
        //        m_expr();
        //        p = p.siguiente;
        //        u_expr();
        //    }

        //    if (p.token == 127)
        //    {
        //        m_expr();
        //        p = p.siguiente;
        //        u_expr();
        //    }
        //}

        //private void u_expr()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    power();

        //    if (p.token == 104)
        //    {
        //        p = p.siguiente;
        //        u_expr();
        //    }

        //    if (p.token == 103)
        //    {
        //        p = p.siguiente;
        //        u_expr();
        //    }

        //}

        //private void power()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    atom();

        //    if (p.token == 105)
        //    {
        //        p = p.siguiente;

        //        if (p.token == 105)
        //        {
        //            p = p.siguiente;
        //            u_expr();
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba el símbolo '*' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //}

        //private void input_stmt()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 210)
        //    {
        //        p = p.siguiente;

        //        if (p.token == 113)
        //        {
        //            p = p.siguiente;

        //            if (p.token == 114)
        //            {
        //                p = p.siguiente;
        //            }
        //            else
        //            {
        //                errorEncontradoSintactico = true;
        //                Console.WriteLine("Se esperaba el símbolo ')' en el renglón '" + p.renglon + "'.");
        //                return;
        //            }
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba el símbolo '(' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //}

        //private void atom()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 100 || p.token == 122 || p.token == 101 || p.token == 102)
        //    {
        //        p = p.siguiente;
        //    }
        //}

        //private void assignment_expression()
        //{
        //    if (errorEncontradoSintactico || p == null)
        //    {
        //        return;
        //    }

        //    if (p.token == 100)
        //    {
        //        p = p.siguiente;

        //        if (p.token == 123)
        //        {
        //            p = p.siguiente;

        //            if (p.token == 100 || p.token == 101 || p.token == 102)
        //            {
        //                p = p.siguiente;
        //            }
        //        }
        //        else
        //        {
        //            errorEncontradoSintactico = true;
        //            Console.WriteLine("Se esperaba el símbolo '=' en el renglón '" + p.renglon + "'.");
        //            return;
        //        }
        //    }
        //}
    }
}
