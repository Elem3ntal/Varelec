using System;
using System.Drawing;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Inventario_3._0
{
    public partial class FormLogeo : Form
    {
        //servidor del programa en el servidor.
        public ServidorVarelec.ServicioClient fresiaServer;
        //hilo para consultar el servidor, si no se hace se queda pegado mientras consulta
        public Thread hilo;
        //todas las veces que dentro del programa necesito la version, sale desde aca
        public string version = "3.0alpha1";
        //variables public para poder tomarlas desde la otra ventana
        public string usuario;
        public int nivel;
        public FormLogeo()
        {
            InitializeComponent();
            hilo = new Thread(new ThreadStart(consultaEstado));
            hilo.Start();
            ponerVersion();
            //cargarUsuarios();
        }
        private void ponerVersion()
        {
            labelVersion.Text = "V: " + version;
        }
        //metodo que se realiza en el otro hilo
        private void consultaEstado()
        {
            actualizarInicio(0); //0 intentando
            //ServidorEnLocalhost.ServicioClient Servidor = new ServidorEnLocalhost.ServicioClient();
            ServidorVarelec.ServicioClient Servidor = new ServidorVarelec.ServicioClient();
            bool estatus = false;
            try
            {
                estatus=Servidor.online();
            }
            catch { }
            if (estatus)
                actualizarInicio(1);//1 conectado
            else
                actualizarInicio(2);//2 desconectado
        }
        //delegate para tomar el proceso desde el otro hilo
        public delegate void actualizarInicioCallback(int estado);
        private void actualizarInicio(int estado)
        {
            //invokerequiered para asegurarse de que el objeto no este ocupado
            if (richTextBoxEstadoServidor.InvokeRequired && buttonIngresar.InvokeRequired && labelDesconectado.InvokeRequired)
            {
                actualizarInicioCallback hilo = new actualizarInicioCallback(actualizarInicio);
                this.Invoke(hilo, new object[] { estado });
            }
            else 
            {
                if (estado == 0) 
                {
                    richTextBoxEstadoServidor.Text = "Conectando-Espere";
                    richTextBoxEstadoServidor.BackColor = Color.Yellow;
                    buttonIngresar.Enabled = false;
                }
                else if (estado == 1)
                {
                    labelDesconectado.Text = "";
                    richTextBoxEstadoServidor.Text = "Conectado";
                    richTextBoxEstadoServidor.BackColor = Color.Green;
                    buttonIngresar.Enabled = true;
                }
                else if (estado == 2)
                {
                    labelDesconectado.Text = "click en desconectado para reintentar";
                    richTextBoxEstadoServidor.Text = "Desconectado";
                    richTextBoxEstadoServidor.BackColor = Color.Red;
                    buttonIngresar.Enabled = false;
                }
                else //en caso de que pase algo raro, en teoria, nunca
                {
                    labelDesconectado.Text = "Contacte a administrador, error desconocido";
                    richTextBoxEstadoServidor.Text = "Error";
                    richTextBoxEstadoServidor.BackColor = Color.Red;
                    buttonIngresar.Enabled = false;
                }
            }
        }

        private void labelVersion_Click(object sender, EventArgs e)
        {
            //click en el numero de version, aca podria poner la lectura de un txt para decir los cambios (algun diaa!!!! )
            MessageBox.Show("ocupando la version "+version+"\nInventario Varelec.", "Version Resumen",MessageBoxButtons.OK);
        }

        private void buttonIngresar_Click(object sender, EventArgs e)
        {
            realizarLogin();
        }
        //toma el usuario y contraseña y lo analiza
        private void realizarLogin()
        {
            //permisos de administrador, no se consulta al servicio
            if (textBoxUsuario.Text == "SuperUser" && textBoxConstraseña.Text == "sudoUser")
            {
                usuario = "Administrador";
                nivel = 211111111;
                this.Hide();
                FormInventario inventario = new FormInventario(this);
                inventario.Show();
            }
            //si no es administrador
            else
            {
                string hash = hashear(textBoxConstraseña.Text + textBoxUsuario.Text);
                nivel = logearUsuario(textBoxUsuario.Text, hash);
                if (nivel == 0)
                {
                    MessageBox.Show("Usuario o Contraseña no Validos");
                }
                else
                {
                    this.Hide();
                    FormInventario nuevo = new FormInventario(this);
                    nuevo.Show();
                }
            }
        }
        private int logearUsuario(string usuario, string hash)
        {
            //instancia el servidor
            fresiaServer = new ServidorVarelec.ServicioClient();
            //obtengo la tabla de usuarios
            fresiaServer.usuariosObtener().Save("usuarios.xml");
            XmlDocument usuarios = new XmlDocument();
            usuarios.Load("usuarios.xml");
            foreach (XmlNode nodo in usuarios.DocumentElement.LastChild)
            {
                //recorro los usuarios
                string nivel = "", nombre = "", pass = "";
                //recorro los datos
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "nombre")
                        nombre = nodo2.InnerText;
                    else if (nodo2.Name == "hash")
                        pass = nodo2.InnerText;
                    else if (nodo2.Name == "nivel")
                        nivel = nodo2.InnerText;
                }
                //una vez tomado los 3 datos, comparo los nombre y usuarios para saber si corresponden
                if (nombre == usuario && pass == hash)
                    return Convert.ToInt32(nivel);
            }
            return 0;
        }
        //encriptacion MD5
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
        //para detectar los enter
        private void textBoxConstraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            //e.keychar 13 es el enter 
            if (e.KeyChar == 13 && buttonIngresar.Enabled == true)
            {
                realizarLogin();
            }
        }
        //para volver a probar la conexión
        private void richTextBoxEstadoServidor_Click(object sender, EventArgs e)
        {
            if (richTextBoxEstadoServidor.Text.Equals("Desconectado"))
            {
                hilo = new Thread(new ThreadStart(consultaEstado));
                hilo.Start();
            }
        }
    }
}
