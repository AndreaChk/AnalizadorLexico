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
            txtProgramaFuente.Location = new Point(46, 120);
            txtProgramaFuente.Margin = new Padding(4);
            txtProgramaFuente.Multiline = true;
            txtProgramaFuente.Name = "txtProgramaFuente";
            txtProgramaFuente.Size = new Size(810, 604);
            txtProgramaFuente.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Dubai", 22.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LightCyan;
            label1.Location = new Point(179, 24);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(549, 76);
            label1.TabIndex = 1;
            label1.Text = "COMPILADOR MODULA-2";
            // 
            // btnAnalizar
            // 
            btnAnalizar.BackColor = Color.AliceBlue;
            btnAnalizar.Cursor = Cursors.Hand;
            btnAnalizar.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnalizar.ForeColor = Color.DarkBlue;
            btnAnalizar.Location = new Point(1032, 38);
            btnAnalizar.Margin = new Padding(4);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(174, 52);
            btnAnalizar.TabIndex = 2;
            btnAnalizar.Text = "Léxico";
            btnAnalizar.UseVisualStyleBackColor = false;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // lbResumen
            // 
            lbResumen.Font = new Font("Cascadia Code", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbResumen.FormattingEnabled = true;
            lbResumen.ItemHeight = 27;
            lbResumen.Location = new Point(903, 562);
            lbResumen.Margin = new Padding(4);
            lbResumen.Name = "lbResumen";
            lbResumen.Size = new Size(635, 571);
            lbResumen.TabIndex = 3;
            // 
            // dgvTablaSimbolos
            // 
            dgvTablaSimbolos.AllowUserToDeleteRows = false;
            dgvTablaSimbolos.BackgroundColor = SystemColors.ControlLightLight;
            dgvTablaSimbolos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTablaSimbolos.Location = new Point(902, 120);
            dgvTablaSimbolos.Margin = new Padding(4);
            dgvTablaSimbolos.Name = "dgvTablaSimbolos";
            dgvTablaSimbolos.ReadOnly = true;
            dgvTablaSimbolos.RowHeadersWidth = 51;
            dgvTablaSimbolos.Size = new Size(636, 382);
            dgvTablaSimbolos.TabIndex = 4;
            // 
            // btnAnalizarSintaxis
            // 
            btnAnalizarSintaxis.BackColor = Color.AliceBlue;
            btnAnalizarSintaxis.Cursor = Cursors.Hand;
            btnAnalizarSintaxis.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnalizarSintaxis.ForeColor = Color.DarkBlue;
            btnAnalizarSintaxis.Location = new Point(1244, 38);
            btnAnalizarSintaxis.Margin = new Padding(4);
            btnAnalizarSintaxis.Name = "btnAnalizarSintaxis";
            btnAnalizarSintaxis.Size = new Size(174, 52);
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
            lbErroresSintacticos.ItemHeight = 27;
            lbErroresSintacticos.Location = new Point(47, 755);
            lbErroresSintacticos.Margin = new Padding(4);
            lbErroresSintacticos.Name = "lbErroresSintacticos";
            lbErroresSintacticos.Size = new Size(809, 382);
            lbErroresSintacticos.TabIndex = 6;
            // 
            // tvArbolSintactico
            // 
            tvArbolSintactico.Location = new Point(1563, 177);
            tvArbolSintactico.Name = "tvArbolSintactico";
            tvArbolSintactico.Size = new Size(477, 956);
            tvArbolSintactico.TabIndex = 7;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(2061, 1395);
            Controls.Add(tvArbolSintactico);
            Controls.Add(lbErroresSintacticos);
            Controls.Add(btnAnalizarSintaxis);
            Controls.Add(dgvTablaSimbolos);
            Controls.Add(lbResumen);
            Controls.Add(btnAnalizar);
            Controls.Add(label1);
            Controls.Add(txtProgramaFuente);
            Margin = new Padding(4);
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
