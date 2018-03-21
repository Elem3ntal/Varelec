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
                    FacturaNuevo.Add(new XElement("flete", item.SubItems[11].Text));
                    FacturaNuevo.Add(new XElement("tipo", item.SubItems[10].Text)); //el ultimo nuevo o usado.
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
                        string nuparte = "", familia = "", grupo = "", cantidad = "", pvp = "", descripcion = "", moneda = "", PrecioMoneda = "", costoExt = "", kilos = "", costoFTR = "", flete = "";
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
                                kilos = nodo2.InnerText;
                            else if (nodo2.Name.Equals("CostoFTR"))
                            {
                                moneda = nodo2.InnerText;
                                costoFTR = new Numeros(nodo2.InnerText).numeroSolo();
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
                                costoExt = new Numeros(nodo2.InnerText).numeroSolo();
                            else if (nodo2.Name.Equals("PVP"))
                                pvp = new Numeros(nodo2.InnerText).numeroSolo();
                            else if (nodo2.Name.Equals("flete"))
                                flete = nodo2.InnerText;
                            else if (nodo2.InnerText.Equals("Nuevo"))
                            {
                                barraEstadoOcupado("Cargando Producto Nuevo al Inventario");
                                string[] producto = { nuparte, familia, grupo, descripcion, kilos, cantidad, costoFTR, costoExt, pvp, datosFactura, fecha, fecha, moneda, PrecioMoneda, flete, ""+Math.Round(importe,2) };
                                if (servidor.inventarioAgregar(producto) == 1)
                                {//Convert.ToInt32(Convert.ToDouble(costoFTR) * 100), Convert.ToInt32(Convert.ToDouble(costoExt) * 100)
                                    if (servidor.ingresoPMP(producto[0], 1, 1, Convert.ToInt32(producto[5]), Convert.ToInt32(Convert.ToDouble(costoFTR) * 100), Convert.ToInt32(Convert.ToDouble(costoExt) * 100), moneda) == false)
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
                                servidor.inventarioObtener().Save("inventario.dbxml");
                                XmlDocument nuevo = new XmlDocument();
                                nuevo.Load("inventario.dbxml");
                                foreach (XmlNode nodo3 in nuevo.DocumentElement.ChildNodes)
                                {
                                    string[] producto = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                                    foreach (XmlNode nodo4 in nodo.ChildNodes)
                                    {
                                        if (nodo4.Name == "nuParte")
                                            producto[0] = nodo4.InnerText;
                                        //else if (nodo4.Name == "familia")
                                        //    producto[1] = nodo4.InnerText;
                                        //else if (nodo4.Name == "grupo")
                                        //    producto[2] = nodo4.InnerText;
                                        //else if (nodo4.Name == "descrip")
                                        //    producto[3] = nodo4.InnerText;
                                        //else if (nodo4.Name == "kilos")
                                        //    producto[4] = nodo4.InnerText;
                                        else if (nodo4.Name == "cantidad")
                                            producto[5] = nodo4.InnerText;
                                        else if (nodo4.Name == "costoFTR")
                                            producto[6] = nodo4.InnerText;
                                        else if (nodo4.Name == "costoEXT")
                                            producto[7] = nodo4.InnerText;
                                        //else if (nodo4.Name == "pvp")
                                        //    producto[8] = nodo4.InnerText;
                                        //else if (nodo4.Name == "ultimaFacturaIngreso")
                                        //    producto[9] = nodo4.InnerText;
                                        //else if (nodo4.Name == "ultimaFechaIngreso")
                                        //    producto[10] = nodo4.InnerText;
                                        //else if (nodo4.Name == "ultimoMovimiento")
                                        //    producto[11] = nodo4.InnerText;
                                        else if (nodo4.Name == "tipoMoneda")
                                            producto[12] = nodo4.InnerText;
                                        else if (nodo4.Name == "valorMoneda")
                                            producto[13] = nodo4.InnerText;
                                        //else if (nodo4.Name == "flete")
                                        //    producto[14] = nodo4.InnerText;
                                        //else if (nodo4.Name == "importe")
                                        ////    producto[15] = nodo4.InnerText;
                                    }
                                    productos.Add(producto);
                                }
                                foreach (string[] producto in productos)
                                {
                                    if (producto[0].Equals(nuparte))
                                    {
                                        int cantAnt = Convert.ToInt32(producto[5]);
                                        int cantNue = Convert.ToInt32(cantidad);
                                        double costoExtNue = Convert.ToDouble(costoFTR);
                                        double costoExtAnt = Convert.ToDouble(producto[7]);
                                        double costoFtrNue = Convert.ToDouble(costoExt);
                                        double costoFtrAnt = Convert.ToDouble(producto[6]);
                                        double CantidadTotal = cantAnt + cantNue;
                                        double precioMonedaAnt = Convert.ToDouble(producto[13]);
                                        double precioMonedaNue = 1;
                                        //si ninguno de estos dos se cumple,es CLP
                                        if (producto[12] == "USD")
                                            precioMonedaNue = Convert.ToDouble(dolar);
                                        else if (producto[12] == "EUR")
                                            precioMonedaNue = Convert.ToDouble(euro);
                                        //promedio de la moneda
                                        double precioMonedaTotal = (precioMonedaNue * (cantNue / CantidadTotal)) + (precioMonedaAnt * (cantAnt / CantidadTotal));
                                        //paso el costoFTR a peso y lo promedio, luego lo divido en el precioMonedaTotal
                                        double costoFtrAntCLP = costoFtrAnt * precioMonedaAnt;
                                        double costoFtrNueCLP = costoFtrNue * precioMonedaNue;
                                        //mismo paso pero con el costoANT
                                        double costoExtAntCLP = costoExtAnt* precioMonedaAnt;
                                        double costoExtNueCLP = costoExtNue * precioMonedaNue;
                                        //ahora a promediarlos
                                        double costoFtrTotalCLP = (costoFtrNueCLP * (cantNue / CantidadTotal)) + (costoFtrAntCLP * (cantAnt / CantidadTotal));
                                        double costoExtTotalCLP = (costoExtNueCLP * (cantNue / CantidadTotal)) + (costoExtAntCLP * (cantAnt / CantidadTotal));
                                        double costoFtrTotal = costoFtrTotalCLP / precioMonedaTotal;
                                        double costoExtTotal = costoExtTotalCLP / precioMonedaTotal;
                                        string datoComplejoNuev = "" + "¬" + Math.Round(precioMonedaTotal, 0) + "¬" + datosFactura;
                                        producto[1] = familia;
                                        producto[2] = grupo;
                                        producto[3] = descripcion;
                                        producto[4] = kilos;
                                        producto[5] = "" + Math.Round(CantidadTotal, 0);
                                        producto[6] = "" + Math.Round(costoFtrTotal, 0);
                                        producto[7] = "" + Math.Round(costoExtTotal, 0);
                                        producto[8] = new Numeros(pvp).numeroSolo();
                                        producto[9] = datosFactura; 
                                        producto[10] = fecha;
                                        producto[11] = producto[10];
                                        producto[12] = moneda;
                                        producto[13] = "" + Math.Round(precioMonedaTotal, 0);
                                        producto[14] = flete;
                                        producto[15] = "" + Math.Round(importe, 2);
                                        if (servidor.inventarioModificar(producto) == 1)
                                        {

                                            if (servidor.ingresoPMP(producto[0], 0, 1, Convert.ToInt32(CantidadTotal), Convert.ToInt32(costoFtrTotal), Convert.ToInt32(costoExtTotal), moneda) == false)
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
            if (servidor.IngresarFacturaIngreso(XElement.Parse(FacturaPendiente.OuterXml)))
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
