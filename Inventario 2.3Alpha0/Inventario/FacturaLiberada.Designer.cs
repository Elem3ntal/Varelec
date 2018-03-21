namespace Inventario
{
    partial class FacturaLiberada
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FacturaLiberada));
            this.buttonImprimirFactura = new System.Windows.Forms.Button();
            this.listViewFactura = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonAgregarProducto = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxProCant = new System.Windows.Forms.TextBox();
            this.D = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPrecioUnitario = new System.Windows.Forms.TextBox();
            this.textBoxNeto = new System.Windows.Forms.TextBox();
            this.textBoxIVA = new System.Windows.Forms.TextBox();
            this.textBoxTOTAL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxGiro = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxCondicion = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxDespacho = new System.Windows.Forms.TextBox();
            this.textBoxOrdenCompra = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxDireccion = new System.Windows.Forms.TextBox();
            this.textBoxCiudad = new System.Windows.Forms.TextBox();
            this.textBoxTelefono = new System.Windows.Forms.TextBox();
            this.comboBoxDescripcion = new System.Windows.Forms.ComboBox();
            this.comboBoxSeñores = new System.Windows.Forms.ComboBox();
            this.comboBoxRUT = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxSeñores = new System.Windows.Forms.TextBox();
            this.textBoxRut = new System.Windows.Forms.TextBox();
            this.textBoxDescripcion = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxNP = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.listViewNotas = new System.Windows.Forms.ListView();
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxNotas = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.eliminarToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.comboBoxNuParte = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonImprimirFactura
            // 
            this.buttonImprimirFactura.Location = new System.Drawing.Point(805, 13);
            this.buttonImprimirFactura.Name = "buttonImprimirFactura";
            this.buttonImprimirFactura.Size = new System.Drawing.Size(100, 23);
            this.buttonImprimirFactura.TabIndex = 0;
            this.buttonImprimirFactura.Text = "Imprimir Factura";
            this.buttonImprimirFactura.UseVisualStyleBackColor = true;
            this.buttonImprimirFactura.Click += new System.EventHandler(this.buttonImprimirFactura_Click);
            // 
            // listViewFactura
            // 
            this.listViewFactura.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewFactura.FullRowSelect = true;
            this.listViewFactura.GridLines = true;
            this.listViewFactura.Location = new System.Drawing.Point(13, 90);
            this.listViewFactura.Name = "listViewFactura";
            this.listViewFactura.Size = new System.Drawing.Size(892, 210);
            this.listViewFactura.TabIndex = 1;
            this.listViewFactura.UseCompatibleStateImageBehavior = false;
            this.listViewFactura.View = System.Windows.Forms.View.Details;
            this.listViewFactura.Click += new System.EventHandler(this.listViewFactura_Click);
            this.listViewFactura.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewFactura_MouseClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Cantidad";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Descripcion:";
            this.columnHeader2.Width = 644;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Precio Unitario:";
            this.columnHeader3.Width = 98;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Total:";
            this.columnHeader4.Width = 85;
            // 
            // buttonAgregarProducto
            // 
            this.buttonAgregarProducto.Location = new System.Drawing.Point(805, 306);
            this.buttonAgregarProducto.Name = "buttonAgregarProducto";
            this.buttonAgregarProducto.Size = new System.Drawing.Size(100, 23);
            this.buttonAgregarProducto.TabIndex = 8;
            this.buttonAgregarProducto.Text = "Agregar Producto";
            this.buttonAgregarProducto.UseVisualStyleBackColor = true;
            this.buttonAgregarProducto.Click += new System.EventHandler(this.buttonAgregarProducto_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 315);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Cantidad:";
            // 
            // textBoxProCant
            // 
            this.textBoxProCant.Location = new System.Drawing.Point(71, 312);
            this.textBoxProCant.Name = "textBoxProCant";
            this.textBoxProCant.Size = new System.Drawing.Size(100, 20);
            this.textBoxProCant.TabIndex = 4;
            this.textBoxProCant.Leave += new System.EventHandler(this.textBoxProCant_Leave);
            // 
            // D
            // 
            this.D.AutoSize = true;
            this.D.Location = new System.Drawing.Point(178, 315);
            this.D.Name = "D";
            this.D.Size = new System.Drawing.Size(66, 13);
            this.D.TabIndex = 5;
            this.D.Text = "Descripcion:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(417, 315);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "PrecioUnitario:";
            // 
            // textBoxPrecioUnitario
            // 
            this.textBoxPrecioUnitario.Location = new System.Drawing.Point(499, 312);
            this.textBoxPrecioUnitario.Name = "textBoxPrecioUnitario";
            this.textBoxPrecioUnitario.Size = new System.Drawing.Size(100, 20);
            this.textBoxPrecioUnitario.TabIndex = 7;
            this.textBoxPrecioUnitario.Leave += new System.EventHandler(this.textBoxPrecioUnitario_Leave);
            // 
            // textBoxNeto
            // 
            this.textBoxNeto.Location = new System.Drawing.Point(593, 368);
            this.textBoxNeto.Name = "textBoxNeto";
            this.textBoxNeto.ReadOnly = true;
            this.textBoxNeto.Size = new System.Drawing.Size(100, 20);
            this.textBoxNeto.TabIndex = 9;
            this.textBoxNeto.Text = "0";
            // 
            // textBoxIVA
            // 
            this.textBoxIVA.Location = new System.Drawing.Point(699, 368);
            this.textBoxIVA.Name = "textBoxIVA";
            this.textBoxIVA.ReadOnly = true;
            this.textBoxIVA.Size = new System.Drawing.Size(100, 20);
            this.textBoxIVA.TabIndex = 10;
            this.textBoxIVA.Text = "0";
            // 
            // textBoxTOTAL
            // 
            this.textBoxTOTAL.Location = new System.Drawing.Point(805, 368);
            this.textBoxTOTAL.Name = "textBoxTOTAL";
            this.textBoxTOTAL.ReadOnly = true;
            this.textBoxTOTAL.Size = new System.Drawing.Size(100, 20);
            this.textBoxTOTAL.TabIndex = 11;
            this.textBoxTOTAL.Text = "0";
            this.textBoxTOTAL.TextChanged += new System.EventHandler(this.textBox3_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(593, 349);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Neto:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(699, 348);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "IVA (19%):";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(805, 348);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Total:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Señores:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(287, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "RUT:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Giro:";
            // 
            // textBoxGiro
            // 
            this.textBoxGiro.Location = new System.Drawing.Point(68, 37);
            this.textBoxGiro.Name = "textBoxGiro";
            this.textBoxGiro.Size = new System.Drawing.Size(183, 20);
            this.textBoxGiro.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(589, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Condicion:";
            // 
            // textBoxCondicion
            // 
            this.textBoxCondicion.Location = new System.Drawing.Point(654, 7);
            this.textBoxCondicion.Name = "textBoxCondicion";
            this.textBoxCondicion.Size = new System.Drawing.Size(130, 20);
            this.textBoxCondicion.TabIndex = 22;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(574, 37);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(72, 13);
            this.label10.TabIndex = 23;
            this.label10.Text = "N° despacho:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(553, 63);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 13);
            this.label11.TabIndex = 24;
            this.label11.Text = "Orden de Compra:";
            // 
            // textBoxDespacho
            // 
            this.textBoxDespacho.Location = new System.Drawing.Point(654, 32);
            this.textBoxDespacho.Name = "textBoxDespacho";
            this.textBoxDespacho.Size = new System.Drawing.Size(130, 20);
            this.textBoxDespacho.TabIndex = 25;
            this.textBoxDespacho.TextChanged += new System.EventHandler(this.textBoxDespacho_TextChanged);
            // 
            // textBoxOrdenCompra
            // 
            this.textBoxOrdenCompra.Location = new System.Drawing.Point(654, 56);
            this.textBoxOrdenCompra.Name = "textBoxOrdenCompra";
            this.textBoxOrdenCompra.Size = new System.Drawing.Size(130, 20);
            this.textBoxOrdenCompra.TabIndex = 26;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(13, 63);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(55, 13);
            this.label12.TabIndex = 27;
            this.label12.Text = "Direccion:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(287, 63);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 13);
            this.label13.TabIndex = 28;
            this.label13.Text = "Ciudad:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(278, 37);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(52, 13);
            this.label14.TabIndex = 29;
            this.label14.Text = "Telefono:";
            // 
            // textBoxDireccion
            // 
            this.textBoxDireccion.Location = new System.Drawing.Point(68, 60);
            this.textBoxDireccion.Name = "textBoxDireccion";
            this.textBoxDireccion.Size = new System.Drawing.Size(183, 20);
            this.textBoxDireccion.TabIndex = 30;
            // 
            // textBoxCiudad
            // 
            this.textBoxCiudad.Location = new System.Drawing.Point(336, 60);
            this.textBoxCiudad.Name = "textBoxCiudad";
            this.textBoxCiudad.Size = new System.Drawing.Size(141, 20);
            this.textBoxCiudad.TabIndex = 31;
            // 
            // textBoxTelefono
            // 
            this.textBoxTelefono.Location = new System.Drawing.Point(336, 34);
            this.textBoxTelefono.Name = "textBoxTelefono";
            this.textBoxTelefono.Size = new System.Drawing.Size(141, 20);
            this.textBoxTelefono.TabIndex = 32;
            // 
            // comboBoxDescripcion
            // 
            this.comboBoxDescripcion.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxDescripcion.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxDescripcion.FormattingEnabled = true;
            this.comboBoxDescripcion.Location = new System.Drawing.Point(250, 312);
            this.comboBoxDescripcion.Name = "comboBoxDescripcion";
            this.comboBoxDescripcion.Size = new System.Drawing.Size(159, 21);
            this.comboBoxDescripcion.TabIndex = 33;
            this.comboBoxDescripcion.SelectedIndexChanged += new System.EventHandler(this.comboBoxDescripcion_SelectedIndexChanged);
            // 
            // comboBoxSeñores
            // 
            this.comboBoxSeñores.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxSeñores.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxSeñores.FormattingEnabled = true;
            this.comboBoxSeñores.Location = new System.Drawing.Point(68, 10);
            this.comboBoxSeñores.Name = "comboBoxSeñores";
            this.comboBoxSeñores.Size = new System.Drawing.Size(183, 21);
            this.comboBoxSeñores.TabIndex = 34;
            this.comboBoxSeñores.SelectedIndexChanged += new System.EventHandler(this.comboBoxSeñores_SelectedIndexChanged);
            // 
            // comboBoxRUT
            // 
            this.comboBoxRUT.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxRUT.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxRUT.FormattingEnabled = true;
            this.comboBoxRUT.Location = new System.Drawing.Point(336, 7);
            this.comboBoxRUT.Name = "comboBoxRUT";
            this.comboBoxRUT.Size = new System.Drawing.Size(141, 21);
            this.comboBoxRUT.TabIndex = 35;
            this.comboBoxRUT.SelectedIndexChanged += new System.EventHandler(this.comboBoxRUT_SelectedIndexChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(484, 10);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(71, 17);
            this.checkBox1.TabIndex = 36;
            this.checkBox1.Text = "No Existe";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBoxSeñores
            // 
            this.textBoxSeñores.Location = new System.Drawing.Point(68, 10);
            this.textBoxSeñores.Name = "textBoxSeñores";
            this.textBoxSeñores.Size = new System.Drawing.Size(183, 20);
            this.textBoxSeñores.TabIndex = 37;
            this.textBoxSeñores.Visible = false;
            // 
            // textBoxRut
            // 
            this.textBoxRut.Location = new System.Drawing.Point(336, 7);
            this.textBoxRut.Name = "textBoxRut";
            this.textBoxRut.Size = new System.Drawing.Size(141, 20);
            this.textBoxRut.TabIndex = 38;
            this.textBoxRut.Visible = false;
            this.textBoxRut.Leave += new System.EventHandler(this.textBoxRut_Leave);
            // 
            // textBoxDescripcion
            // 
            this.textBoxDescripcion.Location = new System.Drawing.Point(250, 312);
            this.textBoxDescripcion.Name = "textBoxDescripcion";
            this.textBoxDescripcion.Size = new System.Drawing.Size(159, 20);
            this.textBoxDescripcion.TabIndex = 5;
            this.textBoxDescripcion.Visible = false;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(605, 314);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(91, 17);
            this.checkBox2.TabIndex = 40;
            this.checkBox2.Text = "Sin Inventario";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(195, 343);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(49, 13);
            this.label15.TabIndex = 41;
            this.label15.Text = "N° parte:";
            this.label15.Visible = false;
            // 
            // textBoxNP
            // 
            this.textBoxNP.Location = new System.Drawing.Point(250, 341);
            this.textBoxNP.Name = "textBoxNP";
            this.textBoxNP.Size = new System.Drawing.Size(159, 20);
            this.textBoxNP.TabIndex = 6;
            this.textBoxNP.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(118, 26);
            // 
            // eliminarToolStripMenuItem
            // 
            this.eliminarToolStripMenuItem.Name = "eliminarToolStripMenuItem";
            this.eliminarToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.eliminarToolStripMenuItem.Text = "Eliminar";
            this.eliminarToolStripMenuItem.Click += new System.EventHandler(this.eliminarToolStripMenuItem_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(453, 365);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 42;
            this.button1.Text = "Mostrar Notas";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listViewNotas
            // 
            this.listViewNotas.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader5});
            this.listViewNotas.GridLines = true;
            this.listViewNotas.Location = new System.Drawing.Point(12, 410);
            this.listViewNotas.Name = "listViewNotas";
            this.listViewNotas.Size = new System.Drawing.Size(893, 154);
            this.listViewNotas.TabIndex = 43;
            this.listViewNotas.UseCompatibleStateImageBehavior = false;
            this.listViewNotas.View = System.Windows.Forms.View.Details;
            this.listViewNotas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listViewNotas_MouseClick);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Notas";
            this.columnHeader5.Width = 885;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(13, 573);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(33, 13);
            this.label16.TabIndex = 44;
            this.label16.Text = "Nota:";
            // 
            // textBoxNotas
            // 
            this.textBoxNotas.Location = new System.Drawing.Point(52, 570);
            this.textBoxNotas.Name = "textBoxNotas";
            this.textBoxNotas.Size = new System.Drawing.Size(703, 20);
            this.textBoxNotas.TabIndex = 45;
            this.textBoxNotas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxNotas_KeyPress);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(761, 568);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(144, 23);
            this.button2.TabIndex = 46;
            this.button2.Text = "Agregar";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eliminarToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(118, 26);
            // 
            // eliminarToolStripMenuItem1
            // 
            this.eliminarToolStripMenuItem1.Name = "eliminarToolStripMenuItem1";
            this.eliminarToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.eliminarToolStripMenuItem1.Text = "Eliminar";
            this.eliminarToolStripMenuItem1.Click += new System.EventHandler(this.eliminarToolStripMenuItem1_Click);
            // 
            // comboBoxNuParte
            // 
            this.comboBoxNuParte.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxNuParte.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxNuParte.FormattingEnabled = true;
            this.comboBoxNuParte.Location = new System.Drawing.Point(250, 341);
            this.comboBoxNuParte.Name = "comboBoxNuParte";
            this.comboBoxNuParte.Size = new System.Drawing.Size(159, 21);
            this.comboBoxNuParte.TabIndex = 8;
            this.comboBoxNuParte.SelectedIndexChanged += new System.EventHandler(this.comboBoxNuParte_SelectedIndexChanged);
            // 
            // FacturaLiberada
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 400);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBoxNotas);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.listViewNotas);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxNP);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.textBoxDescripcion);
            this.Controls.Add(this.textBoxRut);
            this.Controls.Add(this.textBoxSeñores);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.comboBoxRUT);
            this.Controls.Add(this.comboBoxSeñores);
            this.Controls.Add(this.comboBoxDescripcion);
            this.Controls.Add(this.textBoxTelefono);
            this.Controls.Add(this.textBoxCiudad);
            this.Controls.Add(this.textBoxDireccion);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxOrdenCompra);
            this.Controls.Add(this.textBoxDespacho);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxCondicion);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxGiro);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTOTAL);
            this.Controls.Add(this.textBoxIVA);
            this.Controls.Add(this.textBoxNeto);
            this.Controls.Add(this.textBoxPrecioUnitario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.D);
            this.Controls.Add(this.textBoxProCant);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAgregarProducto);
            this.Controls.Add(this.listViewFactura);
            this.Controls.Add(this.buttonImprimirFactura);
            this.Controls.Add(this.comboBoxNuParte);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FacturaLiberada";
            this.Text = "Factura Liberada";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FacturaLiberada_FormClosed);
            this.Load += new System.EventHandler(this.FacturaLiberada_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonImprimirFactura;
        private System.Windows.Forms.ListView listViewFactura;
        private System.Windows.Forms.Button buttonAgregarProducto;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxProCant;
        private System.Windows.Forms.Label D;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPrecioUnitario;
        private System.Windows.Forms.TextBox textBoxNeto;
        private System.Windows.Forms.TextBox textBoxIVA;
        private System.Windows.Forms.TextBox textBoxTOTAL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxGiro;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxCondicion;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxDespacho;
        private System.Windows.Forms.TextBox textBoxOrdenCompra;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox textBoxDireccion;
        private System.Windows.Forms.TextBox textBoxCiudad;
        private System.Windows.Forms.TextBox textBoxTelefono;
        private System.Windows.Forms.ComboBox comboBoxDescripcion;
        private System.Windows.Forms.ComboBox comboBoxSeñores;
        private System.Windows.Forms.ComboBox comboBoxRUT;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBoxSeñores;
        private System.Windows.Forms.TextBox textBoxRut;
        private System.Windows.Forms.TextBox textBoxDescripcion;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxNP;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listViewNotas;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxNotas;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem eliminarToolStripMenuItem1;
        private System.Windows.Forms.ComboBox comboBoxNuParte;
    }
}