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
        private void cargarFiltrosInventario()
        {
            mensajeAccion("Cargando filtros");
            if (consultarVersionInventario())
                servidor.inventarioObtener().Save("inventario.DBXML");
            XmlDocument inventario = new XmlDocument();
            inventario.Load("inventario.DBXML");
            foreach (XmlNode producto in inventario.DocumentElement.LastChild.ChildNodes)
            {
                foreach (XmlNode datos in producto.ChildNodes)
                {
                    if (datos.Name.Equals("DATO0"))//numero de parte, como es unico, no lo comparo si existe
                        comboBoxVerStockNuparte.Items.Add(datos.InnerText);
                    else if (datos.Name.Equals("DATO1"))//familia
                    {
                        if(!comboBoxVerStockFamilia.Items.Contains(datos.InnerText))
                            comboBoxVerStockFamilia.Items.Add(datos.InnerText);
                    }
                    else if (datos.Name.Equals("DATO2"))//grupo
                    {
                        if (!comboBoxVerStockGrupo.Items.Contains(datos.InnerText))
                            comboBoxVerStockGrupo.Items.Add(datos.InnerText);
                    }
                }
            }
            mensajeAccion("");
        }
        private void filtrar(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                filtrarDescripcionVerStock();
            if (e.KeyChar == 13 && textBoxVerStockDescripcion.Text == "")
                pasarInventarioAVerStock();
        }
        private void filtrarDescripcionVerStock()
        {
            mensajeAccion("Aplicando filtro");
            foreach (ListViewItem item in listViewVerStock.Items)
            {
                if (!item.SubItems[3].Text.ToLower().Contains(textBoxVerStockDescripcion.Text))
                    listViewVerStock.Items.Remove(item);
            }
            mensajeAccion("");
        }
        private void filtrarNuParteStock(object sender, EventArgs e)
        {
            foreach(ListViewItem item in listViewVerStock.Items)
            {
                if (!item.SubItems[2].Text.Equals(comboBoxVerStockNuparte.SelectedItem.ToString()))
                    listViewVerStock.Items.Remove(item);
            }
        }
    }
}
