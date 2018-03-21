using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Xml;
using System.IO;
using System.Net;

namespace Inventario
{
    public partial class FormLogeo : Form
    {
        //servidor del programa en el servidor.
        public ServidorVarelec.ServicioClient servidor;
        public Usuario usuario = new Usuario();
        public string version = "2.2.Alpha5";
        public Thread t;
        public FormLogeo()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            XmlDocument actual = new XmlDocument();
            /*if (!File.Exists("C:/Inventario/ExtraFiles"+version))
            {
                StreamWriter ExtraFIles = new StreamWriter("C:/Inventario/ExtraFiles" + version);
                ExtraFIles.WriteLine();
                ExtraFIles.Close();
                new WebClient().DownloadFile("http://25.109.196.97/programa/app.config", @"C:/Inventario/Service References/Debug/Invenario.exe.config");
                DirectoryInfo directorioServer = new DirectoryInfo(@"c:\Inventario\Service References\ServidorVarelec");
                if (!directorioServer.Exists)
                    directorioServer.Create();
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/configuration.svcinfo", @"C:/Inventario/Service References/ServidorVarelec/configuration.svcinfo");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/configuration91.svcinfo", @"C:/Inventario/Service References/ServidorVarelec/configuration91.svcinfo");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/Reference.cs", @"C:/Inventario/Service References/ServidorVarelec/Reference.cs");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/Reference.svcmap", @"C:/Inventario/Service References/ServidorVarelec/Reference.svcmap");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/Servicio.wsdl", @"C:/Inventario/Service References/ServidorVarelec/Servicio.wsdl");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/ServidorVarelec.disco", @"C:/Inventario/Service References/ServidorVarelec/ServidorVarelec.disco");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/ServidorVarelec.xsd", @"C:/Inventario/Service References/ServidorVarelec/ServidorVarelec.xsd");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/ServidorVarelec1.xsd", @"C:/Inventario/Service References/ServidorVarelec/ServidorVarelec1.xsd");
                new WebClient().DownloadFile("http://25.109.196.97/programa/ServidorVarelec/ServidorVarelec2.xsd", @"C:/Inventario/Service References/ServidorVarelec/ServidorVarelec2.xsd");
                
            }*/
            t = new Thread(new ThreadStart(server));
            t.Start();
            labelVersion.Text = "V: " + version;
        }
        public delegate void actualizarProgressbarCallback(int estado);
        private void actualizarProgressbar(int estado)
        {
            //invokerequiered para asegurarse de que el objeto no este ocupado
            if (richTextBoxEstadoFresia.InvokeRequired && buttonIngresar.InvokeRequired)
            {
                actualizarProgressbarCallback hilo = new actualizarProgressbarCallback(actualizarProgressbar);
                this.Invoke(hilo, new object[] { estado });
            }
            else
            {
                if (estado == 0)
                {
                    richTextBoxEstadoFresia.Text = "Conectando-Espere";
                    richTextBoxEstadoFresia.BackColor = Color.Yellow;
                    buttonIngresar.Enabled = false;
                }
                else if (estado == 1)
                {
                    richTextBoxEstadoFresia.Text = "Conectado";
                    richTextBoxEstadoFresia.BackColor = Color.Green;
                    buttonIngresar.Enabled = true;
                }
                else if (estado == 2)
                {
                    richTextBoxEstadoFresia.Text = "Desconectado";
                    richTextBoxEstadoFresia.BackColor = Color.Red;
                    buttonIngresar.Enabled = false;
                }
                else //en caso de que pase algo raro, en teoria, nunca
                {
                    richTextBoxEstadoFresia.Text = "Error";
                    richTextBoxEstadoFresia.BackColor = Color.Red;
                    buttonIngresar.Enabled = false;
                }
            }
        }
        private void server()
        {
            actualizarProgressbar(0); //0 intentando
            //ServidorEnLocalhost.ServicioClient Servidor = new ServidorEnLocalhost.ServicioClient();
            ServidorVarelec.ServicioClient Servidor = new ServidorVarelec.ServicioClient();
            bool estatus = false;
            try
            {
                estatus = Servidor.online();
            }
            catch { }
            if (estatus)
                actualizarProgressbar(1);//1 conectado
            else
                actualizarProgressbar(2);//2 desconectado
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
        private int logearUsuario(string usuario, string hash)
        {
            //instancia el servidor
            servidor = new ServidorVarelec.ServicioClient();
            //obtengo la tabla de usuarios
            servidor.usuariosObtener().Save("usuarios.xml");
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
        private void buttonIngresar_pewpew()
        {
            realizarLogin();
        }
        //toma el usuario y contraseña y lo analiza
        private void realizarLogin()
        {
            //permisos de administrador, no se consulta al servicio
            if (textBoxUsuario.Text == "SuperUser" && textBoxConstraseña.Text == "sudoUser")
            {
                usuario = new Usuario("Administrador", 211111111);
                this.Hide();
                FormInventario inventario = new FormInventario(this);
                inventario.Show();
            }
            //si no es administrador
            else
            {
                string hash = hashear(textBoxConstraseña.Text + textBoxUsuario.Text);
                int nivel = logearUsuario(textBoxUsuario.Text, hash);
                if (nivel == 0)
                {
                    MessageBox.Show("Usuario o Contraseña no Validos");
                }
                else
                {
                    usuario = new Usuario(textBoxUsuario.Text, nivel);
                    this.Hide();
                    FormInventario nuevo = new FormInventario(this);
                    nuevo.Show();
                }
            }
        }
        private void FormLogeo_Load(object sender, EventArgs e)
        {

        }
        private void textBoxConstraseña_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && buttonIngresar.Enabled==true)
            {
                buttonIngresar_pewpew();
            }
        }
        private void buttonIngresar_Click(object sender, EventArgs e)
        {
            buttonIngresar_pewpew();
        }

        private void FormLogeo_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("ocupando la version "+version+"\nInventario Varelec.", "Sin Version Resumen", MessageBoxButtons.OKCancel);
        }
    }
}
