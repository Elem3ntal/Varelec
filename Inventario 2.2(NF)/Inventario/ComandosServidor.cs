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
        public List<string[]> productos;
        private void cambioMonedasServidor()
        /*toma los € y los cambia por EUR
         * los $ por USD        */
        {
            cargarWeas();
            XmlDocument lista = new XmlDocument();
            lista.Load("items.xml");
            foreach (string[] producto in productos)
                //Console.WriteLine(producto);
                servidor.inventarioModificar(producto);
        }
        public void descargarXML()
        {
            ServiceReference2.VarelecServiceClient fresia = new ServiceReference2.VarelecServiceClient();
            fresia.getClientesXML().Save(@"C:\clientes.xml");
            fresia.getInventarioXML().Save(@"c:\inventario.xml");
        }
        public void cargarWeas()
        {
            productos = new List<string[]>();
            servidor.inventarioObtener().Save("items.dbxml");
            XmlDocument nuevo = new XmlDocument();
            nuevo.Load("items.dbxml");
            foreach (XmlNode nodo in nuevo.DocumentElement.LastChild.ChildNodes)
            {
                string[] producto = { "", "", "", "", "", "", "", "", "" };
                foreach (XmlNode nodo2 in nodo.ChildNodes)
                {
                    if (nodo2.Name == "Codigo")
                    {
                        producto[0] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Origen")
                    {
                        string[] origen = { nodo2.InnerText.Split('¬')[0], nodo2.InnerText.Split('¬')[1], nodo2.InnerText.Split('¬')[2], nodo2.InnerText.Split('¬')[3], nodo2.InnerText.Split('¬')[4] };
                        if (origen[0] == "$")
                            origen[0] = "USD";
                        else
                            origen[0] = "EUR";
                        string salida = origen[0] + "¬" + origen[1] + "¬" + origen[2] + "¬" + origen[3] + "¬" + origen[4];
                        producto[1] = salida;
                        Console.WriteLine(nodo2.InnerText + "\n" + salida);
                    }
                    else if (nodo2.Name == "Familia")
                    {
                        producto[2] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Grupo")
                    {
                        producto[3] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Cant")
                    {
                        producto[4] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "CostoUnitEuto")
                    {
                        producto[5] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "CostoUnitPeso")
                    {
                        producto[6] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "PVP")
                    {
                        producto[7] = nodo2.InnerText;
                    }
                    else if (nodo2.Name == "Descripcion")
                    {
                        producto[8] = nodo2.InnerText;
                    }
                }
                productos.Add(producto);
            }
        }
    }
}
