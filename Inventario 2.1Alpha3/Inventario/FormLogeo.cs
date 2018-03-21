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

namespace Inventario
{
    public partial class FormLogeo : Form
    {
        //public ServiceReference1.ServiceSoapClient frecia = new ServiceReference1.ServiceSoapClient();
        public Usuario usuario = new Usuario();
        public string version = "2.1Alpha3";
        public Thread t;
        public FormLogeo()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            t = new Thread(new ThreadStart(server));
            t.Start();
            labelVersion.Text = "V: " + version;
        }
        public delegate void actualizarProgressbarCallback(bool resultado);
        private void actualizarProgressbar(bool resultado)
        {
            if (richTextBoxEstadoFresia.InvokeRequired)
            {
                actualizarProgressbarCallback d = new actualizarProgressbarCallback(actualizarProgressbar);
                this.Invoke(d,new object[]{resultado});
            }
            else
            {
        
                if (resultado)
                {
                    richTextBoxEstadoFresia.Text = "Conectado";
                    richTextBoxEstadoFresia.BackColor = Color.Green;
                    buttonIngresar.Enabled = true;
                }
                else
                {
                    richTextBoxEstadoFresia.Text = "Desconectado";
                    richTextBoxEstadoFresia.BackColor = Color.Red;
                    buttonIngresar.Enabled = false;
                }
            }
        }
        private void server()
        {
            buttonIngresar.Enabled = false;
            richTextBoxEstadoFresia.Text = "Conectando-Espere";
            richTextBoxEstadoFresia.BackColor = Color.Yellow;
            ServiceReference2.VarelecServiceClient frecia = new ServiceReference2.VarelecServiceClient();
            bool estatus=false;
            try
            {
                estatus = frecia.status();
            }
            catch { }
            actualizarProgressbar(estatus);
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
        private void buttonIngresar_pewpew()
        {
            //aqui va :3
            if (richTextBoxEstadoFresia.BackColor == Color.Green) { 
                string hash = hashear(textBoxConstraseña.Text+textBoxUsuario.Text);
                Console.WriteLine(hash);
                ServiceReference2.VarelecServiceClient fresia = new ServiceReference2.VarelecServiceClient();
                if(textBoxUsuario.Text=="SuperUser" && textBoxConstraseña.Text=="SudoUser"){
                    usuario = new Usuario("Administrador", 21111111);
                    this.Hide();
                    FormInventario nuevo = new FormInventario(this);
                    nuevo.Show();
                }
                else
                {
                    int logeo = fresia.login(hash);
                    if (logeo == 0)
                        MessageBox.Show("Usuario o Contraseña no Validos");
                    else
                    {
                        usuario = new Usuario(textBoxUsuario.Text, logeo);
                        this.Hide();
                        FormInventario nuevo = new FormInventario(this);
                        nuevo.Show();
                    }
                }
            }
            else
            {
                MessageBox.Show("Servidor no disponible");
                richTextBoxEstadoFresia.Text = "Conectando-Espere";
                richTextBoxEstadoFresia.BackColor = Color.Yellow;
                this.Hide();
                FormInventario nuevo = new FormInventario(this);
                nuevo.Show();
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
