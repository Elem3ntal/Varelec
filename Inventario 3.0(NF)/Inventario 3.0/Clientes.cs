using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Inventario_3._0
{
    // todos los metodos de la ventana de clientes.
    public partial class FormInventario
    {
        //ClientesAgregarMoficiar, 1 para agregar, 2 para modificar.
        public int clientesAgregarModificar = 0;
        private void mostrarAgregarModificarClientes()
        {
            if (buttonClientesVerOcultar.Text == "Agregar")
            {
                clientesAgregarModificar = 1;
                tableLayoutPanelClientes.RowStyles[2].Height = 180;
                buttonClientesVerOcultar.Text = "Ocultar";
            }
            else
            {
                tableLayoutPanelClientes.RowStyles[2].Height = 40;
                buttonClientesVerOcultar.Text = "Agregar";
            }
        }
        private void agregarOModificar()
        {
            if (clientesAgregarModificar == 1) //1 para agregar
            {
                string[] nuevo = { textBoxClientesRut.Text, textBoxClientesRazonSocial.Text, textBoxClientesGiro.Text, textBoxClientesDireccion.Text, textBoxClientesComuna.Text, textBoxClientesCiudad.Text, textBoxClientesTelefono.Text, textBoxClientesCorreo.Text, "No registra."};
                mensajeAccion("Enviando usuario a servidor");
                int retorno=servidor.clientesAgregar(nuevo);
                //0 - excepcio, 1 - realiazdo con exito, 2 - error, 3- llave primaria no existe. 
                if (retorno == 0)
                    MessageBox.Show("Se ha producido un error en el servidor,\nContactar Administrador","Excepción encontrada",MessageBoxButtons.OK,MessageBoxIcon.Error);
                else if (retorno == 1)
                {
                    //limpiar textboxs
                }
                else if (retorno == 2)
                {
                    //error, rut ya existe
                }
                else if (retorno == 3)
                    MessageBox.Show("Faltan datos", "Error encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if(clientesAgregarModificar==0)
            {
                string[] nuevo = { textBoxClientesRut.Text, textBoxClientesRazonSocial.Text, textBoxClientesGiro.Text, textBoxClientesDireccion.Text, textBoxClientesComuna.Text, textBoxClientesCiudad.Text, textBoxClientesTelefono.Text, textBoxClientesCorreo.Text, "No registra." };
                mensajeAccion("Enviando modificacion de usuario a servidor");
                int retorno = servidor.clientesModificar(nuevo);
                if (retorno == 0)
                    MessageBox.Show("Se ha producido un error en el servidor,\nContactar Administrador", "Excepción encontrada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (retorno == 1)
                {
                    //realizado con exito, limpiar los textbox
                }
                else if (retorno == 2)
                {
                    MessageBox.Show("Faltan datos", "Error encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (retorno == 3)
                    MessageBox.Show("Cliente no encontrado", "Error encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool consultarVersionClientes()
        {
            string version = "0";
            try
            {
                XmlDocument versionInventario = new XmlDocument();
                versionInventario.Load("clientes.DBXML");
                version = versionInventario.DocumentElement.FirstChild.LastChild.InnerText;
            }
            catch { }
            return !version.Equals(servidor.inventarioNVersion());
        }
        private void cargarClientes() 
        {
            mensajeAccion("Consultando por Clientes nuevos");
            if (consultarVersionClientes())
                servidor.clientesObtener().Save("clientes.DBXML");
            cargarTotalClientes();
        }
        private void cargarTotalClientes()
        {
            mensajeAccion("Cargando clientes");
            XmlDocument clientes = new XmlDocument();
            clientes.Load("clientes.DBXML");
            foreach (XmlNode cliente in clientes.DocumentElement.LastChild.ChildNodes)
            {
                ListViewItem item = new ListViewItem();
                for (int i = 0; i < 8; i++)//iteracion para crear los 15 sub items de un producto
                {
                    item.SubItems.Add("");
                }
                foreach (XmlNode datos in cliente.ChildNodes)
                {
                    if (datos.Name.Equals("DATO0"))//rut
                        item.SubItems[0].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO1"))//razon social
                        item.SubItems[1].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO2"))//giro
                        item.SubItems[2].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO3"))//direccion
                        item.SubItems[3].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO4"))//comuna
                        item.SubItems[4].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO5"))//ciudad
                        item.SubItems[6].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO6"))//telefono
                        item.SubItems[7].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO7"))//correo
                        item.SubItems[8].Text = datos.InnerText;
                    else if (datos.Name.Equals("DATO8"))//ultima fecha compra - monto
                        item.SubItems[9].Text = datos.InnerText;
                }
                listViewClientes.Items.Add(item);
                mensajeAccion("Clientes cargados");
            }
        }
    }
}
