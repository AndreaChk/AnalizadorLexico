using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AnalizadorLexico
{
    public partial class Form1 : Form
    {
        private List<Token> listaTokensGlobal = new();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConfigurarTablaSimbolos();
        }

        private void ConfigurarTablaSimbolos()
        {
            dgvTablaSimbolos.Columns.Clear();
            dgvTablaSimbolos.Columns.Add("Nombre", "Nombre");
            dgvTablaSimbolos.Columns.Add("Tipo", "Tipo");
            dgvTablaSimbolos.Columns.Add("Valor", "Valor");
            dgvTablaSimbolos.Columns.Add("Línea", "Línea");
        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            string codigoFuente = txtProgramaFuente.Text;
            AnalizarCodigo(codigoFuente);
        }

        private void AnalizarCodigo(string codigo)
        {
            lbResumen.Items.Clear();
            dgvTablaSimbolos.Rows.Clear();
            listaTokensGlobal.Clear(); // Limpiar tokens anteriores

            var tokenPatterns = new List<TokenPattern>
            {
// Comentarios
    new TokenPattern("Comentario", @"\(\*.*?\*\)", RegexOptions.Singleline),

    // Palabras reservadas (debe ir antes que identificadores)
    new TokenPattern("PalabraReservada",
        @"\b(MODULE|BEGIN|END|VAR|CONST|PROCEDURE|INTEGER|REAL|CHAR|BOOLEAN|WHILE|DO|IF|THEN|ELSE|ELSIF|LOOP|EXIT|DIV|MOD|AND|OR|NOT|FOR|TO)\b",
        RegexOptions.IgnoreCase),

    // Operadores y símbolos
    new TokenPattern("Asignacion", @":=|:"), // := debe venir antes que :
    new TokenPattern("OpRelacional", @"(=|#|<|<=|>|>=)"),
    new TokenPattern("OpAritmetico", @"[+\-*/]"),
    new TokenPattern("Terminador", @";|\."), // ; y .
    new TokenPattern("Agrupacion", @"[\(\)\[\]\{\}]"),

    // Literales
    new TokenPattern("Hexadecimal", @"\b[0-9A-F]+[Hh]\b"),
    new TokenPattern("Real", @"\b\d+\.\d+([eE][+-]?\d+)?\b"),
    new TokenPattern("Entero", @"\b\d+\b"),
    new TokenPattern("Cadena", @"""([^""]|(""""))*"""),

    // Identificadores (va hasta el final para no interceptar palabras reservadas)
    new TokenPattern("Identificador", @"\b[A-Za-z][A-Za-z0-9_]*\b")
};


            string[] lineas = codigo.Split('\n');
            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {
                string linea = lineas[numLinea];
                int posicion = 0;

                while (posicion < linea.Length)
                {
                    if (char.IsWhiteSpace(linea[posicion]))
                    {
                        posicion++;
                        continue;
                    }

                    bool encontrado = false;

                    foreach (var pattern in tokenPatterns)
                    {
                        var match = pattern.Regex.Match(linea, posicion);
                        if (match.Success && match.Index == posicion)
                        {
                            if (pattern.Nombre != "Comentario")
                            {
                                string valor = match.Value;
                                string tipo = pattern.Nombre;
                                lbResumen.Items.Add($"{valor} -> {tipo} (Línea {numLinea + 1})");

                                listaTokensGlobal.Add(new Token(tipo, valor, numLinea + 1));

                                if (tipo == "PalabraReservada" && valor.Equals("VAR", StringComparison.OrdinalIgnoreCase))
                                {
                                    ProcesarDeclaracionesVAR(lineas, numLinea, ref posicion);
                                }
                            }

                            posicion += match.Length;
                            encontrado = true;
                            break;
                        }
                    }

                    if (!encontrado)
                    {
                        lbResumen.Items.Add($"Error léxico: Carácter inesperado '{linea[posicion]}' en línea {numLinea + 1}");
                        posicion++;
                    }
                }
            }
        }

        private void ProcesarDeclaracionesVAR(string[] lineas, int numLineaInicial, ref int posicion)
        {
            string lineaActual = lineas[numLineaInicial].Substring(posicion);

            for (int i = numLineaInicial; i < lineas.Length; i++)
            {
                string linea = (i == numLineaInicial) ? lineaActual : lineas[i];

                var declaraciones = Regex.Matches(linea, @"(\w+)\s*:\s*(\w+)(?:\s*:=\s*([^;]+))?\s*;");
                foreach (Match declaracion in declaraciones)
                {
                    string nombre = declaracion.Groups[1].Value;
                    string tipo = declaracion.Groups[2].Value;
                    string valor = declaracion.Groups[3].Success ? declaracion.Groups[3].Value.Trim() : "N/A";

                    dgvTablaSimbolos.Rows.Add(nombre, tipo, valor, i + 1);
                }

                if (Regex.IsMatch(linea, @"\bBEGIN\b", RegexOptions.IgnoreCase))
                {
                    break;
                }
            }
        }

        private void btnAnalizarSintaxis_Click_1(object sender, EventArgs e)
        {
            lbErroresSintacticos.Items.Clear();

            if (listaTokensGlobal.Count == 0)
            {
                lbErroresSintacticos.Items.Add("Primero realiza el análisis léxico.");
                return;
            }

            var parser = new Parser(listaTokensGlobal);
            bool correcto = parser.Analizar();

            if (correcto)
            {
                lbErroresSintacticos.Items.Add("Análisis sintáctico exitoso.");
            }
            else
            {
                foreach (var error in parser.ObtenerErrores())
                {
                    lbErroresSintacticos.Items.Add(error);
                }
            }

            tvArbolSintactico.Nodes.Clear();
            tvArbolSintactico.Nodes.Add(parser.Arbol.ToTreeNode());
            tvArbolSintactico.ExpandAll();

        }
    }

    public class TokenPattern
    {
        public string Nombre { get; }
        public Regex Regex { get; }

        public TokenPattern(string nombre, string pattern, RegexOptions options = RegexOptions.None)
        {
            Nombre = nombre;
            Regex = new Regex(@"\G" + pattern, options);
        }
    }
}
