using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexico
{
    public class Token
    {
        public string Tipo { get; set; }
        public string Valor { get; set; }
        public int Linea { get; set; }

        public Token(string tipo, string valor, int linea)
        {
            Tipo = tipo;
            Valor = valor;
            Linea = linea;
        }
    }
}
