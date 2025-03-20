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
            txtProgramaFuente.Size = new Size(862, 484);
            txtProgramaFuente.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Dubai", 22.1999989F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LightCyan;
            label1.Location = new Point(298, 19);
            label1.Name = "label1";
            label1.Size = new Size(383, 63);
            label1.TabIndex = 1;
            label1.Text = "ANALIZADOR LÉXICO";
            // 
            // btnAnalizar
            // 
            btnAnalizar.BackColor = Color.AliceBlue;
            btnAnalizar.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAnalizar.ForeColor = Color.DarkBlue;
            btnAnalizar.Location = new Point(976, 96);
            btnAnalizar.Name = "btnAnalizar";
            btnAnalizar.Size = new Size(124, 42);
            btnAnalizar.TabIndex = 2;
            btnAnalizar.Text = "Analizar";
            btnAnalizar.UseVisualStyleBackColor = false;
            btnAnalizar.Click += btnAnalizar_Click;
            // 
            // lbResumen
            // 
            lbResumen.Font = new Font("Cascadia Code", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbResumen.FormattingEnabled = true;
            lbResumen.ItemHeight = 22;
            lbResumen.Location = new Point(930, 156);
            lbResumen.Name = "lbResumen";
            lbResumen.Size = new Size(217, 422);
            lbResumen.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.MidnightBlue;
            ClientSize = new Size(1202, 628);
            Controls.Add(lbResumen);
            Controls.Add(btnAnalizar);
            Controls.Add(label1);
            Controls.Add(txtProgramaFuente);
            Name = "Form1";
            Text = "Modula-2";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtProgramaFuente;
        private Label label1;
        private Button btnAnalizar;
        private ListBox lbResumen;
    }
}
