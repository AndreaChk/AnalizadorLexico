using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace AnalizadorLexico
{
    public class Parser
    {
        private List<Token> tokens;
        private int indice = 0;
        private Token Actual => indice < tokens.Count ? tokens[indice] : null;
        private List<string> errores = new();
        private int ultimaLinea = 1;

        public Arbol Arbol { get; private set; }

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public bool Analizar()
        {
            Arbol = Programa();
            return errores.Count == 0;
        }

        public List<string> ObtenerErrores() => errores;

        private Arbol Programa()
        {
            var nodo = new Arbol("Programa");

            Esperar("PalabraReservada", "MODULE");
            nodo.Hijos.Add(new Arbol("MODULE"));

            Identificador();
            nodo.Hijos.Add(new Arbol("Identificador"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            nodo.Hijos.Add(Bloque());

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            Identificador();
            nodo.Hijos.Add(new Arbol("Identificador"));

            Esperar("Terminador", ".");
            nodo.Hijos.Add(new Arbol("."));

            return nodo;
        }

        private Arbol Bloque()
        {
            var nodo = new Arbol("Bloque");

            // 📌 Bloque CONST (opcional)
            if (Actual != null && Actual.Tipo == "PalabraReservada" &&
                Actual.Valor.ToUpper() == "CONST")
            {
                nodo.Hijos.Add(Constantes());
            }

            // 📌 Múltiples PROCEDUREs (opcional antes del cuerpo principal)
            while (Actual != null && Actual.Tipo == "PalabraReservada" &&
                   Actual.Valor.ToUpper() == "PROCEDURE")
            {
                nodo.Hijos.Add(Procedure());
            }

            // 📌 Declaraciones VAR (opcional)
            if (Actual != null && Actual.Tipo == "PalabraReservada" &&
                Actual.Valor.ToUpper() == "VAR")
            {
                nodo.Hijos.Add(Declaraciones());
            }

            // 📌 Bloque principal de ejecución
            Esperar("PalabraReservada", "BEGIN");
            nodo.Hijos.Add(new Arbol("BEGIN"));

            nodo.Hijos.Add(Sentencias());

            return nodo;
        }



        private Arbol Procedure()
        {
            var nodo = new Arbol("PROCEDURE");

            Esperar("PalabraReservada", "PROCEDURE");
            nodo.Hijos.Add(new Arbol("PROCEDURE"));

            // Nombre del procedimiento
            string nombreProc = Actual?.Valor;
            Identificador();
            nodo.Hijos.Add(new Arbol("Identificador"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            // Declaraciones internas del procedimiento
            if (Actual != null && Actual.Tipo == "PalabraReservada" &&
                Actual.Valor.ToUpper() == "VAR")
            {
                nodo.Hijos.Add(Declaraciones());
            }

            Esperar("PalabraReservada", "BEGIN");
            nodo.Hijos.Add(new Arbol("BEGIN"));

            nodo.Hijos.Add(Sentencias());

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            // Validación opcional: el nombre debe coincidir
            if (Actual != null && Actual.Tipo == "Identificador" && Actual.Valor == nombreProc)
            {
                Identificador();
                nodo.Hijos.Add(new Arbol("Identificador"));
            }
            else
            {
                errores.Add($"Error: se esperaba el identificador '{nombreProc}' después de END en línea {Actual?.Linea ?? ultimaLinea}");
                Avanzar();
            }

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }


        private Arbol Declaraciones()
        {
            var nodo = new Arbol("Declaraciones");

            Esperar("PalabraReservada", "VAR");
            nodo.Hijos.Add(new Arbol("VAR"));

            while (Actual != null && Actual.Tipo == "Identificador")
            {
                nodo.Hijos.Add(Declaracion());
            }

            return nodo;
        }

        private Arbol Declaracion()
        {
            var nodo = new Arbol("Declaracion");

            // Lista de identificadores separados por comas
            do
            {
                Identificador();
                nodo.Hijos.Add(new Arbol("Identificador"));

                if (Actual != null && Actual.Tipo == "Separador" && Actual.Valor == ",")
                {
                    Avanzar(); // Consumir la coma
                }
                else
                {
                    break; // Salir si no hay más comas
                }

            } while (Actual != null && Actual.Tipo == "Identificador");

            Esperar("Asignacion", ":");
            nodo.Hijos.Add(new Arbol(":"));

            Tipo();
            nodo.Hijos.Add(new Arbol("Tipo"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }

        private Arbol Sentencias()
        {
            var nodo = new Arbol("Sentencias");

            while (Actual != null)
            {
                string valor = Actual.Valor.ToUpper();
                bool esSentenciaValida =
                    Actual.Tipo == "Identificador" ||
                    (Actual.Tipo == "PalabraReservada" &&
                     (valor == "IF" || valor == "FOR" || valor == "WHILE" ||
                      valor == "REPEAT" || valor == "CASE" ||
                      valor == "LOOP" || valor == "EXIT"));

                if (!esSentenciaValida)
                    break;

                nodo.Hijos.Add(Sentencia());
            }

            return nodo;
        }


        private Arbol LoopSentencia()
        {
            var nodo = new Arbol("LOOP");

            Esperar("PalabraReservada", "LOOP");
            nodo.Hijos.Add(new Arbol("LOOP"));

            nodo.Hijos.Add(Sentencias());

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }

        private Arbol Sentencia()
        {
            if (Actual != null && Actual.Tipo == "PalabraReservada")
            {
                string palabra = Actual.Valor.ToUpper();

                if (palabra == "IF")
                {
                    var nodo = new Arbol("Sentencia");
                    nodo.Hijos.Add(IfSentencia());
                    return nodo;
                }

                if (palabra == "FOR")
                {
                    var nodo = new Arbol("Sentencia");
                    nodo.Hijos.Add(ForSentencia());
                    return nodo;
                }

                if (palabra == "WHILE")
                {
                    var nodo = new Arbol("Sentencia");
                    nodo.Hijos.Add(WhileSentencia());
                    return nodo;
                }

                if (palabra == "REPEAT")
                {
                    var nodo = new Arbol("Sentencia");
                    nodo.Hijos.Add(RepeatSentencia());
                    return nodo;
                }

                if (palabra == "CASE")
                {
                    var nodo = new Arbol("Sentencia");
                    nodo.Hijos.Add(CaseSentencia());
                    return nodo;
                }

                if (palabra == "LOOP")
                {
                    var nodo = new Arbol("Sentencia");
                    nodo.Hijos.Add(LoopSentencia());
                    return nodo;
                }

                if (palabra == "EXIT")
                {
                    var nodo = new Arbol("Sentencia");
                    Esperar("PalabraReservada", "EXIT");
                    nodo.Hijos.Add(new Arbol("EXIT"));

                    Esperar("Terminador", ";");
                    nodo.Hijos.Add(new Arbol(";"));

                    return nodo;
                }
            }

            // Sentencia simple (asignación)
            var nodoSimple = new Arbol("Sentencia");

            Identificador();
            nodoSimple.Hijos.Add(new Arbol("Identificador"));

            Esperar("Asignacion", ":=");
            nodoSimple.Hijos.Add(new Arbol(":="));

            nodoSimple.Hijos.Add(Expresion());

            Esperar("Terminador", ";");
            nodoSimple.Hijos.Add(new Arbol(";"));

            return nodoSimple;
        }


        private Arbol RepeatSentencia()
        {
            var nodo = new Arbol("REPEAT");

            Esperar("PalabraReservada", "REPEAT");
            nodo.Hijos.Add(new Arbol("REPEAT"));

            nodo.Hijos.Add(Sentencias());

            Esperar("PalabraReservada", "UNTIL");
            nodo.Hijos.Add(new Arbol("UNTIL"));

            nodo.Hijos.Add(Expresion());

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }



        private Arbol Constantes()
        {
            var nodo = new Arbol("Constantes");

            Esperar("PalabraReservada", "CONST");
            nodo.Hijos.Add(new Arbol("CONST"));

            while (Actual != null && Actual.Tipo == "Identificador")
            {
                var declaracion = new Arbol("Constante");

                Identificador();
                declaracion.Hijos.Add(new Arbol("Identificador"));

                Esperar("OpRelacional", "=");
                declaracion.Hijos.Add(new Arbol("="));

                if (Actual != null &&
                    (Actual.Tipo == "Entero" || Actual.Tipo == "Real" || Actual.Tipo == "Cadena"))
                {
                    declaracion.Hijos.Add(new Arbol(Actual.Valor));
                    Avanzar();
                }
                else
                {
                    errores.Add($"Se esperaba un valor constante en línea {Actual?.Linea ?? ultimaLinea}");
                    Avanzar();
                }

                Esperar("Terminador", ";");
                declaracion.Hijos.Add(new Arbol(";"));

                nodo.Hijos.Add(declaracion);
            }

            return nodo;
        }



        private Arbol WhileSentencia()
        {
            var nodo = new Arbol("WHILE");

            Esperar("PalabraReservada", "WHILE");
            nodo.Hijos.Add(new Arbol("WHILE"));

            nodo.Hijos.Add(Expresion());

            Esperar("PalabraReservada", "DO");
            nodo.Hijos.Add(new Arbol("DO"));

            nodo.Hijos.Add(Sentencias());

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }



        private Arbol IfSentencia()
        {
            var nodo = new Arbol("IF");

            Esperar("PalabraReservada", "IF");
            nodo.Hijos.Add(new Arbol("IF"));

            nodo.Hijos.Add(Expresion());

            Esperar("PalabraReservada", "THEN");
            nodo.Hijos.Add(new Arbol("THEN"));

            nodo.Hijos.Add(Sentencias());

            while (Actual != null && Actual.Tipo == "PalabraReservada" && Actual.Valor.ToUpper() == "ELSIF")
            {
                var elsif = new Arbol("ELSIF");

                Esperar("PalabraReservada", "ELSIF");
                elsif.Hijos.Add(new Arbol("ELSIF"));

                elsif.Hijos.Add(Expresion());

                Esperar("PalabraReservada", "THEN");
                elsif.Hijos.Add(new Arbol("THEN"));

                elsif.Hijos.Add(Sentencias());

                nodo.Hijos.Add(elsif);
            }

            if (Actual != null && Actual.Tipo == "PalabraReservada" && Actual.Valor.ToUpper() == "ELSE")
            {
                Esperar("PalabraReservada", "ELSE");
                var elseNodo = new Arbol("ELSE");
                elseNodo.Hijos.Add(Sentencias());
                nodo.Hijos.Add(elseNodo);
            }

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }



        private Arbol ForSentencia()
        {
            var nodo = new Arbol("FOR");

            Esperar("PalabraReservada", "FOR");
            nodo.Hijos.Add(new Arbol("FOR"));

            Identificador();
            nodo.Hijos.Add(new Arbol("Identificador"));

            Esperar("Asignacion", ":=");
            nodo.Hijos.Add(new Arbol(":="));

            nodo.Hijos.Add(Expresion());

            Esperar("PalabraReservada", "TO");
            nodo.Hijos.Add(new Arbol("TO"));

            nodo.Hijos.Add(Expresion());

            Esperar("PalabraReservada", "DO");
            nodo.Hijos.Add(new Arbol("DO"));

            nodo.Hijos.Add(Sentencias());

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }


        private Arbol CaseSentencia()
        {
            var nodo = new Arbol("CASE");

            Esperar("PalabraReservada", "CASE");
            nodo.Hijos.Add(new Arbol("CASE"));

            nodo.Hijos.Add(Expresion());

            Esperar("PalabraReservada", "OF");
            nodo.Hijos.Add(new Arbol("OF"));

            // Manejar cada alternativa (por ejemplo: 1: sentencia;)
            while (Actual != null && (Actual.Tipo == "Entero" || Actual.Tipo == "Cadena"))
            {
                var alternativa = new Arbol("Alternativa");

                alternativa.Hijos.Add(new Arbol(Actual.Valor)); // valor constante
                Avanzar();

                Esperar("Asignacion", ":");
                alternativa.Hijos.Add(new Arbol(":"));

                alternativa.Hijos.Add(Sentencia());

                nodo.Hijos.Add(alternativa);
            }

            // ELSE opcional
            if (Actual != null && Actual.Tipo == "PalabraReservada" && Actual.Valor.ToUpper() == "ELSE")
            {
                var elseNodo = new Arbol("ELSE");

                Esperar("PalabraReservada", "ELSE");
                elseNodo.Hijos.Add(Sentencias());

                nodo.Hijos.Add(elseNodo);
            }

            Esperar("PalabraReservada", "END");
            nodo.Hijos.Add(new Arbol("END"));

            Esperar("Terminador", ";");
            nodo.Hijos.Add(new Arbol(";"));

            return nodo;
        }


        private Arbol Expresion()
        {
            return ParseOr();
        }

        // OR tiene la menor precedencia
        private Arbol ParseOr()
        {
            var nodo = ParseAnd();

            while (Actual != null && Actual.Tipo == "PalabraReservada" && Actual.Valor.ToUpper() == "OR")
            {
                var operador = new Arbol("OR");
                Avanzar();
                operador.Hijos.Add(nodo);
                operador.Hijos.Add(ParseAnd());
                nodo = operador;
            }

            return nodo;
        }

        private Arbol ParseAnd()
        {
            var nodo = ParseNot();

            while (Actual != null && Actual.Tipo == "PalabraReservada" && Actual.Valor.ToUpper() == "AND")
            {
                var operador = new Arbol("AND");
                Avanzar();
                operador.Hijos.Add(nodo);
                operador.Hijos.Add(ParseNot());
                nodo = operador;
            }

            return nodo;
        }

        private Arbol ParseNot()
        {
            if (Actual != null && Actual.Tipo == "PalabraReservada" && Actual.Valor.ToUpper() == "NOT")
            {
                var operador = new Arbol("NOT");
                Avanzar();
                operador.Hijos.Add(ParseRelacional());
                return operador;
            }

            return ParseRelacional();
        }

        private Arbol ParseRelacional()
        {
            var izquierda = ParseExpresionAritmetica();

            if (Actual != null && Actual.Tipo == "OpRelacional")
            {
                var operador = new Arbol(Actual.Valor);
                Avanzar();
                operador.Hijos.Add(izquierda);
                operador.Hijos.Add(ParseExpresionAritmetica());
                return operador;
            }

            return izquierda;
        }

        private Arbol ParseExpresionAritmetica()
        {
            var nodo = ParseTermino();

            while (Actual != null && Actual.Tipo == "OpAritmetico" &&
                  (Actual.Valor == "+" || Actual.Valor == "-"))
            {
                var operador = new Arbol(Actual.Valor);
                Avanzar();
                operador.Hijos.Add(nodo);
                operador.Hijos.Add(ParseTermino());
                nodo = operador;
            }

            return nodo;
        }

        private Arbol ParseTermino()
        {
            var nodo = ParseFactor();

            while (Actual != null && Actual.Tipo == "OpAritmetico" &&
                  (Actual.Valor == "*" || Actual.Valor == "/" || Actual.Valor.ToUpper() == "DIV" || Actual.Valor.ToUpper() == "MOD"))
            {
                var operador = new Arbol(Actual.Valor);
                Avanzar();
                operador.Hijos.Add(nodo);
                operador.Hijos.Add(ParseFactor());
                nodo = operador;
            }

            return nodo;
        }

        private Arbol ParseFactor()
        {
            var nodo = new Arbol("Factor");

            if (Actual == null)
            {
                errores.Add("Se esperaba un factor pero no hay más tokens.");
                return nodo;
            }

            if (Actual.Tipo == "Entero" || Actual.Tipo == "Real" || Actual.Tipo == "Caracter" || Actual.Tipo == "Identificador")
            {
                nodo.Hijos.Add(new Arbol(Actual.Valor));
                Avanzar();
            }
            else if (Actual.Valor == "(")
            {
                Avanzar(); // consumir '('
                nodo = Expresion(); // usar Expresion completa, no solo aritmética

                if (Actual != null && Actual.Valor == ")")
                {
                    Avanzar(); // consumir ')'
                }
                else
                {
                    errores.Add($"Se esperaba ')' en línea {Actual?.Linea ?? ultimaLinea}");
                }
            }
            else
            {
                errores.Add($"Token inesperado '{Actual.Valor}' en línea {Actual.Linea}");
                Avanzar();
            }

            return nodo;
        }



        private void Tipo()
        {
            if (Actual != null && Actual.Tipo == "PalabraReservada" &&
                (Actual.Valor.ToUpper() == "INTEGER" ||
                 Actual.Valor.ToUpper() == "REAL" ||
                 Actual.Valor.ToUpper() == "BOOLEAN" ||
                 Actual.Valor.ToUpper() == "CHAR"))
            {
                Avanzar();
            }
            else
            {
                errores.Add($"Tipo no válido en línea {Actual?.Linea ?? ultimaLinea}");
                Avanzar();
            }

        }

        private void Identificador()
        {
            if (Actual != null && Actual.Tipo == "Identificador")
                Avanzar();
            else
            {
                errores.Add($"Se esperaba identificador en línea {Actual?.Linea ?? ultimaLinea}");
                Avanzar();
            }
        }

        private void Esperar(string tipoEsperado, string valorEsperado = null)
        {
            if (Actual != null && Actual.Tipo == tipoEsperado &&
                (valorEsperado == null || Actual.Valor.ToUpper() == valorEsperado.ToUpper()))
            {
                Avanzar();
            }
            else
            {
                errores.Add($"Error de sintaxis: se esperaba {valorEsperado ?? tipoEsperado} en línea {Actual?.Linea ?? ultimaLinea}");
                Avanzar();
            }
        }

        private void Avanzar()
        {
            if (Actual != null)
                ultimaLinea = Actual.Linea;

            indice++;
        }
    }
}
