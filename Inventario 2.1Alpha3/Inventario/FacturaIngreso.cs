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
using System.Globalization;

namespace Inventario
{
    /* clase donde estan contenidas todas las funciones ocupadas en la ventana de Ingreso Inventario.
     */
    public partial class FormInventario
    {
        private void enviarProductosAlServidor()
        //metodo llamado por el boton aceptar, este es el principal
        {
            barraEstadoOcupado("Ingresando al Inventario");
            XElement NuevaFacturaIngreso;
            if (!revisarXMLIngresarFactura())//en caso de que no exista una factura de ingreso pendiente
            {
                barraEstadoOcupado("Respaldando Factura de Ingreso");
                NuevaFacturaIngreso = new XElement("FacturaIngreso");
                XElement FacturaNuevo;
                XElement FacturaProductos = new XElement("productos");
                XElement Data = new XElement("Datos");
                XElement DatosEconomicos = new XElement("indicadores");
                Data.Add(new XElement("ingreso", nombreUser));
                Data.Add(new XElement("proveedor", textBoxFacturaProveedor.Text));
                Data.Add(new XElement("NumeroFactura", textBoxFacturaNumero.Text));
                Data.Add(new XElement("Fecha", textBoxFacturaFecha.Text));
                DatosEconomicos.Add(new XElement("Dolar", textBoxIngresarFacturaDolar.Text));
                DatosEconomicos.Add(new XElement("Euro", textBoxIngresarFacturaEuro.Text));
                DatosEconomicos.Add(new XElement("Importe", textBoxIngresarFacturaImporte.Text));
                int i = 1;
                foreach (ListViewItem item in listViewIngresarFacturaItem.Items)
                {
                    FacturaNuevo = new XElement("producto" + (i++));
                    FacturaNuevo.Add(new XElement("Familia", item.Text));
                    FacturaNuevo.Add(new XElement("Grupo", item.SubItems[1].Text));
                    FacturaNuevo.Add(new XElement("NP", item.SubItems[2].Text));
                    FacturaNuevo.Add(new XElement("Descripcion", item.SubItems[3].Text));
                    FacturaNuevo.Add(new XElement("Cantidad", item.SubItems[4].Text));
                    FacturaNuevo.Add(new XElement("Kilos", item.SubItems[5].Text));
                    FacturaNuevo.Add(new XElement("CostoFTR", item.SubItems[6].Text));
                    FacturaNuevo.Add(new XElement("CostoEXT", item.SubItems[7].Text));
                    FacturaNuevo.Add(new XElement("PVP", item.SubItems[8].Text));
                    FacturaNuevo.Add(new XElement("tipo", item.SubItems[10].Text));
                    FacturaProductos.Add(FacturaNuevo);

                }
                NuevaFacturaIngreso.Add(Data);
                NuevaFacturaIngreso.Add(DatosEconomicos);
                NuevaFacturaIngreso.Add(FacturaProductos);
                NuevaFacturaIngreso.Save("FacturaIngresoPendiente.xml");
                //hasta aca se crea la factura de ingreso nueva
            }
            string datosFactura = "", fecha = "";
            double importe = 0;
            int dolar = 0, euro = 0;
            XmlDocument FacturaPendiente = new XmlDocument();
            barraEstadoOcupado("Cargando Productos");
            FacturaPendiente.Load("FacturaIngresoPendiente.xml");
            foreach (XmlNode nodo in FacturaPendiente.DocumentElement.ChildNodes)
            {
                if (nodo.Name.Equals("Datos"))
                {
                    foreach (XmlNode nodo2 in nodo.ChildNodes)
                    {
                        if (nodo2.Name.Equals("proveedor"))
                            datosFactura = nodo2.InnerText;
                        else if (nodo2.Name.Equals("NumeroFactura"))
                            datosFactura = datosFactura + "-" + nodo2.InnerText;
                        else if (nodo2.Name.Equals("Fecha"))
                            fecha = nodo2.InnerText;
                    }
                }
                if (nodo.Name.Equals("indicadores"))
                {
                    foreach (XmlNode nodo2 in nodo.ChildNodes)
                    {
                        if (nodo2.Name.Equals("Dolar"))
                            dolar = Convert.ToInt32(nodo2.InnerText);
                        if (nodo2.Name.Equals("Euro"))
                            euro = Convert.ToInt32(nodo2.InnerText);
                        if (nodo2.Name.Equals("Importe"))
                            importe = Convert.ToDouble(nodo2.InnerText);
                    }
                    Console.WriteLine("Dolar: " + dolar + "\tEuro: " + euro + "\tImporte: " + importe);
                }
                if ((nodo.Name.Equals("productos")))
                {
                    foreach (XmlNode nodoMedio in nodo.ChildNodes)
                    {
                        string nuparte = "", complejo = "", familia = "", grupo = "", cantidad = "", pvp = "", descripcion = "", moneda = "", PrecioMoneda = "";
                        double costoExt = 0, kilos = 0, costoFTR = 0;
                        foreach (XmlNode nodo2 in nodoMedio.ChildNodes)
                        {
                            if (nodo2.Name.Equals("Familia"))
                                familia = nodo2.InnerText;
                            else if (nodo2.Name.Equals("Grupo"))
                                grupo = nodo2.InnerText;
                            else if (nodo2.Name.Equals("NP"))
                                nuparte = nodo2.InnerText;
                            else if (nodo2.Name.Equals("Descripcion"))
                                descripcion = nodo2.InnerText;
                            else if (nodo2.Name.Equals("Cantidad"))
                                cantidad = nodo2.InnerText;
                            else if (nodo2.Name.Equals("Kilos"))
                                kilos = Convert.ToDouble(nodo2.InnerText) * 100;
                            else if (nodo2.Name.Equals("CostoFTR"))
                            {
                                moneda = nodo2.InnerText;
                                costoExt = Convert.ToDouble(new Numeros(nodo2.InnerText).numeroSolo()) * 100;
                                CompareInfo myComp = CultureInfo.InvariantCulture.CompareInfo;
                                if (myComp.IsPrefix(moneda, "€")) // si es euro
                                {
                                    moneda = "EUR";
                                    PrecioMoneda = "" + euro;
                                }
                                else if (myComp.IsPrefix(moneda, "U$")) // si es dolar
                                {
                                    moneda = "USD";
                                    PrecioMoneda = "" + dolar;
                                }
                                else if (myComp.IsPrefix(moneda, "$"))// si es pesho chileno
                                {
                                    moneda = "CLP";
                                    PrecioMoneda = "1";
                                }
                            }
                            else if (nodo2.Name.Equals("CostoEXT"))
                                costoFTR = Convert.ToDouble(new Numeros(nodo2.InnerText).numeroSolo()) * 100;
                            else if (nodo2.Name.Equals("PVP"))
                                pvp = new Numeros(nodo2.InnerText).numeroSolo();
                            else if (nodo2.InnerText.Equals("Nuevo"))
                            {
                                barraEstadoOcupado("Cargando Producto Nuevo al Inventario");
                                complejo = moneda + "¬" + fecha + "¬" + costoFTR + "¬" + PrecioMoneda + "¬" + datosFactura;
                                string[] producto = { nuparte, complejo, familia, grupo, cantidad, "" + Math.Round(costoExt, 0), "" + Math.Round(kilos, 0), new Numeros(pvp).numeroSolo(), descripcion };
                                if (fresia.crearProducto(producto))
                                {
                                    if (frecia.ingresoPMP(producto[0], 1, 1, Convert.ToInt32(producto[4]), Convert.ToInt32(costoExt), Convert.ToInt32(costoExt), producto[1].Split('¬')[0]) == false)
                                    {
                                        MessageBox.Show("error al ingresar producto a ficha PMP");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("producto NO ha podido ser ingresado");
                                }
                            }
                            else if (nodo2.InnerText.Equals("Existe"))
                            {
                                barraEstadoOcupado("Cargando Elemento Existente al inventario");
                                List<string[]> productos = new List<string[]>();
                                fresia.getInventarioXML().Save("inventario.xml.ftr");
                                XmlDocument nuevo = new XmlDocument();
                                nuevo.Load("inventario.xml.ftr");
                                foreach (XmlNode nodo3 in nuevo.DocumentElement.ChildNodes)
                                {
                                    string[] producto = { "", "", "", "", "", "", "", "", "" };
                                    foreach (XmlNode nodo4 in nodo3.ChildNodes)
                                    {
                                        if (nodo4.Name == "Codigo")
                                        {
                                            producto[0] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "Origen")
                                        {
                                            producto[1] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "Familia")
                                        {
                                            producto[2] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "Grupo")
                                        {
                                            producto[3] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "Cant")
                                        {
                                            producto[4] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "CostoUnitEuto")
                                        {
                                            producto[5] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "CostoUnitPeso")
                                        {
                                            producto[6] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "PVP")
                                        {
                                            producto[7] = nodo4.InnerText;
                                        }
                                        else if (nodo4.Name == "Descripcion")
                                        {
                                            producto[8] = nodo4.InnerText;
                                        }
                                    }
                                    productos.Add(producto);
                                }
                                foreach (string[] producto in productos)
                                {
                                    if (producto[0].Equals(nuparte))
                                    {
                                        int cantAnt = Convert.ToInt32(producto[4]);
                                        int cantNue = Convert.ToInt32(cantidad);
                                        double costoExtNue = costoFTR;
                                        double costoExtAnt = Convert.ToDouble(producto[1].Split('¬')[2]);
                                        double costoFtrNue = costoExt;
                                        double costoFtrAnt = Convert.ToDouble(producto[6]);
                                        double CantidadTotal = cantAnt + cantNue;
                                        double precioMonedaAnt = Convert.ToDouble(producto[1].Split('¬')[3]);
                                        double precioMonedaNue = 1;
                                        if (producto[1].Split('¬')[0] == "U$")
                                            precioMonedaNue = Convert.ToDouble(dolar);
                                        else if (producto[1].Split('¬')[0] == "EUR")
                                            precioMonedaNue = Convert.ToDouble(euro);
                                        double precioMonedaTotal = (precioMonedaNue * (cantNue / CantidadTotal)) + (precioMonedaAnt * (cantAnt / CantidadTotal));
                                        double costoFtrTotal = (costoFtrNue * (cantNue / CantidadTotal)) + (costoFtrAnt * (cantAnt / CantidadTotal));
                                        double costoExtTotal = (costoExtNue * (cantNue / CantidadTotal)) + (costoExtAnt * (cantAnt / CantidadTotal));
                                        string datoComplejoNuev = producto[1].Split('¬')[0] + "¬" + fecha + "¬" + Math.Round(costoExtTotal, 0) + "¬" + Math.Round(precioMonedaTotal, 0) + "¬" + datosFactura;
                                        producto[1] = datoComplejoNuev;
                                        producto[4] = "" + Math.Round(CantidadTotal, 0);
                                        producto[5] = "" + Math.Round(costoFtrTotal, 0);
                                        producto[7] = new Numeros(pvp).numeroSolo();
                                        producto[8] = descripcion;
                                        if (fresia.editarProducto(producto))
                                        {
                                            if (frecia.ingresoPMP(producto[0], 0, 1, Convert.ToInt32(CantidadTotal), Convert.ToInt32(costoExtTotal), Convert.ToInt32(costoFtrTotal), producto[1].Split('¬')[0]) == false)
                                            {
                                                MessageBox.Show("error al ingresar a ficha PMP");
                                            }
                                            MessageBox.Show("producto editado con exito");
                                        }
                                        else
                                        {
                                            MessageBox.Show("producto no ha podido ser editado");
                                        }
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            barraEstadoOcupado("ingresando Factura al servidor");
            if (frecia.IngresarFacturaIngreso(XElement.Parse(FacturaPendiente.OuterXml)))
            {
                MessageBox.Show("Factura ingresada");
                File.Delete("FacturaIngresoPendiente.xml");
            }
            barraEstadoLibre();
        }
        private bool revisarXMLIngresarFactura()
            //funcion para revisar si existe una factura pendiente por mandar
        {
            try
            {
                XmlDocument facturaPendiente = new XmlDocument();
                facturaPendiente.Load("FacturaIngresoPendiente.xml");
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
