namespace servidorVarelec
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mostrarVentanaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonMinimizar = new System.Windows.Forms.Button();
            this.buttonCerrar = new System.Windows.Forms.Button();
            this.richTextBoxEstado = new System.Windows.Forms.RichTextBox();
            this.buttonIniciarServidor = new System.Windows.Forms.Button();
            this.buttonDetenerServidor = new System.Windows.Forms.Button();
            this.buttonCrearTabla = new System.Windows.Forms.Button();
            this.buttonEliminarFila = new System.Windows.Forms.Button();
            this.buttonObtenerVersion = new System.Windows.Forms.Button();
            this.textBoxExtrada = new System.Windows.Forms.TextBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.notifyIcon1, "notifyIcon1");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mostrarVentanaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            // 
            // mostrarVentanaToolStripMenuItem
            // 
            this.mostrarVentanaToolStripMenuItem.Name = "mostrarVentanaToolStripMenuItem";
            resources.ApplyResources(this.mostrarVentanaToolStripMenuItem, "mostrarVentanaToolStripMenuItem");
            this.mostrarVentanaToolStripMenuItem.Click += new System.EventHandler(this.mostrarVentanaToolStripMenuItem_Click);
            // 
            // buttonMinimizar
            // 
            resources.ApplyResources(this.buttonMinimizar, "buttonMinimizar");
            this.buttonMinimizar.Name = "buttonMinimizar";
            this.buttonMinimizar.UseVisualStyleBackColor = true;
            this.buttonMinimizar.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonCerrar
            // 
            resources.ApplyResources(this.buttonCerrar, "buttonCerrar");
            this.buttonCerrar.Name = "buttonCerrar";
            this.buttonCerrar.UseVisualStyleBackColor = true;
            this.buttonCerrar.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBoxEstado
            // 
            resources.ApplyResources(this.richTextBoxEstado, "richTextBoxEstado");
            this.richTextBoxEstado.Name = "richTextBoxEstado";
            // 
            // buttonIniciarServidor
            // 
            resources.ApplyResources(this.buttonIniciarServidor, "buttonIniciarServidor");
            this.buttonIniciarServidor.Name = "buttonIniciarServidor";
            this.buttonIniciarServidor.UseVisualStyleBackColor = true;
            this.buttonIniciarServidor.Click += new System.EventHandler(this.buttonIniciarServidorClick);
            // 
            // buttonDetenerServidor
            // 
            resources.ApplyResources(this.buttonDetenerServidor, "buttonDetenerServidor");
            this.buttonDetenerServidor.Name = "buttonDetenerServidor";
            this.buttonDetenerServidor.UseVisualStyleBackColor = true;
            this.buttonDetenerServidor.Click += new System.EventHandler(this.buttonDetenerServidorClick);
            // 
            // buttonCrearTabla
            // 
            resources.ApplyResources(this.buttonCrearTabla, "buttonCrearTabla");
            this.buttonCrearTabla.Name = "buttonCrearTabla";
            this.buttonCrearTabla.UseVisualStyleBackColor = true;
            this.buttonCrearTabla.Click += new System.EventHandler(this.buttonCrearTabla_Click);
            // 
            // buttonEliminarFila
            // 
            resources.ApplyResources(this.buttonEliminarFila, "buttonEliminarFila");
            this.buttonEliminarFila.Name = "buttonEliminarFila";
            this.buttonEliminarFila.UseVisualStyleBackColor = true;
            this.buttonEliminarFila.Click += new System.EventHandler(this.buttonEliminarFila_Click);
            // 
            // buttonObtenerVersion
            // 
            resources.ApplyResources(this.buttonObtenerVersion, "buttonObtenerVersion");
            this.buttonObtenerVersion.Name = "buttonObtenerVersion";
            this.buttonObtenerVersion.UseVisualStyleBackColor = true;
            this.buttonObtenerVersion.Click += new System.EventHandler(this.buttonObtenerVersion_Click);
            // 
            // textBoxExtrada
            // 
            resources.ApplyResources(this.textBoxExtrada, "textBoxExtrada");
            this.textBoxExtrada.Name = "textBoxExtrada";
            // 
            // labelVersion
            // 
            resources.ApplyResources(this.labelVersion, "labelVersion");
            this.labelVersion.Name = "labelVersion";
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.textBoxExtrada);
            this.Controls.Add(this.buttonObtenerVersion);
            this.Controls.Add(this.buttonEliminarFila);
            this.Controls.Add(this.buttonCrearTabla);
            this.Controls.Add(this.buttonDetenerServidor);
            this.Controls.Add(this.buttonIniciarServidor);
            this.Controls.Add(this.richTextBoxEstado);
            this.Controls.Add(this.buttonCerrar);
            this.Controls.Add(this.buttonMinimizar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mostrarVentanaToolStripMenuItem;
        private System.Windows.Forms.Button buttonMinimizar;
        private System.Windows.Forms.Button buttonCerrar;
        private System.Windows.Forms.RichTextBox richTextBoxEstado;
        private System.Windows.Forms.Button buttonIniciarServidor;
        private System.Windows.Forms.Button buttonDetenerServidor;
        private System.Windows.Forms.Button buttonCrearTabla;
        private System.Windows.Forms.Button buttonEliminarFila;
        private System.Windows.Forms.Button buttonObtenerVersion;
        private System.Windows.Forms.TextBox textBoxExtrada;
        private System.Windows.Forms.Label labelVersion;
    }
}

