using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

namespace Inventario
{
    class cotizacionAPDF
    {
        public int numero;
        public string vendedor;
        public cotizacionAPDF(XElement cotizacion, int numero, string vendedor)
        {
            cotizacion.Save("cotiApdf.temp");
            this.numero = numero;
            this.vendedor = vendedor;
        }
        public bool imprimir()
        {
            Microsoft.Office.Interop.Excel._Worksheet xlHoja;
            Microsoft.Office.Interop.Excel.Sheets xlHojas;
            var misValue = Type.Missing;
            Microsoft.Office.Interop.Excel.Application excelapp = new Microsoft.Office.Interop.Excel.Application();
            excelapp.Visible = false;
            Microsoft.Office.Interop.Excel._Workbook libro = excelapp.Workbooks.Open("C:\\Inventario\\cotizacion.xlsx");
            xlHojas = libro.Sheets;
            xlHoja = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];
            //el estilo para ponerlo subrayadado
            Microsoft.Office.Interop.Excel.Style estilo = excelapp.Application.ActiveWorkbook.Styles.Add("Kawaii");
            estilo.Font.Name = "Century Gothic";
            estilo.Font.Size = 11;
            estilo.Font.Italic = true;
            estilo.Font.Underline = true;
            //llenar la cotizacion.
            xlHoja.Cells[2, 7] = DateTime.Now.Day.ToString() + "-" + DateTime.Now.Month.ToString() + "-" + DateTime.Now.Year.ToString();
            xlHoja.Cells[3, 7] = numero;
            XmlDocument archivo = new XmlDocument();
            archivo.Load("cotiApdf.temp");
            Console.WriteLine("pasando datos a excel");
            int i = 15; //columna donde parte a poner los items en la cotizacion
            foreach(XmlNode nodo in archivo.DocumentElement.ChildNodes){
                int diferencia = 0,total=0;
                if (nodo.Name == "Cliente")
                {
                    foreach (XmlNode nodos in nodo.ChildNodes)
                    {
                        if (nodos.Name == "RUT")
                        {
                            xlHoja.Cells[7, 1] = "RUT: " + nodos.InnerText;
                        }
                        else if (nodos.Name == "Señores")
                        {
                            xlHoja.Cells[8, 1] = "Señores: " + nodos.InnerText;
                        }
                        else if (nodos.Name == "Contacto")
                        {
                            xlHoja.Cells[9, 1] = "Contacto: " + nodos.InnerText;
                        }
                        else if (nodos.Name == "Giro")
                        {
                            xlHoja.Cells[7, 3] = "Giro: " + nodos.InnerText;
                        }
                        else if (nodos.Name == "Correo")
                        {
                            xlHoja.Cells[10, 3] = "Correo: " + nodos.InnerText;
                        }
                        else if (nodos.Name == "Fono")
                        {
                            xlHoja.Cells[10, 1] = "Telefono: " + nodos.InnerText;
                        }
                    }
                }
                else if (nodo.Name == "Productos")
                {
                    i++;
                    foreach (XmlNode nodoIntermedio in nodo.ChildNodes)
                    {
                        int costo=0, cantidad=0;
                        foreach(XmlNode nodo2 in nodoIntermedio.ChildNodes)
                        {
                            Console.WriteLine("nodo2 nombre->"+nodo2.Name);
                            if (nodo2.Name == "Descripcion")
                            {
                                xlHoja.Cells[i, 1] = nodo2.InnerText.Split('-')[0];
                                xlHoja.Cells[i, 2] = nodo2.InnerText.Split('-')[1];
                            }
                            //cantidad
                            else if (nodo2.Name == "cantidad")
                            {
                                string cant = new Numeros(nodo2.InnerText).numeroSolo();
                                xlHoja.Cells[i, 3] = cant;
                                cantidad = Convert.ToInt32(cant);
                            }
                            //precio
                            else if (nodo2.Name == "Precio")
                            {
                                string precio = new Numeros(nodo2.InnerText).numeroSolo();
                                xlHoja.Cells[i, 4] = precio;
                                costo = Convert.ToInt32(precio);
                            }//valor total
                            else if (nodo2.Name == "valorTotal")
                            {
                                Numeros elemento = new Numeros(nodo2.InnerText);
                                total+=Convert.ToInt32(Math.Round(Convert.ToDouble(elemento.numeroSolo()),0));
                                if ((costo*cantidad) > Convert.ToInt32(new Numeros(nodo2.InnerText.Split('$')[1]).numeroSolo()))
                                {
                                    diferencia +=  (Convert.ToInt32(new Numeros(nodo2.InnerText).numeroSolo()) - (costo * cantidad));
                                }
                                xlHoja.Cells[i, 5] = costo * cantidad;
                            }
                            else if (nodo2.Name == "PlazoEntrega")
                            {
                                xlHoja.Cells[i, 6] = nodo2.InnerText;
                            }
                        }
                        i++;
                    }
                    if (diferencia != 0)
                    {
                        i++;
                        xlHoja.Cells[i, 1] = "descuento";
                        xlHoja.Cells[i, 5] = "" + (diferencia);
                    }
                    i++;
                    i++;
                    xlHoja.Cells[i, 4] = "Total Neto";
                    xlHoja.Cells[i, 5] = "=suma(E14:E" + (i - 1) + ")";
                    i++;
                    xlHoja.Cells[i, 4] = "IVA (19%)";
                    xlHoja.Cells[i, 5] = Math.Round((Convert.ToDouble(total) *0.19), 0);
                    i++;
                    xlHoja.Cells[i, 4] = "Total";
                    xlHoja.Cells[i, 5] = Math.Round((Convert.ToDouble(total) * 1.19), 0);
                }
                else if (nodo.Name == "Condiciones")
                {
                    i = 29; //las condiciones generales parten desde el 29
                    string rango = "A" + (i - 1);
                    xlHoja.Cells[(i - 1), 1] = "Condiciones Generales:";
                    xlHoja.Range[rango, rango].Style = estilo;
                    foreach (XmlNode condicion in nodo.ChildNodes)
                    {
                        xlHoja.Cells[i++, 1] = condicion.InnerText;
                    }
                }
            }
            //completar pie de firma
            string[] luis = { "Luis Ayala A.", "Ejecutivo Comercial.", "layala@varelec.cl", "09 9885 8947" };
            string[] juan = { "Juan Gallardo U.","Ejecutivo Comercial.","jgallardo@varelec.cl","09 6236 8274"};
            string[] mauricio = { "Mauricio Contreras V.","Gerente General","mcontreras@varelec.cl","09 9885 9110"};
            string[] eduardo = { "Eduardo Gonzales A.","Jefe Taller","egonzales@varelec.cl","09 5738 9691"};
            string[] javier = { "Javier Rodriguez B.","Desarrollador Informatico","javier.rodriguezb@usach.com","9 9940 2264"};
            string[] andres = { "Andrés Cabello B.", "Ejecutivo Comercial.", "acabello@varelec.cl", "09 5229 9120" };
            string[] datos = {"Nombre","Cargo","correo","telefono"};
            if (vendedor == "luis")
            {
                datos=luis;
            }
            else if (vendedor == "juan")
            {
                datos = juan;
            }
            else if (vendedor == "mauricio")
            {
                datos = mauricio;
            }
            else if (vendedor == "eduardo")
            {
                datos = eduardo;
            }
            else if (vendedor == "javier" || vendedor == "DIOS")
            {
                datos = javier;
            }
            else if (vendedor =="andres")
            {
                datos = andres;
            }
            xlHoja.Cells[41, 3] = datos[0];
            xlHoja.Cells[42, 3] = datos[1];
            xlHoja.Cells[43, 3] = datos[2];
            xlHoja.Cells[44, 3] = datos[3];
            //llenar hasta aca.
            string path = "C:\\Inventario\\cotizacionOLD.xlsx";
            try
            {
                File.Delete(path);
            }
            catch { }
            xlHoja.SaveAs(path); // guardar y a pdf y ENUMERAR
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (Directory.Exists(desktopPath + "/Cotizaciones")==false)
            {
                Directory.CreateDirectory(desktopPath + "/Cotizaciones");
            }
            xlHoja.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, desktopPath + "/Cotizaciones/Cotizacion" + numero + ".pdf", misValue, true, true, misValue, misValue, true, misValue);
            libro.Close();
            excelapp.Quit();
            return true;
        }
        public string rangoCalculadora(int numero,int letra)
        {
            string salida ="";
            char letraS='A';
            for(int i=1;i<letra;i++){
                letraS++;
            }
            salida = "" + letraS + numero;
            return salida;
        }
    }
}
