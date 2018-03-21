using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// [System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService {
    public WebService () {

        //Uncomment the following line if using designed components
        //InitializeComponent();
    }
    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }
    [WebMethod]
    public bool escribirMemo(string usuario, string entrada)
    {
        try
        {
            XmlDocument actual = new XmlDocument();
            actual.Load("C:/wwwroot/servicios/MemosGenerales.xml");
            XmlNode nodo = actual.DocumentElement;
            XmlNode memo = actual.CreateElement("" + usuario);
            memo.InnerText = entrada;
            nodo.AppendChild(memo);
            actual.Save("C:/wwwroot/servicios/MemosGenerales.xml");
        }
        catch
        {
            XElement actual = new XElement("Memos");
            actual.Add(new XElement(usuario, entrada));
            actual.Save("C:/wwwroot/servicios/MemosGenerales.xml");
        }
        return true;
    }
    [WebMethod]
    public int estado()
    {
        return 1;
    }
    [WebMethod]
    public XmlDocument obtenerMemosGenrales()
    {
        XmlDocument actual = new XmlDocument();
        try
        {
            actual.Load("C:/wwwroot/servicios/memosGenerales.xml");
        }
        catch
        {
            XElement nuevo = new XElement("memos");
            nuevo.Save("C:/wwwroot/servicios/MemosGenerales.xml");
            actual.Load("C:/wwwroot/servicios/MemosGenerales.xml");
        }
        return actual;
    }
    [WebMethod]
    public int guardarCotizacion(XElement cotizacion)
    {
        int id=1;
        try
        {
            XmlDocument actual = new XmlDocument();
            actual.Load("c:/wwwroot/servicios/cantidadC.cnt");
            XmlNode nodo = actual.DocumentElement;
            id = Convert.ToInt32(nodo.FirstChild.InnerText);
            id++;
            nodo.FirstChild.InnerText = ""+id;
            actual.Save("c:/wwwroot/servicios/cantidadC.cnt");
        }
        catch
        {
            XElement cant = new XElement("cantidad");
            cant.Add(new XElement("id", id));
            cant.Save("c:/wwwroot/servicios/cantidadC.cnt");
        }
        cotizacion.Add(new XElement("ID"), id);
        cotizacion.Save("c:/wwwroot/servicios/cotizaciones/cotizacion" + id + ".xml");
        return id;
    }
    [WebMethod]
    public XmlDocument listaCotizaciones()
    {
        System.IO.DirectoryInfo directorio = new System.IO.DirectoryInfo("c:/wwwroot/servicios/cotizaciones/");
        System.IO.FileInfo[] archivos = directorio.GetFiles();
        int id = archivos.Length;
        XmlDocument salida = new XmlDocument();
        XElement temp = new XElement("temp");
        for (int i = 0; i < id; i++)
        {
            XElement nodo = new XElement("venta");
            XmlDocument file = new XmlDocument();
            file.Load("c:/wwwroot/servicios/cotizaciones/" + archivos[i].Name);
            XmlNode actual = file.DocumentElement;
            foreach (XmlNode nodo1 in actual.ChildNodes)
            {
                if (nodo1.Name == "MetaData")
                {
                    nodo.Add(new XElement(nodo1.Name, nodo1.FirstChild.InnerText));
                }
                else if (nodo1.Name == "Vendedor")
                {
                    nodo.Add(new XElement("Vendedor", nodo1.InnerText));
                }
                else if (nodo1.Name == "Cliente")
                {
                    nodo.Add(new XElement(nodo1.Name, nodo1.FirstChild.InnerText));
                }
                else if (nodo1.Name == "ID")
                {
                    nodo.Add(new XElement("Cotizacion", actual.LastChild.InnerText));
                    temp.Add(nodo);
                }
            }
        }
        temp.Save("C:/wwwroot/temp.xml");
        salida.Load("C:/wwwroot/temp.xml");
        return salida;
    }
    [WebMethod]
    public XmlDocument obtenerFactura(int numero)
    {
        string ID=""+numero;
        XElement nuevo = new XElement("factura");
        nuevo.Save("c:/wwwroot/servicios/tem.xml");
        int salida = 0;
        DirectoryInfo directorio = new DirectoryInfo("c:/wwwroot/servicios/cotizaciones/");
        FileInfo[] archivos = directorio.GetFiles();
        int id = archivos.Length;
        for (int i = 0; i < id; i++)
        {
            salida = 0;
            XmlDocument file = new XmlDocument();
            file.Load("c:/wwwroot/servicios/cotizaciones/"+archivos[i].Name);
            if (file.DocumentElement.LastChild.InnerText == ID)
            {
                file.Save("c:/wwwroot/servicios/tem.xml");
            }
        }
        XmlDocument factura = new XmlDocument();
        factura.Load("c:/wwwroot/servicios/tem.xml");
        return factura;
    }
    [WebMethod]
    public XmlDocument indicadoresEconomicos()
    {
        XmlDocument salida = new XmlDocument();
        try
        {
            salida.Load("C:/wwwroot/servicios/indicadoresEconomicos.xml");
        }
        catch
        {
            XElement nuevo = new XElement("indicadores");
            nuevo.Add(new XElement("Dolar", 1));
            nuevo.Add(new XElement("Euro", 1));
            nuevo.Add(new XElement("Importe", 1));
            nuevo.Save("C:/wwwroot/servicios/indicadoresEconomicos.xml");
            salida.Load("C:/wwwroot/servicios/indicadoresEconomicos.xml");
        }
        return salida;
    }
    [WebMethod]
    public bool setearIndicadoresEconomicos(string dolar, string euro, string importe)
    {
        XElement nuevo = new XElement("indicadores");
        nuevo.Add(new XElement("Dolar", dolar));
        nuevo.Add(new XElement("Euro", euro));
        nuevo.Add(new XElement("Importe", importe));
        nuevo.Save("C:/wwwroot/servicios/indicadoresEconomicos.xml");
        return true;
    }
    [WebMethod]
    public bool ingresarFacturaLiberada(XElement factura)
    {
        string mes = ""+DateTime.Now.Month;
        string año = "" + DateTime.Now.Year;
        DirectoryInfo directorioMes = new DirectoryInfo("c:/wwwroot/servicios/facturas/"+año+"/"+mes+"/");
        DirectoryInfo directorioAño = new DirectoryInfo("c:/wwwroot/servicios/facturas/"+año+"/");
        if (directorioAño.Exists)
        {
        }
        else
        {
            directorioAño.Create();
        }
        if (directorioMes.Exists)
        {
        }
        else
        {
            directorioMes.Create();
        }
        directorioMes.Create();
        FileInfo[] archivos = directorioMes.GetFiles();
        int id = 1;
        try
        {
            XmlDocument actual = new XmlDocument();
            actual.Load("c:/wwwroot/servicios/cantidadF.cnt");
            XmlNode nodo = actual.DocumentElement;
            id = Convert.ToInt32(nodo.FirstChild.InnerText);
            id++;
            nodo.FirstChild.InnerText = "" + id;
            actual.Save("c:/wwwroot/servicios/cantidadF.cnt");
        }
        catch
        {
            XElement cant = new XElement("cantidad");
            cant.Add(new XElement("id", id));
            cant.Save("c:/wwwroot/servicios/cantidadF.cnt");
        } factura.Add(new XElement("ID"), id);
        factura.Save("c:/wwwroot/servicios/facturas/" + año + "/" + mes + "/factura" + id + ".xml");
        return true;
    }
    [WebMethod]
    public string ingresarFacturaPorCotizacion(int identificacion)
    {
        string IDENTI = "" + identificacion;
        string ruta = "0";
        int salida = 0;
        DirectoryInfo directorio = new DirectoryInfo("c:/wwwroot/servicios/cotizaciones/");
        FileInfo[] archivos = directorio.GetFiles();
        int id = archivos.Length;
        for (int i = 0; i < id; i++)
        {
            salida = 0;
            XmlDocument file = new XmlDocument();
            file.Load("c:/wwwroot/servicios/cotizaciones/" + archivos[i].Name);
            if (file.DocumentElement.LastChild.InnerText==IDENTI)
            {
                salida = 3;
                string mes = DateTime.Now.Month.ToString();
                string año = DateTime.Now.Year.ToString();
                DirectoryInfo directorioMes = new DirectoryInfo("c:/wwwroot/servicios/facturas/" + año + "/" + mes + "/");
                DirectoryInfo directorioAño = new DirectoryInfo("c:/wwwroot/servicios/facturas/" + año + "/");
                if (directorioAño.Exists){}
                else{
                    directorioAño.Create();
                }
                if (directorioMes.Exists){}
                else{
                    directorioMes.Create();
                }
                FileInfo[] archivos2 = directorioMes.GetFiles();
                int id2 = 2;
                try
                {
                    XmlDocument actual = new XmlDocument();
                    actual.Load("c:/wwwroot/servicios/cantidadF.cnt");
                    XmlNode nodo = actual.DocumentElement;
                    id2 = Convert.ToInt32(nodo.FirstChild.InnerText);
                    id2++;
                    nodo.FirstChild.InnerText = "" + id2;
                    actual.Save("c:/wwwroot/servicios/cantidadF.cnt");
                }
                catch
                {
                    XElement cant = new XElement("cantidad");
                    cant.Add(new XElement("id", id2));
                    cant.Save("c:/wwwroot/servicios/cantidadF.cnt");
                } XmlNode ID = file.CreateElement("ID");
                ID.InnerText=""+id2;
                foreach (XmlNode nodo in file.DocumentElement.ChildNodes)
                {
                    if (nodo.Name == "MetaData")
                    {
                        nodo.InnerText = "I" + nodo.InnerText;
                    }
                }
                try
                {
                    File.Delete("c:/wwwroot/servicios/cotizaciones/" + archivos[i].Name);
                }
                catch { }
                ruta = "c:/wwwroot/servicios/facturas/" + año + "/" + mes + "/factura" + id2 + ".xml";
                file.Save(ruta);
                return ruta;
            }
        }
        return ruta;
    }
    [WebMethod]
    public XmlDocument facturasResumen(int año, int mes)
    {
        XElement nuevo = new XElement("factura");
        nuevo.Save("c:/wwwroot/servicios/LIBRO.xml");
        DirectoryInfo directorioMes = new DirectoryInfo("c:/wwwroot/servicios/facturas/" + año + "/" + mes + "/");
        if (directorioMes.Exists)
        {
            FileInfo[] archivos = directorioMes.GetFiles();
            int id = archivos.Length;
            for (int i = 0; i < id; i++)
            {
                XElement factura = new XElement("Factura" + (i + 1));
                XmlDocument file = new XmlDocument();
                file.Load("c:/wwwroot/servicios/facturas/" + año + "/" + mes + "/" + archivos[i].Name);
                XmlNode fileIn = file.DocumentElement;
                foreach (XmlNode nodo in fileIn.ChildNodes)
                {
                    if (nodo.Name == "MetaData")
                    {
                        factura.Add(new XElement(nodo.Name, nodo.InnerXml));
                    }
                    else if (nodo.Name == "Vendedor")
                    {
                        factura.Add(new XElement(nodo.Name, nodo.InnerXml));
                    }
                    else if (nodo.Name == "Cliente")
                    {
                        XElement cliente = new XElement("Cliente");
                        foreach (XmlNode datosCliente in nodo.ChildNodes)
                        {
                            cliente.Add(new XElement(datosCliente.Name, datosCliente.InnerText));
                        }
                        factura.Add(cliente);
                    }
                    else if (nodo.Name == "Productos")
                    {
                        XElement NewProductos = new XElement("productos");
                        foreach (XmlNode productos in nodo.ChildNodes)
                        {
                            XElement elemento = new XElement(productos.Name);
                            foreach (XmlNode detalleProductos in productos.ChildNodes)
                            {
                                XElement elemento2 = new XElement(detalleProductos.Name);
                                elemento2.Value=detalleProductos.InnerText;
                                elemento.Add(elemento2);
                            }
                            NewProductos.Add(elemento);
                        }
                        factura.Add(NewProductos);
                    }
                }
                nuevo.Add(factura);
            }
        }
        nuevo.Save("c:/wwwroot/servicios/LIBRO.xml");
        XmlDocument libroFinal = new XmlDocument();
        libroFinal.Load("c:/wwwroot/servicios/LIBRO.xml");
        //Console.WriteLine(libroFinal.DocumentElement.InnerXml);
        return libroFinal;
    }
    [WebMethod]
    public string consultarActualizacion()
    {
        try
        {
            XmlDocument file = new XmlDocument();
            file.Load("C:/wwwroot/servicios/programa/version.xml");
            return file.DocumentElement.FirstChild.InnerText;
        }
        catch
        {
            return "NULL";
        }
    }
    [WebMethod]
    public bool subirActualizacionAServidor(string version)
    {
        try
        {
            DirectoryInfo directorioVer = new DirectoryInfo("C:/wwwroot/servicios/programa/");
            if (directorioVer.Exists) { }
            else
            {
                directorioVer.Create();
            }
            XElement nuevo = new XElement("Programa");
            nuevo.Add(new XElement("Version", version));
            nuevo.Save("C:/wwwroot/servicios/programa/version.xml");
            new WebClient().DownloadFile("http://25.108.141.130/Actualizacion/Inventario.exe", @"C:/wwwroot/servicios/programa/Inventario.exe");
            return true;
        }
        catch
        {
            return false;
        }
        return true;
    }
    [WebMethod]
    public bool IngresarFacturaIngreso(XElement factura)
    {
        try
        {
            int id = 1;
            try
            {
                XmlDocument actual = new XmlDocument();
                actual.Load("c:/wwwroot/servicios/cantidadFI.cnt");
                XmlNode nodo = actual.DocumentElement;
                id = Convert.ToInt32(nodo.FirstChild.InnerText);
                id++;
                nodo.FirstChild.InnerText = "" + id;
                actual.Save("c:/wwwroot/servicios/cantidadFI.cnt");
            }
            catch
            {
                XElement cant = new XElement("cantidad");
                cant.Add(new XElement("id", id));
                cant.Save("c:/wwwroot/servicios/cantidadFI.cnt");
            }
            string mes = "" + DateTime.Now.Month;
            string año = "" + DateTime.Now.Year;
            DirectoryInfo directorioMes = new DirectoryInfo("c:/wwwroot/servicios/facturasIngreso/" + año + "/" + mes + "/");
            DirectoryInfo directorioAño = new DirectoryInfo("c:/wwwroot/servicios/facturasIngreso/" + año + "/");
            if (directorioAño.Exists)
            {
            }
            else
            {
                directorioAño.Create();
            }
            if (directorioMes.Exists)
            {
            }
            else
            {
                directorioMes.Create();
            }
            directorioMes.Create();
            factura.Save("c:/wwwroot/servicios/facturasIngreso/" + año + "/" + mes + "/factura" + id + ".xml");
            return true;
        }
        catch
        {
            return false;
        }
    }
    [WebMethod]
    public XmlDocument facturasIngresoResumen(int año, int mes)
    {
        XElement nuevo = new XElement("factura");
        nuevo.Save("c:/wwwroot/servicios/LIBROIngreso.xml");
        DirectoryInfo directorioMes = new DirectoryInfo("c:/wwwroot/servicios/facturasIngreso/" + año + "/" + mes + "/");
        if (directorioMes.Exists)
        {
            FileInfo[] archivos = directorioMes.GetFiles();
            int id = archivos.Length;
            for (int i = 0; i < id; i++)
            {
                XElement factura = new XElement(archivos[i].Name);
                XmlDocument file = new XmlDocument();
                file.Load("c:/wwwroot/servicios/facturasIngreso/" + año + "/" + mes + "/" + archivos[i].Name);
                XmlNode fileIn = file.DocumentElement;
                foreach (XmlNode nodo in fileIn.ChildNodes)
                {
                    if (nodo.Name == "Datos")
                    {
                        XElement Datos = new XElement("Datos");
                        foreach (XmlNode datos in nodo.ChildNodes)
                        {
                            Datos.Add(new XElement(datos.Name, datos.InnerText));
                        }
                        factura.Add(Datos);
                    }
                    else if (nodo.Name == "indicadores")
                    {
                        XElement Datos = new XElement("indicadores");
                        foreach (XmlNode datos in nodo.ChildNodes)
                        {
                            Datos.Add(new XElement(datos.Name, datos.InnerText));
                        }
                        factura.Add(Datos);
                    }
                    else if (nodo.Name == "Cliente")
                    {
                        XElement cliente = new XElement("Cliente");
                        foreach (XmlNode datosCliente in nodo.ChildNodes)
                        {
                            cliente.Add(new XElement(datosCliente.Name, datosCliente.InnerText));
                        }
                        factura.Add(cliente);
                    }
                    else if (nodo.Name == "productos")
                    {
                        XElement NewProductos = new XElement(nodo.Name);
                        foreach (XmlNode productos in nodo.ChildNodes)
                        {
                            XElement elemento = new XElement(productos.Name);
                            foreach (XmlNode detalleProductos in productos.ChildNodes)
                            {
                                XElement adentro = new XElement(detalleProductos.Name);
                                adentro.Value = detalleProductos.InnerText;
                                elemento.Add(adentro);
                            }
                            NewProductos.Add(elemento);
                        }
                        factura.Add(NewProductos);
                    }
                }
                nuevo.Add(factura);
            }
        }
        nuevo.Save("c:/wwwroot/servicios/LIBROIngreso.xml");
        XmlDocument libroFinal = new XmlDocument();
        libroFinal.Load("c:/wwwroot/servicios/LIBROIngreso.xml");
        //Console.WriteLine(libroFinal.DocumentElement.InnerXml);
        return libroFinal;
    }
    [WebMethod]
    public bool ingresoPMP(string NP, int NoE, int EoS, int cantidad, int precioFTR, int precioEXP, string moneda)
    {
        try
        {
            if (File.Exists("C:/wwwroot/servicios/HistorialPMP.xml") == false)
            {
                XElement test = new XElement("PMPTOTAL");
                test.Save("C:/wwwroot/servicios/HistorialPMP.xml");
            }
            XmlDocument actual = new XmlDocument();
            actual.Load("C:/wwwroot/servicios/HistorialPMP.xml");
            XmlNode nodo = actual.DocumentElement;
            XmlNode elemento = actual.CreateElement("NP-" + NP);
            XmlNode fecha = actual.CreateElement("fecha");
            XmlNode estado = actual.CreateElement("estado");
            XmlNode entraosale = actual.CreateElement("fue");
            XmlNode cant = actual.CreateElement("cantidad");
            XmlNode preFTR = actual.CreateElement("costoFTR");
            XmlNode preEXP = actual.CreateElement("costoEXP");
            XmlNode coin = actual.CreateElement("moneda");
            fecha.InnerText = DateTime.Now.Date.Day + "/" + DateTime.Now.Date.Month + "/" + DateTime.Now.Date.Year;
            estado.InnerText = "" + NoE;
            entraosale.InnerText = "" + EoS;
            cant.InnerText = "" + cantidad;
            preFTR.InnerText = ""+precioFTR;
            preEXP.InnerText = ""+precioEXP;
            coin.InnerText = moneda;
            elemento.AppendChild(fecha);
            elemento.AppendChild(coin);
            elemento.AppendChild(estado);
            elemento.AppendChild(entraosale);
            elemento.AppendChild(cant);
            elemento.AppendChild(preFTR);
            elemento.AppendChild(preEXP);
            nodo.AppendChild(elemento);
            actual.Save("C:/wwwroot/servicios/HistorialPMP.xml");
        }
        catch
        {
            return false;
        }
        return true;
    }
    [WebMethod]
    public XmlDocument obtenerPMP()
    {
        XmlDocument actual = new XmlDocument();
        try
        {
            actual.Load("C:/wwwroot/servicios/HistorialPMP.xml");
        }
        catch
        {
            XElement nuevo = new XElement("memos");
            nuevo.Save("C:/wwwroot/servicios/HistorialPMP.xml");
            actual.Load("C:/wwwroot/servicios/HistorialPMP.xml");
        }
        return actual;
    }
    [WebMethod]
    public bool uploadFile(string name)
    {
        try
        {
            DirectoryInfo directorioVer = new DirectoryInfo("C:/wwwroot/servicios/programa/");
            if (directorioVer.Exists) { }
            else
            {
                directorioVer.Create();
            }
            new WebClient().DownloadFile("http://25.108.141.130/Actualizacion/" + name, @"C:/wwwroot/servicios/programa/" + name);
            return true;
        }
        catch
        {
            return false;
        }
        return true;
    }
}
