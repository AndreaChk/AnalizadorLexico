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

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        public bool Analizar()
        {
            Programa();
            return errores.Count == 0;
        }

        public List<string> ObtenerErrores() => errores;

        private void Programa()
        {
            Esperar("PalabraReservada", "MODULE");
            Identificador();
            Esperar("Terminador", ";");
            Bloque();
            Esperar("PalabraReservada", "END");
            Identificador();
            Esperar("Terminador", ".");
        }

        private void Bloque()
        {
            Declaraciones();
            Esperar("PalabraReservada", "BEGIN");
            Sentencias();
        }

        private void Declaraciones()
        {
            Esperar("PalabraReservada", "VAR");
            while (Actual != null && Actual.Tipo == "Identificador")
            {
                Declaracion();
            }
        }

        private void Declaracion()
        {
            Identificador();
            Esperar("Asignacion", ":");
            Tipo();
            Esperar("Terminador", ";");
        }

        private void Sentencias()
        {
            while (Actual != null && Actual.Tipo == "Identificador")
            {
                Sentencia();
            }
        }

        private void Sentencia()
        {
            Identificador();
            Esperar("Asignacion", ":=");
            Expresion();
            Esperar("Terminador", ";");
        }

        private void Expresion()
        {
            if (Actual != null && (Actual.Tipo == "Entero" || Actual.Tipo == "Identificador"))
            {
                Avanzar();
            }
            else
            {
                errores.Add($"Se esperaba número o identificador en línea {Actual?.Linea ?? 0}");
                Avanzar();
            }
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
                errores.Add($"Tipo no válido en línea {Actual?.Linea ?? 0}");
                Avanzar();
            }
        }

        private void Identificador()
        {
            if (Actual != null && Actual.Tipo == "Identificador")
                Avanzar();
            else
            {
                errores.Add($"Se esperaba identificador en línea {Actual?.Linea ?? 0}");
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
                errores.Add($"Error de sintaxis: se esperaba {valorEsperado ?? tipoEsperado} en línea {Actual?.Linea ?? 0}");
                Avanzar();
            }
        }

        private void Avanzar() => indice++;
    }
}

