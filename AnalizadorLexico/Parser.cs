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

            // Soporte para múltiples procedimientos antes del cuerpo principal
            while (Actual != null && Actual.Tipo == "PalabraReservada" &&
                   Actual.Valor.ToUpper() == "PROCEDURE")
            {
                nodo.Hijos.Add(Procedure());
            }

            // Declaraciones VAR
            if (Actual != null && Actual.Tipo == "PalabraReservada" &&
                Actual.Valor.ToUpper() == "VAR")
            {
                nodo.Hijos.Add(Declaraciones());
            }

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

            Identificador();
            nodo.Hijos.Add(new Arbol("Identificador"));

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
                     (valor == "IF" || valor == "FOR" || valor == "WHILE"));

                if (!esSentenciaValida)
                    break;

                nodo.Hijos.Add(Sentencia());
            }

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

                // Puedes agregar aquí WHILE, LOOP, etc.
            }

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




        private Arbol Expresion()
        {
            var nodo = new Arbol("Expresion");

            if (Actual != null && (Actual.Tipo == "Entero" || Actual.Tipo == "Identificador"))
            {
                nodo.Hijos.Add(new Arbol(Actual.Valor));
                Avanzar();
            }
            else
            {
                errores.Add($"Se esperaba número o identificador en línea {Actual?.Linea ?? ultimaLinea}");
                Avanzar(); // para que no se quede trabado
            }

            return nodo;
        }


        private void Tipo()
        {
            if (Actual != null && Actual.Tipo == "PalabraReservada" &&
                (Actual.Valor.ToUpper() == "INTEGER" || Actual.Valor.ToUpper() == "REAL" || Actual.Valor.ToUpper() == "BOOLEAN"))
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
