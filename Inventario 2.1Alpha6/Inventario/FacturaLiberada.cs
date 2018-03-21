using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Inventario
{
    public partial class FacturaLiberada : Form
    {
        public FormInventario padre;
        public List<String[]> clientes;
        public List<string[]> productos;
        public string np;
        public FacturaLiberada(FormInventario padre_)
        {
            padre = padre_;
            InitializeComponent();
            clientes = new List<string[]>();
            productos = new List<string[]>();
            cargarClientes();
            cargarProductos();
            cargarCombobox();
            cargarComboboxClientes();
            if (File.Exists("c:\\Inventario\\notasFacturas.ftr"))
            {
                File.Delete("c:\\Inventario\\notasFacturas.ftr");
            }
        }
        public void cargarComboboxClientes()
        {
            foreach (string[] cliente in clientes)
            {
                comboBoxRUT.Items.Add(cliente[0]);
                comboBoxSeñores.Items.Add(cliente[1]);
            }
        }
        public void cargarCombobox()
        {
            foreach (string[] producto in productos)
            {
                comboBoxNuParte.Items.Add(producto[0]);
                comboBoxDescripcion.Items.Add(producto[8]);
            }
        }
        public void cargarClientes()
        {
            padre.fresia.getClientesXML().Save("clientes.old.ftr");
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("clientes.old.ftr");
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                string[] cliente = { "", "", "", "", "", "", "" };
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "RUT")
                    {
                        cliente[0] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Nombre")
                    {
                        cliente[1] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Giro")
                    {
                        cliente[2] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Contacto")
                    {
                        cliente[5] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Direccion")
                    {
                        cliente[4] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Ciudad")
                    {
                        cliente[5] +=" - " + nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Fono")
                    {
                        cliente[6] = nodo2.InnerText;
                    }
                }
                clientes.Add(cliente);
            }
        }
        public void cargarProductos()
        {
            padre.fresia.getInventarioXML().Save("inventario.xml.ftr");
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("inventario.xml.ftr");
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                string[] producto = { "", "", "", "", "", "", "", "", "" };
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "Codigo")
                    {
                        producto[0] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Origen")
                    {
                        producto[1] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Familia")
                    {
                        producto[2] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Grupo")
                    {
                        producto[3] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Cant")
                    {
                        producto[4] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "CostoUnitEuto")
                    {
                        producto[5] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "CostoUnitPeso")
                    {
                        producto[6] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "PVP")
                    {
                        producto[7] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Descripcion")
                    {
                        producto[8] = nodo2.InnerText;
                    }
                }
                productos.Add(producto);
            }
        }
        private void buttonImprimirFactura_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewFactura.Items)
            {
                foreach (string[] producto in productos)
                {
                    int letra = cantidad(item.SubItems[1].Text, '-');
                    //cantidad(textBox1.Text, '-')
                    //item.SubItems[1].Text
                    //if (producto[0].Equals(item.SubItems[1].Text.Split('-')[1].ToString()))
                    if (producto[0].Equals(item.SubItems[1].Text.Split('-')[letra].ToString()))
                    {
                        producto[4] = "" + (Convert.ToInt32(producto[4]) - Convert.ToInt32(Math.Round(Convert.ToDouble(item.Text),0)));
                        padre.fresia.editarProducto(producto);
                        if (padre.frecia.ingresoPMP(producto[0], 0, 0, Convert.ToInt32(producto[4]), Convert.ToInt32(producto[1].Split('¬')[2]), Convert.ToInt32(producto[5]), producto[1].Split('¬')[0]) == false)
                        {
                            MessageBox.Show("error al ingresar a ficha PMP");
                        }
                    }
                }
            }
            mandarAFrecia();
            imprimirFactura();
        }
        private int cantidad(string cadena, char X)
        {
            int cant = 0;
            foreach (char Y in cadena)
            {
                if (X == Y)
                {
                    cant++;
                }
            }
            return cant;
        }
        private void mandarAFrecia()
        {
            XElement productos = new XElement("Productos"), data = new XElement("MetaData"), elementos;
            data.Add(new XElement("Fecha", DateTime.Now.Date.Day + "/" + DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Year));
            XElement factura = new XElement("factura");
            XElement vendedor = new XElement("Vendedor", "Venta de meson");
            XElement cliente = new XElement("Cliente");
            cliente.Add(new XElement("RUT", textBoxRut.Text));
            cliente.Add(new XElement("Señores", textBoxSeñores.Text));
            cliente.Add(new XElement("Giro", textBoxGiro.Text));
            cliente.Add(new XElement("Direccion", textBoxDireccion.Text));
            cliente.Add(new XElement("Fono", textBoxTelefono.Text));
            int i = 0;
            foreach (ListViewItem item in listViewFactura.Items)
            {
                elementos = new XElement("Elemento" + (i++));
                elementos.Add(new XElement("Descripcion",item.SubItems[1].Text));
                elementos.Add(new XElement("Cantidad", item.Text));
                elementos.Add(new XElement("Precio", item.SubItems[2].Text));
                productos.Add(elementos);
            }
            factura.Add(data);
            factura.Add(vendedor);
            factura.Add(cliente);
            factura.Add(productos);
            if (padre.frecia.ingresarFacturaLiberada(factura))
            {
                MessageBox.Show("Factura Ingresada en forma exitosa");
            }
            else
            {
                MessageBox.Show("Error al ingresar factura, contactar a Javier");
            }
        }
        private bool imprimirFactura()
        {
            try
            {
                File.Delete("c:\\Inventario\\ultimaFacturaImpresa.xlsx");
            }
            catch { }
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\modeloFactura.xlsx");
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja"];
            xlHoja.Cells[2, 2] = DateTime.Now.Day;
            xlHoja.Cells[2, 4] = DateTime.Now.ToString("MMMM");
            xlHoja.Cells[2, 6] = DateTime.Now.Year.ToString().Split('0')[1];
            xlHoja.Cells[3, 3] = textBoxSeñores.Text;
            xlHoja.Cells[4, 3] = textBoxGiro.Text;
            xlHoja.Cells[5, 3] = textBoxDireccion.Text;
            xlHoja.Cells[6, 4] = textBoxCiudad.Text;
            xlHoja.Cells[3, 9] = textBoxRut.Text;
            xlHoja.Cells[4, 9] = "       " + textBoxCondicion.Text;
            xlHoja.Cells[5, 9] = textBoxTelefono.Text;
            xlHoja.Cells[6, 9] = "         " + textBoxDespacho.Text;
            xlHoja.Cells[6, 8] = "  "+textBoxOrdenCompra.Text;
            int item = 9, precioCorrespondiente = 0, diferencial = 0;
            foreach (ListViewItem producto in listViewFactura.Items)
            {
                xlHoja.Cells[item, 1] = producto.Text;
                xlHoja.Cells[item, 3] = producto.SubItems[1].Text;
                xlHoja.Cells[item, 9] = producto.SubItems[2].Text;
                precioCorrespondiente += Convert.ToInt32(new Numeros(Convert.ToInt32(Convert.ToDouble(producto.Text))).numeroSolo()) * Convert.ToInt32(new Numeros(producto.SubItems[2].Text).numeroSolo());
                diferencial += Convert.ToInt32(new Numeros(producto.SubItems[3].Text).numeroSolo());
                xlHoja.Cells[item++, 10] = "" + Convert.ToInt32((Convert.ToDouble(producto.Text) * Convert.ToInt32(new Numeros(producto.SubItems[2].Text).numeroSolo())));
                item++;
            }
            int notas =33 - listViewNotas.Items.Count;
            foreach (ListViewItem nota in listViewNotas.Items)
            {
                xlHoja.Cells[notas, 3] = nota.Text;
                notas++;
            }
            xlHoja.SaveAs("C:\\Inventario\\ultimaFacturaImpresa.xlsx");
            libro.PrintOut(1,1,1,false,misValue,false,false);
            libro.Close();
            excelapp.Quit();
            try
            {
                File.Delete("c:\\Inventario\\ultimaFacturaImpresa.xlsx");
            }
            catch { }
            return true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonAgregarProducto_Click(object sender, EventArgs e)
        {
            //agrega producto
            ListViewItem nuevo = new ListViewItem(textBoxProCant.Text);
            nuevo.SubItems.Add(textBoxDescripcion.Text + " -" + textBoxNP.Text);
            nuevo.SubItems.Add(new Numeros(textBoxPrecioUnitario.Text).numeroMiles());
            double total=Convert.ToInt32(new Numeros(textBoxPrecioUnitario.Text).numeroSolo())*Convert.ToDouble(textBoxProCant.Text);
            nuevo.SubItems.Add(new Numeros(Convert.ToInt32(total)).numeroMiles());
            listViewFactura.Items.Add(nuevo);
            recalcular();
            try
            {
                comboBoxDescripcion.SelectedText = "";
            }
            catch { }
            textBoxDescripcion.Text = "";
            textBoxNP.Text = "";
            textBoxPrecioUnitario.Text = "";
        }
        private void recalcular() 
        {
            int total = 0,neto=0,iba;
            foreach (ListViewItem items in listViewFactura.Items) 
            {
                neto += Convert.ToInt32(new Numeros(items.SubItems[3].Text).numeroSolo());
            }
            textBoxNeto.Text=""+new Numeros(neto).numeroMiles();
            iba=(neto*(19)/100);
            total=neto+iba;
            textBoxIVA.Text =""+new Numeros(iba).numeroMiles();
            textBoxTOTAL.Text=""+ new Numeros(total).numeroMiles();
        }
        private void FacturaLiberada_FormClosed(object sender, FormClosedEventArgs e)
        {
            padre.Show();
        }
        private void FacturaLiberada_Load(object sender, EventArgs e)
        {
        }
        private void comboBoxDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (string[] producto in productos)
            {
                if (comboBoxDescripcion.SelectedItem.ToString() == producto[8])
                {
                    textBoxDescripcion.Text = comboBoxDescripcion.SelectedItem.ToString();
                    textBoxProCant.Text = producto[4];
                    textBoxPrecioUnitario.Text = producto[7];
                    textBoxNP.Text = producto[0];
                    comboBoxNuParte.SelectedItem = producto[0];
                    //comboBoxNuParte.SelectedText = producto[0];
                    break;
                }
            }
        }

        private void comboBoxSeñores_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxSeñores.Text = comboBoxSeñores.SelectedItem.ToString();
            foreach (string[] cliente in clientes)
            {
                if (cliente[1] == comboBoxSeñores.SelectedItem.ToString())
                {
                    foreach (string rut in comboBoxRUT.Items)
                    {
                        if (rut == cliente[0])
                        {
                            comboBoxRUT.SelectedItem = rut;
                        }
                    }
                    textBoxGiro.Text = cliente[2];
                    textBoxDireccion.Text = cliente[4];
                    textBoxCiudad.Text = cliente[5];
                    textBoxTelefono.Text = cliente[6];
                }
            }
        }

        private void comboBoxRUT_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxRut.Text = comboBoxRUT.SelectedItem.ToString();
            foreach (string[] cliente in clientes)
            {
                if (cliente[0] == comboBoxRUT.SelectedItem.ToString())
                {
                    foreach (string rut in comboBoxSeñores.Items)
                    {
                        if (rut == cliente[1])
                        {
                            comboBoxSeñores.SelectedItem = rut;
                        }
                    }
                    textBoxGiro.Text = cliente[2];
                    textBoxDireccion.Text = cliente[4];
                    textBoxCiudad.Text = cliente[5];
                    textBoxTelefono.Text = cliente[6];
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBoxSeñores.Visible = true;
                textBoxRut.Visible = true;
            }
            else
            {
                textBoxSeñores.Visible = false;
                textBoxRut.Visible = false;
            }
        }

        private void textBoxRut_Leave(object sender, EventArgs e)
        {
            textBoxRut.Text = new Numeros(textBoxRut.Text).validarRut() ;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBoxNP.Visible = true;
                textBoxNP.Text = "";
                textBoxDescripcion.Text = "";
                textBoxProCant.Text = "1";
                textBoxPrecioUnitario.Text = "";
                textBoxDescripcion.Visible = true;
            }
            else
            {
                textBoxDescripcion.Text = "";
                textBoxProCant.Text = "1";
                textBoxPrecioUnitario.Text = "";
                textBoxDescripcion.Visible = false;
                textBoxNP.Visible = false;
            }
        }

        private void textBoxPrecioUnitario_Leave(object sender, EventArgs e)
        {
            textBoxPrecioUnitario.Text = new Numeros(textBoxPrecioUnitario.Text).numeroMiles();
        }

        private void textBoxDespacho_TextChanged(object sender, EventArgs e)
        {

        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewFactura.Items.Remove(listViewFactura.SelectedItems[0]);
            recalcular();
        }

        private void listViewFactura_Click(object sender, EventArgs e)
        {
        }

        private void listViewFactura_MouseClick(object sender, MouseEventArgs e)
        {
            if (listViewFactura.SelectedItems[0].Selected) { 
                if (e.Button.ToString()=="Right"){
                    contextMenuStrip1.Show((this.Location.X + e.Location.X)+20, (this.Location.Y + e.Location.Y)+120);
                }
            }
        }
        private void textBoxProCant_Leave(object sender, EventArgs e)
        {
            textBoxProCant.Text = textBoxProCant.Text.Replace('.', ',');
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text=="Mostrar Notas")
            {
                button1.Text = "Ocultar Notas";
                this.Size = new System.Drawing.Size(933, 635);
            }
            else if (button1.Text == "Ocultar Notas")
            {
                button1.Text = "Mostrar Notas";
                this.Size = new System.Drawing.Size(933, 436);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listViewNotas.Items.Add(textBoxNotas.Text);
            textBoxNotas.Text = "";
        }

        private void listViewNotas_MouseClick(object sender, MouseEventArgs e)
        {
            if (listViewNotas.SelectedItems[0].Selected)
            {
                if (e.Button.ToString() == "Right")
                {
                    contextMenuStrip2.Show((this.Location.X + e.Location.X) + 20, (this.Location.Y + e.Location.Y) + 440);
                }
            }
        }

        private void eliminarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            listViewNotas.Items.Remove(listViewNotas.SelectedItems[0]);
        }

        private void textBoxNotas_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                listViewNotas.Items.Add(textBoxNotas.Text);
                textBoxNotas.Text = "";
            }
        }

        private void comboBoxNuParte_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (string[] producto in productos)
            {
                if (comboBoxNuParte.SelectedItem.ToString() == producto[0])
                {
                    textBoxDescripcion.Text = producto[8];
                    textBoxProCant.Text = producto[4];
                    textBoxPrecioUnitario.Text = producto[7];
                    textBoxNP.Text = producto[0];
                    comboBoxDescripcion.SelectedItem = producto[8];
                    break;
                }
            }
        }
    }
}
