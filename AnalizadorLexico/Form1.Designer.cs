namespace AnalizadorLexico
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtProgramaFuente = new TextBox();
            label1 = new Label();
            btnAnalizar = new Button();
            lbResumen = new ListBox();
            dgvTablaSimbolos = new DataGridView();
            btnAnalizarSintaxis = new Button();
            lbErroresSintacticos = new ListBox();
            tvArbolSintactico = new TreeView();
            ((System.ComponentModel.ISupportInitialize)dgvTablaSimbolos).BeginInit();
            SuspendLayout();
            // 
            // txtProgramaFuente
            // 
            txtProgramaFuente.BackColor = SystemColors.InactiveCaptionText;
            txtProgramaFuente.BorderStyle = BorderStyle.FixedSingle;
            txtProgramaFuente.Font = new Font("Cascadia Code", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtProgramaFuente.ForeColor = Color.LawnGreen;
            txtProgramaFuente.Location = new Point(37, 96);
            txtProgramaFuente.Multiline = true;
            txtProgramaFuente.Name = "txtProgramaFuente";
            txtProgramaFuente.Size = new Size(648, 484);
            txtProgramaFuente.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Dubai", 22.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LightCyan;
            label1.Location = new Point(143, 19);
            label1.Name = "label1";
            label1.Size = new Size(450, 63);
            label1.TabIndex = 1;
            label1.Text = "COMPILADOR MODULA-2";
            // 
            // btnAnalizar
            // 
            btnAnalizar.BackColor = Color.AliceBlue;
            btnAnalizar.Cursor = Cursors.Hand;
            btnAnalizar.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnalizar.ForeColor = Color.DarkBlue;
            btnAnalizar.Location = new Point(826, 30);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(139, 42);
            btnAnalizar.TabIndex = 2;
            btnAnalizar.Text = "Léxico";
            btnAnalizar.UseVisualStyleBackColor = false;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // lbResumen
            // 
            lbResumen.Font = new Font("Cascadia Code", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbResumen.FormattingEnabled = true;
            lbResumen.ItemHeight = 22;
            lbResumen.Location = new Point(722, 426);
            lbResumen.Name = "lbResumen";
            lbResumen.Size = new Size(509, 400);
            lbResumen.TabIndex = 3;
            // 
            // dgvTablaSimbolos
            // 
            dgvTablaSimbolos.AllowUserToDeleteRows = false;
            dgvTablaSimbolos.BackgroundColor = SystemColors.ControlLightLight;
            dgvTablaSimbolos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTablaSimbolos.Location = new Point(722, 96);
            dgvTablaSimbolos.Name = "dgvTablaSimbolos";
            dgvTablaSimbolos.ReadOnly = true;
            dgvTablaSimbolos.RowHeadersWidth = 51;
            dgvTablaSimbolos.Size = new Size(509, 306);
            dgvTablaSimbolos.TabIndex = 4;
            // 
            // btnAnalizarSintaxis
            // 
            btnAnalizarSintaxis.BackColor = Color.AliceBlue;
            btnAnalizarSintaxis.Cursor = Cursors.Hand;
            btnAnalizarSintaxis.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnalizarSintaxis.ForeColor = Color.DarkBlue;
            btnAnalizarSintaxis.Location = new Point(995, 30);
            btnAnalizarSintaxis.Name = "btnAnalizarSintaxis";
            btnAnalizarSintaxis.Size = new Size(139, 42);
            btnAnalizarSintaxis.TabIndex = 5;
            btnAnalizarSintaxis.Text = "Sintáctico";
            btnAnalizarSintaxis.UseVisualStyleBackColor = false;
            btnAnalizarSintaxis.Click += btnAnalizarSintaxis_Click_1;
            // 
            // lbErroresSintacticos
            // 
            lbErroresSintacticos.BackColor = SystemColors.WindowFrame;
            lbErroresSintacticos.Font = new Font("Cascadia Code SemiBold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lbErroresSintacticos.ForeColor = Color.SandyBrown;
            lbErroresSintacticos.FormattingEnabled = true;
            lbErroresSintacticos.ItemHeight = 22;
            lbErroresSintacticos.Location = new Point(37, 602);
            lbErroresSintacticos.Name = "lbErroresSintacticos";
            lbErroresSintacticos.Size = new Size(648, 224);
            lbErroresSintacticos.TabIndex = 6;
            // 
            // tvArbolSintactico
            // 
            tvArbolSintactico.Location = new Point(1256, 96);
            tvArbolSintactico.Margin = new Padding(2, 2, 2, 2);
            tvArbolSintactico.Name = "tvArbolSintactico";
            tvArbolSintactico.Size = new Size(382, 730);
            tvArbolSintactico.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(1649, 847);
            Controls.Add(tvArbolSintactico);
            Controls.Add(lbErroresSintacticos);
            Controls.Add(btnAnalizarSintaxis);
            Controls.Add(dgvTablaSimbolos);
            Controls.Add(lbResumen);
            Controls.Add(btnAnalizar);
            Controls.Add(label1);
            Controls.Add(txtProgramaFuente);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Modula-2";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTablaSimbolos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtProgramaFuente;
        private Label label1;
        private Button btnAnalizar;
        private ListBox lbResumen;
        private DataGridView dgvTablaSimbolos;
        private Button btnAnalizarSintaxis;
        private ListBox lbErroresSintacticos;
        private TreeView tvArbolSintactico;
    }
}
