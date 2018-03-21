namespace Inventario_3._0
{
    partial class FormLogeo
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogeo));
            this.labelVersion = new System.Windows.Forms.Label();
            this.richTextBoxEstadoServidor = new System.Windows.Forms.RichTextBox();
            this.labelServidor = new System.Windows.Forms.Label();
            this.buttonIngresar = new System.Windows.Forms.Button();
            this.textBoxUsuario = new System.Windows.Forms.TextBox();
            this.textBoxConstraseña = new System.Windows.Forms.TextBox();
            this.labelContraseña = new System.Windows.Forms.Label();
            this.labelUsuario = new System.Windows.Forms.Label();
            this.labelDesconectado = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(2, 157);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(17, 13);
            this.labelVersion.TabIndex = 17;
            this.labelVersion.Text = "V:";
            this.labelVersion.Click += new System.EventHandler(this.labelVersion_Click);
            // 
            // richTextBoxEstadoServidor
            // 
            this.richTextBoxEstadoServidor.Location = new System.Drawing.Point(198, 135);
            this.richTextBoxEstadoServidor.Name = "richTextBoxEstadoServidor";
            this.richTextBoxEstadoServidor.ReadOnly = true;
            this.richTextBoxEstadoServidor.Size = new System.Drawing.Size(104, 25);
            this.richTextBoxEstadoServidor.TabIndex = 16;
            this.richTextBoxEstadoServidor.Text = "";
            this.richTextBoxEstadoServidor.Click += new System.EventHandler(this.richTextBoxEstadoServidor_Click);
            // 
            // labelServidor
            // 
            this.labelServidor.AutoSize = true;
            this.labelServidor.Location = new System.Drawing.Point(143, 138);
            this.labelServidor.Name = "labelServidor";
            this.labelServidor.Size = new System.Drawing.Size(49, 13);
            this.labelServidor.TabIndex = 15;
            this.labelServidor.Text = "Servidor:";
            // 
            // buttonIngresar
            // 
            this.buttonIngresar.Location = new System.Drawing.Point(202, 64);
            this.buttonIngresar.Name = "buttonIngresar";
            this.buttonIngresar.Size = new System.Drawing.Size(100, 23);
            this.buttonIngresar.TabIndex = 12;
            this.buttonIngresar.Text = "Ingresar";
            this.buttonIngresar.UseVisualStyleBackColor = true;
            this.buttonIngresar.Click += new System.EventHandler(this.buttonIngresar_Click);
            // 
            // textBoxUsuario
            // 
            this.textBoxUsuario.Location = new System.Drawing.Point(202, 12);
            this.textBoxUsuario.Name = "textBoxUsuario";
            this.textBoxUsuario.Size = new System.Drawing.Size(100, 20);
            this.textBoxUsuario.TabIndex = 10;
            // 
            // textBoxConstraseña
            // 
            this.textBoxConstraseña.Location = new System.Drawing.Point(202, 38);
            this.textBoxConstraseña.Name = "textBoxConstraseña";
            this.textBoxConstraseña.Size = new System.Drawing.Size(100, 20);
            this.textBoxConstraseña.TabIndex = 11;
            this.textBoxConstraseña.UseSystemPasswordChar = true;
            this.textBoxConstraseña.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxConstraseña_KeyPress);
            // 
            // labelContraseña
            // 
            this.labelContraseña.AutoSize = true;
            this.labelContraseña.Location = new System.Drawing.Point(132, 41);
            this.labelContraseña.Name = "labelContraseña";
            this.labelContraseña.Size = new System.Drawing.Size(64, 13);
            this.labelContraseña.TabIndex = 13;
            this.labelContraseña.Text = "Contraseña:";
            // 
            // labelUsuario
            // 
            this.labelUsuario.AutoSize = true;
            this.labelUsuario.Location = new System.Drawing.Point(150, 15);
            this.labelUsuario.Name = "labelUsuario";
            this.labelUsuario.Size = new System.Drawing.Size(46, 13);
            this.labelUsuario.TabIndex = 14;
            this.labelUsuario.Text = "Usuario:";
            // 
            // labelDesconectado
            // 
            this.labelDesconectado.AutoSize = true;
            this.labelDesconectado.Location = new System.Drawing.Point(118, 114);
            this.labelDesconectado.Name = "labelDesconectado";
            this.labelDesconectado.Size = new System.Drawing.Size(0, 13);
            this.labelDesconectado.TabIndex = 18;
            // 
            // FormLogeo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(314, 172);
            this.Controls.Add(this.labelDesconectado);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.richTextBoxEstadoServidor);
            this.Controls.Add(this.labelServidor);
            this.Controls.Add(this.buttonIngresar);
            this.Controls.Add(this.textBoxUsuario);
            this.Controls.Add(this.textBoxConstraseña);
            this.Controls.Add(this.labelContraseña);
            this.Controls.Add(this.labelUsuario);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(330, 211);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(330, 211);
            this.Name = "FormLogeo";
            this.Text = "Inicio de sesión";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.RichTextBox richTextBoxEstadoServidor;
        private System.Windows.Forms.Label labelServidor;
        private System.Windows.Forms.Button buttonIngresar;
        private System.Windows.Forms.TextBox textBoxUsuario;
        private System.Windows.Forms.TextBox textBoxConstraseña;
        private System.Windows.Forms.Label labelContraseña;
        private System.Windows.Forms.Label labelUsuario;
        private System.Windows.Forms.Label labelDesconectado;
    }
}

