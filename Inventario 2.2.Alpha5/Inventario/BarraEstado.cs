using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Inventario
{
    public partial class FormInventario
    {
        public delegate void barraEstadoOcupadoCallback(string mensaje);
        private void barraEstadoOcupado(string mensaje)
        {
            if (richTextBoxBarraEstado.InvokeRequired)
            {
                barraEstadoOcupadoCallback d = new barraEstadoOcupadoCallback(barraEstadoOcupado);
                this.Invoke(d, new object[] { mensaje });
            }
            else
            {
                richTextBoxBarraEstado.BackColor = Color.Yellow;
                richTextBoxBarraEstado.Text = "Ocupado en: "+ mensaje;
            }
        }
        public delegate void barraEstadoLibreCallback();
        private void barraEstadoLibre()
        {
            if (richTextBoxBarraEstado.InvokeRequired)
            {
                barraEstadoLibreCallback d = new barraEstadoLibreCallback(barraEstadoLibre);
                this.Invoke(d);
            }
            else
            {
                richTextBoxBarraEstado.BackColor = Color.White;
                richTextBoxBarraEstado.Text = "";
            }
        }
        public delegate void barraEstadoHechoCallback(string mensaje);
        private void barraEstadoHecho(string mensaje)
        {
            if (richTextBoxBarraEstado.InvokeRequired)
            {
                barraEstadoHechoCallback d = new barraEstadoHechoCallback(barraEstadoHecho);
                this.Invoke(d, new object[] { mensaje });
            }
            else
                richTextBoxBarraEstado.Text=mensaje;
        }
    }
}
