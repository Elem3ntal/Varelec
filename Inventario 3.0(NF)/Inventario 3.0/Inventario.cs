using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Inventario_3._0
{
    public partial class FormInventario
    {
        private bool consultarVersionInventario()
        {
            string version = "0";
            try
            {
                XmlDocument versionInventario = new XmlDocument();
                versionInventario.Load("inventario.DBXML");
                version = versionInventario.DocumentElement.FirstChild.LastChild.InnerText;
            }
            catch { }
            return !version.Equals(servidor.inventarioNVersion());
        }
        private void cargarInventario()
        {
            mensajeAccion("Consultando por inventario nuevo");
            if (consultarVersionInventario())
                servidor.inventarioObtener().Save("inventario.DBXML");
            cargarTotalInventario();
        }
        private void cargarTotalInventario()
        {
            string[] monedas = cargarMonedas();
            mensajeAccion("Cargando inventario");
            XmlDocument inventario = new XmlDocument();
            inventario.Load("inventario.DBXML");
            foreach (XmlNode producto in inventario.DocumentElement.LastChild.ChildNodes)
            {
                ListViewItem item = new ListViewItem();
                for (int i = 0; i < 15; i++)//iteracion para crear los 15 sub items de un producto
                {
                    item.SubItems.Add("");
                }
                string signoMoneda = "";
                int precioMoneda = 0;
                foreach (XmlNode datos in producto.ChildNodes)
                {
                    if (datos.Name.Equals("DATO0"))//contiene en numero de parte
                        item.SubItems[2].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO1"))//familia
                        item.SubItems[0].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO2"))//grupo
                        item.SubItems[1].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO3"))//descripcion
                        item.SubItems[3].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO4"))//kilos*100 1kg es 100, 1,23 kilos es 123
                        item.SubItems[4].Text = "" + Math.Round(Convert.ToDouble(datos.InnerText) / 100, 2);
                    else if (datos.Name.Equals("DATO5"))//cantidad
                        item.SubItems[5].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO6"))//costo de factura *100 1euro es 100, 1,12e es 112
                        item.SubItems[6].Text = "" + Math.Round(Convert.ToDouble(datos.InnerText) / 100, 2);
                    else if (datos.Name.Equals("DATO7"))//costo ext, incluye importacion, mismo formato*100
                        item.SubItems[7].Text = "" + Math.Round(Convert.ToDouble(datos.InnerText) / 100, 2);
                    else if (datos.Name.Equals("DATO8"))//precioVenta Publico
                        item.SubItems[11].Text = "$" + new Numeros(datos.InnerText).numeroMiles();
                    else if (datos.Name.Equals("DATO9"))//ultima factura
                        item.SubItems[14].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO10"))//ultima fecha
                        item.SubItems[15].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO11"))//tipo de moneda
                    {
                        foreach (string moneda in monedas)//USD-U$, EUR-€,CLP-$, etc....
                        {
                            if (moneda.Split('-')[0].Equals(datos.InnerText))
                            { // si calza con alguna moneda, le pone la moneda en costo factura, costo ext
                                signoMoneda = moneda.Split('-')[1];
                                item.SubItems[6].Text = signoMoneda + item.SubItems[6].Text;
                                item.SubItems[7].Text = signoMoneda + item.SubItems[7].Text;
                            }
                        }
                    }
                    else if (datos.Name.Equals("DATO12"))//valor de la moneda
                        precioMoneda = Convert.ToInt32(datos.InnerText);
                }
                //costo de inventario, es la multiplicacion de costo exterior por cantidad
                item.SubItems[8].Text = signoMoneda + Math.Round(((Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo())) * (Convert.ToDouble(item.SubItems[5].Text))), 2);
                //costo unitario chile, multiplicacion del precio de la moneda por el costo exterior
                item.SubItems[9].Text = "$" + new Numeros(Convert.ToInt32(precioMoneda * Convert.ToDouble(new Numeros(item.SubItems[7].Text).numeroSolo()))).numeroMiles();
                //Costo de inventario en Chile, costo unitario chile por la cantidad
                item.SubItems[10].Text = "$" + new Numeros(Convert.ToInt32(Convert.ToDouble(item.SubItems[5].Text) * Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()))).numeroMiles();
                //margen monetario
                item.SubItems[12].Text = "" + (Convert.ToInt32(new Numeros(item.SubItems[11].Text).numeroSolo()) - Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()));
                double numero = ((Convert.ToDouble(item.SubItems[12].Text) * 100) / Convert.ToInt32(new Numeros(item.SubItems[9].Text).numeroSolo()));
                //porcentaje de ganancia
                item.SubItems[13].Text = (Math.Round(numero, 1)) + "%";
                item.SubItems[12].Text = "$" + new Numeros(item.SubItems[12].Text).numeroMiles();
                //se añade el item completo
                listViewTotalInventario.Items.Add(item);
            }
            mensajeAccion("Inventario cargado");
        }
        //------------------METODOS PARA TRASPASAR INVENTARIO-------------
        //pasa el inventario a modificar Inventario
        private void pasarInventarioAmodIventario()
        {
            if (consultarVersionInventario())
            {
                servidor.inventarioObtener().Save("inventario.DBXML");
                cargarTotalInventario();
            }
            listViewModInventario.Items.Clear();
            foreach (ListViewItem item in listViewTotalInventario.Items)
            {
                listViewModInventario.Items.Add((ListViewItem)item.Clone());
            }
        }
        //pasa el inventario a ver inventario
        private void pasarInventarioAVerInventario()
        {
            if (consultarVersionInventario())
            {
                servidor.inventarioObtener().Save("inventario.DBXML");
                cargarTotalInventario();
            }
            listViewVerInventario.Items.Clear();
            foreach (ListViewItem item in listViewTotalInventario.Items)
            {
                listViewVerInventario.Items.Add((ListViewItem)item.Clone());
            }
        }
        //pasa el inventario a ver stock
        private void pasarInventarioAVerStock()
        {
            if (consultarVersionInventario())
            {
                servidor.inventarioObtener().Save("inventario.DBXML");
                cargarTotalInventario();
            }
            listViewVerStock.Items.Clear();
            foreach (ListViewItem item in listViewTotalInventario.Items)
            {
                ListViewItem nuevo = new ListViewItem();
                nuevo.SubItems[0].Text = item.SubItems[0].Text;
                nuevo.SubItems.Add(item.SubItems[1].Text);
                nuevo.SubItems.Add(item.SubItems[2].Text);
                nuevo.SubItems.Add(item.SubItems[3].Text);
                nuevo.SubItems.Add(item.SubItems[5].Text);
                nuevo.SubItems.Add(item.SubItems[12].Text);
                listViewVerStock.Items.Add(nuevo);
            }
        }
    }
}
