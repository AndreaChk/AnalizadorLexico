using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AnalizadorLexico
{
    public partial class Form1 : Form
    {
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
            // Limpiar resultados anteriores
            lbResumen.Items.Clear();
            dgvTablaSimbolos.Rows.Clear();

            // Lista ordenada de patrones (de más específico a más general)
            var tokenPatterns = new List<TokenPattern>
            {
                // Comentarios (deben procesarse primero)
                new TokenPattern("Comentario", @"\(\.?\*\)", RegexOptions.Singleline),
                
                // Palabras reservadas
                new TokenPattern("PalabraReservada", @"\b(MODULE|BEGIN|END|VAR|CONST|PROCEDURE|INTEGER|REAL|CHAR|BOOLEAN|WHILE|DO|IF|THEN|ELSE|ELSIF|LOOP|EXIT|DIV|MOD|AND|OR|NOT)\b", RegexOptions.IgnoreCase),
                
                // Operadores y símbolos
                new TokenPattern("Asignacion", @":=|:"),
                new TokenPattern("OpRelacional", @"(=|#|<|<=|>|>=)"),
                new TokenPattern("OpAritmetico", @"[+\-*/]"),
                new TokenPattern("Terminador", @"[;]"),
                new TokenPattern("Agrupacion", @"[\(\)\[\]\{\}]"),
                
                // Literales
                new TokenPattern("Hexadecimal", @"0x[0-9A-Fa-f]+"),
                new TokenPattern("Octal", @"0[0-7]+"),
                new TokenPattern("Real", @"\b\d+\.\d+\b"),
                new TokenPattern("Entero", @"\b\d+\b"),
                new TokenPattern("Cadena", @"""([^""\\]|\\.)*"""),
                
                // Identificadores (debe ir al final)
                new TokenPattern("Identificador", @"\b[A-Za-z_][A-Za-z0-9_]*\b")
            };

            // Analizar línea por línea
            string[] lineas = codigo.Split('\n');
            for (int numLinea = 0; numLinea < lineas.Length; numLinea++)
            {
                string linea = lineas[numLinea];
                int posicion = 0;

                while (posicion < linea.Length)
                {
                    // Saltar espacios en blanco
                    if (char.IsWhiteSpace(linea[posicion]))
                    {
                        posicion++;
                        continue;
                    }

                    bool encontrado = false;

                    // Probar cada patrón en orden
                    foreach (var pattern in tokenPatterns)
                    {
                        var match = pattern.Regex.Match(linea, posicion);
                        if (match.Success && match.Index == posicion)
                        {
                            // Ignorar comentarios
                            if (pattern.Nombre != "Comentario")
                            {
                                lbResumen.Items.Add($"{match.Value} -> {pattern.Nombre} (Línea {numLinea + 1})");

                                // Procesar declaraciones VAR
                                if (pattern.Nombre == "PalabraReservada" && match.Value.Equals("VAR", StringComparison.OrdinalIgnoreCase))
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
            // Buscar el bloque de declaraciones VAR
            string lineaActual = lineas[numLineaInicial].Substring(posicion);

            // Continuar hasta encontrar BEGIN o el final
            for (int i = numLineaInicial; i < lineas.Length; i++)
            {
                string linea = (i == numLineaInicial) ? lineaActual : lineas[i];

                // Buscar declaraciones de variables
                var declaraciones = Regex.Matches(linea, @"(\w+)\s*:\s*(\w+)(?:\s*:=\s*([^;]+))?\s*;");
                foreach (Match declaracion in declaraciones)
                {
                    string nombre = declaracion.Groups[1].Value;
                    string tipo = declaracion.Groups[2].Value;
                    string valor = declaracion.Groups[3].Success ? declaracion.Groups[3].Value.Trim() : "N/A";

                    dgvTablaSimbolos.Rows.Add(nombre, tipo, valor, i + 1);
                }

                // Detectar fin del bloque VAR
                if (Regex.IsMatch(linea, @"\bBEGIN\b", RegexOptions.IgnoreCase))
                {
                    break;
                }
            }
        }
    }

    // Clase auxiliar para manejar patrones de tokens
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