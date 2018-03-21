using System;
using System.IO;
using System.Xml;
using System.Net;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Inventario
{
    public partial class FormInventario : Form
    {
        moneda.CurrencyConvertorSoapClient internacional = new moneda.CurrencyConvertorSoapClient("CurrencyConvertorSoap");
        //public ServiceReference1.WebServiceSoapClient frecia = new ServiceReference1.WebServiceSoapClient(); //de pruebasd
        public ServiceReference1.ServiceSoapClient frecia = new ServiceReference1.ServiceSoapClient(); //de varelec
        public ServiceReference2.VarelecServiceClient fresia = new ServiceReference2.VarelecServiceClient();
        public Thread fresiaEstado;
        public int precioExtrangero = 0;
        public int moneda = 0;
        public string nombreUser;
        public int ventanaActual;
        public FormLogeo padre;
        public FormInventario(FormLogeo padre_)
        {
            padre = padre_;
            InitializeComponent();
            cargarVentanas();
            estadoServidor();
        }
        public void estadoServidor()
        {
            fresiaEstado = new Thread(new ThreadStart(status));
            fresiaEstado.Start();
        }
        public void status()
        {
            //ServiceReference1.WebServiceSoapClient server = new ServiceReference1.WebServiceSoapClient();
            ServiceReference1.ServiceSoapClient server = new ServiceReference1.ServiceSoapClient(); //de varelec
            while (true)
            {
                try
                {
                    fresiaEstatus(server.estado());
                }
                catch
                {
                    fresiaEstatus(0);
                }
                Thread.Sleep(3000);
            }
        }
        public void fresiaEstatus(int estado)
        {
            if (richTextBox1.InvokeRequired)
            {
                fresiaEstatusCallback d = new fresiaEstatusCallback(fresiaEstatus);
                this.Invoke(d, new object[] { estado });
            }
            else
            {
                if (estado==1)
                {
                    richTextBox1.Text = "ON";
                    richTextBox1.BackColor = Color.Green;
                }
                else
                {
                    richTextBox1.Text = "OFF";
                    richTextBox1.BackColor = Color.Red;
                }
            }
        }

        public delegate void fresiaEstatusCallback(int estado);
        public void cargarVentanas()
        {

            XmlDocument usuario = new XmlDocument();
            usuario.Load("ingreso.xml");
            XmlNode user = usuario.DocumentElement;
            nombreUser = user.FirstChild.InnerText;
            tabControlVentanas.SelectTab(6);
            ventanaActual = 6;
            tipoDeMenu(Convert.ToInt32(user.LastChild.InnerText));
            tamañoControlVentanas(1);
            cargarMemos();
            //cargarInventario();
        }
        public void cargarMemos()
        {
            listBoxMemos.Items.Clear();
            try
            {
                frecia.obtenerMemosGenrales().Save("memos.tmp");
                XmlDocument memo = new XmlDocument();
                memo.Load("memos.tmp");
                foreach (XmlNode nodo1 in memo.DocumentElement.ChildNodes)
               { 
                    listBoxMemos.Items.Add(nodo1.Name + "- " + nodo1.InnerText);
                    if (listBoxMemos.Items.Count > 22)
                    {
                        listBoxMemos.Items.Remove(listBoxMemos.Items[0]);
                    }
                }
            }
            catch
            {
                listBoxMemos.Items.Add("-.-");
            }
        }
        private void tipoDeMenu(int indice)
        {
            int y = 9;
            if (indice % 10 == 1)
            {
                buttonIngresarInventario.Visible = true;
                buttonIngresarInventario.Location = new Point(5, y);
                y += 29;
                buttonModInventario.Visible = true;
                buttonModInventario.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonVerInventario.Visible = true;
                buttonVerInventario.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonStock.Visible = true;
                buttonStock.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonCotizacion.Visible = true;
                buttonCotizacion.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonFactura.Visible = true;
                buttonFactura.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonUsuarios.Visible = true;
                buttonUsuarios.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonClientes.Visible = true;
                buttonClientes.Location = new Point(5, y);
                y += 29;
            }
            indice = indice / 10;
            if (indice % 10 == 1 || indice % 10 == 2)
            {
                buttonInicio.Visible = true;
                buttonInicio.Location = new Point(5, y);
                y += 29;
                if (indice % 10 == 2)
                {
                    buttonEnviarMemo.Enabled = true;
                }
                else
                {
                    buttonEnviarMemo.Enabled = false;
                }
            }
            if (nombreUser.Equals("mauricio") || nombreUser.Equals("DIOS") || nombreUser.Equals("javier"))
            {
                buttonExtras.Visible = true;
                buttonExtras.Location = new Point(5, y);
                y = +29;
            }
            indice = indice / 10;
        }
        private void tamañoControlVentanas(int indice)
        {
            this.Text = tabControlVentanas.SelectedTab.Text + " - " + nombreUser;
            if (indice == 1)
            {
                this.MinimumSize = new System.Drawing.Size(800, 515);
                this.Size = new System.Drawing.Size(800, 515);
            }
            else if (indice == 2)
            {
                this.MinimumSize = new System.Drawing.Size(1260, 515);
                this.Size = new System.Drawing.Size(1260, 515);
            }
            else if (indice == 3) // facturas
            {
                this.MinimumSize = new System.Drawing.Size(1180, 515);
                this.Size = new System.Drawing.Size(1180, 515);
            }
            else if (indice == 4) //cotizacion
            {
                this.MinimumSize = new System.Drawing.Size(1110, 630);
                this.Size = new System.Drawing.Size(1190, 630);
            }
            else if (indice == 5) // para los clientes
            {
                this.MinimumSize = new System.Drawing.Size(1200, 530);
                this.Size = new System.Drawing.Size(1200, 530);
            }
            else if(indice==6) //para ingresar inventario
            {
                this.MinimumSize = new System.Drawing.Size(1300, 788);
                this.Size = new System.Drawing.Size(1300, 788);
            }
         }
        private void buttonUsuarios_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(5);
            tamañoControlVentanas(1);
            cargarUsuarios();
        }
        private void cargarUsuarios()
        {

            fresia.getUsersXML().Save("usuarios.old");
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("usuarios.old");
            listViewUsuarios.Items.Clear();
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                ListViewItem user = new ListViewItem(nodo.FirstChild.InnerText);
                user.SubItems.Add(nodo.LastChild.InnerText);
                listViewUsuarios.Items.Add(user);
            }
        }
        private void buttonFactura_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(4);
            tamañoControlVentanas(3);
            if (analizarOFFICE()) // si esta office
            {
                checkBoxBusquedaEnFactura.Checked = false;
                panel6.Hide();
                panelFacturaUtil.Show();
                cargarIndexCotizaciones();
                matchingCombobox();
                checkBoxBusquedaEnFactura.Checked = true;
            }
            else //no office, es decir, no factura
            {
                panelFacturaUtil.Hide();
                panel6.Show();
            }
        }
        private void buttonCotizacion_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(3);
            tamañoControlVentanas(4);
            sumaTotalCotizacion();
            cargarClientesCotizacion();
            cargarFiltrosClientesCotizacion();
            cargarFiltrosInventarioFromTotal();
        }
        private void cargarClientesCotizacion()
        {
            cargarClientes();
        }
        private void buttonStock_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(2);
            tamañoControlVentanas(1);
            tamañoColumnasVerStock();
            cargarInventario();
            deInventarioAVerInventario();
        }
        private void tamañoColumnas()
        {
            int dif = 0;
            if (this.Size.Width - 1200 > 0)
            {
                dif = this.Size.Width - 1260;
            }
            listViewVerInventario.Columns[0].Width = 90;
            listViewVerInventario.Columns[1].Width = 70;
            listViewVerInventario.Columns[2].Width = 80;
            listViewVerInventario.Columns[3].Width = 260 + dif;
            listViewVerInventario.Columns[4].Width = 68;
            listViewVerInventario.Columns[5].Width = 50;
            listViewVerInventario.Columns[6].Width = 50;
            listViewVerInventario.Columns[7].Width = 60;
            listViewVerInventario.Columns[8].Width = 90;
            listViewVerInventario.Columns[9].Width = 80;
            listViewVerInventario.Columns[10].Width = 80;
            listViewVerInventario.Columns[11].Width = 60;
            listViewVerInventario.Columns[12].Width = 60;
        }
        private void tamañoColumnasMod()
        {
            int dif = 0;
            int alto = panel2.Size.Height, ancho = panel2.Size.Width;
            if (this.Size.Width - 1200 > 0)
            {
                dif = this.Size.Width - 1260;
                panel2.Size = new System.Drawing.Size(ancho + dif, alto);
            }
            listViewModInventario.Columns[0].Width = 90;
            listViewModInventario.Columns[1].Width = 70;
            listViewModInventario.Columns[2].Width = 80;
            listViewModInventario.Columns[3].Width = 260 + dif;
            listViewModInventario.Columns[4].Width = 61;
            listViewModInventario.Columns[5].Width = 60;
            listViewModInventario.Columns[6].Width = 50;
            listViewModInventario.Columns[7].Width = 60;
            listViewModInventario.Columns[8].Width = 90;
            listViewModInventario.Columns[9].Width = 80;
            listViewModInventario.Columns[10].Width = 80;
            listViewModInventario.Columns[11].Width = 60;
            listViewModInventario.Columns[12].Width = 60;
        }
        private void tamañoColumnasVerStock()
        {
            int dif = 0;
            if (panel5.Size.Width - 643 > 0)
            {
                dif = panel5.Size.Width - 643;
            }
            listViewVerStock.Columns[0].Width = 100;
            listViewVerStock.Columns[1].Width = 100;
            listViewVerStock.Columns[2].Width = 100;
            listViewVerStock.Columns[3].Width = 218 + dif;
            listViewVerStock.Columns[4].Width = 60;
            listViewVerStock.Columns[5].Width = 60;
            comboBoxVerStockDescrip.Width = 270 + dif;
        }
        private void buttonVerInventario_Click(object sender, EventArgs e)
        {
            bool hacerMatching = false;
            if (tabControlVentanas.SelectedIndex != 0)
            {
                hacerMatching = true;
            }
            tabControlVentanas.SelectTab(1);
            tamañoControlVentanas(2);
            tamañoColumnas();
            cargarInventario();
            if (hacerMatching)
            {
                foreach (ListViewItem item in listViewCotizacion.Items)
                {
                    foreach (ListViewItem item2 in listViewVerInventario.Items)
                    {
                        if (item.SubItems[1].Text.Equals(item2.SubItems[2].Text))
                        {
                            item2.BackColor = Color.Green;
                            break;
                        }
                    }
                }
            }
        }
        private void cargarInventario()
        {
            listViewVerInventario.Items.Clear();
            listViewTOTALinventario.Items.Clear();
            fresia.getInventarioXML().Save("items.xml");
            XmlDocument lista = new XmlDocument();
            lista.Load("items.xml");
            foreach (XmlNode nodo in lista.DocumentElement.ChildNodes)
            {
                ListViewItem item = new ListViewItem();
                for (int i = 0; i < 15; i++)//iteracion para crear los 15 sub items de un producto
                {
                    item.SubItems.Add(""); 
                }
                string signoMoneda = "€";
                int moneda = 0;
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "Familia") //familia del producto
                    {
                        item.Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Codigo") //N°P
                    {
                        item.SubItems[2].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Origen") // origen contiene signo moneda - fecha - costo importe - precio moneda - ultimaFactura
                    {
                        signoMoneda = nodo2.InnerText.Split('¬')[0];
                        moneda=Convert.ToInt32(nodo2.InnerText.Split('¬')[3]);
                        item.SubItems[15].Text=nodo2.InnerText.Split('¬')[1];
                        item.SubItems[7].Text=signoMoneda+Math.Round((Convert.ToDouble(nodo2.InnerText.Split('¬')[2])/100),2);
                        item.SubItems[14].Text=nodo2.InnerText.Split('¬')[4];
                    }
                    else if (nodo2.Name == "Grupo")
                    {
                        item.SubItems[1].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Cant")
                    {
                        item.SubItems[5].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "CostoUnitEuto")
                    {
                        item.SubItems[6].Text = signoMoneda + Math.Round((Convert.ToDouble(nodo2.InnerText) / 100), 2);
                    }
                    else if (nodo2.Name == "CostoUnitPeso")//peso kilogramos del producto
                    {
                        item.SubItems[4].Text = new Numeros(""+Math.Round(((Convert.ToDouble(nodo2.InnerText))/100),2)).numeroSolo();
                    }
                    else if (nodo2.Name == "PVP") 
                    {
                        item.SubItems[11].Text = "$" + new Numeros(nodo2.InnerText).numeroMiles();
                    }
                    else if (nodo2.Name == "Descripcion")
                    {
                        item.SubItems[3].Text = nodo2.InnerText;
                    }
                }
                item.SubItems[8].Text = signoMoneda + Math.Round((Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo()) * Convert.ToInt32(item.SubItems[5].Text)), 2);
                item.SubItems[9].Text = "$" + new Numeros(Convert.ToInt32((moneda * Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo())))).numeroMiles();
                item.SubItems[10].Text="$" + new Numeros(Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo())* Convert.ToInt32(item.SubItems[5].Text)).numeroMiles();
                item.SubItems[12].Text = "" + (Convert.ToInt32(new Numeros(item.SubItems[11].Text).numeroSolo()) - Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()));
                double numero = ((Convert.ToDouble(item.SubItems[12].Text) * 100) / Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()));
                item.SubItems[13].Text = (Math.Round(numero, 1)) + "%";
                item.SubItems[12].Text = "$"+new Numeros(item.SubItems[12].Text).numeroMiles();
                listViewVerInventario.Items.Add(item);
                listViewTOTALinventario.Items.Add((ListViewItem)item.Clone());
            }
            cargarFiltros();
        }
        private void buttonModInventario_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(0);
            tamañoControlVentanas(2);
            tamañoColumnasMod();
            deInventarioAModificar();
            Thread precio = new Thread(new ThreadStart(server));
            precio.Start();
            cargarTotalInventario();
        }
        public void cargarTotalInventario()
        {
            frecia.indicadoresEconomicos().Save("Indicadores.old");
            XmlDocument indicadores = new XmlDocument();
            indicadores.Load("Indicadores.old");
            foreach (XmlNode nodo in indicadores.DocumentElement.ChildNodes)
            {
                if (nodo.Name == "Euro")
                {
                    textBoxIngresarFacturaDolarRecomendado.Text = nodo.InnerText;
                }
            }
            int euro = Convert.ToInt32(textBoxIngresarFacturaDolarRecomendado.Text);
            double numero = 0;
            foreach (ListViewItem item in listViewModInventario.Items)
            {
                if (item.SubItems[8].Text.Contains("€"))
                {
                    numero += Math.Round((Convert.ToDouble(new Numeros(item.SubItems[8].Text).numeroSolo())),2);
                }
            }
            int cantidad = Convert.ToInt32(Math.Round(numero, 0));
            textBoxTotalEUROinventario.Text = "€" + new Numeros(cantidad).numeroMiles();
        }
        public void server()
        {
            double cambio = 0;
            try
            {
                cambio = internacional.ConversionRate(Inventario.moneda.Currency.EUR, Inventario.moneda.Currency.CLP);
            }
            catch { }
            precioDivisasEuro(cambio);
            cambio = 0;
            try
            {
                cambio = internacional.ConversionRate(Inventario.moneda.Currency.USD, Inventario.moneda.Currency.CLP);
            }
            catch { }
            precioDivisasDolar(cambio);
        }
        public void precioDivisasEuro(double cambio)
        {
            if (textBoxIngresarFacturaEuroRecomendado.InvokeRequired)
            {
                precioDivisasEuroCallback d = new precioDivisasEuroCallback(precioDivisasEuro);
                this.Invoke(d, new object[] { cambio });
            }
            else
            {
                textBoxIngresarFacturaEuroRecomendado.Text = "" + Math.Round(cambio, 0);
            }
        }
        public delegate void precioDivisasEuroCallback(double cambio);
        public void precioDivisasDolar(double cambio)
        {
            if (textBoxIngresarFacturaDolarRecomendado.InvokeRequired)
            {
                precioDivisasDolarCallback d = new precioDivisasDolarCallback(precioDivisasDolar);
                this.Invoke(d, new object[] { cambio });
            }
            else
            {
                textBoxIngresarFacturaDolarRecomendado.Text = "" + Math.Round(cambio, 0);
            }
        }
        public delegate void precioDivisasDolarCallback(double cambio);
        private void buttonInicio_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(6);
            tamañoControlVentanas(1);
            cargarMemos();
        }
        private void tamañoCambio(object sender, EventArgs e)
        {
        }
        private void buttonClientes_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(7);
            tamañoControlVentanas(5);
            cargarClientes();
            cargarFiltrosClientesClientes();
            filtrosEnNadaClientesClientes();
        }
        private void button14_Click(object sender, EventArgs e)
        {
            bool funciona = false;
            try
            {
                funciona = frecia.escribirMemo(nombreUser, textBox5.Text);
            }
            catch
            {
                funciona = false;
            }
            if (funciona)
            {
                textBox5.Text = "";
                cargarMemos();
            }
            else
            {
                textBox5.Text = "No funciona";
            }
        }
        private void clickModifcaOElimina(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString().Equals("Left"))
            {
                textBoxCotizacionELementoCotizado.Visible = true;
                textBoxCotizacionELementoCotizado.Enabled = false;
                textBoxCotizacionELementoCotizado.Text = listViewCotizacion.SelectedItems[0].Text;
                textBoxCotizacionNoInventarioNuParte.Text = listViewCotizacion.SelectedItems[0].SubItems[1].Text;
                textBoxCotizacionNoInventarioCant.Text = listViewCotizacion.SelectedItems[0].SubItems[2].Text;
                textBoxCotizacionNoInventarioValor.Text = listViewCotizacion.SelectedItems[0].SubItems[3].Text;
                textBoxCotizacionNoInventarioDescuento.Text = listViewCotizacion.SelectedItems[0].SubItems[4].Text;
                textBoxCotizacionNoInventarioValorTotal.Text = listViewCotizacion.SelectedItems[0].SubItems[5].Text.Split('$')[1];
                textBoxCotizacionNoInventarioPlazoEntrega.Text = listViewCotizacion.SelectedItems[0].SubItems[6].Text;
                listViewCotizacion.SelectedItems[0].Selected = false;
            }
        }
        private void sumaTotalCotizacion()
        {
            try
            {
                bool sale = true;
                double descuento = 0;
                Numeros precio = new Numeros();
                foreach (ListViewItem item in listViewCotizacion.Items)
                {
                    //sub item 2 cantidad, 3 precio unidad, 4 descuento 5 total, 6 entrega
                    if (item.SubItems[5].Text != "")
                    {
                        precio = new Numeros(item.SubItems[5].Text);
                        descuento += Convert.ToDouble(precio.numeroSolo());
                    }
                    else
                    {
                        sale = false;
                    }
                }
                precio = new Numeros("" + Math.Round(descuento, 0));
                textBoxCotiTotal.Text = "$" + precio.numeroMiles();
                precio = new Numeros("" + Math.Round(descuento * 0.19, 0));
                textBoxCotiIva.Text = "$" + precio.numeroMiles();
                precio = new Numeros("" + Math.Round(descuento * 1.19, 0));
                textBoxCotiToalConIva.Text = "$" + precio.numeroMiles();
                if (sale)
                {
                    buttonCotizacionRealizar.Enabled = true;
                }
                else
                {
                    buttonCotizacionRealizar.Enabled = false;
                }
            }
            catch { }
        }
        private void buttonCotizacionRealizar_Click(object sender, EventArgs e)
        {
            int numero = 0;
            bool guardado = false;
            XElement cotizacion = new XElement("cotizacion");
            try
            {
                int i = 0;
                XElement vendedor = new XElement("Vendedor", nombreUser), cliente = new XElement("Cliente"), productos = new XElement("Productos"), data = new XElement("MetaData"), elementos, condiciones = new XElement("Condiciones");
                data.Add(new XElement("Fecha", DateTime.Now.Date.Day + "/" + DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Year));
                cliente.Add(new XElement("RUT", comboBoxCotizacionRUT.SelectedItem));
                cliente.Add(new XElement("Señores", comboBoxCotizacionSeñores.SelectedItem));
                cliente.Add(new XElement("Giro", textBoxCotizacionGiro.Text));
                cliente.Add(new XElement("Direccion", textBoxCotizacionDireccion.Text));
                cliente.Add(new XElement("Fono", comboBoxCotizacionFono.SelectedItem));
                cliente.Add(new XElement("Contacto", textBoxCotiContacto.Text));
                cliente.Add(new XElement("Correo", textBoxCotiCorreo.Text));
                foreach (ListViewItem item in listViewCotizacion.Items)
                {
                    elementos = new XElement("Elemento" + (i++));
                    elementos.Add(new XElement("Descripcion", item.Text.ToString() + " - " + item.SubItems[1].Text));
                    elementos.Add(new XElement("cantidad", item.SubItems[2].Text.Split('-')[0]));
                    elementos.Add(new XElement("Precio", item.SubItems[3].Text));
                    elementos.Add(new XElement("valorTotal", item.SubItems[5].Text));
                    elementos.Add(new XElement("PlazoEntrega", item.SubItems[6].Text));
                    productos.Add(elementos);
                }
                i = 0;
                foreach (ListViewItem item in listViewCondicionesGenerales.Items)
                {
                    condiciones.Add(new XElement("condiciones" + (i++), item.Text + " " + item.SubItems[1].Text));
                }
                condiciones.Add(new XElement("condiciones" + i++, "Validez de la Cotización: " + textBoxCotizacion.Text));
                //se agregan al xml
                cotizacion.Add(data);
                cotizacion.Add(vendedor);
                cotizacion.Add(cliente);
                cotizacion.Add(productos);
                cotizacion.Add(condiciones);
                numero = frecia.guardarCotizacion(cotizacion);
                MessageBox.Show("Cotizacion Numero: " + numero); // es guardado en el servidor
                guardado = true;
            }
            catch
            {
                MessageBox.Show("Servidor Desconectado");
            }
            if (guardado)
            {
                cotizacionAPDF(cotizacion, numero); // la hace pdf y la deja en escritorio
            }
        }
        public void cotizacionAPDF(XElement cotizacion, int numero)
        {
            if (analizarOFFICE())
            {
                cotizacionAPDF nueva = new Inventario.cotizacionAPDF(cotizacion, numero, nombreUser);
                if (nueva.imprimir() == false)
                {
                    MessageBox.Show("PDF no ha podido ser generado");
                }
            }
            else
            {
                MessageBox.Show("imposible conectar con Office 2010 para generar PDF");
            }
        }
        private void textBoxCotizacion_TextChanged(object sender, EventArgs e)
        {
            textBoxCotizacion.Text += " Dias";
        }
        private bool analizarOFFICE()
        {
            if (File.Exists("C:\\Program Files\\Microsoft Office\\Office14\\EXCEL.exe"))//para windows 7 
            {
                return true;
            }
            else if (File.Exists("C:\\Program Files(x86)\\Microsoft Office\\Office14\\EXCEL.exe")) // win 7 64 bits
            {
                return true;
            }
            else if (File.Exists("C:\\Archivos de programa\\Microsoft Office\\Office14\\EXCEL.exe"))
            { // para windows xp
                return true;
            }
            else if (File.Exists("C:\\Archivos de programa (x86)\\Microsoft Office\\Office14\\EXCEL.exe"))// para windows xp 64 bits
            {
                return true;
            }
            return false;
        }
        private void cargarIndexCotizaciones()
        {
            comboBoxFacturaCliente.Items.Clear();
            comboBoxFacturaVendedor.Items.Clear();
            comboBoxFacturaFecha.Items.Clear();
            listViewCotizaciones.Items.Clear();
            listViewTOTALintermedio.Items.Clear();
            frecia.listaCotizaciones().Save("indexCoti.xml");
            XmlDocument cotizaciones = new XmlDocument();
            cotizaciones.Load("indexCoti.xml");
            List<string> vendedores = new List<string>();
            List<string> clientes = new List<string>();
            List<string> fecha = new List<string>();
            vendedores.Add("---");
            clientes.Add("---");
            fecha.Add("---");
            comboBoxFacturaVendedor.Items.Add("---");
            comboBoxFacturaCliente.Items.Add("---");
            comboBoxFacturaFecha.Items.Add("---");
            foreach (XmlNode datos in cotizaciones.DocumentElement.ChildNodes)
            {
                ListViewItem nuevo = new ListViewItem();
                nuevo.SubItems.Add("");
                nuevo.SubItems.Add("");
                nuevo.SubItems.Add("");
                foreach (XmlNode adentro in datos.ChildNodes)
                {
                    if (adentro.Name == "Vendedor")
                    {
                        nuevo.SubItems[1].Text = adentro.InnerText;
                        if (vendedores.Contains(adentro.InnerText) == false)
                        {
                            comboBoxFacturaVendedor.Items.Add(adentro.InnerText);
                            vendedores.Add(adentro.InnerText);
                        }
                    }
                    else if (adentro.Name == "Cliente")
                    {
                        nuevo.SubItems[2].Text = adentro.InnerText;
                        if (clientes.Contains(adentro.InnerText) == false)
                        {
                            comboBoxFacturaCliente.Items.Add(adentro.InnerText);
                            clientes.Add(adentro.InnerText);
                        }
                    }
                    else if (adentro.Name == "MetaData")
                    {
                        nuevo.SubItems[3].Text = adentro.InnerText;
                        if (fecha.Contains(adentro.InnerText) == false)
                        {
                            comboBoxFacturaFecha.Items.Add(adentro.InnerText);
                            fecha.Add(adentro.InnerText);
                        }
                    }
                    else if (adentro.Name == "Cotizacion")
                    {
                        nuevo.Text = adentro.InnerText;
                    }
                }
                listViewCotizaciones.Items.Add(nuevo);
                listViewTOTALintermedio.Items.Add((ListViewItem)nuevo.Clone());
            }
            comboBoxFacturaVendedor.SelectedItem = "---";
            comboBoxFacturaCliente.SelectedItem = "---";
            comboBoxFacturaFecha.SelectedItem = "---";
        }
        private void matchingCombobox()
        {
            if (checkBoxBusquedaEnFactura.Checked == true) {
                checkBoxBusquedaEnFactura.Checked = false;
                listViewCotizaciones.Items.Clear();
                foreach(ListViewItem item in listViewTOTALintermedio.Items){
                    listViewCotizaciones.Items.Add((ListViewItem) item.Clone());
                }
                if (comboBoxFacturaVendedor.SelectedItem != "---")
                {
                    foreach (ListViewItem item in listViewCotizaciones.Items)
                    {
                        if (item.Text.Equals(comboBoxFacturaVendedor.SelectedItem)==false)
                        {
                            item.Remove();
                        }
                    }
                }
                if (comboBoxFacturaCliente.SelectedItem != "---")
                {
                    foreach (ListViewItem item in listViewCotizaciones.Items)
                    {
                        if (item.SubItems[1].Text.Equals(comboBoxFacturaCliente.SelectedItem) == false)
                        {
                            item.Remove();
                        }
                    }
                }
                if (comboBoxFacturaFecha.SelectedItem != "---")
                {
                    foreach (ListViewItem item in listViewCotizaciones.Items)
                    {
                        if (item.SubItems[2].Text.Equals(comboBoxFacturaFecha.SelectedItem) == false)
                        {
                            item.Remove();
                        }
                    }
                }
                checkBoxBusquedaEnFactura.Checked = true;
            }
        }
        private void comboBoxFacturaVendedor_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            matchingCombobox();
        }
        private void comboBoxFacturaCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            matchingCombobox();
        }
        private void comboBoxFacturaFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            matchingCombobox();
        }
        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                File.Delete("c:\\Inventario\\ultimaFacturaImpresa.xlsx");
            }
            catch { }
            int monto = 0;
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\inventario\\modeloFactura.xlsx");
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja"];
            xlHoja.Cells[2, 2] = DateTime.Now.Day;
            xlHoja.Cells[2, 4] = DateTime.Now.Month;
            xlHoja.Cells[2, 6] = DateTime.Now.Year.ToString().Split('0')[1];
            //poner cliente;
            cargarClientes();
            ListViewItem cliente = new ListViewItem();
            cliente.SubItems.Add("");
            cliente.SubItems.Add("");
            cliente.SubItems.Add("");
            cliente.SubItems.Add("");
            cliente.SubItems.Add("");
            cliente.SubItems.Add("");
            foreach (ListViewItem itemC in listViewClientes.Items)
            {
                if (itemC.Text.Equals(comboBoxFacturaCliente.SelectedItem.ToString()))
                {
                    cliente = (ListViewItem)itemC.Clone();
                    break;
                }
            }
            xlHoja.Cells[3, 3] = cliente.SubItems[1].Text;
            xlHoja.Cells[4, 3] = cliente.SubItems[2].Text;
            xlHoja.Cells[5, 3] = cliente.SubItems[3].Text;
            xlHoja.Cells[6, 4] = cliente.SubItems[4].Text + " - " + cliente.SubItems[5].Text;
            xlHoja.Cells[3, 9] = cliente.Text;
            xlHoja.Cells[4, 9] = "       " + textBoxCondicion.Text;
            xlHoja.Cells[5, 9] = cliente.SubItems[6].Text;
            xlHoja.Cells[6, 9] = "         " + textBoxDespacho.Text;
            xlHoja.Cells[6, 8] = textBoxOrdenCompra.Text;

            //hasta aca el poner cliente;
            int item = 9, precioCorrespondiente = 0, diferencial = 0;
            foreach (ListViewItem producto in listViewFactura.Items)
            {
                int cantidad = Convert.ToInt32(producto.Text);
                string id = producto.SubItems[1].Text.Split('-')[1];
                xlHoja.Cells[item, 1] = producto.Text;
                xlHoja.Cells[item, 3] = producto.SubItems[1].Text;
                xlHoja.Cells[item, 9] = producto.SubItems[2].Text;
                monto += Convert.ToInt32(new Numeros(producto.SubItems[2].Text).numeroSolo());
                precioCorrespondiente += Convert.ToInt32(new Numeros(producto.Text).numeroSolo()) * Convert.ToInt32(new Numeros(producto.SubItems[2].Text).numeroSolo());
                diferencial += Convert.ToInt32(new Numeros(producto.SubItems[3].Text).numeroSolo());
                xlHoja.Cells[item++, 10] = "" + Convert.ToInt32(new Numeros(producto.Text).numeroSolo()) * Convert.ToInt32(new Numeros(producto.SubItems[2].Text).numeroSolo());
                item++;
            }
            descontarInventario();
            precioCorrespondiente -= diferencial;
            xlHoja.Cells[36, 3] = "Descuento";
            xlHoja.Cells[36, 10] = "" + (-1 * precioCorrespondiente);
            xlHoja.SaveAs("c:\\Inventario\\ultimaFacturaImpresa.xlsx");
            libro.PrintOut(1, 1, 1, false, misValue, false, false);
            buttonFacturaImprimir.Enabled = false;
            string ruta = frecia.ingresarFacturaPorCotizacion(Convert.ToInt32(listViewCotizaciones.SelectedItems[0].Text));
            string[] datos = { "---", DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year, "" + monto, ruta };
            try
            {
                File.Delete("c:\\Inventario\\ultimaFacturaImpresa.xlsx");
            }
            catch { }
            libro.Close();
            excelapp.Quit();
            listViewFactura.Items.Clear();
            if (ruta == "0")
            {
                MessageBox.Show("Error al ingresar factura, contacte a Javier");
            }
            else
            {
                MessageBox.Show("factura ingresada en forma correcta");
            }
            comboBoxFacturaCliente.Items.Clear();
            comboBoxFacturaFecha.Items.Clear();
            comboBoxFacturaVendedor.Items.Clear();
            //recargo la pag de facturas
            checkBoxBusquedaEnFactura.Checked = false;
            cargarIndexCotizaciones();
            matchingCombobox();
            checkBoxBusquedaEnFactura.Checked = true;
        }
        public void descontarInventario()
        {
            List<string[]> productos = new List<string[]>();
            fresia.getInventarioXML().Save("inventario.xml.ftr");
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
            foreach (ListViewItem item in listViewFactura.Items)
            {
                foreach (string[] producto in productos)
                {
                    if (producto[0] == item.SubItems[1].Text.Split('-')[1])
                    {
                        producto[4] = "" + (Convert.ToInt32(producto[4]) - Convert.ToInt32(item.Text));
                        fresia.editarProducto(producto);
                        if (frecia.ingresoPMP(producto[0], 0, 0, Convert.ToInt32(producto[4]), Convert.ToInt32(producto[1].Split('¬')[2]), Convert.ToInt32(producto[5]), producto[1].Split('¬')[0]) == false)
                        {
                            MessageBox.Show("error al ingresar a ficha PMP");
                        }
                    }
                }
            }
        }
        public void deInventarioAModificar()
        {
            listViewModInventario.Items.Clear();
            foreach (ListViewItem item in listViewTOTALinventario.Items)
           { 
                ListViewItem nuevo = new ListViewItem(item.Text);
                for (int i = 1; i <= 13; i++)
                {
                    nuevo.SubItems.Add(item.SubItems[i].Text);
                }
                listViewModInventario.Items.Add(nuevo);
            }
        }
        public void cargarFiltros()
        {
            List<string> familia = new List<string>();
            List<string> grupo = new List<string>();
            List<string> nuParte = new List<string>();
            List<string> descripcion = new List<string>();
            familia.Add("---");
            grupo.Add("---");
            nuParte.Add("---");
            descripcion.Add("---");
            foreach (ListViewItem item in listViewVerInventario.Items)
            {
                if (familia.Contains(item.Text) == false)
                {
                    familia.Add(item.Text);
                }
                if (grupo.Contains(item.SubItems[1].Text) == false)
                {
                    grupo.Add(item.SubItems[1].Text);
                }
                if (nuParte.Contains(item.SubItems[2].Text) == false)
                {
                    nuParte.Add(item.SubItems[2].Text);
                }
                if (descripcion.Contains(item.SubItems[3].Text) == false)
                {
                    descripcion.Add(item.SubItems[3].Text);
                }
            }
            comboBoxModInventarioFamilia.Items.Clear();
            comboBoxVerInventarioFamilia.Items.Clear();
            comboBoxVerStockFamilia.Items.Clear();
            comboBoxModInventarioGrupo.Items.Clear();
            comboBoxVerInventarioGrupo.Items.Clear();
            comboBoxVerStockGrupo.Items.Clear();
            comboBoxModInventarioNuParte.Items.Clear();
            comboBoxVerInventarioNP.Items.Clear();
            comboBoxVerStockNP.Items.Clear();
            comboBoxModInvetarioDescripcion.Items.Clear();
            comboBoxVerInventarioDescripcion.Items.Clear();
            comboBoxVerStockDescrip.Items.Clear();
            foreach (string palabra in familia)
            {
                comboBoxModInventarioFamilia.Items.Add(palabra);
                comboBoxVerInventarioFamilia.Items.Add(palabra);
                comboBoxVerStockFamilia.Items.Add(palabra);
            }
            comboBoxModInventarioFamilia.SelectedItem = "---";
            comboBoxVerInventarioFamilia.SelectedItem = "---";
            comboBoxVerStockFamilia.SelectedItem = "---";
            foreach (string palabra in grupo)
            {
                comboBoxModInventarioGrupo.Items.Add(palabra);
                comboBoxVerInventarioGrupo.Items.Add(palabra);
                comboBoxVerStockGrupo.Items.Add(palabra);
            }
            comboBoxModInventarioGrupo.SelectedItem = "---";
            comboBoxVerInventarioGrupo.SelectedItem = "---";
            comboBoxVerStockGrupo.SelectedItem = "---";
            foreach (string palabra in nuParte)
            {
                comboBoxModInventarioNuParte.Items.Add(palabra);
                comboBoxVerInventarioNP.Items.Add(palabra);
                comboBoxVerStockNP.Items.Add(palabra);
            }
            comboBoxModInventarioNuParte.SelectedItem = "---";
            comboBoxVerInventarioNP.SelectedItem = "---";
            comboBoxVerStockNP.SelectedItem = "---";
            foreach (string palabra in descripcion)
            {
                comboBoxModInvetarioDescripcion.Items.Add(palabra);
                comboBoxVerInventarioDescripcion.Items.Add(palabra);
                comboBoxVerStockDescrip.Items.Add(palabra);
            }
            comboBoxModInvetarioDescripcion.SelectedItem = "---";
            comboBoxVerInventarioDescripcion.SelectedItem = "---";
            comboBoxVerStockDescrip.SelectedItem = "---";
        }
        private void seleccionarModInventario(object sender, MouseEventArgs e)
        {
            double imporE=1;
            frecia.indicadoresEconomicos().Save("Indicadores.old");
            XmlDocument indicadores = new XmlDocument();
            indicadores.Load("Indicadores.old");
            foreach (XmlNode nodo in indicadores.DocumentElement.ChildNodes)
            {
                if (nodo.Name == "Importe")
                {
                    imporE = Convert.ToDouble(nodo.InnerText);
                }
            }
            if (e.Button.ToString() == "Left")
            {
                try
                {
                    textBoxModInventNuParte.Text = listViewModInventario.SelectedItems[0].SubItems[2].Text;
                    textBoxModInventDescripcion.Text = listViewModInventario.SelectedItems[0].SubItems[3].Text;
                    textBoxModInventPesoKilos.Text = listViewModInventario.SelectedItems[0].SubItems[4].Text;
                    Numeros precio = new Numeros(listViewModInventario.SelectedItems[0].SubItems[6].Text);
                    textBoxModInventCostoE.Text = precio.numeroSolo();
                    textBoxModInventarioFlete.Text = ""+Math.Round(Convert.ToDouble(new Numeros(listViewModInventario.SelectedItems[0].SubItems[7].Text).numeroSolo()) - (Convert.ToDouble(textBoxModInventCostoE.Text) / imporE), 2);
                    textBoxModInventCant.Text = listViewModInventario.SelectedItems[0].SubItems[5].Text;
                    textBoxModInvtPVP.Text = listViewModInventario.SelectedItems[0].SubItems[11].Text;
                }
                catch
                {
                    MessageBox.Show("Error llamar al 911");
                    MessageBox.Show("Mentira... llame a javier 99402264");
                }
            }
        }
        private void buttonModificarRealizar_Click(object sender, EventArgs e)
        {
            double importE=1;
            List<string[]> productos = new List<string[]>();
            fresia.getInventarioXML().Save("inventario.xml.ftr");
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
            frecia.indicadoresEconomicos().Save("Indicadores.old");
            XmlDocument indicadores = new XmlDocument();
            indicadores.Load("Indicadores.old");
            foreach (XmlNode nodo in indicadores.DocumentElement.ChildNodes)
            {
                if (nodo.Name == "Importe")
                {
                    importE = Convert.ToDouble(nodo.InnerText);
                }
            }
            foreach (string[] producto in productos)
            {
                if (producto[0].Equals(textBoxModInventNuParte.Text))
                {
                    producto[4] = textBoxModInventCant.Text;
                    producto[5] = "" + Math.Round((Convert.ToDouble(new Numeros(textBoxModInventCostoE.Text).numeroSolo()) * 100), 0);
                    producto[6] = "" + Math.Round((Convert.ToDouble(new Numeros(textBoxModInventPesoKilos.Text).numeroSolo()) * 100), 0);
                    producto[7] = new Numeros(textBoxModInvtPVP.Text).numeroSolo();
                    producto[8] = textBoxModInventDescripcion.Text;
                    double costoImporte;
                    costoImporte = Convert.ToDouble(producto[5]) / importE;
                    if (producto[1].Split('¬')[0].Equals("$") == false)
                    {
                        costoImporte += (Convert.ToDouble(producto[6]) * 7);
                    }
                    else
                    {
                        costoImporte += (Convert.ToDouble(textBoxModInventarioFlete.Text) * 100);
                    }
                    string nuevoComplejo = producto[1].Split('¬')[0] + "¬" + producto[1].Split('¬')[1] + "¬" + Math.Round(costoImporte, 0) + "¬" + producto[1].Split('¬')[3] + "¬" + producto[1].Split('¬')[4];
                    producto[1] = nuevoComplejo;
                    fresia.editarProducto(producto);
                    if (frecia.ingresoPMP(producto[0], 0, 0, Convert.ToInt32(producto[4]), Convert.ToInt32(producto[1].Split('¬')[2]), Convert.ToInt32(producto[5]), producto[1].Split('¬')[0]) == false)
                    {
                        MessageBox.Show("error al ingresar a ficha PMP");
                    }
                    break;
                }
            }
            cargarInventario();
            deInventarioAModificar();
        }
        private void deInventarioAVerInventario()
        {
            listViewTOTALintermedio.Items.Clear();
            listViewVerStock.Items.Clear();
            foreach (ListViewItem item in listViewVerInventario.Items)
            {
                ListViewItem nuevo = new ListViewItem(item.Text);
                nuevo.SubItems.Add(item.SubItems[1].Text);
                nuevo.SubItems.Add(item.SubItems[2].Text);
                nuevo.SubItems.Add(item.SubItems[3].Text);
                nuevo.SubItems.Add(item.SubItems[5].Text);
                nuevo.SubItems.Add(item.SubItems[11].Text);
                listViewVerStock.Items.Add(nuevo);
                listViewTOTALintermedio.Items.Add((ListViewItem)nuevo.Clone());
            }
        }
        private string obtenerNumeroParte(string entrada)
        {
            string salida = "";
            if (entrada == "Linde" || entrada == "Otras Marcas" || entrada == "Motores")
            {
                salida = textBoxIngresarFacturaNumeroParte.Text;
            }
            else
            {
                if (entrada == "Accesorios")
                {
                    salida = "A";
                }
                else if (entrada == "Baterias")
                {
                    salida = "B";
                }
                else if (entrada == "Cargadores")
                {
                    salida = "C";
                }
                else if (entrada == "Contactores")
                {
                    salida = "K";
                }
                else if (entrada == "Controladores")
                {
                    salida = "V";
                }
                else if (entrada == "Equipos")
                {
                    salida = "E";
                }
                else if (entrada == "Fusibles")
                {
                    salida = "F";
                }
                else if (entrada == "Repuestos")
                {
                    salida = "R";
                }
                int i = 1;
                foreach (ListViewItem item in listViewTOTALinventario.Items)
                {
                    if (item.Text == entrada)
                    {
                        i++;
                    }
                }
                if (i < 10)
                {
                    salida += "00000000" + i;
                }
                else if (i < 100)
                {
                    salida += "0000000" + i;
                }
                else if (i < 1000)
                {
                    salida += "000000" + i;
                }
                else if (i < 10000)
                {
                    salida += "00000" + i;
                }
                else if (i < 100000)
                {
                    salida += "0000" + i;
                }
                else if (i < 1000000)
                {
                    salida += "000" + i;
                }
                else if (i < 10000000)
                {
                    salida += "00" + i;
                }
            }
            MessageBox.Show("numero de parte es: "+salida);
            return salida;
        }
        private void matchearModInventario()
        {
            listViewModInventario.Items.Clear();
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                listViewModInventario.Items.Add((ListViewItem)item.Clone());
            }
        }
        private void comboBoxModInventarioFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            matchearModInventario();
            if (comboBoxModInventarioFamilia.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewModInventario.Items)
                {
                    if (item.Text != comboBoxModInventarioFamilia.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void buttonClientesRealizar_Click(object sender, EventArgs e)
        {
            string[] cliente = { textBoxClientesRUT.Text, textBoxClientesSeñores.Text };
            string[] datosCliente = { textBoxClientesRUT.Text, textBoxClientesSeñores.Text, textBoxClientesGiro.Text, textBoxClientesComuna.Text, textBoxClientesDireccion.Text, textBoxClientesCiudad.Text, textBoxClientesFono.Text, "" };
            //RUT,NOMBRE,GIRO,CONTACTO,DIRECCION,CIUDAD,FONO,CORREO
            if (radioButtonClienteAgregar.Checked)
            {
                fresia.crearCliente(datosCliente);
            }
            else if (radioButtonClienteModificar.Checked)
            {
                fresia.editarCliente(datosCliente);
            }
            else
            {
                fresia.eliminarCliente(textBoxClientesRUT.Text);
            }
            textBoxClientesRUT.Text = "";
            textBoxClientesSeñores.Text = "";
            textBoxClientesGiro.Text = "";
            textBoxClientesComuna.Text = "";
            textBoxClientesDireccion.Text = "";
            textBoxClientesCiudad.Text = "";
            textBoxClientesFono.Text = "";
            textBoxClientesComuna.Text = "";
            cargarClientes();
        }
        private void cargarClientes()
        {
            fresia.getClientesXML().Save("clientes.OLD");
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("Clientes.OLD");
            listViewTOTALClientes.Items.Clear();
            listViewClientes.Items.Clear();
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                ListViewItem cliente = new ListViewItem();
                for (int i = 0; i < 7; i++)
                {
                    cliente.SubItems.Add("");
                }
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "RUT")
                    {
                        cliente.Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Nombre")
                    {
                        cliente.SubItems[1].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Giro")
                    {
                        cliente.SubItems[2].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Direccion")
                    {
                        cliente.SubItems[3].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Contacto") // se cambio contacto por comuna
                    {
                        cliente.SubItems[4].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Ciudad")
                    {
                        cliente.SubItems[5].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Fono")
                    {
                        cliente.SubItems[6].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Correo")
                    {
                        cliente.SubItems[7].Text = nodo2.InnerText;
                    }
                }
                listViewClientes.Items.Add(cliente);
                listViewTOTALClientes.Items.Add((ListViewItem)cliente.Clone());
            }
        }
        private void buttonGuardarIndicadoresEconomicos_Click(object sender, EventArgs e)
        {
            frecia.setearIndicadoresEconomicos(textBoxIngresarFacturaDolar.Text, textBoxIngresarFacturaEuro.Text, textBoxIngresarFacturaImporte.Text);
            cargarInventario();
        }
        private void radioButtonClienteModificar_CheckedChanged(object sender, EventArgs e)
        {
            textBoxClientesRUT.Enabled = false;
            //textBoxClientesSeñores.Enabled = false;
            textBoxClientesGiro.Enabled = true;
            textBoxClientesComuna.Enabled = true;
            textBoxClientesDireccion.Enabled = true;
            textBoxClientesCiudad.Enabled = true;
            textBoxClientesFono.Enabled = true;
            textBoxClientesRUT.Text = "";
            textBoxClientesSeñores.Text = "";
            textBoxClientesGiro.Text = "";
            textBoxClientesComuna.Text = "";
            textBoxClientesDireccion.Text = "";
            textBoxClientesCiudad.Text = "";
            textBoxClientesFono.Text = "";
        }
        private void radioButtonClienteAgregar_CheckedChanged(object sender, EventArgs e)
        {
            textBoxClientesRUT.Enabled = true;
            textBoxClientesSeñores.Enabled = true;
            textBoxClientesGiro.Enabled = true;
            textBoxClientesComuna.Enabled = true;
            textBoxClientesDireccion.Enabled = true;
            textBoxClientesCiudad.Enabled = true;
            textBoxClientesFono.Enabled = true;
            textBoxClientesRUT.Text = "";
            textBoxClientesSeñores.Text = "";
            textBoxClientesGiro.Text = "";
            textBoxClientesComuna.Text = "";
            textBoxClientesDireccion.Text = "";
            textBoxClientesCiudad.Text = "";
            textBoxClientesFono.Text = "";
        }
        private void radioButtonClientesEliminar_CheckedChanged(object sender, EventArgs e)
        {
            textBoxClientesRUT.Enabled = false;
            textBoxClientesSeñores.Enabled = false;
            textBoxClientesGiro.Enabled = false;
            textBoxClientesComuna.Enabled = false;
            textBoxClientesDireccion.Enabled = false;
            textBoxClientesCiudad.Enabled = false;
            textBoxClientesFono.Enabled = false;
        }
        private void clientesFiltrarClientes(object sender, EventArgs e)
        {
            if (checkBoxClientesBusquedas.Checked == true)
            {
                if (comboBoxClientesRUT.SelectedIndex != 0) // primero filtra por el rut
                {
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALClientes.Items)
                    {
                        if (item.Text == comboBoxClientesRUT.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                if (comboBoxClientesSeñores.SelectedIndex != 0) // luego por el señores
                {
                    listViewTOTALintermedio.Items.Clear();
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        listViewTOTALintermedio.Items.Add((ListViewItem)item.Clone());
                    }
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALintermedio.Items)
                    {
                        if (item.SubItems[1].Text == comboBoxClientesSeñores.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                if (comboBoxClientesGiro.SelectedIndex != 0) // luego por el giro
                {
                    listViewTOTALintermedio.Items.Clear();
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        listViewTOTALintermedio.Items.Add((ListViewItem)item.Clone());
                    }
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALintermedio.Items)
                    {
                        if (item.SubItems[2].Text == comboBoxClientesGiro.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                if (comboBoxClientesContacto.SelectedIndex != 0) // luego por el Contacto
                {
                    listViewTOTALintermedio.Items.Clear();
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        listViewTOTALintermedio.Items.Add((ListViewItem)item.Clone());
                    }
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALintermedio.Items)
                    {
                        if (item.SubItems[3].Text == comboBoxClientesContacto.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                if (comboBoxClientesDireccion.SelectedIndex != 0) // luego por la direccion
                {
                    listViewTOTALintermedio.Items.Clear();
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        listViewTOTALintermedio.Items.Add((ListViewItem)item.Clone());
                    }
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALintermedio.Items)
                    {
                        if (item.SubItems[4].Text == comboBoxClientesDireccion.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                if (comboBoxClientesCiudad.SelectedIndex != 0) // luego por la ciudad
                {
                    listViewTOTALintermedio.Items.Clear();
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        listViewTOTALintermedio.Items.Add((ListViewItem)item.Clone());
                    }
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALintermedio.Items)
                    {
                        if (item.SubItems[5].Text == comboBoxClientesCiudad.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                if (comboBoxClientesFono.SelectedIndex != 0) // luego por el fono
                {
                    listViewTOTALintermedio.Items.Clear();
                    foreach (ListViewItem item in listViewClientes.Items)
                    {
                        listViewTOTALintermedio.Items.Add((ListViewItem)item.Clone());
                    }
                    listViewClientes.Items.Clear();
                    foreach (ListViewItem item in listViewTOTALintermedio.Items)
                    {
                        if (item.SubItems[6].Text == comboBoxClientesFono.SelectedItem.ToString())
                        {
                            listViewClientes.Items.Add((ListViewItem)item.Clone());
                        }
                    }
                }
                cargarFiltrosClientesClientes();
            }
        }
        private void cargarFiltrosClientesClientes()
        {
            checkBoxClientesBusquedas.Checked = false;
            //vacio los filtros.
            comboBoxClientesRUT.Items.Clear();
            comboBoxClientesSeñores.Items.Clear();
            comboBoxClientesGiro.Items.Clear();
            comboBoxClientesContacto.Items.Clear();
            comboBoxClientesDireccion.Items.Clear();
            comboBoxClientesCiudad.Items.Clear();
            comboBoxClientesFono.Items.Clear();
            //se carga el basico "---"
            comboBoxClientesRUT.Items.Add("---");
            comboBoxClientesSeñores.Items.Add("---");
            comboBoxClientesGiro.Items.Add("---");
            comboBoxClientesContacto.Items.Add("---");
            comboBoxClientesDireccion.Items.Add("---");
            comboBoxClientesCiudad.Items.Add("---");
            comboBoxClientesFono.Items.Add("---");
            //se deja precargado el vacio.
            foreach (ListViewItem item in listViewClientes.Items)
            {
                if (comboBoxClientesRUT.Items.Contains(item.Text) == false)
                {
                    comboBoxClientesRUT.Items.Add(item.Text);
                }
                if (comboBoxClientesSeñores.Items.Contains(item.SubItems[1].Text) == false)
                {
                    comboBoxClientesSeñores.Items.Add(item.SubItems[1].Text);
                }
                if (comboBoxClientesGiro.Items.Contains(item.SubItems[2].Text) == false)
                {
                    comboBoxClientesGiro.Items.Add(item.SubItems[2].Text);
                }
                if (comboBoxClientesContacto.Items.Contains(item.SubItems[3].Text) == false)
                {
                    comboBoxClientesContacto.Items.Add(item.SubItems[3].Text);
                }
                if (comboBoxClientesDireccion.Items.Contains(item.SubItems[4].Text) == false)
                {
                    comboBoxClientesDireccion.Items.Add(item.SubItems[4].Text);
                }
                if (comboBoxClientesCiudad.Items.Contains(item.SubItems[5].Text) == false)
                {
                    comboBoxClientesCiudad.Items.Add(item.SubItems[5].Text);
                }
                if (comboBoxClientesFono.Items.Contains(item.SubItems[6].Text) == false)
                {
                    comboBoxClientesFono.Items.Add(item.SubItems[6].Text);
                }
            }
            comboBoxClientesRUT.SelectedIndex = 0;
            comboBoxClientesSeñores.SelectedIndex = 0;
            comboBoxClientesGiro.SelectedIndex = 0;
            comboBoxClientesContacto.SelectedIndex = 0;
            comboBoxClientesDireccion.SelectedIndex = 0;
            comboBoxClientesCiudad.SelectedIndex = 0;
            comboBoxClientesFono.SelectedIndex = 0;
            checkBoxClientesBusquedas.Checked = true;
        }
        private void filtrosEnNadaClientesClientes()
        {
            checkBoxClientesBusquedas.Checked = false;
            comboBoxClientesRUT.SelectedIndex = 0;
            comboBoxClientesSeñores.SelectedIndex = 0;
            comboBoxClientesGiro.SelectedIndex = 0;
            comboBoxClientesContacto.SelectedIndex = 0;
            comboBoxClientesDireccion.SelectedIndex = 0;
            comboBoxClientesCiudad.SelectedIndex = 0;
            comboBoxClientesFono.SelectedIndex = 0;
            checkBoxClientesBusquedas.Checked = true;
        }
        private void checkBoxLimpiarDestinatario_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLimpiarDestinatario.Checked)
            {
                cargarFiltrosClientesCotizacion();
                checkBoxLimpiarDestinatario.Checked = false;
            }
        }
        public void cargarFiltrosClientesCotizacion()
        {
            comboBoxCotizacionRUT.Enabled = true;
            comboBoxCotizacionSeñores.Enabled = true;
            comboBoxCotizacionFono.Enabled = true;
            textBoxCotizacionDireccion.Enabled = true;
            textBoxCotizacionGiro.Enabled = true;
            checkBoxBUsquedaCLienteCotizacion.Enabled = false;
            //limpio las opciones.
            comboBoxCotizacionRUT.Items.Clear();
            comboBoxCotizacionSeñores.Items.Clear();
            comboBoxCotizacionFono.Items.Clear();
            textBoxCotizacionDireccion.Text = "";
            textBoxCotizacionGiro.Text = "";
            textBoxCotiContacto.Text = "";
            textBoxCotiCorreo.Text = "";
            //les pongo el basico
            comboBoxCotizacionRUT.Items.Add("---");
            comboBoxCotizacionSeñores.Items.Add("---");
            comboBoxCotizacionFono.Items.Add("---");
            //los fijo en cero
            comboBoxCotizacionRUT.SelectedIndex = 0;
            comboBoxCotizacionSeñores.SelectedIndex = 0;
            comboBoxCotizacionFono.SelectedIndex = 0;
            //se rellenan con el cliente
            foreach (ListViewItem item in listViewTOTALClientes.Items)
            {
                if (comboBoxCotizacionRUT.Items.Contains(item.Text) == false)
                {
                    comboBoxCotizacionRUT.Items.Add(item.Text);
                }
                if (comboBoxCotizacionSeñores.Items.Contains(item.SubItems[1].Text) == false)
                {
                    comboBoxCotizacionSeñores.Items.Add(item.SubItems[1].Text);
                }
                if (comboBoxCotizacionFono.Items.Contains(item.SubItems[6].Text) == false)
                {
                    comboBoxCotizacionFono.Items.Add(item.SubItems[6].Text);
                }
            }
            checkBoxBUsquedaCLienteCotizacion.Checked = true;
        }
        private void comboBoxCotizacionMatchearCliente(object sender, EventArgs e)
        {
            if (checkBoxBUsquedaCLienteCotizacion.Checked == true)
            {
                if (comboBoxCotizacionRUT.SelectedIndex != 0) // cuando es seleciconado el rut
                {
                    foreach (ListViewItem item in listViewTOTALClientes.Items)
                    {
                        if (item.Text == comboBoxCotizacionRUT.SelectedItem)
                        {
                            fijarClienteEnCotizacion(item);
                        }
                    }
                }
                if (comboBoxCotizacionSeñores.SelectedIndex != 0) // cuando se selecciona el nombre de la empresa
                {
                    foreach (ListViewItem item in listViewTOTALClientes.Items)
                    {
                        if (item.SubItems[1].Text == comboBoxCotizacionSeñores.SelectedItem)
                        {
                            fijarClienteEnCotizacion(item);
                        }
                    }
                }
                if (comboBoxCotizacionFono.SelectedIndex != 0) // cuando se selecciona el telefono de la empresa
                {
                    foreach (ListViewItem item in listViewTOTALClientes.Items)
                    {
                        if (item.SubItems[6].Text == comboBoxCotizacionFono.SelectedItem)
                        {
                            fijarClienteEnCotizacion(item);
                        }
                    }
                }
            }
        }
        private void fijarClienteEnCotizacion(ListViewItem item)
        {
            comboBoxCotizacionRUT.Enabled = false;
            comboBoxCotizacionSeñores.Enabled = false;
            comboBoxCotizacionFono.Enabled = false;
            textBoxCotizacionDireccion.Enabled = false;
            textBoxCotizacionGiro.Enabled = false;
            //limpio el box
            comboBoxCotizacionRUT.Items.Clear();
            comboBoxCotizacionSeñores.Items.Clear();
            comboBoxCotizacionFono.Items.Clear();
            //le pongo el correcto
            comboBoxCotizacionRUT.Items.Add(item.Text);
            comboBoxCotizacionSeñores.Items.Add(item.SubItems[1].Text);
            comboBoxCotizacionFono.Items.Add(item.SubItems[6].Text);
            textBoxCotizacionDireccion.Text = item.SubItems[3].Text;
            textBoxCotizacionGiro.Text = item.SubItems[2].Text;
            //los fijo en cero
            comboBoxCotizacionRUT.SelectedIndex = 0;
            comboBoxCotizacionSeñores.SelectedIndex = 0;
            comboBoxCotizacionFono.SelectedIndex = 0;
            MessageBox.Show("Debe Ingresar Contacto y correo");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FacturaLiberada nueva = new FacturaLiberada(this);
            this.Hide();
            nueva.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Numeros numero = new Numeros(textBoxCotizacionNoInventarioValorTotal.Text);
            ListViewItem nuevo;
            if (checkBoxNoBuscarInventario.Checked)
            {
                nuevo = new ListViewItem(comboBoxCotizacionProductoDescrip.SelectedItem.ToString());
            }
            else
            {
                nuevo = new ListViewItem(textBoxCotizacionELementoCotizado.Text);
                textBoxCotizacionELementoCotizado.Text = "";
            }
            nuevo.SubItems.Add(textBoxCotizacionNoInventarioNuParte.Text);
            nuevo.SubItems.Add(textBoxCotizacionNoInventarioCant.Text);
            nuevo.SubItems.Add(textBoxCotizacionNoInventarioValor.Text);
            nuevo.SubItems.Add(textBoxCotizacionNoInventarioDescuento.Text);
            nuevo.SubItems.Add("$" + numero.numeroMiles());
            nuevo.SubItems.Add(textBoxCotizacionNoInventarioPlazoEntrega.Text);
            foreach (ListViewItem items in listViewCotizacion.Items)
            {
                if (items.Text == nuevo.Text)
                {
                    items.Remove();
                }
            }
            listViewCotizacion.Items.Add(nuevo);
            textBoxCotizacionNoInventarioCant.Text = "";
            textBoxCotizacionNoInventarioValor.Text = "";
            textBoxCotizacionNoInventarioDescuento.Text = "";
            textBoxCotizacionNoInventarioValorTotal.Text = "";
            textBoxCotizacionNoInventarioPlazoEntrega.Text = "";
            sumaTotalCotizacion();
        }
        private void Agregar_Click(object sender, EventArgs e)
        {
            if (textBoxCondicionesGeneralesDescripcion.Enabled == false)
            {
                foreach (ListViewItem items in listViewCondicionesGenerales.Items)
                {
                    if (items.Text == textBoxCondicionesGeneralesDescripcion.Text)
                    {
                        items.SubItems[1].Text = textBoxCondicionesGeneralescontenido.Text;
                        textBoxCondicionesGeneralesDescripcion.Enabled = true;
                    }
                }
            }
            else
            {
                ListViewItem nuevo = new ListViewItem(textBoxCondicionesGeneralesDescripcion.Text);
                nuevo.SubItems.Add(textBoxCondicionesGeneralescontenido.Text);
                listViewCondicionesGenerales.Items.Add(nuevo);
            }
            textBoxCondicionesGeneralesDescripcion.Text = "";
            textBoxCondicionesGeneralescontenido.Text = "";
        }
        private void buttonGuardarUsuario_Click(object sender, EventArgs e)
        {
            string niveles = "";
            if (checkBoxUsuarioNivel8.Checked)
            {
                niveles += "2";
            }
            else
            {
                niveles += "1";
            }
            if (checkBoxUsuarioNivel7.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel6.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel5.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel4.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel3.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel2.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel1.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            string[] cadena = { hashear(textBoxUsuario.Text), textBoxUsuario.Text, niveles };
            fresia.editarUsuario(cadena, hashear(textBoxUsuario.Text));
        }
        private string hashear(string value)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(value);
            data = provider.ComputeHash(data);
            string md5 = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                md5 += data[i].ToString("x2").ToLower();
            }
            return md5;
        }

        private void buttonCrearUsuario_Click(object sender, EventArgs e)
        {
            string niveles = "";
            if (checkBoxUsuarioNivel8.Checked)
            {
                niveles += "2";
            }
            else
            {
                niveles += "1";
            }
            if (checkBoxUsuarioNivel7.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel6.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel5.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel4.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel3.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel2.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            if (checkBoxUsuarioNivel1.Checked)
            {
                niveles += "1";
            }
            else
            {
                niveles += "0";
            }
            string[] cadena = { hashear(textBoxContraseña.Text + textBoxUsuario.Text), textBoxUsuario.Text, niveles };
            fresia.crearUsuario(cadena);
        }

        private void buttonEliminarUsuario_Click(object sender, EventArgs e)
        {
            fresia.eliminarUsuario(textBoxUsuario.Text);
        }

        private void listViewCondicionesGenerales_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxCondicionesGeneralesDescripcion.Text = listViewCondicionesGenerales.SelectedItems[0].Text;
            textBoxCondicionesGeneralesDescripcion.Enabled = false;
            listViewCondicionesGenerales.SelectedItems[0].Selected = false;
        }

        private void editarCondicionGeneral(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString().Equals("Left")) // si es click secundario
            {
                if (listViewCondicionesGenerales.SelectedItems[0].Text != "Orden de Compra a nombre de:")
                {
                    textBoxCondicionesGeneralesDescripcion.Text = listViewCondicionesGenerales.SelectedItems[0].Text;
                    textBoxCondicionesGeneralesDescripcion.Enabled = false;
                }
                listViewCondicionesGenerales.SelectedItems[0].Selected = false;
            }
        }

        private void seleccionarClienteClientes(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString().Equals("Left"))
            {
                if (radioButtonClienteAgregar.Checked == false)
                {
                    textBoxClientesRUT.Text = listViewClientes.SelectedItems[0].Text;
                    textBoxClientesSeñores.Text = listViewClientes.SelectedItems[0].SubItems[1].Text;
                    textBoxClientesGiro.Text = listViewClientes.SelectedItems[0].SubItems[2].Text;
                    textBoxClientesDireccion.Text = listViewClientes.SelectedItems[0].SubItems[3].Text;
                    textBoxClientesComuna.Text = listViewClientes.SelectedItems[0].SubItems[4].Text;
                    textBoxClientesCiudad.Text = listViewClientes.SelectedItems[0].SubItems[5].Text;
                    textBoxClientesFono.Text = listViewClientes.SelectedItems[0].SubItems[6].Text;
                    listViewClientes.SelectedItems[0].Selected = false;
                }
            }
        }
        private void cargarFiltrosInventarioFromTotal()
        {
            comboBoxCotizacionProductoDescrip.Items.Clear();
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                if (comboBoxCotizacionProductoDescrip.Items.Contains(item.SubItems[3].Text) == false)
                {
                    comboBoxCotizacionProductoDescrip.Items.Add(item.SubItems[3].Text);
                }
            }
        }

        private void CargarProductoCotizacion(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                if (comboBoxCotizacionProductoDescrip.SelectedItem.Equals(item.SubItems[3].Text)) // si el nombre calza
                {
                    buttonAñadirCotizacion.Enabled = true; 
                    textBoxCotizacionNoInventarioNuParte.Text = item.SubItems[2].Text; //numero de parte
                    textBoxCotizacionNoInventarioCant.Text = item.SubItems[5].Text; //la cantidad
                    textBoxCotizacionNoInventarioValor.Text = item.SubItems[11].Text; //su pvp
                    textBoxCotizacionNoInventarioDescuento.Text = "0";
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxLimpiarProducto.Checked)
            {
                if (checkBoxNoBuscarInventario.Checked)
                {
                    foreach (ListViewItem item in listViewCotizacion.Items)
                    {
                        if (item.Text == comboBoxCotizacionProductoDescrip.SelectedItem.ToString())
                        {
                            item.Remove();
                            textBoxCotizacionELementoCotizado.Visible = false;
                            textBoxCotizacionELementoCotizado.Text = "";
                            textBoxCotizacionNoInventarioNuParte.Text = "";
                            textBoxCotizacionNoInventarioCant.Text = "";
                            textBoxCotizacionNoInventarioValor.Text = "";
                            textBoxCotizacionNoInventarioDescuento.Text = "";
                            textBoxCotizacionNoInventarioValorTotal.Text = "";
                            textBoxCotizacionNoInventarioPlazoEntrega.Text = "";
                        }
                    }
                }
                else
                {
                    foreach (ListViewItem item in listViewCotizacion.Items)
                    {
                        if (item.Text == textBoxCotizacionELementoCotizado.Text)
                        {
                            item.Remove();
                        }
                    }
                }
                checkBoxLimpiarProducto.Checked = false;
                sumaTotalCotizacion();
            }
        }

        private void checkBoxNoBuscarInventario_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNoBuscarInventario.Checked == false)
            {
                label16.Text = "Cant.";
                textBoxCotizacionELementoCotizado.Visible = true;
                textBoxCotizacionELementoCotizado.Enabled = true;
                textBoxCotizacionELementoCotizado.Text = "";
                textBoxCotizacionNoInventarioNuParte.Text = "";
                textBoxCotizacionNoInventarioCant.Text = "";
                textBoxCotizacionNoInventarioValor.Text = "";
                textBoxCotizacionNoInventarioDescuento.Text = "0";
                textBoxCotizacionNoInventarioValorTotal.Text = "";
                textBoxCotizacionNoInventarioPlazoEntrega.Text = "";
            }
            else
            {
                label16.Text = "Cant. Max:";
                textBoxCotizacionELementoCotizado.Visible = false;
                textBoxCotizacionELementoCotizado.Text = "";
                textBoxCotizacionNoInventarioNuParte.Text = "";
                textBoxCotizacionNoInventarioCant.Text = "";
                textBoxCotizacionNoInventarioValor.Text = "";
                textBoxCotizacionNoInventarioDescuento.Text = "";
                textBoxCotizacionNoInventarioValorTotal.Text = "";
                textBoxCotizacionNoInventarioPlazoEntrega.Text = "";
            }
        }

        private void mantenerCantidad(object sender, KeyPressEventArgs e)
        {
        }
        private void textBoxCotizacionNoInventarioCant_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBoxNoBuscarInventario.Checked)
                {
                    try
                    {
                        int number = Convert.ToInt32(textBoxCotizacionNoInventarioCant.Text);
                        if (number < 0)
                        {
                            textBoxCotizacionNoInventarioCant.Text = "";
                        }
                        sumaDePrecio();
                    }
                    catch
                    {
                        textBoxCotizacionNoInventarioCant.Text = "";
                    }
                }
            }
            catch { }
        }
        private void textBoxCotizacionNoInventarioValor_TextChanged(object sender, EventArgs e)
        {
            Numeros nuevo = new Numeros(textBoxCotizacionNoInventarioValor.Text);
            int number = Convert.ToInt32(nuevo.numeroSolo());
            if (number < 0)
            {
                textBoxCotizacionNoInventarioValor.Text = "";
            }
            sumaDePrecio();
        }
        private void textBoxCotizacionNoInventarioDescuento_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int number = Convert.ToInt32(textBoxCotizacionNoInventarioDescuento.Text);
                if (number < 0 && number > 100)
                {
                    textBoxCotizacionNoInventarioDescuento.Text = "";
                }
                sumaDePrecio();
            }
            catch
            {
                textBoxCotizacionNoInventarioDescuento.Text = "";
            }
        }
        public void sumaDePrecio()
        {
            try
            {
                string pre = new Numeros(textBoxCotizacionNoInventarioValor.Text).numeroSolo();
                string cant = new Numeros(textBoxCotizacionNoInventarioCant.Text).numeroSolo();
                string desc = new Numeros(textBoxCotizacionNoInventarioDescuento.Text).numeroSolo();
                double precio = Convert.ToInt32(pre), cantidad = Convert.ToInt32(cant), descuento = Convert.ToInt32(desc);
                int valor = Convert.ToInt32(((precio * cantidad) * ((100 - descuento) / 100)));
                Numeros numero = new Numeros(valor);
                textBoxCotizacionNoInventarioValorTotal.Text = "$" + numero.numeroMiles();
            }
            catch { }
        }

        private void textBoxCotizacionELementoCotizado_TextChanged(object sender, EventArgs e)
        {
            if (textBoxCotizacionELementoCotizado.Text != "")
            {
                buttonAñadirCotizacion.Enabled = true;
            }
        }

        private void radioButtonMonedaEuro_CheckedChanged(object sender, EventArgs e)
        {
            Numeros costo = new Numeros(textBoxModInventCostoE.Text);
            textBoxModInventCostoE.Enabled = true;
            textBoxModInventCostoE.Text = "€" + costo.numeroSolo();
        }

        private void radioButtonMonedaDolar_CheckedChanged(object sender, EventArgs e)
        {
            Numeros costo = new Numeros(textBoxModInventCostoE.Text);
            textBoxModInventCostoE.Enabled = true;
            textBoxModInventCostoE.Text = "$" + costo.numeroSolo();
            textBoxModInventarioFlete.Text = "0";
        }

        private void selectUser(object sender, MouseEventArgs e)
        {
            textBoxUsuario.Text = listViewUsuarios.SelectedItems[0].Text;
            listViewUsuarios.SelectedItems[0].Selected = false;
        }

        private void generarRut(object sender, EventArgs e)
        {
            Numeros nuevo = new Numeros(textBoxClientesRUT.Text);
            textBoxClientesRUT.Text = nuevo.validarRut();
        }

        private void precioMiles(object sender, EventArgs e)
        {
            Numeros nuevo = new Numeros(textBoxCotizacionNoInventarioValor.Text);
            textBoxCotizacionNoInventarioValor.Text = nuevo.numeroMiles();
            sumaDePrecio();
        }
        private String FormatoDeLinea(String nombre)
        {
            if (nombre != String.Empty)
            {
                String inicial;
                nombre = nombre.ToLower();
                inicial = nombre.Substring(0, 1);
                inicial = inicial.ToUpper();
                nombre = inicial + nombre.Remove(0, 1);
            }
            return nombre;
        }

        private void formatearTexto(object sender, EventArgs e)
        {
            textBoxCotizacionELementoCotizado.Text = FormatoDeLinea(textBoxCotizacionELementoCotizado.Text);
        }

        private void textBoxCondicionesGeneralesDescripcion_Leave(object sender, EventArgs e)
        {
            textBoxCondicionesGeneralesDescripcion.Text = FormatoDeLinea(textBoxCondicionesGeneralesDescripcion.Text);
        }

        private void textBoxCondicionesGeneralescontenido_Leave(object sender, EventArgs e)
        {
            textBoxCondicionesGeneralescontenido.Text = FormatoDeLinea(textBoxCondicionesGeneralescontenido.Text);
        }

        private void textBoxCotizacionNoInventarioPlazoEntrega_Leave(object sender, EventArgs e)
        {
            textBoxCotizacionNoInventarioPlazoEntrega.Text = FormatoDeLinea(textBoxCotizacionNoInventarioPlazoEntrega.Text);
        }
        private void textBoxModInventDescripcion_Leave(object sender, EventArgs e)
        {
            textBoxModInventDescripcion.Text = FormatoDeLinea(textBoxModInventDescripcion.Text);
        }

        private void textBoxClientesGiro_Leave(object sender, EventArgs e)
        {
            textBoxClientesGiro.Text = FormatoDeLinea(textBoxClientesGiro.Text);
        }

        private void textBoxClientesContacto_Leave(object sender, EventArgs e)
        {
            textBoxClientesComuna.Text = FormatoDeLinea(textBoxClientesComuna.Text);
        }

        private void textBoxClientesDireccion_Leave(object sender, EventArgs e)
        {
            textBoxClientesDireccion.Text = FormatoDeLinea(textBoxClientesDireccion.Text);
        }
        private void textBoxModInventCostoE_Leave(object sender, EventArgs e)
        {
            //
        }
        private void textBoxModInvtPVP_Leave(object sender, EventArgs e)
        {
            Numeros precio = new Numeros(textBoxModInvtPVP.Text);
            textBoxModInvtPVP.Text = "" + precio.numeroMiles();
        }
        private void FormInventario_FormClosing(object sender, FormClosingEventArgs e)
        {
            fresiaEstado.Abort();
            padre.Show();
            padre.Close();
        }
        private void textBoxModInventPesoKilos_Leave(object sender, EventArgs e)
        {
            textBoxModInventPesoKilos.Text = textBoxModInventPesoKilos.Text.Replace('.', ',');
        }

        private void comboBoxVerStockNP_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerStock();
            if (comboBoxVerStockNP.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerStock.Items)
                {
                    if (item.SubItems[2].Text != comboBoxVerStockNP.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxVerStockGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerStock();
            if (comboBoxVerStockGrupo.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerStock.Items)
                {
                    if (item.SubItems[1].Text != comboBoxVerStockGrupo.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxModInventarioNuParte_SelectedIndexChanged(object sender, EventArgs e)
        {
            matchearModInventario();
            if (comboBoxModInventarioNuParte.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewModInventario.Items)
                {
                    if (item.SubItems[2].Text != comboBoxModInventarioNuParte.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }

        private void comboBoxModInvetarioDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            matchearModInventario();
            if (comboBoxModInvetarioDescripcion.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewModInventario.Items)
                {
                    if (item.SubItems[3].Text != comboBoxModInvetarioDescripcion.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxVerInventarioFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerInventario();
            if (comboBoxVerInventarioFamilia.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerInventario.Items)
                {
                    if (item.Text != comboBoxVerInventarioFamilia.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxVerInventarioGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerInventario();
            if (comboBoxVerInventarioGrupo.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerInventario.Items)
                {
                    if (item.SubItems[1].Text != comboBoxVerInventarioGrupo.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxVerInventarioNP_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerInventario();
            if (comboBoxVerInventarioNP.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerInventario.Items)
                {
                    if (item.SubItems[2].Text != comboBoxVerInventarioNP.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxVerInventarioDescripcion_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerInventario();
            if (comboBoxVerInventarioDescripcion.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerInventario.Items)
                {
                    if (item.SubItems[3].Text != comboBoxVerInventarioDescripcion.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void recargarVerInventario()
        {
            listViewVerInventario.Items.Clear();
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                listViewVerInventario.Items.Add((ListViewItem)item.Clone());
            }
        }
        private void recargarVerStock()
        {
            listViewVerStock.Items.Clear();
            foreach (ListViewItem item in listViewTOTALintermedio.Items)
            {
                listViewVerStock.Items.Add((ListViewItem)item.Clone());
            }
        }
        private void comboBoxVerStockFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerStock();
            if (comboBoxVerStockFamilia.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerStock.Items)
                {
                    if (item.Text != comboBoxVerStockFamilia.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }

        private void comboBoxVerStockDescrip_SelectedIndexChanged(object sender, EventArgs e)
        {
            recargarVerStock();
            if (comboBoxVerStockDescrip.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewVerStock.Items)
                {
                    if (item.SubItems[3].Text != comboBoxVerStockDescrip.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }
        private void comboBoxModInventarioGrupo_SelectedIndexChanged(object sender, EventArgs e)
        {
            matchearModInventario();
            if (comboBoxModInventarioGrupo.SelectedItem != "---")
            {
                foreach (ListViewItem item in listViewModInventario.Items)
                {
                    if (item.SubItems[1].Text != comboBoxModInventarioGrupo.SelectedItem.ToString())
                    {
                        item.Remove();
                    }
                }
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBoxCotiCorreo_Leave(object sender, EventArgs e)
        {
            if (textBoxCotiCorreo.Text.Contains('@') == false)
            {
                MessageBox.Show("El correo ingresado no tiene formato de correo");
            }
        }

        private void textBoxClientesCiudad_Leave(object sender, EventArgs e)
        {
            textBoxClientesCiudad.Text = FormatoDeLinea(textBoxClientesCiudad.Text);
        }

        private void FormInventario_Resize(object sender, EventArgs e)
        {
            tableLayoutPanel10.Height = this.Size.Height;
            tableLayoutPanel10.Width = this.Size.Width-20;
        }
        private void panelFacturaUtil_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listViewCotizaciones_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            string COTIZACION = listViewCotizaciones.SelectedItems[0].Text;
            checkBoxBusquedaEnFactura.Checked = false;
            comboBoxFacturaVendedor.SelectedItem = listViewCotizaciones.SelectedItems[0].SubItems[1].Text;
            comboBoxFacturaCliente.SelectedItem = listViewCotizaciones.SelectedItems[0].SubItems[2].Text;
            comboBoxFacturaFecha.SelectedItem = listViewCotizaciones.SelectedItems[0].SubItems[3].Text;
            checkBoxBusquedaEnFactura.Checked = true;
            listViewFactura.Items.Clear();
            textBoxNETO.Text = "0";
            frecia.obtenerFactura(Convert.ToInt32(COTIZACION)).Save("prefactura.ftr");
            XmlDocument factura = new XmlDocument();
            factura.Load("prefactura.ftr");
            XmlElement file = factura.DocumentElement;
            if (Convert.ToInt16(file.LastChild.InnerText) >= 0)
            {
                foreach (XmlNode buscaDetalle in file.ChildNodes)
                {
                    if (buscaDetalle.Name == "Productos")
                    {
                        foreach (XmlNode productos in buscaDetalle.ChildNodes)
                        {
                            listViewFactura.Items.Clear();
                            string descripcion = "", cantidad = "", precio = "", total = "";
                            foreach (XmlNode caracteristica in productos.ChildNodes)
                            {
                                if (caracteristica.Name == "Descripcion")
                                {
                                    descripcion = caracteristica.InnerText;
                                }
                                else if (caracteristica.Name == "cantidad")
                                {
                                    cantidad = caracteristica.InnerText;
                                }
                                else if (caracteristica.Name == "Precio")
                                {
                                    precio = caracteristica.InnerText;
                                }
                                else if (caracteristica.Name == "valorTotal")
                                {
                                    total = caracteristica.InnerText;
                                    int antes = Convert.ToInt32(new Numeros(textBoxNETO.Text).numeroSolo());
                                    int actual = Convert.ToInt32(new Numeros(total).numeroSolo());
                                    textBoxNETO.Text = "$" + new Numeros(antes + actual).numeroMiles();
                                }
                            }
                            ListViewItem producto = new ListViewItem(cantidad);
                            producto.SubItems.Add(descripcion);
                            producto.SubItems.Add(precio);
                            producto.SubItems.Add(total);
                            listViewFactura.Items.Add(producto);
                        }
                    }
                }
            }
            buttonFacturaImprimir.Enabled = true;
            factura.Save("temp.ftr");
        }
        private void textBoxNETO_TextChanged(object sender, EventArgs e)
        {
            int precio = Convert.ToInt32(new Numeros(textBoxNETO.Text).numeroSolo());
            double aux = precio * 0.19;
            textBoxIVA.Text = "$" + new Numeros(Convert.ToInt32(aux)).numeroMiles();
            aux = precio * 1.19;
            textBoxTOTAL.Text = "$" + new Numeros(Convert.ToInt32(aux)).numeroMiles();
        }
        private void FormInventario_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click_2(object sender, EventArgs e)
        {
            if (frecia.subirActualizacionAServidor(textBoxActualizacion.Text))
            {
                MessageBox.Show("actualizacion Cargada Correctamente");
            }
            else
            {
                MessageBox.Show("Actualizacion no pudo ser Cargada al servidor");
            }
        }
        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }
        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int dolar = 0, euro = 0;
            frecia.indicadoresEconomicos().Save("Indicadores.old");
            XmlDocument indicadores = new XmlDocument();
            indicadores.Load("Indicadores.old");
            foreach (XmlNode nodo in indicadores.DocumentElement.ChildNodes)
            {
                if (nodo.Name == "Dolar")
                {
                    dolar = Convert.ToInt32(nodo.InnerText);
                    textBoxIngresarFacturaDolar.Text = nodo.InnerText;
                }
                else if (nodo.Name == "Euro")
                {
                    euro = Convert.ToInt32(nodo.InnerText);
                    textBoxIngresarFacturaEuro.Text = nodo.InnerText;
                }
            }
            fresia.getInventarioXML().Save("items.xml");
            XmlDocument lista = new XmlDocument();
            lista.Load("C:\\Inventario\\items.xml");
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            /*Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Add(Missing.Value);*/
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\InventarioExportable.xlsx");
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            int i = 4;
            foreach (XmlNode nodo in lista.DocumentElement.ChildNodes)
            {

                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "Codigo")
                    {
                        xlHoja.Cells[i, 3] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Origen")
                    {
                        string[] texto = nodo2.InnerText.Split('-');
                        xlHoja.Cells[i, 7] = texto[1];
                        xlHoja.Cells[i, 6] = texto[0];
                        xlHoja.Cells[i, 5] = "=SI(f" + i + "=\"€\";g" + i + "/"+euro+";g" + i + "/"+dolar+")";
                    }
                    else if (nodo2.Name == "Familia")
                    {
                        xlHoja.Cells[i, 1] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Grupo")
                    {
                        xlHoja.Cells[i, 2] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Cant")
                    {
                        xlHoja.Cells[i, 8] = nodo2.InnerText;
                        xlHoja.Cells[i, 9] = "=E" + i + "*H" + i;
                        xlHoja.Cells[i, 10] = "=F" + i;
                    }
                    else if (nodo2.Name == "Descripcion")
                    {
                        xlHoja.Cells[i, 4] = nodo2.InnerText;
                    }
                }
                i++;
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string nombre = "/Inventario" + DateTime.Today.Day + "-" + DateTime.Today.Month + ".xlsx";
            try
            {
                File.Delete(desktopPath + nombre);
            }
            catch { }
            xlHoja.SaveAs(desktopPath + nombre);
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("ingreso finalizado, excel creado con exito\n archivo nombre: " + nombre.Split('/')[1]);
        }
        private void button4_Click(object sender, EventArgs e)
        {
            //
            fresia.getInventarioXML().Save("items.xml");
            XmlDocument lista = new XmlDocument();
            lista.Load("C:\\Inventario\\items.xml");
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\FormatoExportarPrecios.xlsx");
            //Microsoft.Office.Interop.Excel._Workbook libro = (Microsoft.Office.Interop.Excel._Workbook)(excelapp.Workbooks.Add(Missing.Value)); ;
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            int i = 2;
            foreach (XmlNode nodo in lista.DocumentElement.ChildNodes)
            {

                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "Familia")
                    {
                        xlHoja.Cells[i, 1] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Grupo")
                    {
                        xlHoja.Cells[i, 2] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Descripcion")
                    {
                        xlHoja.Cells[i, 3] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "PVP")
                    {
                        int precio = Convert.ToInt32(new Numeros(nodo2.InnerText).numeroSolo());
                        xlHoja.Cells[i, 4] = precio;
                        xlHoja.Cells[i, 5] = Math.Round((precio*0.19),0);
                        xlHoja.Cells[i, 6] = Math.Round((precio * 1.19), 0);
                    }
                }
                i++;
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string nombre = "/Precios del Inventario " + DateTime.Today.Day + "-" + DateTime.Today.Month + ".xlsx";
            try
            {
                File.Delete(desktopPath + nombre);
            }
            catch { }
            xlHoja.SaveAs(desktopPath + nombre);
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("Exportacion completa en Escritorio\nNombre:" + nombre.Split('/')[1]);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = true;
            Microsoft.Office.Interop.Excel._Workbook libro = (Microsoft.Office.Interop.Excel._Workbook)(excelapp.Workbooks.Add(Missing.Value)); ;
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            int i = 1;
            foreach(ListViewItem item in listViewModInventario.Items)
            {
                xlHoja.Cells[i, 1] = item.Text;
                xlHoja.Cells[i, 2] = item.SubItems[1].Text;
                xlHoja.Cells[i, 3] = item.SubItems[2].Text;
                xlHoja.Cells[i, 4] = item.SubItems[3].Text;
                xlHoja.Cells[i, 5] = item.SubItems[4].Text;
                xlHoja.Cells[i, 6] = item.SubItems[5].Text;
                xlHoja.Cells[i, 7] = item.SubItems[6].Text;
                xlHoja.Cells[i, 8] = item.SubItems[7].Text;
                xlHoja.Cells[i, 9] = item.SubItems[8].Text;
                xlHoja.Cells[i, 10] = item.SubItems[9].Text;
                xlHoja.Cells[i, 11] = item.SubItems[10].Text;
                xlHoja.Cells[i, 12] = item.SubItems[11].Text;
                xlHoja.Cells[i, 13] = item.SubItems[12].Text;
                xlHoja.Cells[i, 14] = item.SubItems[13].Text;
                i++;
            }
        }

        private void FormInventario_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                padre.Show();
                padre.Close();
            }
            catch { }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (textBox1.Text.Equals("obtener clientes"))
                {
                    fresia.getClientesXML().Save("clientes.OLD");
                    XmlDocument nuevo = new XmlDocument();
                    nuevo.Load("Clientes.OLD");
                    Microsoft.Office.Interop.Excel._Worksheet xlHoja;
                    Microsoft.Office.Interop.Excel.Sheets xlHojas;
                    var misValue = Type.Missing;
                    Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
                    excelapp.Visible = true;
                    Microsoft.Office.Interop.Excel._Workbook libro = (Microsoft.Office.Interop.Excel._Workbook)(excelapp.Workbooks.Add(Missing.Value)); ;
                    xlHojas = libro.Sheets;
                    xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
                    int i = 2;
                    xlHoja.Cells[1, 8] = nuevo.DocumentElement.ChildNodes.Count;
                    foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
                    {
                        int j = 1;
                        foreach (XmlNode nodo2 in nodo.ChildNodes)
                        {
                            xlHoja.Cells[i, j] = nodo2.InnerText;
                            j++;
                        }
                        i++;
                    }
                }
                else if (textBox1.Text.Equals("mandar clientes")) 
                {
                    if (File.Exists("c:\\clientesOrdenados.xlsx"))
                    {
                        Microsoft.Office.Interop.Excel._Worksheet xlHoja;
                        Microsoft.Office.Interop.Excel.Sheets xlHojas;
                        var misValue = Type.Missing;
                        Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
                        excelapp.Visible = false;
                        Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\clientesOrdenados.xlsx");
                        xlHojas = libro.Sheets;
                        xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
                        int cantidad = Convert.ToInt32(xlHoja.get_Range("H1", "H1").Value2.ToString());
                        MessageBox.Show("clientes: " + cantidad + "\n deverian ser: " + xlHoja.get_Range("H1", "H1").Value2.ToString());
                        for (int i = 2; i <= cantidad; i++)
                        {
                            fresia.eliminarCliente(xlHoja.get_Range("A" + i, "A" + i).Value2.ToString());
                        }
                        for (int i = 2; i <= cantidad; i++)
                        {
                            fresia.eliminarCliente(xlHoja.get_Range("A" + i, "A" + i).Value2.ToString());
                        }
                        MessageBox.Show("ingresando clientes");
                        for (int i = 2; i <= cantidad; i++)
                        {                           // 1,  2, 3,  4,   5,  6,  7,  8
                            string[] datosCliente = { "", "", "", "", "", "", "", "" };
                            datosCliente[0] = xlHoja.get_Range("A" + i, "A" + i).Value2.ToString();
                            datosCliente[1] = xlHoja.get_Range("B" + i, "B" + i).Value2.ToString();
                            datosCliente[2] = xlHoja.get_Range("C" + i, "C" + i).Value2.ToString();
                            datosCliente[3] = xlHoja.get_Range("D" + i, "D" + i).Value2.ToString();
                            datosCliente[4] = xlHoja.get_Range("E" + i, "E" + i).Value2.ToString();
                            datosCliente[5] = xlHoja.get_Range("F" + i, "F" + i).Value2.ToString();
                            datosCliente[6] = xlHoja.get_Range("G" + i, "G" + i).Value2.ToString();
                            fresia.crearCliente(datosCliente);
                        }
                        xlHoja.SaveAs("C:\\Salida.xlsx");
                        libro.Close();
                        excelapp.Quit();
                        File.Delete("C:\\Salida.xlsx");
                    }
                    else
                    {
                        MessageBox.Show("Falta clientesOrdenados en C://");
                    }
                }
                else if (textBox1.Text.Equals("obtener inventario")) 
                {
                    fresia.getInventarioXML().Save("items.xml");
                    XmlDocument lista = new XmlDocument();
                    lista.Load("items.xml");
                    Microsoft.Office.Interop.Excel._Worksheet xlHoja;
                    Microsoft.Office.Interop.Excel.Sheets xlHojas;
                    var misValue = Type.Missing;
                    Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
                    excelapp.Visible = true;
                    Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\SinInventario.xlsx");
                    xlHojas = libro.Sheets;
                    xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
                    int i = 2;
                    xlHoja.Cells[1, 8] = lista.DocumentElement.ChildNodes.Count;
                    foreach (XmlNode nodo in lista.DocumentElement.ChildNodes)
                    {
                        int j = 1;
                        foreach (XmlNode nodo2 in nodo.ChildNodes)
                        {
                            xlHoja.Cells[i, j] = nodo2.InnerText;
                            j++;
                        }
                        i++;
                    }
                }
                else if (textBox1.Text.Equals("mandar inventario")) 
                {
                    if (File.Exists("c:\\InventarioOrdenado.xlsx"))
                    {
                        Microsoft.Office.Interop.Excel._Worksheet xlHoja;
                        Microsoft.Office.Interop.Excel.Sheets xlHojas;
                        var misValue = Type.Missing;
                        Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
                        excelapp.Visible = false;
                        Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\InventarioOrdenado.xlsx");
                        xlHojas = libro.Sheets;
                        xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
                        int cantidad = Convert.ToInt32(xlHoja.get_Range("H1", "H1").Value2.ToString());
                        MessageBox.Show("Inventario: " + cantidad + "\n deverian ser: " + xlHoja.get_Range("H1", "H1").Value2.ToString());
                        for (int i = 2; i < (2+cantidad); i++)
                        {
                            fresia.eliminarProducto(xlHoja.get_Range("A" + i, "A" + i).Value2.ToString());
                        }
                        MessageBox.Show("ingresando Productos");
                        for (int i = 2; i < (2+cantidad); i++)
                        {                           // 1,  2, 3,  4,   5,  6,  7,  8, 9
                            string[] datosCliente = { "", "", "", "", "", "", "", "","" };
                            datosCliente[0] = xlHoja.get_Range("A" + i, "A" + i).Value2.ToString();
                            datosCliente[1] = xlHoja.get_Range("B" + i, "B" + i).Value2.ToString();
                            datosCliente[2] = xlHoja.get_Range("C" + i, "C" + i).Value2.ToString();
                            datosCliente[3] = xlHoja.get_Range("D" + i, "D" + i).Value2.ToString();
                            datosCliente[4] = xlHoja.get_Range("E" + i, "E" + i).Value2.ToString();
                            datosCliente[5] = xlHoja.get_Range("F" + i, "F" + i).Value2.ToString();
                            datosCliente[6] = xlHoja.get_Range("G" + i, "G" + i).Value2.ToString();
                            datosCliente[7] = xlHoja.get_Range("H" + i, "H" + i).Value2.ToString();
                            datosCliente[8] = xlHoja.get_Range("I" + i, "I" + i).Value2.ToString();
                            fresia.crearProducto(datosCliente);
                        }
                        xlHoja.SaveAs("C:\\Salida.xlsx");
                        libro.Close();
                        excelapp.Quit();
                        File.Delete("C:\\Salida.xlsx");
                    }
                    else
                    {
                        MessageBox.Show("Falta InventarioOrdenado en C://");
                    }
                }
                else if (textBox1.Text.Equals("limpiar archivos")) { }
                else if (textBox1.Text.Split(' ')[0].Equals("eliminarProducto")){
                    if (fresia.eliminarProducto(textBox1.Text.Split(' ')[1]))
                    {
                        MessageBox.Show("Producto eliminado en forma correcta");
                    }
                }
                else if (textBox1.Text.Split(' ')[0].Equals("uploadFile"))
                {
                    if(frecia.uploadFile(textBox1.Text.Split(' ')[1]))
                    {
                        MessageBox.Show("Archivo cargado en forma correcta");
                    }
                    else
                    {
                        MessageBox.Show("Error al cargar el archivo");
                    }
                }
                textBox1.Text = "";
            }
        }
        private void buttonIngresarInventario_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(9);
            preCargarFacturaIngreso();//se encarwga de setear el valor de la fecha y poner los textbox en blanco
            tamañoControlVentanas(6);
            Thread precio = new Thread(new ThreadStart(server));
            precio.Start();
            filtrosNuParte();
        }
        private void filtrosNuParte()
        {
            comboBoxIngresarFacturaNuParte.Items.Clear();
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                comboBoxIngresarFacturaNuParte.Items.Add(item.SubItems[2].Text);
            }
        }
        private void preCargarFacturaIngreso()
        {
            buttonIngresoFacturaRealizar.Visible = true;
            buttonIngresarFacturaIngresar.Text = "Ingresar Productos";
            listViewIngresarFacturaItem.Items.Clear();
            textBoxIngresarFacturaGrupo.Text = "";
            textBoxIngresarFacturaNumeroParte.Text = "";
            textBoxIngresarFacturaDescripcion.Text = "";
            textBoxIngresarFacturaCant.Text = "";
            textBoxIngresarFacturaKilos.Text = "";
            textBoxIngresarFacturaCostoExt.Text = "";
            textBoxIngresarFacturaPVP.Text = "";
            buttonIngresarFacturaConfirmar.Visible = false;
            textBoxFacturaFecha.Text = DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + DateTime.Now.Year;
            textBoxFacturaNumero.Text = "";
            textBoxFacturaProveedor.Text = ""; 
            frecia.indicadoresEconomicos().Save("Indicadores.old");
            XmlDocument indicadores = new XmlDocument();
            indicadores.Load("Indicadores.old");
            foreach (XmlNode nodo in indicadores.DocumentElement.ChildNodes)
            {
                if (nodo.Name == "Dolar")
                {
                    textBoxIngresarFacturaDolar.Text = nodo.InnerText;
                }
                else if (nodo.Name == "Euro")
                {
                    textBoxIngresarFacturaEuro.Text = nodo.InnerText;
                }
                else if (nodo.Name == "Importe")
                {
                    textBoxIngresarFacturaImporte.Text = nodo.InnerText;
                }
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBoxIngresarFacturaFlete.ReadOnly = false;
        }
        private void button9_Click(object sender, EventArgs e)
        {
            int tasaDeCambio = 0;
            double flete = 0;
            if (radioButtonIngresarFacturaEuro.Checked)
            {
                tasaDeCambio = Convert.ToInt32(textBoxIngresarFacturaEuro.Text);
                flete = Convert.ToSingle(new Numeros(textBoxIngresarFacturaKilos.Text).numeroSolo()) * 7;
            }
            else
            {
                tasaDeCambio = Convert.ToInt32(textBoxIngresarFacturaDolar.Text);
                flete = Convert.ToSingle(new Numeros(textBoxIngresarFacturaFlete.Text).numeroSolo());
            }
            double costo = Convert.ToSingle(new Numeros(textBoxIngresarFacturaCostoExt.Text).numeroSolo());
            double importe = Convert.ToSingle(textBoxIngresarFacturaImporte.Text);
            int precioVenta = Convert.ToInt32(new Numeros(textBoxIngresarFacturaPVP.Text).numeroSolo());
            //cambio de formula peticion mauricio
            //double costoChile = (costo / importe) + flete;
            double costoChile = (costo + flete) / importe;
            double costoChilePesos = costoChile * tasaDeCambio;
            ListViewItem itemnuevo = new ListViewItem(textBoxIngresarFacturaFamiliaFantasma.Text); //agrega la familia 
            itemnuevo.SubItems.Add(textBoxIngresarFacturaGrupo.Text); //agrega el grupo del productor
            itemnuevo.SubItems.Add(textBoxIngresarFacturaNumeroParte.Text); //agrega el numero de parte 
            itemnuevo.SubItems.Add(textBoxIngresarFacturaDescripcion.Text); //agrega la descripcion del producto
            itemnuevo.SubItems.Add(textBoxIngresarFacturaCant.Text);//cantidad de producto.
            itemnuevo.SubItems.Add(textBoxIngresarFacturaKilos.Text); //ingresa los kilos del producto
            itemnuevo.SubItems.Add(textBoxIngresarFacturaCostoExt.Text); //ingresa el costo de factura del producto
            itemnuevo.SubItems.Add("" + Math.Round(costoChile, 2)); //ingresa el costo de inventario del producto
            itemnuevo.SubItems.Add("$" + textBoxIngresarFacturaPVP.Text);// ingreso Precio de Venta Producto
            itemnuevo.SubItems.Add("" + Math.Round((((precioVenta - costoChilePesos) / costoChilePesos) * 100), 1) + "%"); //ganancia del producto
            if (radioButtonIngresarFacturaExiste.Checked)
            {
                itemnuevo.SubItems.Add("Existe");
            }
            else
            {
                itemnuevo.SubItems.Add("Nuevo");
            }
            listViewIngresarFacturaItem.Items.Add(itemnuevo);
            textBoxIngresarFacturaGrupo.Text = "";
            textBoxIngresarFacturaNumeroParte.Text = "";
            textBoxIngresarFacturaDescripcion.Text = "";
            textBoxIngresarFacturaCant.Text = "";
            textBoxIngresarFacturaKilos.Text = "";
            textBoxIngresarFacturaCostoExt.Text = "";
            textBoxIngresarFacturaPVP.Text="";
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBoxIngresarFacturaFlete.ReadOnly = true;
        }
        private void comboBoxIngresarFacturaFamilia_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxIngresarFacturaFamiliaFantasma.Text = comboBoxIngresarFacturaFamilia.SelectedItem.ToString();
            if (comboBoxIngresarFacturaFamilia.SelectedItem.ToString() == "Linde" || comboBoxIngresarFacturaFamilia.SelectedItem.ToString() == "Otras Marcas" || comboBoxIngresarFacturaFamilia.SelectedItem.ToString() == "Motores")
            {
                panel7.Visible = false;
                textBoxIngresarFacturaNumeroParte.ReadOnly = false;
            }
            else
            {
                panel7.Visible = true;
                //textBoxIngresarFacturaNumeroParte.ReadOnly = true;
            }
        }
        private void textBoxIngresarFacturaKilos_Leave(object sender, EventArgs e)
        {
            textBoxIngresarFacturaKilos.Text = textBoxIngresarFacturaKilos.Text.Replace(".", ",");
            textBoxIngresarFacturaKilos.Text = "" + Math.Round(Convert.ToSingle(new Numeros(textBoxIngresarFacturaKilos.Text).numeroSolo()),2);
        }
        private void textBoxIngresarFacturaCostoExt_Leave(object sender, EventArgs e)
        {
            textBoxIngresarFacturaCostoExt.Text = textBoxIngresarFacturaCostoExt.Text.Replace(".", ",");
            textBoxIngresarFacturaCostoExt.Text = "" + Math.Round(Convert.ToSingle(new Numeros(textBoxIngresarFacturaCostoExt.Text).numeroSolo()), 2);
            if (radioButtonIngresarFacturaEuro.Checked)
            {
                textBoxIngresarFacturaCostoExt.Text = "€" + textBoxIngresarFacturaCostoExt.Text;
            }
            else
            {
                textBoxIngresarFacturaCostoExt.Text = "$" + textBoxIngresarFacturaCostoExt.Text;
            }
        }
        private void textBoxIngresarFacturaPVP_Leave(object sender, EventArgs e)
        {
            textBoxIngresarFacturaPVP.Text = new Numeros(textBoxIngresarFacturaPVP.Text).numeroMiles();
        }
        private void buttonIngresarFacturaIngresar_Click(object sender, EventArgs e)
        {
            if (buttonIngresarFacturaIngresar.Text.Equals("Ingresar Productos"))
            {
                buttonIngresarFacturaIngresar.Text = "Cancelar";
                buttonIngresarFacturaConfirmar.Visible = true;
                buttonIngresoFacturaRealizar.Visible = false;
            }
            else
            {
                buttonIngresarFacturaIngresar.Text = "Ingresar Productos";
                buttonIngresoFacturaRealizar.Visible = true;
                buttonIngresarFacturaConfirmar.Visible = false;
            }
        }
        private void buttonIngresarFacturaConfirmar_Click(object sender, EventArgs e)
        {
            //envia la factura al servidor
            XElement NuevaFacturaIngreso = new XElement("FacturaIngreso"), FacturaNuevo;
            XElement FacturaProductos = new XElement("productos");
            XElement Data = new XElement("Datos");
            XElement DatosEconomicos = new XElement("indicadores");
            Data.Add(new XElement("ingreso",nombreUser));
            Data.Add(new XElement("proveedor",textBoxFacturaProveedor.Text));
            Data.Add(new XElement("NumeroFactura",textBoxFacturaNumero.Text));
            Data.Add(new XElement("Fecha", textBoxFacturaFecha.Text));
            DatosEconomicos.Add(new XElement("Dolar", textBoxIngresarFacturaDolar.Text));
            DatosEconomicos.Add(new XElement("Euro", textBoxIngresarFacturaEuro.Text));
            DatosEconomicos.Add(new XElement("Importe", textBoxIngresarFacturaImporte.Text));
            int i = 1;
            //todo el proceso para mandar el producto va aca (Y);
            foreach (ListViewItem item in listViewIngresarFacturaItem.Items)
            {
                if (item.SubItems[10].Text.Equals("Nuevo"))
                {
                    string nuparte = "";
                    if (item.Text == "Linde" || item.Text == "Otras Marcas" || item.Text == "Motores")
                    {
                        nuparte = item.SubItems[2].Text;
                    }
                    else
                    {
                        nuparte=obtenerNumeroParte(item.Text);
                    }
                    //desde aca creo el producto para mandarlo a la factura
                    FacturaNuevo = new XElement("producto" + (i++));
                    FacturaNuevo.Add(new XElement("Familia", item.Text));
                    FacturaNuevo.Add(new XElement("Grupo", item.SubItems[1].Text));
                    FacturaNuevo.Add(new XElement("NP", nuparte));
                    FacturaNuevo.Add(new XElement("Descripcion", item.SubItems[3].Text));
                    FacturaNuevo.Add(new XElement("Cantidad", item.SubItems[4].Text));
                    FacturaNuevo.Add(new XElement("Kilos", item.SubItems[5].Text));
                    FacturaNuevo.Add(new XElement("CostoFTR", item.SubItems[6].Text));
                    FacturaNuevo.Add(new XElement("CostoEXT", item.SubItems[7].Text));
                    FacturaNuevo.Add(new XElement("PVP", item.SubItems[8].Text));
                    FacturaProductos.Add(FacturaNuevo);
                    //new XElement("Descripcion",item.SubItems[1].Text)
                    string moneda = "" + item.SubItems[6].Text[0];
                    string fecha = textBoxFacturaFecha.Text;
                    string PrecioMoneda = textBoxIngresarFacturaEuro.Text;
                    if (moneda == "$")
                    {
                        PrecioMoneda = textBoxIngresarFacturaDolar.Text;
                    }
                    double costoExt = Convert.ToDouble(new Numeros(item.SubItems[6].Text).numeroSolo()) * 100;
                    double kilos = Convert.ToDouble(new Numeros(item.SubItems[5].Text).numeroSolo()) * 100;
                    double costoImporte = Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo()) * 100;
                    string datosFactura = textBoxFacturaProveedor.Text + "-" + textBoxFacturaNumero.Text;
                    string complejo = moneda + "¬" + fecha + "¬" + costoImporte + "¬" + PrecioMoneda + "¬" + datosFactura;
                    string[] producto = { nuparte, complejo, item.Text, item.SubItems[1].Text, item.SubItems[4].Text, "" + Math.Round(costoExt, 0), "" + Math.Round(kilos, 0), new Numeros(item.SubItems[8].Text).numeroSolo(), item.SubItems[3].Text };
                    if (fresia.crearProducto(producto))
                    {
                        if (frecia.ingresoPMP(producto[0], 1, 1, Convert.ToInt32(producto[4]), Convert.ToInt32(costoExt), Convert.ToInt32(costoImporte), producto[1].Split('¬')[0]) == false)
                        {
                            MessageBox.Show("error al ingresar producto a ficha PMP");
                        }
                        MessageBox.Show("producto ingresado con exito");
                        listViewIngresarFacturaItem.Items.Remove(item);
                        cargarInventario();
                    }
                    else
                    {
                        MessageBox.Show("producto NO ha podido ser ingresado");
                    }
                }
                else // si ya existe el producto
                {
                    List<string[]> productos = new List<string[]>();
                    fresia.getInventarioXML().Save("inventario.xml.ftr");
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
                    foreach (string[] producto in productos)
                    {
                        if (producto[0].Equals(item.SubItems[2].Text))
                        {
                            FacturaNuevo = new XElement("producto" + (i++));
                            FacturaNuevo.Add(new XElement("Familia", item.Text));
                            FacturaNuevo.Add(new XElement("Grupo", item.SubItems[1].Text));
                            FacturaNuevo.Add(new XElement("NP", item.SubItems[2].Text));
                            FacturaNuevo.Add(new XElement("Descripcion", item.SubItems[3].Text));
                            FacturaNuevo.Add(new XElement("Cantidad", item.SubItems[4].Text));
                            FacturaNuevo.Add(new XElement("Kilos", item.SubItems[5].Text));
                            FacturaNuevo.Add(new XElement("CostoFTR", item.SubItems[6].Text));
                            FacturaNuevo.Add(new XElement("CostoEXT", item.SubItems[7].Text));
                            FacturaNuevo.Add(new XElement("PVP", item.SubItems[8].Text));
                            FacturaProductos.Add(FacturaNuevo);
                            int cantAnt = Convert.ToInt32(producto[4]);
                            int cantNue = Convert.ToInt32(item.SubItems[4].Text);
                            double costoExtNue = Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo()) * 100;
                            double costoExtAnt = Convert.ToDouble(producto[1].Split('¬')[2]);
                            double costoFtrNue = Convert.ToDouble(new Numeros(item.SubItems[6].Text).numeroSolo()) * 100;
                            double costoFtrAnt = Convert.ToDouble(producto[6]);
                            double CantidadTotal = cantAnt + cantNue;
                            double precioMonedaAnt = Convert.ToDouble(producto[1].Split('¬')[3]);
                            double precioMonedaNue = Convert.ToDouble(textBoxIngresarFacturaEuro.Text);
                            if(producto[1].Split('¬')[0]=="$"){
                                precioMonedaNue = Convert.ToDouble(textBoxIngresarFacturaDolar.Text);
                            }
                            double precioMonedaTotal=(precioMonedaNue * (cantNue / CantidadTotal)) + (precioMonedaAnt * (cantAnt / CantidadTotal));
                            double costoFtrTotal = (costoFtrNue * (cantNue / CantidadTotal)) + (costoFtrAnt * (cantAnt / CantidadTotal));
                            double costoExtTotal = (costoExtNue * (cantNue / CantidadTotal)) + (costoExtAnt * (cantAnt / CantidadTotal));
                            string datoComplejoNuev = producto[1].Split('¬')[0]+"¬"+ textBoxFacturaFecha.Text+"¬"+Math.Round(costoExtTotal,0)+"¬"+Math.Round(precioMonedaTotal,0)+"¬"+textBoxFacturaProveedor.Text+"-"+textBoxFacturaNumero.Text;
                            producto[1] = datoComplejoNuev;
                            producto[4] = "" + Math.Round(CantidadTotal, 0);
                            producto[5] = "" + Math.Round(costoFtrTotal, 0);
                            producto[7] = new Numeros(item.SubItems[8].Text).numeroSolo();
                            producto[8] = item.SubItems[3].Text;
                            if (fresia.editarProducto(producto))
                            {
                                if (frecia.ingresoPMP(producto[0], 0, 1, Convert.ToInt32(CantidadTotal), Convert.ToInt32(costoExtTotal), Convert.ToInt32(costoFtrTotal), producto[1].Split('¬')[0]) == false)
                                {
                                    MessageBox.Show("error al ingresar a ficha PMP");
                                }
                                MessageBox.Show("producto editado con exito");
                                cargarInventario();
                            }
                            else
                            {
                                MessageBox.Show("producto no ha podido ser editado");
                            }
                            break;
                        }
                    }
                    //hasta aca!!
                    preCargarFacturaIngreso();
                }
            } // termina el ciclo de cargar productos
            NuevaFacturaIngreso.Add(Data);
            NuevaFacturaIngreso.Add(DatosEconomicos);
            NuevaFacturaIngreso.Add(FacturaProductos);
            if (frecia.IngresarFacturaIngreso(NuevaFacturaIngreso))
            {
                MessageBox.Show("Factura ingresada");
            }
        }
        private void radioButtonIngresarFacturaExiste_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonIngresarFacturaExiste.Checked)
            {
                //comboBoxIngresarFacturaNuParte.Visible = true;
                textBoxIngresarFacturaKilos.ReadOnly = true;
            }
            else
            {
                //omboBoxIngresarFacturaNuParte.Visible = false;
                textBoxIngresarFacturaKilos.ReadOnly = false;
            }
        }

        private void comboBoxIngresarFacturaNuParte_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxIngresarFacturaNumeroParte.Text = comboBoxIngresarFacturaNuParte.SelectedItem.ToString();
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                if (item.SubItems[2].Text.Equals(textBoxIngresarFacturaNumeroParte.Text))
                {
                    textBoxIngresarFacturaDescripcion.Text = item.SubItems[3].Text;
                }
            }
        }
        private void listViewIngresarFacturaItem_MouseClick(object sender, MouseEventArgs e)
        {
            if (listViewIngresarFacturaItem.SelectedItems[0].Selected)
            {
                if (e.Button.ToString() == "Right")
                {
                    contextMenuStrip1.Show((this.Location.X + e.Location.X) + 150, (this.Location.Y + e.Location.Y) + 100);
                }
            }
        }
        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listViewIngresarFacturaItem.Items.Remove(listViewIngresarFacturaItem.SelectedItems[0]);
        }
        private void button6_Click_1(object sender, EventArgs e)
        {
            //FormatoTotalInventario.xlsx
            if (File.Exists("c:\\Inventario\\FormatoTotalInventario2.xlsx") == false)
            {
                new WebClient().DownloadFile("http://25.109.196.97/programa/FormatoTotalInventario2.xlsx", @"C:/Inventario/FormatoTotalInventario2.xlsx");
            }
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("C:/Inventario/FormatoTotalInventario2.xlsx");
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            int i=3;
            xlHoja.Cells[i, 1] = "Familia";
            xlHoja.Cells[i, 2] = "grupo";
            xlHoja.Cells[i, 3] = "N°P";
            xlHoja.Cells[i, 4] = "Descripcion";
            xlHoja.Cells[i, 5] = "Kilos";
            xlHoja.Cells[i, 6] = "Cant";
            xlHoja.Cells[i, 7] = "Costo FTR";
            xlHoja.Cells[i, 8] = "Costo EXT";
            xlHoja.Cells[i, 9] = "Costo Inv Ext";
            xlHoja.Cells[i, 11] = "Costo Unit Ch";
            xlHoja.Cells[i, 12] = "Costo Inv Ch";
            xlHoja.Cells[i, 13] = "PVP";
            xlHoja.Cells[i, 14] = "Margen $";
            xlHoja.Cells[i, 15] = "Margen %";
            xlHoja.Cells[i, 16] = "ultima Factura";
            xlHoja.Cells[i, 17] = "Fecha";
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                i++;
                xlHoja.Cells[i, 1] = item.Text;
                xlHoja.Cells[i, 2] = item.SubItems[1].Text;
                xlHoja.Cells[i, 3] = item.SubItems[2].Text;
                xlHoja.Cells[i, 4] = item.SubItems[3].Text;
                xlHoja.Cells[i, 5] = item.SubItems[4].Text;
                xlHoja.Cells[i, 6] = item.SubItems[5].Text;
                xlHoja.Cells[i, 7] = item.SubItems[6].Text;
                xlHoja.Cells[i, 8] = item.SubItems[7].Text;
                xlHoja.Cells[i, 9] = item.SubItems[8].Text;
                xlHoja.Cells[i, 10] = "=VALOR(SI(\"€\"=IZQUIERDA(I" + i + ");SUSTITUIR(I" + i + ";\"€\";\"\");SUSTITUIR(I" + i + ";\"$\";\"\")))";
                xlHoja.Cells[i, 11] = item.SubItems[9].Text;
                xlHoja.Cells[i, 12] = item.SubItems[10].Text;
                xlHoja.Cells[i, 13] = item.SubItems[11].Text;
                xlHoja.Cells[i, 14] = item.SubItems[12].Text;
                xlHoja.Cells[i, 15] = item.SubItems[13].Text;
                xlHoja.Cells[i, 16] = item.SubItems[14].Text;
                xlHoja.Cells[i, 17] = item.SubItems[15].Text;
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            xlHoja.SaveAs(desktopPath + "/Vista Inventario "+DateTime.Now.Month+"-"+DateTime.Now.Day+".xlsx");
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("Archivo creado con exito en escritorio como Vista Inventario");
        }

        private void textBoxIngresarFacturaDescripcion_Leave(object sender, EventArgs e)
        {
            textBoxIngresarFacturaDescripcion.Text = FormatoDeLinea(textBoxIngresarFacturaDescripcion.Text);
        }

        private void textBoxIngresarFacturaGrupo_Leave(object sender, EventArgs e)
        {
            textBoxIngresarFacturaGrupo.Text = FormatoDeLinea(textBoxIngresarFacturaGrupo.Text);
        }

        private void clickEnColumna(object sender, ColumnClickEventArgs e)
        {

        }
        private void buttonExportarFacturaMes_Click(object sender, EventArgs e)
        {
            if (File.Exists("c:\\Inventario\\formatoResumenFacturasEmitidas2.xlsx") == false)
            {
                new WebClient().DownloadFile("http://25.109.196.97/programa/formatoResumenFacturasEmitidas2.xlsx", @"C:/Inventario/formatoResumenFacturasEmitidas2.xlsx");
            }
            frecia.facturasResumen(Convert.ToInt32(textBoxExtraFechaAño.Text), Convert.ToInt32(textBoxExtraFechaMes.Text)).Save("FacturasEmitidasMes.XML");
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            //Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Add(Missing.Value);
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\formatoResumenFacturasEmitidas2.xlsx");
            xlHojas = libro.Sheets;
            //xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("FacturasEmitidasMes.XML");
            int i = 1;
            xlHoja.Cells[1, 7] = "Neto";
            xlHoja.Cells[1, 8] = "IVA";
            xlHoja.Cells[1, 9] = "Total";
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                xlHoja.Cells[i++, 1] = nodo.Name; //factura por factura
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "MetaData")
                    {
                        xlHoja.Cells[i, 2] = "fecha";
                        xlHoja.Cells[i++, 3] = nodo2.LastChild.InnerText.Split('>')[1].Split('<')[0];
                    }
                    else if (nodo2.Name == "Cliente")
                    {
                        xlHoja.Cells[i++, 2] = "Receptor";
                        foreach (XmlNode cliente in nodo2.ChildNodes)
                        {
                            xlHoja.Cells[i, 3] = cliente.Name; //metadata
                            xlHoja.Cells[i++, 4] = cliente.InnerText;
                        }
                    }
                    else if (nodo2.Name == "productos")
                    {
                        xlHoja.Cells[i, 2] = "Productos";
                        xlHoja.Cells[i, 4] = "Descripcion";
                        xlHoja.Cells[i, 3] = "Cantidad";
                        xlHoja.Cells[i, 5] = "Precio";
                        xlHoja.Cells[i, 6] = "Total";
                        i++;
                        int j = i;
                        foreach (XmlNode productos in nodo2.ChildNodes)
                        {
                            foreach (XmlNode producto in productos.ChildNodes)
                            {
                                if (producto.Name == "Descripcion")
                                {
                                    xlHoja.Cells[i, 4] = producto.InnerText;
                                }
                                else if (producto.Name == "Cantidad")
                                {
                                    xlHoja.Cells[i, 3] = producto.InnerText;
                                }
                                else if (producto.Name == "Precio")
                                {
                                    xlHoja.Cells[i, 5] = producto.InnerText;
                                    xlHoja.Cells[i, 6] = "=C" + i + "*E" + i;
                                    i++;
                                }
                            }
                        }
                        xlHoja.Cells[(i), 7] = "=SUMA(F" + j + ":F" + (i - 1) + ")";
                        xlHoja.Cells[i, 8] = "=G" + i + "*0,19";
                        xlHoja.Cells[i,9] ="=G"+i+"+H"+i;
                        i++;
                    }
                }
            }
            xlHoja.Cells[2, 7] = "=SUMA(G3:G"+i+")";
            xlHoja.Cells[2, 8] = "=SUMA(H3:H"+i+")";
            xlHoja.Cells[2, 9] = "=SUMA(I3:I"+i+")";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            xlHoja.SaveAs(desktopPath + "/Resumen Facturas Emitidas "+textBoxExtraFechaAño.Text+"-"+textBoxExtraFechaMes.Text+".xlsx");
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("Archivo creado con exito en escritorio");
        }
        private void buttonExtras_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectTab(10);
            tamañoControlVentanas(1);
            textBoxExtraFechaAño.Text = DateTime.Now.Year.ToString();
            textBoxExtraFechaMes.Text = DateTime.Now.Month.ToString();
            textBoxExtraNuParte.Text = "";
        }

        private void buttonFechaMesSig_Click(object sender, EventArgs e)
        {
            int año = Convert.ToInt32(textBoxExtraFechaAño.Text);
            int mes = Convert.ToInt32(textBoxExtraFechaMes.Text);
            mes++;
            if (mes > 12)
            {
                año++;
                mes=1;
            }
            textBoxExtraFechaAño.Text = ""+año;
            textBoxExtraFechaMes.Text = ""+mes;
        }

        private void buttonFechaMesAntes_Click(object sender, EventArgs e)
        {
            int año = Convert.ToInt32(textBoxExtraFechaAño.Text);
            int mes = Convert.ToInt32(textBoxExtraFechaMes.Text);
            mes--;
            if (mes == 0)
            {
                año--;
                mes = 12;
            }
            textBoxExtraFechaAño.Text = ""+año;
            textBoxExtraFechaMes.Text = ""+mes;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //formatoResumenFacturasIngresadas.xlsx
            if (File.Exists("c:\\Inventario\\formatoResumenFacturasIngresadas4.xlsx") == false)
            {
                new WebClient().DownloadFile("http://25.109.196.97/programa/formatoResumenFacturasIngresadas4.xlsx", @"C:/Inventario/formatoResumenFacturasIngresadas4.xlsx");
            }
            frecia.facturasIngresoResumen(Convert.ToInt32(textBoxExtraFechaAño.Text), Convert.ToInt32(textBoxExtraFechaMes.Text)).Save("ResumenFacturasIngreso.xml");
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            //Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Add(Missing.Value);
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\formatoResumenFacturasIngresadas4.xlsx");
            xlHojas = libro.Sheets;
            //xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("ResumenFacturasIngreso.xml");
            int i = 4;
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                string sumaEuro="=sumar.si(G";
                string sumaDolar="=sumar.si(G";
                int dondeVaSuma=1;
                xlHoja.Cells[i++, 1] = nodo.Name.Split('.')[0];
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {

                    if (nodo2.Name == "Datos")
                    {
                        foreach (XmlNode nodo3 in nodo2.ChildNodes)
                        {
                            xlHoja.Cells[i, 2] = nodo3.Name;
                            xlHoja.Cells[i++, 3] = nodo3.InnerText;
                        }
                    }
                    else if (nodo2.Name == "indicadores")
                    {
                        i = i - 3;
                        foreach (XmlNode nodo3 in nodo2.ChildNodes)
                        {
                            xlHoja.Cells[i, 5] = nodo3.Name;
                            xlHoja.Cells[i++, 6] = nodo3.InnerText;
                        }
                        i = i - 3;
                        xlHoja.Cells[i++, 7] = "Total";
                        dondeVaSuma = i;
                        xlHoja.Cells[i++, 7] = "€";
                        xlHoja.Cells[i++, 7] = "$";
                    }
                    else if (nodo2.Name == "productos")
                    {
                        xlHoja.Cells[++i,1]="Familia";
                        xlHoja.Cells[i,2]="Grupo";
                        xlHoja.Cells[i,3]="NP";
                        xlHoja.Cells[i,4]="Descripcion";
                        xlHoja.Cells[i,5]="Cant";
                        xlHoja.Cells[i,6]="Kilos";
                        xlHoja.Cells[i,7]="valor FTR";
                        xlHoja.Cells[i, 8] = "Valor FTR Total";
                        xlHoja.Cells[i,9]="Cst IMPORT Total";
                        xlHoja.Cells[i,10]="PVP";
                        sumaEuro = sumaEuro + (i+1) + ":G" + (i + nodo2.ChildNodes.Count) + ";\"€*\";I" + (i+1) + ":I"+(i+nodo2.ChildNodes.Count)+")";
                        sumaDolar = sumaDolar + (i+1) + ":G" + (i + nodo2.ChildNodes.Count) + ";\"$*\";I" + (i+1) + ":I" + (i + nodo2.ChildNodes.Count) + ")";
                        foreach (XmlNode nodo3 in nodo2.ChildNodes)
                        {
                            int j = 1;
                            i++;
                            foreach (XmlNode nodo4 in nodo3.ChildNodes)
                            {
                                if (nodo4.Name == "CostoEXT")
                                {
                                    xlHoja.Cells[i, j++] = "=E" + i + "*" + nodo4.InnerText;
                                }
                                else
                                {
                                    xlHoja.Cells[i, j++] = nodo4.InnerText;
                                    if (nodo4.Name == "CostoFTR")
                                    {
                                        xlHoja.Cells[i, j++] = "=E"+i+"*"+new Numeros(nodo4.InnerText).numeroSolo();
                                    }
                                }
                            }
                        }
                        xlHoja.Cells[dondeVaSuma++, 8] = sumaEuro;
                        xlHoja.Cells[dondeVaSuma, 8] = sumaDolar;
                        i+=2;
                    }
                }
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            xlHoja.SaveAs(desktopPath + "/Resumen Facturas Ingreso " + textBoxExtraFechaAño.Text + "-" + textBoxExtraFechaMes.Text + ".xlsx");
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("Archivo creado con exito en escritorio");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            frecia.obtenerPMP().Save("FichaPMPGeneral.xml");
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Add(Missing.Value);
            //Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\formatoResumenFacturasIngresadas.xlsx");
            xlHojas = libro.Sheets;
            //xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("FichaPMPGeneral.xml");
            xlHoja.Cells[1, 1] = "NP";
            xlHoja.Cells[1, 2] = "Fecha";
            xlHoja.Cells[1, 3] = "Accion";
            xlHoja.Cells[1, 4] = "Cant Total";
            xlHoja.Cells[1, 5] = "Cost Inventario";
            //poner los encabezados de la pmp
            int i = 2;
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                XmlNode preEXP = nuevo.CreateElement("visto");
                preEXP.InnerText = "" + 0;
                nodo.AppendChild(preEXP);
            }
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                if (nodo.LastChild.InnerText == "0")
                {
                    nodo.LastChild.InnerText = "1";
                    string NP=nodo.Name.Split('-')[1];
                    xlHoja.Cells[i, 1] = NP;
                    int j=2;
                    foreach (XmlNode nodo2 in nodo.ChildNodes)
                    {
                        if (nodo2.Name == "fecha")
                        {
                            xlHoja.Cells[i, j++] = nodo2.InnerText;
                        }
                        else if (nodo2.Name == "fue")
                        {
                            if (nodo2.InnerText == "1")
                            {
                                xlHoja.Cells[i, j++] = "Compra";
                            }
                            else
                            {
                                xlHoja.Cells[i, j++] = "Venta";
                            }
                        }
                        else if (nodo2.Name == "cantidad")
                        {
                            xlHoja.Cells[i, j++] = nodo2.InnerText;
                        }
                        else if (nodo2.Name == "costoEXP")
                        {
                            xlHoja.Cells[i, j++] = Math.Round(Convert.ToDouble(nodo2.InnerText)/100,2);
                        }
                    }
                    i++;
                    foreach (XmlNode rebusca in nuevo.DocumentElement.ChildNodes)
                    {
                        if (rebusca.Name == nodo.Name && rebusca.LastChild.InnerText=="0")
                        {
                            rebusca.LastChild.InnerText = "1";
                            j = 2;
                            foreach (XmlNode nodo2 in rebusca.ChildNodes)
                            {
                                if (nodo2.Name == "fecha")
                                {
                                    xlHoja.Cells[i, j++] = nodo2.InnerText;
                                }
                                else if (nodo2.Name == "fue")
                                {
                                    if (nodo2.InnerText == "1")
                                    {
                                        xlHoja.Cells[i, j++] = "Compra";
                                    }
                                    else
                                    {
                                        xlHoja.Cells[i, j++] = "Venta";
                                    }
                                }
                                else if (nodo2.Name == "cantidad")
                                {
                                    xlHoja.Cells[i, j++] = nodo2.InnerText;
                                }
                                else if (nodo2.Name == "costoEXP")
                                {
                                    xlHoja.Cells[i, j++] = Math.Round(Convert.ToDouble(nodo2.InnerText)/100,2);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            xlHoja.SaveAs(desktopPath + "/Ficha PMP" + textBoxExtraFechaAño.Text + "-" + textBoxExtraFechaMes.Text + ".xlsx");
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("Archivo creado con exito en escritorio");
        }

        private void button9_Click_1(object sender, EventArgs e)
        {

            frecia.obtenerPMP().Save("FichaPMPGeneral.xml");
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Add(Missing.Value);
            //Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("c:\\Inventario\\formatoResumenFacturasIngresadas.xlsx");
            xlHojas = libro.Sheets;
            //xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)libro.ActiveSheet;
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("FichaPMPGeneral.xml");
            int i = 1;
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                XmlNode preEXP = nuevo.CreateElement("visto");
                preEXP.InnerText = "" + 0;
                nodo.AppendChild(preEXP);
            }
            foreach (XmlNode nodo in nuevo.DocumentElement.ChildNodes)
            {
                if (nodo.LastChild.InnerText == "0")
                {
                    nodo.LastChild.InnerText = "1";
                    string NP = nodo.Name.Split('-')[1];
                    xlHoja.Cells[i, 1] = NP;
                    int j = 2;
                    foreach (XmlNode nodo2 in nodo.ChildNodes)
                    {
                        if (nodo2.Name == "fecha")
                        {
                            xlHoja.Cells[i, j++] = nodo2.InnerText;
                        }
                        else if (nodo2.Name == "fue")
                        {
                            if (nodo2.InnerText == "1")
                            {
                                xlHoja.Cells[i, j++] = "Compra";
                            }
                            else
                            {
                                xlHoja.Cells[i, j++] = "Venta";
                            }
                        }
                        else if (nodo2.Name == "cantidad")
                        {
                            xlHoja.Cells[i, j++] = nodo2.InnerText;
                        }
                        else if (nodo2.Name == "costoEXP")
                        {
                            xlHoja.Cells[i, j++] = Math.Round(Convert.ToDouble(nodo2.InnerText) / 100, 2);
                        }
                    }
                    i++;
                    foreach (XmlNode rebusca in nuevo.DocumentElement.ChildNodes)
                    {
                        if (rebusca.Name == nodo.Name && rebusca.LastChild.InnerText == "0")
                        {
                            rebusca.LastChild.InnerText = "1";
                            j = 2;
                            foreach (XmlNode nodo2 in rebusca.ChildNodes)
                            {
                                if (nodo2.Name == "fecha")
                                {
                                    xlHoja.Cells[i, j++] = nodo.InnerText;
                                }
                                else if (nodo2.Name == "fue")
                                {
                                    if (nodo2.InnerText == "1")
                                    {
                                        xlHoja.Cells[i, j++] = "Compra";
                                    }
                                    else
                                    {
                                        xlHoja.Cells[i, j++] = "Venta";
                                    }
                                }
                                else if (nodo2.Name == "cantidad")
                                {
                                    xlHoja.Cells[i, j++] = nodo2.InnerText;
                                }
                                else if (nodo2.Name == "costoEXP")
                                {
                                    xlHoja.Cells[i, j++] = Math.Round(Convert.ToDouble(nodo2.InnerText) / 100, 2);
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            xlHoja.SaveAs(desktopPath + "/Ficha PMP" + textBoxExtraFechaAño.Text + "-" + textBoxExtraFechaMes.Text + ".xlsx");
            libro.Close();
            excelapp.Quit();
            MessageBox.Show("Archivo creado con exito en escritorio");
        }
        private void button8_Click(object sender, EventArgs e)
        {
            frecia.setearIndicadoresEconomicos(textBoxIngresarFacturaDolar.Text, textBoxIngresarFacturaEuro.Text, textBoxIngresarFacturaImporte.Text);
        }

        private void comboBoxIngresarFacturaNuParte_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxIngresarFacturaNuParte.Items.Contains(comboBoxIngresarFacturaNuParte.Text))
                radioButtonIngresarFacturaExiste.Checked = true;
            else
                radioButtonIngresarFacturaNuevo.Checked = true;
        }

        private void radioButtonIngresarFacturaNuevo_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}