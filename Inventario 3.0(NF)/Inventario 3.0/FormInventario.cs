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

namespace Inventario_3._0
{
    public partial class FormInventario : Form
    {
        //los dos servicios, el de monedas, y el propio
        public ServidorMoneda.CurrencyConvertorSoapClient internacional = new ServidorMoneda.CurrencyConvertorSoapClient("CurrencyConvertorSoap");
        public ServidorVarelec.ServicioClient servidor;
        //public ServidorEnLocalhost.ServicioClient servidor = new ServidorEnLocalhost.ServicioClient();
        //control de la ventana maestra
        public FormLogeo padre;
        //hilo para mostrar el estado del servidor
        public Thread estadoServidor;
        public FormInventario(FormLogeo padre_)
        {
            padre = padre_;
            servidor = padre.fresiaServer;
            InitializeComponent();
            datosLogin();
            cargarBotones();
            consultarEstadoServidor();
            cargarMemos();
            cargarInventario();
            //OBSDC activar estas dos opciones
            //buscarActualizaciones();
            //descargarActualizaciones();
        }
        private void buscarActualizaciones()
        {
            Thread actualizaciones = new Thread(new ThreadStart(descargarActualizaciones));
            actualizaciones.Start();
        }
        public void descargarActualizaciones() 
        {
            ServidorVarelec.ServicioClient servidor = new ServidorVarelec.ServicioClient();
            string disponible = servidor.consultarActualizacion();
            if (!disponible.Equals(padre.version)) 
            {
                mensajeAccion("Descargando nueva version desde servidor");
                //ruta de donde descarga la actualiacion para que un 2º programa lo tome.
                string rutaCarpetaPublica =@"C:\Inventario\actualizaciones\";
                DirectoryInfo directorioVer = new DirectoryInfo(rutaCarpetaPublica);
                if (directorioVer.Exists == false)
                    directorioVer.Create();
                new WebClient().DownloadFile("http://25.109.196.97/ActualizadorVarelec/Inventario.XML", rutaCarpetaPublica + "Inventario.XML");
                XmlDocument data2 = new XmlDocument();
                data2.Load(rutaCarpetaPublica+"Inventario.XML");
                foreach (XmlNode archivo in data2.DocumentElement.LastChild.ChildNodes)
                {
                    string nombreArchivo = archivo.InnerText.Replace("http://25.109.196.97/ActualizadorVarelec/", "");
                    if (nombreArchivo.Contains("/"))
                    {
                        string nombre = nombreArchivo.Split('/')[0];
                        DirectoryInfo carpeta = new DirectoryInfo(rutaCarpetaPublica + nombre + @"\");
                        if (carpeta.Exists == false)
                            carpeta.Create();
                    }
                    new WebClient().DownloadFile(archivo.InnerText, rutaCarpetaPublica + nombreArchivo);
                }
                MessageBox.Show("Nueva actualizacion descargada, V:" + disponible + "\nReinicie el programa para instalar");
                mensajeAccion("Nueva Version descargada");
            }
        }
        private void datosLogin()
        {
            this.Text = padre.usuario;
        }
        private void cargarBotones()
        {
            richTextBoxAccionServidor.Text = "Cargando botones y ventanas";
            //indice son los permisos del nivel del usuario
            // la variable "y" es es la altura que va cambiando
            int indice = padre.nivel, y = 9;
            //tomo uno a uno los valores sacando el modulo de 10 y comprando
            if (indice % 10 == 1)
            {
                buttonIngresarInventario.Visible = true;
                buttonIngresarInventario.Location = new Point(5, y);
                y += 29;
                buttonModInventario.Visible = true;
                buttonModInventario.Location = new Point(5, y);
                y += 29;
            }
            else { 
                tabControlVentanas.TabPages.Remove(tabPageIngresarInventario);
                tabControlVentanas.TabPages.Remove(tabPageModificarInventario);
            }
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonVerInventario.Visible = true;
                buttonVerInventario.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageVerInventario);
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonStock.Visible = true;
                buttonStock.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageVerStock);
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonCotizacion.Visible = true;
                buttonCotizacion.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageRevisarCotizacion);
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonFactura.Visible = true;
                buttonFactura.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageGenerarFactura);
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonUsuarios.Visible = true;
                buttonUsuarios.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageUsuarios);
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonClientes.Visible = true;
                buttonClientes.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageClientes);
            indice = indice / 10;
            if (indice % 10 == 1)
            {
                buttonDocumentos.Visible = true;
                buttonDocumentos.Location = new Point(5, y);
                y += 29;
            }
            else
                tabControlVentanas.TabPages.Remove(tabPageDocumentos);
            indice = indice / 10;
            if (indice % 10 == 1 || indice % 10 == 2)
            {
                buttonInicio.Visible = true;
                buttonInicio.Location = new Point(5, y);
                y += 29;
                if (indice % 10 == 2)
                    buttonEnviarMemo.Enabled = true;
                else
                    buttonEnviarMemo.Enabled = false;
            }
            richTextBoxAccionServidor.Text = "";
        }

        private void resizearPanelVentanas()
        {
            tabControlVentanas.Height = 20 + panelVentanas.Height;
            tabControlVentanas.Width = panelVentanas.Width;
        }
        private void buttonIngresarInventario_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en Ingregar Inventario";
            tabControlVentanas.SelectedTab = tabPageIngresarInventario;
        }

        private void buttonStock_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en ver stock";
            tabControlVentanas.SelectedTab = tabPageVerStock;
            pasarInventarioAVerStock();
            cargarFiltrosInventario();
        }

        private void buttonCotizacion_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en revisar cotización";
            tabControlVentanas.SelectedTab = tabPageRevisarCotizacion;
        }

        private void buttonFactura_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en generar factura";
            tabControlVentanas.SelectedTab = tabPageGenerarFactura;
        }

        private void buttonUsuarios_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en usuarios";
            tabControlVentanas.SelectedTab = tabPageUsuarios;
        }

        private void buttonClientes_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en ver clientes";
            tabControlVentanas.SelectedTab = tabPageClientes;
            cargarClientes();
        }

        private void buttonDocumentos_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en documentos extras";
            tabControlVentanas.SelectedTab = tabPageDocumentos;
        }

        private void buttonInicio_Click(object sender, EventArgs e)
        {
            tabControlVentanas.SelectedTab = tabPageInicial;
            cargarMemos();
        }

        private void buttonVerInventario_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en ver inventario";
            tabControlVentanas.SelectedTab = tabPageVerInventario;
            pasarInventarioAVerInventario();
        }

        private void buttonModInventario_Click(object sender, EventArgs e)
        {
            richTextBoxAccionServidor.Text = "Click en modificar inventario";
            tabControlVentanas.SelectedTab = tabPageModificarInventario;
            pasarInventarioAmodIventario();
        }
        // si se manda a cerrar, el metodo para cerrar el hilo
        private void FormInventario_FormClosing(object sender, FormClosingEventArgs e)
        {
            richTextBoxAccionServidor.Text = "Cerrando Programa";
            cerrarPadre();
        }
        private void cerrarPadre()
        {
            //cerrar el hilo, y luego la ventana anterior
            estadoServidor.Abort();
            padre.Close();
        }
        private void consultarEstadoServidor()
        {
            estadoServidor = new Thread(new ThreadStart(status));
            estadoServidor.Start();
        }
        //status es el proceso que corre en otro hilo
        private void status()
        {
            //instancio el server para consultar
            ServidorVarelec.ServicioClient servidor = new ServidorVarelec.ServicioClient();
            //ServidorEnLocalhost.ServicioClient servidor = new ServidorEnLocalhost.ServicioClient();
            //corre mientras corra el programa
            while (true)
            {
                try
                {
                    //si esta offline falla
                    actualizarEstadoServidor(servidor.online());
                }
                catch 
                {
                    actualizarEstadoServidor(false);
                }
                //lo hago descanzar para no estresar el programa y servidor
                Thread.Sleep(1000);
            }
        }
        //delegado para pasar info a traves de los hilos
        public delegate void actualizarEstadoServidorCallback(bool estado);
        public void actualizarEstadoServidor(bool estado)
        {
            if (richTextBoxEstadoServidor.InvokeRequired)
            {
                try
                {
                    actualizarEstadoServidorCallback d = new actualizarEstadoServidorCallback(actualizarEstadoServidor);
                    this.Invoke(d, new object[] { estado });
                }
                catch { }
            }
            else
            {
                if (estado)
                {
                    richTextBoxEstadoServidor.Text = "Conectado";
                    richTextBoxEstadoServidor.BackColor = Color.Green;
                }
                else
                {
                    //si estaba en conectado y cambio le informo que se desconecto
                    if (richTextBoxEstadoServidor.Text.Equals("Conectado"))
                        MessageBox.Show("Se ha perdido la conexión con el servidor\nRevise su conexión", "Error de conexión", MessageBoxButtons.OK);
                    richTextBoxEstadoServidor.Text = "Desconectado";
                    richTextBoxEstadoServidor.BackColor = Color.Red;
                }
            }
        }
        public delegate void mensajeAccionCallback(string mensaje);
        public void mensajeAccion(string mensaje)
        {
            if (richTextBoxAccionServidor.InvokeRequired)
            {
                try
                {
                    mensajeAccionCallback d = new mensajeAccionCallback(mensajeAccion);
                    this.Invoke(d, new object[] { mensaje });
                }
                catch { }
            }
            else
            {
                richTextBoxAccionServidor.Text = mensaje;
            }
        }
        private void buttonEnviarMemo_Click(object sender, EventArgs e)
        {
            enviarMemo();
        }
        private void enviarMemo() 
        {
            bool envio = false;
            try
            {
                envio = servidor.escribirMemo(padre.usuario, textBoxMemo.Text);
            }
            catch { }
            if (envio)
            {
                cargarMemos();
            }
            else
                MessageBox.Show("El servicio presenta problemas, contacte administrador");
        }
        private void cargarMemos()
        {
            mensajeAccion("cargando memos");
            textBoxMemo.Text = "";
            listBoxMemos.Items.Clear();
            try
            {
                mensajeAccion("Descargando memos del servidor");
                servidor.obtenerMemosGenrales().Save("memos.XML");
                XmlDocument memos = new XmlDocument();
                memos.Load("memos.XML");
                foreach (XmlNode nodo in memos.DocumentElement.ChildNodes)
                {
                    listBoxMemos.Items.Insert(0, nodo.Name + "- " + nodo.InnerText);
                }
                richTextBoxAccionServidor.Text = "";
            }
            catch
            {
                listBoxMemos.Items.Add("Error al cargar mensajes");
            }
        }
        private void textBoxMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                enviarMemo();
            }
        }
        private void CambioTamaño(object sender, EventArgs e)
        {
            resizearPanelVentanas();
            richTextBoxAccionServidor.Text = "Tamaño Ventana: " + this.Width + " - " + this.Height;
        }
        private string[] cargarMonedas() 
        {
            string[] retorno = { "USD-U$", "EUR-€", "CLP-$" };
            return retorno;
        }
        private void buttonClientesVerOcultar_Click(object sender, EventArgs e)
        {
            mostrarAgregarModificarClientes();
        }
        private void buttonClientesAceptar_Click(object sender, EventArgs e)
        {
            agregarOModificar();
        }        
    }
}
