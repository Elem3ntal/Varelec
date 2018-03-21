namespace Inventario
{
    partial class FormLogeo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogeo));
            this.labelUsuario = new System.Windows.Forms.Label();
            this.labelContraseña = new System.Windows.Forms.Label();
            this.textBoxConstraseña = new System.Windows.Forms.TextBox();
            this.textBoxUsuario = new System.Windows.Forms.TextBox();
            this.buttonIngresar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.richTextBoxEstadoFresia = new System.Windows.Forms.RichTextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelUsuario
            // 
            this.labelUsuario.AutoSize = true;
            this.labelUsuario.Location = new System.Drawing.Point(137, 11);
            this.labelUsuario.Name = "labelUsuario";
            this.labelUsuario.Size = new System.Drawing.Size(46, 13);
            this.labelUsuario.TabIndex = 5;
            this.labelUsuario.Text = "Usuario:";
            // 
            // labelContraseña
            // 
            this.labelContraseña.AutoSize = true;
            this.labelContraseña.Location = new System.Drawing.Point(119, 37);
            this.labelContraseña.Name = "labelContraseña";
            this.labelContraseña.Size = new System.Drawing.Size(64, 13);
            this.labelContraseña.TabIndex = 5;
            this.labelContraseña.Text = "Contraseña:";
            // 
            // textBoxConstraseña
            // 
            this.textBoxConstraseña.Location = new System.Drawing.Point(189, 34);
            this.textBoxConstraseña.Name = "textBoxConstraseña";
            this.textBoxConstraseña.Size = new System.Drawing.Size(100, 20);
            this.textBoxConstraseña.TabIndex = 2;
            this.textBoxConstraseña.UseSystemPasswordChar = true;
            this.textBoxConstraseña.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxConstraseña_KeyPress);
            // 
            // textBoxUsuario
            // 
            this.textBoxUsuario.Location = new System.Drawing.Point(189, 8);
            this.textBoxUsuario.Name = "textBoxUsuario";
            this.textBoxUsuario.Size = new System.Drawing.Size(100, 20);
            this.textBoxUsuario.TabIndex = 1;
            // 
            // buttonIngresar
            // 
            this.buttonIngresar.Location = new System.Drawing.Point(189, 60);
            this.buttonIngresar.Name = "buttonIngresar";
            this.buttonIngresar.Size = new System.Drawing.Size(100, 23);
            this.buttonIngresar.TabIndex = 3;
            this.buttonIngresar.Text = "Ingresar";
            this.buttonIngresar.UseVisualStyleBackColor = true;
            this.buttonIngresar.Click += new System.EventHandler(this.buttonIngresar_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Fresia:";
            // 
            // richTextBoxEstadoFresia
            // 
            this.richTextBoxEstadoFresia.Location = new System.Drawing.Point(186, 126);
            this.richTextBoxEstadoFresia.Name = "richTextBoxEstadoFresia";
            this.richTextBoxEstadoFresia.ReadOnly = true;
            this.richTextBoxEstadoFresia.Size = new System.Drawing.Size(104, 25);
            this.richTextBoxEstadoFresia.TabIndex = 8;
            this.richTextBoxEstadoFresia.Text = "";
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(2, 145);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(50, 13);
            this.labelVersion.TabIndex = 9;
            this.labelVersion.Text = "V: 2.0.9a";
            this.labelVersion.Click += new System.EventHandler(this.label2_Click);
            // 
            // FormLogeo
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.Application;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 163);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.richTextBoxEstadoFresia);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonIngresar);
            this.Controls.Add(this.textBoxUsuario);
            this.Controls.Add(this.textBoxConstraseña);
            this.Controls.Add(this.labelContraseña);
            this.Controls.Add(this.labelUsuario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(317, 199);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(317, 199);
            this.Name = "FormLogeo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Administracion Varelec";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormLogeo_FormClosing);
            this.Load += new System.EventHandler(this.FormLogeo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUsuario;
        private System.Windows.Forms.Label labelContraseña;
        private System.Windows.Forms.TextBox textBoxConstraseña;
        private System.Windows.Forms.TextBox textBoxUsuario;
        private System.Windows.Forms.Button buttonIngresar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBoxEstadoFresia;
        private System.Windows.Forms.Label labelVersion;
    }
}

