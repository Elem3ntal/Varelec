using System;
using System.Data;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.ServiceModel;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace servidorVarelec
{
    public partial class Form1 : Form
    {
        //direccion hamachi servidor: 25.109.196.97
        public Uri baseAddress = new Uri("http://25.109.196.97:931/ServidorVarelec");
        public ServiceHost host;
        public bool localhost = false;
        public bool servidorAbierto = true;
        public string Version = "0.7";
        public Form1()
        {
            InitializeComponent();
            iniciarServidor();
        }
        public void iniciarServidor()
        {
            labelVersion.Text = "V: " + Version;
            richTextBoxEstado.Text = "esperando";
            richTextBoxEstado.BackColor = Color.Yellow;
            iniciarHost();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            aModoIcono();
        }
        private void aModoIcono()
        {
            this.Hide();
            this.Visible = false;
            notifyIcon1.Visible = !this.Visible;
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.BalloonTipText = "Para más información abra la ventana";
            notifyIcon1.BalloonTipTitle = "servidor Activo";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.ShowBalloonTip(100);
        }
        private void mostrarVentanaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = !this.Visible;
            notifyIcon1.Visible = !this.Visible;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            detenerHost();
            this.Dispose();
        }
        private void iniciarHost()
        {
            host = new ServiceHost(typeof(Servicio), baseAddress);
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);
            host.Open();
            richTextBoxEstado.Text = "servicio disponible en " + baseAddress;
            richTextBoxEstado.BackColor = Color.Green;
            buttonIniciarServidor.Enabled = false;
            buttonDetenerServidor.Enabled = true;
            servidorAbierto = true;
        }
        private void detenerHost()
        {
            host.Close();
            richTextBoxEstado.Text = "servicio detenido";
            richTextBoxEstado.BackColor = Color.Red;
            buttonIniciarServidor.Enabled = true;
            buttonDetenerServidor.Enabled = false;
            servidorAbierto = false;
        }
        private void buttonDetenerServidorClick(object sender, EventArgs e)
        {
            detenerHost();
        }
        private void buttonIniciarServidorClick(object sender, EventArgs e)
        {
            iniciarHost();
        }

        private void buttonCrearTabla_Click(object sender, EventArgs e)
        {
            int resultado;
            int cantColumnas=Convert.ToInt32(textBoxExtrada.Text.Split(',')[1]);
            List<string> columnas= new List<string>();
            foreach(string columna in textBoxExtrada.Text.Split(',')[2].Split(' '))
                columnas.Add(columna);
            string [] nombreColumnas = new string[columnas.Count];
            int i =0;
            foreach(string columna in columnas)
                nombreColumnas[i++]=columna;
            if(servidorAbierto)
            {
                detenerHost();
                DBXML temp = new DBXML(textBoxExtrada.Text.Split(',')[0]);
                resultado=temp.crearTabla(cantColumnas,nombreColumnas);
                temp.cerrarTabla();
                iniciarHost();
            }
            else
            {
                DBXML temp = new DBXML(textBoxExtrada.Text.Split(',')[0]);
                resultado = temp.crearTabla(cantColumnas, nombreColumnas);
                temp.cerrarTabla();
            }
            if (resultado == 0)
                textBoxExtrada.Text = "Excepcion realizada, revisar txt";
            else if (resultado == 1)
                textBoxExtrada.Text = "DBXML creada en forma exitosa";
            else if (resultado == 2)
                textBoxExtrada.Text = "Error - El numero de columnas es menor a 1";
            else if (resultado == 3)
                textBoxExtrada.Text = "Error - El nombre contiene datos";
        }

        private void buttonEliminarFila_Click(object sender, EventArgs e)
        {
            int resultado;
            if (servidorAbierto)
            {
                detenerHost();

                DBXML temp = new DBXML(textBoxExtrada.Text.Split(',')[0]);
                temp.abrirTabla();
                resultado = temp.eliminarFila(textBoxExtrada.Text.Split(',')[1]);
                temp.cerrarTabla();
                iniciarHost();
            }
            else
            {
                DBXML temp = new DBXML(textBoxExtrada.Text.Split(',')[0]);
                temp.abrirTabla();
                resultado = temp.eliminarFila(textBoxExtrada.Text.Split(',')[1]);
                temp.cerrarTabla();
            }
            if (resultado == 0)
                textBoxExtrada.Text = "Excepcion realizada, revisar txt";
            else if (resultado == 1)
                textBoxExtrada.Text = "Eliminacion realizada en forma exitosa";
            else if (resultado == 2)
                textBoxExtrada.Text = "Elemento no encontrado para eliminar";
        }

        private void buttonObtenerVersion_Click(object sender, EventArgs e)
        {
            if (servidorAbierto)
            {
                detenerHost();
                DBXML temp = new DBXML(textBoxExtrada.Text);
                temp.abrirTabla();
                string salida = temp.obtenerVersion() + ", del: " + temp.obtenerFecha();
                temp.cerrarTabla();
                textBoxExtrada.Text = "NVersion: " + salida;
                iniciarHost();
            }
            else
            {
                DBXML temp = new DBXML(textBoxExtrada.Text);
                temp.abrirTabla();
                string salida = temp.obtenerVersion();
                temp.cerrarTabla();
                textBoxExtrada.Text = "NVersion: " + salida;
                
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            aModoIcono();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            detenerHost();
            if(!localhost)
                baseAddress = new Uri("http://25.109.196.97:931/ServidorVarelec");
            if(localhost)
                baseAddress = new Uri("http://25.109.196.97:931/ServidorVarelec");
            localhost = !localhost;
            iniciarHost();
        }
    }
}
