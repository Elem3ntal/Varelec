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

namespace Inventario
{
    public partial class FormInventario
    {
        public List<string> filtroFamilia;
        public List<string> filtroGrupo;
        public List<string> filtroNuParte;
        public List<string> filtroDescripcion;
        public void cargarFiltros() //funcion principal que llena las listas de filtros.
        {
            barraEstadoOcupado("Leyendo Filtros");
            filtroFamilia = new List<string>();
            filtroGrupo = new List<string>();
            filtroNuParte = new List<string>();
            filtroDescripcion = new List<string>();
            filtroFamilia.Add("---");
            filtroGrupo.Add("---");
            filtroGrupo.Add("---");
            filtroDescripcion.Add("---");
            foreach (ListViewItem item in listViewTOTALinventario.Items)
            {
                if (!filtroFamilia.Contains(item.Text))
                    filtroFamilia.Add(item.Text);
                if (!filtroGrupo.Contains(item.SubItems[1].Text))
                    filtroGrupo.Add(item.SubItems[1].Text);
                if (!filtroNuParte.Contains(item.SubItems[2].Text))
                    filtroNuParte.Add(item.SubItems[2].Text);
                if (!filtroDescripcion.Contains(item.SubItems[3].Text))
                    filtroDescripcion.Add(item.SubItems[3].Text);
            }
        }
        public void cargarFiltrosModificarInventario()
        {
            cargarFiltros();
            barraEstadoOcupado("Ocultando filtros");
            comboBoxModInventarioFamilia.Visible = false;
            comboBoxModInventarioGrupo.Visible = false;
            comboBoxModInventarioNuParte.Visible = false;
            comboBoxModInvetarioDescripcion.Visible = false;
            barraEstadoOcupado("Cargando Filtros a Modificar Inventario");
            foreach (string palabra in filtroFamilia)
                comboBoxModInventarioFamiliaADD(palabra);
            foreach (string palabra in filtroGrupo)
                comboBoxModInventarioGrupoADD(palabra);
            foreach (string palabra in filtroNuParte)
                comboBoxModInventarioNuParteADD(palabra);
            foreach (string palabra in filtroDescripcion)
                comboBoxModInvetarioDescripcionADD(palabra);
            comboBoxModInventarioFamilia.Visible = true;
            comboBoxModInventarioGrupo.Visible = true;
            comboBoxModInventarioNuParte.Visible = true;
            comboBoxModInvetarioDescripcion.Visible = true;
            barraEstadoLibre();
        }
        public void cargarFiltrosVerInventario()
        {
            cargarFiltros();
            barraEstadoOcupado("Ocultando filtros");
            comboBoxVerInventarioFamilia.Visible = false;
            comboBoxVerInventarioGrupo.Visible = false;
            comboBoxVerInventarioNP.Visible = false;
            comboBoxVerInventarioDescripcion.Visible = false;
            barraEstadoOcupado("Cargando Filtros a Ver Inventario");
            foreach (string palabra in filtroFamilia)
                comboBoxVerInventarioFamiliaADD(palabra);
            foreach (string palabra in filtroGrupo)
                comboBoxVerInventarioGrupoADD(palabra);
            foreach (string palabra in filtroNuParte)
                comboBoxVerInventarioNPADD(palabra);
            foreach (string palabra in filtroDescripcion)
                comboBoxVerInventarioDescripcionADD(palabra);
            comboBoxVerInventarioFamilia.Visible = true;
            comboBoxVerInventarioGrupo.Visible = true;
            comboBoxVerInventarioNP.Visible = true;
            comboBoxVerInventarioDescripcion.Visible = true;
            barraEstadoLibre();
        }
        public void cargarFiltrosVerStock()
        {
            cargarFiltros();
            barraEstadoOcupado("Ocultando filtros");
            comboBoxVerStockFamilia.Visible = false;
            comboBoxVerStockGrupo.Visible = false;
            comboBoxVerStockNP.Visible = false;
            comboBoxVerStockDescrip.Visible = false;
            barraEstadoOcupado("Cargando Filtros a Ver Stock");
            foreach (string palabra in filtroFamilia)
                comboBoxVerStockFamiliaADD(palabra);
            foreach (string palabra in filtroGrupo)
                comboBoxVerStockGrupoADD(palabra);
            foreach (string palabra in filtroNuParte)
                comboBoxVerStockNPADD(palabra);
            foreach (string palabra in filtroDescripcion)
                comboBoxVerStockDescripADD(palabra);
            comboBoxVerStockFamilia.Visible = true;
            comboBoxVerStockGrupo.Visible = true;
            comboBoxVerStockNP.Visible = true;
            comboBoxVerStockDescrip.Visible = true;
            barraEstadoLibre();
        }
        public void limpiarFiltrosModificarInventario()
        {
            barraEstadoOcupado("Limpiando filtros Mod Inventario");
            comboBoxModInventarioFamilia.Items.Clear();
            comboBoxModInventarioGrupo.Items.Clear();
            comboBoxModInventarioNuParte.Items.Clear();
            comboBoxModInvetarioDescripcion.Items.Clear();
        }
        public void limpiarFiltrosVerInventario()
        {
            barraEstadoOcupado("Limpiando filtros Ver Inventario");
            comboBoxVerInventarioFamilia.Items.Clear();
            comboBoxVerInventarioGrupo.Items.Clear();
            comboBoxVerInventarioNP.Items.Clear();
            comboBoxVerInventarioDescripcion.Items.Clear();
        }
        public void limpiarFiltrosVerStock()
        {
            barraEstadoOcupado("Limpiando filtros Ver Stock");
            comboBoxVerStockFamilia.Items.Clear();
            comboBoxVerStockGrupo.Items.Clear();
            comboBoxVerStockNP.Items.Clear();
            comboBoxVerInventarioDescripcion.Items.Clear();
        }
        //todos los callback autogenerados en copy paste
        public delegate void comboBoxModInventarioFamiliaADDCallback(string item);
        private void comboBoxModInventarioFamiliaADD(string item)
        {
            if (comboBoxModInventarioFamilia.InvokeRequired)
            {
                comboBoxModInventarioFamiliaADDCallback d = new comboBoxModInventarioFamiliaADDCallback(comboBoxModInventarioFamiliaADD);
                this.Invoke(d, new object[]{item});
            }
            else
                comboBoxModInventarioFamilia.Items.Add(item);
        }
        public delegate void comboBoxVerInventarioFamiliaADDCallback(string item);
        private void comboBoxVerInventarioFamiliaADD(string item)
        {
            if (comboBoxVerInventarioFamilia.InvokeRequired)
            {
                comboBoxVerInventarioFamiliaADDCallback d = new comboBoxVerInventarioFamiliaADDCallback(comboBoxVerInventarioFamiliaADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerInventarioFamilia.Items.Add(item);
        }
        public delegate void comboBoxVerStockFamiliaADDCallback(string item);
        private void comboBoxVerStockFamiliaADD(string item)
        {
            if (comboBoxVerStockFamilia.InvokeRequired)
            {
                comboBoxVerStockFamiliaADDCallback d = new comboBoxVerStockFamiliaADDCallback(comboBoxVerStockFamiliaADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerStockFamilia.Items.Add(item);
        }
        public delegate void comboBoxModInventarioGrupoADDCallback(string item);
        private void comboBoxModInventarioGrupoADD(string item)
        {
            if (comboBoxModInventarioGrupo.InvokeRequired)
            {
                comboBoxModInventarioGrupoADDCallback d = new comboBoxModInventarioGrupoADDCallback(comboBoxModInventarioGrupoADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxModInventarioGrupo.Items.Add(item);
        }
        public delegate void comboBoxVerInventarioGrupoADDCallback(string item);
        private void comboBoxVerInventarioGrupoADD(string item)
        {
            if (comboBoxVerInventarioGrupo.InvokeRequired)
            {
                comboBoxVerInventarioGrupoADDCallback d = new comboBoxVerInventarioGrupoADDCallback(comboBoxVerInventarioGrupoADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerInventarioGrupo.Items.Add(item);
        }
        public delegate void comboBoxVerStockGrupoADDCallback(string item);
        private void comboBoxVerStockGrupoADD(string item)
        {
            if (comboBoxVerStockGrupo.InvokeRequired)
            {
                comboBoxVerStockGrupoADDCallback d = new comboBoxVerStockGrupoADDCallback(comboBoxVerStockGrupoADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerStockGrupo.Items.Add(item);
        }
        public delegate void comboBoxModInventarioNuParteADDCallback(string item);
        private void comboBoxModInventarioNuParteADD(string item)
        {
            if (comboBoxModInventarioNuParte.InvokeRequired)
            {
                comboBoxModInventarioNuParteADDCallback d = new comboBoxModInventarioNuParteADDCallback(comboBoxModInventarioNuParteADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxModInventarioNuParte.Items.Add(item);
        }
        public delegate void comboBoxVerInventarioNPADDCallback(string item);
        private void comboBoxVerInventarioNPADD(string item)
        {
            if (listViewTOTALinventario.InvokeRequired)
            {
                comboBoxVerInventarioNPADDCallback d = new comboBoxVerInventarioNPADDCallback(comboBoxVerInventarioNPADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerInventarioNP.Items.Add(item);
        }
        public delegate void comboBoxVerStockNPADDCallback(string item);
        private void comboBoxVerStockNPADD(string item)
        {
            if (comboBoxVerStockNP.InvokeRequired)
            {
                comboBoxVerStockNPADDCallback d = new comboBoxVerStockNPADDCallback(comboBoxVerStockNPADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerStockNP.Items.Add(item);
        }
        public delegate void comboBoxModInvetarioDescripcionADDCallback(string item);
        private void comboBoxModInvetarioDescripcionADD(string item)
        {
            if (comboBoxModInvetarioDescripcion.InvokeRequired)
            {
                comboBoxModInvetarioDescripcionADDCallback d = new comboBoxModInvetarioDescripcionADDCallback(comboBoxModInvetarioDescripcionADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxModInvetarioDescripcion.Items.Add(item);
        }
        public delegate void comboBoxVerInventarioDescripcionADDCallback(string item);
        private void comboBoxVerInventarioDescripcionADD(string item)
        {
            if (comboBoxVerInventarioDescripcion.InvokeRequired)
            {
                comboBoxVerInventarioDescripcionADDCallback d = new comboBoxVerInventarioDescripcionADDCallback(comboBoxVerInventarioDescripcionADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerInventarioDescripcion.Items.Add(item);
        }
        public delegate void comboBoxVerStockDescripADDCallback(string item);
        private void comboBoxVerStockDescripADD(string item)
        {
            if (comboBoxVerStockDescrip.InvokeRequired)
            {
                comboBoxVerStockDescripADDCallback d = new comboBoxVerStockDescripADDCallback(comboBoxVerStockDescripADD);
                this.Invoke(d, new object[] { item });
            }
            else
                comboBoxVerStockDescrip.Items.Add(item);
        }
    }
}
