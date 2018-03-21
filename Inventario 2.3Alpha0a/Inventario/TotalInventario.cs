using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
/*
 * contiene:
 * LLenado de totalInventario
 * de totalInventario a:
 *                      modificarInventario
 *                      verinventario
 *                      ver stock
 */
namespace Inventario
{
    public partial class FormInventario
    {
        public Thread TotalInvetario;
        //para modificar iventario
        private void cargarModificarInventarioCompleto() //este metodo es llamado por el boton
        {
            listViewTOTALinventario.Items.Clear();
            TotalInvetario = new Thread(new ThreadStart(cargarModificarInventario));
            TotalInvetario.Start();
        }
        private void cargarModificarInventario() //metodo en segundo hilo
        {
            cargarInventario();
            deTotalInventarioAModificarInventario();
            cargarFiltrosModificarInventario();
            //cargarTotalInventario();
        }
        private void deTotalInventarioAModificarInventario()
        {
            double euro = 0, dolar = 0, pesos = 0;
            listViewModInventario.Visible = false;
            listViewModInventario.Items.Clear();
            CompareInfo myComp = CultureInfo.InvariantCulture.CompareInfo;
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                listViewModInventario.Items.Add((ListViewItem)item.Clone());
                if (myComp.IsPrefix(item.SubItems[8].Text, "€")) // si es euro
                    euro = euro + Convert.ToDouble(new Numeros(item.SubItems[8].Text).numeroSolo());
                if (myComp.IsPrefix(item.SubItems[8].Text, "U$")) // si es euro
                    dolar = dolar + Convert.ToDouble(new Numeros(item.SubItems[8].Text).numeroSolo());
                if (myComp.IsPrefix(item.SubItems[8].Text, "$")) // si es euro
                    pesos = pesos + Convert.ToDouble(new Numeros(item.SubItems[8].Text).numeroSolo());
            }
            enviarTotalEuro(euro);
            enviarTotalDolar(dolar);
            enviarTotalPesos(pesos);
            listViewModInventario.Visible = true;
        }
        public delegate void enviarTotalEuroCallback(double euro);
        private void enviarTotalEuro(double euro)
        {
            if (textBoxModInventarioTotalEuro.InvokeRequired)
            {
                enviarTotalEuroCallback d = new enviarTotalEuroCallback(enviarTotalEuro);
                this.Invoke(d, new object[] { euro });
            }
            else
                textBoxModInventarioTotalEuro.Text = "€" + Math.Round(euro, 2);
        }
        public delegate void enviarTotalDolarCallback(double dolar);
        private void enviarTotalDolar(double dolar)
        {
            if (textBoxModInventarioTotalDolar.InvokeRequired)
            {
                enviarTotalDolarCallback d = new enviarTotalDolarCallback(enviarTotalDolar);
                this.Invoke(d, new object[] { dolar });
            }
            else
                textBoxModInventarioTotalDolar.Text = "U$" + Math.Round(dolar, 2);
        }
        public delegate void enviarTotalPesosCallback(double pesos);
        private void enviarTotalPesos(double pesos)
        {
            if (textBoxModInventarioTotalPesos.InvokeRequired)
            {
                enviarTotalPesosCallback d = new enviarTotalPesosCallback(enviarTotalPesos);
                this.Invoke(d, new object[] { pesos });
            }
            else
                textBoxModInventarioTotalPesos.Text = "$" + Math.Round(pesos, 0);
        }
        //para ver Inventario
        private void cargarVerInventarioCompleto() //este metodo es llamado por el boton
        {
            listViewTOTALinventario.Items.Clear();
            TotalInvetario = new Thread(new ThreadStart(cargarInventarioCompletoHilo2));
            TotalInvetario.Start();
        }
        private void cargarInventarioCompletoHilo2() //se crea este trabajo en un hilo aparte 
        {
            cargarInventario();
            deTotalInventarioAVerInventario();
            cargarFiltrosVerInventario();
        }
        private void cargarVerStockCompleto() //metodo llamado por el boton
        {
            listViewTOTALinventario.Items.Clear();
            TotalInvetario = new Thread(new ThreadStart(cargarVerStock));
            TotalInvetario.Start();
        }
        private void cargarVerStock() //metodo en segundo hilo
        {
            cargarInventario();
            deTotalInventarioAVerStock();
            cargarFiltrosVerStock();
        }
        private void deTotalInventarioAVerStock()
        {
            listViewVerStock.Visible = false;
            listViewVerStock.Items.Clear();
            listViewTOTALintermedio.Items.Clear();
            foreach(ListViewItem item in listViewTOTALinventario.Items)
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
            listViewVerStock.Visible = true;
        }
        public delegate void actualizarTotalInventarioCallback(ListViewItem item);
        private void actualizarTotalInventario(ListViewItem item)
        {
            if (listViewTOTALinventario.InvokeRequired)
            {
                actualizarTotalInventarioCallback d = new actualizarTotalInventarioCallback(actualizarTotalInventario);
                this.Invoke(d, new object[] { item });
            }
            else
                listViewTOTALinventario.Items.Add(item);
        }
        public delegate void actualizarVerInventarioCallback(ListViewItem item);
        private void actualizarVerInventario(ListViewItem item)
        {
            if (listViewVerInventario.InvokeRequired)
            {
                actualizarVerInventarioCallback d = new actualizarVerInventarioCallback(actualizarTotalInventario);
                this.Invoke(d, new object[] { item });
            }
            else
                listViewVerInventario.Items.Add(item);
        }
        private void deTotalInventarioAVerInventario()
        {
            barraEstadoOcupado("pasando inventario a Ver Inventario");
            listViewVerInventario.Items.Clear();
            listViewVerInventario.Visible = false;
            foreach (ListViewItem item in listViewTOTALinventario.Items)
                listViewVerInventario.Items.Add((ListViewItem)item.Clone());
            barraEstadoLibre();
            listViewVerInventario.Visible = true;

        }
        private void cargarInventario()
        {
            barraEstadoOcupado("Descargando Inventario");
            servidor.inventarioObtener().Save("items.dbxml");
            barraEstadoOcupado("Cargando Inventario");
            XmlDocument lista = new XmlDocument();
            lista.Load("items.dbxml");
            foreach (XmlNode nodo in lista.DocumentElement.LastChild.ChildNodes)
            {
                ListViewItem item = new ListViewItem();
                for (int i = 0; i < 19; i++)//iteracion para crear los 19 sub items de un producto
                {
                    item.SubItems.Add("");
                }
                string signoMoneda = "EUR";
                int moneda = 0;
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    //MessageBox.Show("nombre del nodo: " + nodo.Name);
                    //MessageBox.Show("nombre del nodo2: " + nodo2.Name);
                    if (nodo2.Name == "familia") //familia del producto
                        item.Text = nodo2.InnerText;
                    else if (nodo2.Name == "nuParte") //N°P
                        item.SubItems[2].Text = nodo2.InnerText;
                    else if (nodo2.Name == "descrip")
                        item.SubItems[3].Text = nodo2.InnerText;
                    else if (nodo2.Name == "tipoMoneda")
                    {
                        if (nodo2.InnerText == "EUR")
                            signoMoneda = "€";
                        else if (nodo2.InnerText == "USD")
                            signoMoneda = "U$";
                        else if (nodo2.InnerText == "CLP")
                            signoMoneda = "$";
                        item.SubItems[19].Text = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "costoEXT")
                        item.SubItems[7].Text = nodo2.InnerText;
                    else if (nodo2.Name == "valorMoneda")
                        moneda = Convert.ToInt32(nodo2.InnerText);
                    // origen tenia signo moneda - fecha - costo importe - precio moneda - ultimaFactura
                    else if (nodo2.Name == "grupo")
                        item.SubItems[1].Text = nodo2.InnerText;
                    else if (nodo2.Name == "cantidad")
                        item.SubItems[5].Text = nodo2.InnerText;
                    else if (nodo2.Name == "costoFTR")
                        item.SubItems[6].Text = nodo2.InnerText;
                    else if (nodo2.Name == "kilos")//peso kilogramos del producto
                        item.SubItems[4].Text = nodo2.InnerText;
                    else if (nodo2.Name == "pvp")
                        item.SubItems[11].Text = "$" + new Numeros(nodo2.InnerText).numeroMiles();
                    else if (nodo2.Name == "ultimaFacturaIngreso")
                        item.SubItems[14].Text = nodo2.InnerText;
                    else if (nodo2.Name == "ultimaFechaIngreso")
                        item.SubItems[15].Text = nodo2.InnerText;
                    else if (nodo2.Name == "ultimoMovimiento")
                        item.SubItems[16].Text = nodo2.InnerText;
                    else if (nodo2.Name == "flete")
                        item.SubItems[17].Text = nodo2.InnerText;
                    else if (nodo2.Name == "importe")
                        item.SubItems[18].Text = nodo2.InnerText;
                }
                item.SubItems[8].Text = signoMoneda + Math.Round((Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo()) * Convert.ToInt32(item.SubItems[5].Text)), 2);
                item.SubItems[9].Text = "$" + new Numeros(Convert.ToInt64((moneda * Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo())))).numeroMiles();
                item.SubItems[10].Text = "$" + new Numeros(Convert.ToInt64(new Numeros(item.SubItems[9].Text).numeroSolo()) * Convert.ToInt32(item.SubItems[5].Text)).numeroMiles();
                item.SubItems[12].Text = "" + (Convert.ToInt32(new Numeros(item.SubItems[11].Text).numeroSolo()) - Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()));
                double numero = ((Convert.ToDouble(item.SubItems[12].Text) * 100) / Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()));
                item.SubItems[13].Text = (Math.Round(numero, 1)) + "%";
                item.SubItems[12].Text = "$" + new Numeros(item.SubItems[12].Text).numeroMiles();
                actualizarTotalInventario(item);
            }
            barraEstadoLibre();
        }
    }
}
