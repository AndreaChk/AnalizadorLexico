using System.Text.RegularExpressions;

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

        }

        private void btnAnalizar_Click(object sender, EventArgs e)
        {
            String CodigoFuente = txtProgramaFuente.Text;
            AnalizarCodigo(CodigoFuente);
        }

        private void AnalizarCodigo(string codigo)
        {
            // Diccionario con patrones de tokens para Modula-2
            Dictionary<string, string> tokenPatterns = new Dictionary<string, string>
            {
                { "KEYWORD", @"\b(MODULE|BEGIN|END|VAR|IF|THEN|ELSE|WHILE|DO|PROCEDURE|RETURN)\b" },
                { "IDENTIFIER", @"\b[A-Za-z_][A-Za-z0-9_]*\b" },
                { "NUMBER", @"\b\d+(\.\d+)?\b" },
                { "OPERATOR", @"[+\-*/:=]" },
                { "SPECIAL_CHAR", @"[;:,()\.]" }
            };

            lbResumen.Items.Clear(); // Limpiar la lista antes de analizar

            foreach (var pattern in tokenPatterns)
            {
                Regex regex = new Regex(pattern.Value);
                MatchCollection matches = regex.Matches(codigo);

                foreach (Match match in matches)
                {
                    lbResumen.Items.Add($"{match.Value} -> {pattern.Key}");
                }
            }
        }

    }
}
