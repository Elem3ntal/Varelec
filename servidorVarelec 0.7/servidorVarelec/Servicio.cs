using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Data;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Xml.Linq;
using System.Threading;
using System.ServiceModel;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ServiceModel.Description;

namespace servidorVarelec
{
    [ServiceBehavior(InstanceContextMode= InstanceContextMode.PerCall)]
    public class Servicio:IServicio
    {
        private DBXML inventario = new DBXML("inventario");
        private DBXML clientes = new DBXML("clientes");
        private DBXML usuarios = new DBXML("usuarios");
        private readonly static Semaphore PoolInventario = new Semaphore(1, 1);
        private readonly static Semaphore PoolClientes = new Semaphore(1, 1);
        private readonly static Semaphore PoolUsuarios = new Semaphore(1, 1);
        private void tomarInventario()
        {
            PoolInventario.WaitOne();
            inventario.abrirTabla();
        }
        private void soltarInventario()
        {
            inventario.cerrarTabla();
            PoolInventario.Release();
        }
        private void tomarClientes()
        {
            PoolClientes.WaitOne();
            clientes.abrirTabla();
        }
        private void soltarClientes()
        {
            clientes.cerrarTabla();
            PoolClientes.Release();
        }
        private void tomarUsuarios()
        {
            PoolUsuarios.WaitOne();
            usuarios.abrirTabla();
        }
        private void soltarUsuarios()
        {
            usuarios.cerrarTabla();
            PoolUsuarios.Release();
        }
        //---------------metodos
        public bool online()
        {
            return true;
        }
        //---------------Metodos de Inventario
        public int inventarioAgregar(string[] fila)
        {
            tomarInventario();
            int salida = inventario.agregarFila(fila);
            soltarInventario();
            return salida;
        }
        public int inventarioModificar(string[] fila)
        {
            tomarInventario();
            int salida = inventario.editarFila(fila);
            soltarInventario();
            return salida;
        }
        public int inventarioEliminar(string fila)
        {
            tomarInventario();
            int salida = inventario.eliminarFila(fila);
            soltarInventario();
            return salida;
        }
        public string inventarioNVersion() 
        {
            string salida;
            tomarInventario();
            salida = inventario.obtenerVersion();
            soltarInventario();
            return salida;
        }
        public XmlElement inventarioObtener() 
        {
            XmlDocument retorno;
            tomarInventario();
            retorno = inventario.obtenerTabla();
            soltarInventario();
            return retorno.DocumentElement;
        }
        public XmlElement inventarioErrores()
        {
            XmlDocument retorno;
            tomarInventario();
            retorno = inventario.excepcionResumen();
            soltarInventario();
            return retorno.DocumentElement;
        }
        public string inventarioErrorUltimo()
        {
            tomarInventario();
            string retorno = inventario.excepcionLastError();
            soltarInventario();
            return retorno;
        }
        //---------------Metodos de Clientes
        public int clientesAgregar(string[] fila)
        {
            tomarClientes();
            int salida = clientes.agregarFila(fila);
            soltarClientes();
            return salida;
        }
        public int clientesModificar(string[] fila)
        {
            tomarClientes();
            int salida = clientes.editarFila(fila);
            soltarClientes();
            return salida;
        }
        public int clientesEliminar(string fila)
        {
            tomarClientes();
            int salida = clientes.eliminarFila(fila);
            soltarClientes();
            return salida;
        }
        public string clientesNVersion()
        {
            string salida;
            tomarClientes();
            salida = clientes.obtenerVersion();
            soltarClientes();
            return salida;
        }
        public XmlElement clientesObtener()
        {
            XmlDocument retorno;
            tomarClientes();
            retorno = clientes.obtenerTabla();
            soltarClientes();
            return retorno.DocumentElement;
        }
        public XmlElement clientesErrores()
        {
            XmlDocument retorno;
            tomarClientes();
            retorno = clientes.excepcionResumen();
            soltarClientes();
            return retorno.DocumentElement;
        }
        public string clientesErrorUltimo()
        {
            tomarClientes();
            string retorno = clientes.excepcionLastError();
            soltarClientes();
            return retorno;
        }
        //---------------Metodos de Usuarios
        public int usuariosAgregar(string[] fila) 
        {
            tomarUsuarios();
            int salida = usuarios.agregarFila(fila);
            soltarUsuarios();
            return salida;
        }
        public int usuariosModificar(string[] fila)
        {
            tomarUsuarios();
            int salida = usuarios.editarFila(fila);
            soltarUsuarios();
            return salida;
        }
        public int usuariosEliminar(string fila)
        {
            tomarUsuarios();
            int salida = usuarios.eliminarFila(fila);
            soltarUsuarios();
            return salida;
        }
        public string usuariosNVersion()
        {
            string salida;
            tomarUsuarios();
            salida = usuarios.obtenerVersion();
            soltarUsuarios();
            return salida;
        }
        public XmlElement usuariosObtener()
        {
            XmlDocument retorno;
            tomarUsuarios();
            retorno = usuarios.obtenerTabla();
            soltarUsuarios();
            return retorno.DocumentElement;
        }
        public XmlElement usuariosErrores()
        {
            XmlDocument retorno;
            tomarUsuarios();
            retorno = usuarios.excepcionResumen();
            soltarUsuarios();
            return retorno.DocumentElement;
        }
        public string usuariosErrorUltimo()
        {
            tomarUsuarios();
            string retorno = usuarios.excepcionLastError();
            soltarUsuarios();
            return retorno;
        }
        //---------------Metodos de Memos
        public bool escribirMemo(string usuario, string entrada)
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\MemosGenerales.xml";            
            try
            {
                XmlDocument actual = new XmlDocument();
                actual.Load(ruta);
                XmlNode nodo = actual.DocumentElement;
                XmlNode memo = actual.CreateElement("" + usuario);
                memo.InnerText = entrada;
                nodo.AppendChild(memo);
                actual.Save(ruta);
            }
            catch
            {
                XElement actual = new XElement("Memos");
                actual.Add(new XElement(usuario, entrada));
                actual.Save(ruta);
            }
            return true;
        }
        public XmlElement obtenerMemosGenrales()
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\MemosGenerales.xml";
            XmlDocument actual = new XmlDocument();
            try
            {
                actual.Load(ruta);
            }
            catch
            {
                XElement nuevo = new XElement("memos");
                nuevo.Save(ruta);
                actual.Load(ruta);
            }
            return actual.DocumentElement;
        }
        public int guardarCotizacion(XElement cotizacion)
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\cantidadC.cnt";
            int id = 1;
            try
            {
                XmlDocument actual = new XmlDocument();
                actual.Load(ruta);
                XmlNode nodo = actual.DocumentElement;
                id = Convert.ToInt32(nodo.FirstChild.InnerText);
                id++;
                nodo.FirstChild.InnerText = "" + id;
                actual.Save(ruta);
            }
            catch
            {
                XElement cant = new XElement("cantidad");
                cant.Add(new XElement("id", id));
                cant.Save(ruta);
            }
            cotizacion.Add(new XElement("ID"), id);
            cotizacion.Save(folder+@"\cotizaciones\cotizacion" + id + ".xml");
            return id;
        }
        public XmlElement listaCotizaciones()
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\cotizaciones\";
            System.IO.DirectoryInfo directorio = new System.IO.DirectoryInfo(ruta);
            System.IO.FileInfo[] archivos = directorio.GetFiles();
            int id = archivos.Length;
            XmlDocument salida = new XmlDocument();
            XElement temp = new XElement("temp");
            for (int i = 0; i < id; i++)
            {
                XElement nodo = new XElement("venta");
                XmlDocument file = new XmlDocument();
                file.Load(ruta + archivos[i].Name);
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
            temp.Save(ruta+@"\temp.xml");
            salida.Load(ruta + @"\temp.xml");
            return salida.DocumentElement;
        }
        public XmlElement obtenerFactura(int numero)
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\tem.xml";
            string ID = "" + numero;
            XElement nuevo = new XElement("factura");
            nuevo.Save(ruta);
            DirectoryInfo directorio = new DirectoryInfo(folder + @"\cotizaciones\");
            FileInfo[] archivos = directorio.GetFiles();
            int id = archivos.Length;
            for (int i = 0; i < id; i++)
            {
                XmlDocument file = new XmlDocument();
                file.Load(folder+@"\cotizaciones\" + archivos[i].Name);
                if (file.DocumentElement.LastChild.InnerText == ID)
                {
                    file.Save(ruta);
                    break;
                }
            }
            XmlDocument factura = new XmlDocument();
            factura.Load(ruta);
            return factura.DocumentElement;
        }
        public XmlElement indicadoresEconomicos()
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\indicadoresEconomicos.xml";
            XmlDocument salida = new XmlDocument();
            try
            {
                salida.Load(ruta);
            }
            catch
            {
                XElement nuevo = new XElement("indicadores");
                nuevo.Add(new XElement("Dolar", 1));
                nuevo.Add(new XElement("Euro", 1));
                nuevo.Add(new XElement("Importe", 1));
                nuevo.Save(ruta);
                salida.Load(ruta);
            }
            return salida.DocumentElement;
        }
        public bool setearIndicadoresEconomicos(string dolar, string euro, string importe)
        {
            string folder = Environment.CurrentDirectory;
            string ruta = folder + @"\indicadoresEconomicos.xml";
            try
            {
                XElement nuevo = new XElement("indicadores");
                nuevo.Add(new XElement("Dolar", dolar));
                nuevo.Add(new XElement("Euro", euro));
                nuevo.Add(new XElement("Importe", importe));
                nuevo.Save(ruta);
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public bool ingresarFacturaLiberada(XElement factura)
        {
            string folder = Environment.CurrentDirectory;
            string mes = "" + DateTime.Now.Month;
            string año = "" + DateTime.Now.Year;
            DirectoryInfo directorioMes = new DirectoryInfo(folder+@"\facturas\" + año + @"\" + mes + @"\");
            DirectoryInfo directorioAño = new DirectoryInfo(folder+@"\facturas\" + año + @"\");
            if (directorioAño.Exists == false)
                directorioAño.Create();
            if (directorioMes.Exists == false)
                directorioMes.Create();
            directorioMes.Create();
            FileInfo[] archivos = directorioMes.GetFiles();
            int id = 1;
            try
            {
                XmlDocument actual = new XmlDocument();
                actual.Load(folder+@"\cantidadF.cnt");
                XmlNode nodo = actual.DocumentElement;
                id = Convert.ToInt32(nodo.FirstChild.InnerText);
                id++;
                nodo.FirstChild.InnerText = "" + id;
                actual.Save(folder + @"\cantidadF.cnt");
            }
            catch
            {
                XElement cant = new XElement("cantidad");
                cant.Add(new XElement("id", id));
                cant.Save(folder + @"\cantidadF.cnt");
            } factura.Add(new XElement("ID"), id);
            factura.Save(folder+@"\facturas\" + año + @"\" + mes + @"\factura" + id + ".xml");
            return true;
        }
        public string ingresarFacturaPorCotizacion(int identificacion)
        {
            string folder = Environment.CurrentDirectory;
            string IDENTI = "" + identificacion;
            string ruta = "0";
            int salida = 0;
            DirectoryInfo directorio = new DirectoryInfo(folder+"/cotizaciones/");
            FileInfo[] archivos = directorio.GetFiles();
            int id = archivos.Length;
            for (int i = 0; i < id; i++)
            {
                salida = 0;
                XmlDocument file = new XmlDocument();
                file.Load(folder+"/cotizaciones/" + archivos[i].Name);
                if (file.DocumentElement.LastChild.InnerText == IDENTI)
                {
                    salida = 3;
                    string mes = DateTime.Now.Month.ToString();
                    string año = DateTime.Now.Year.ToString();
                    DirectoryInfo directorioMes = new DirectoryInfo(folder + "/facturas/" + año + "/" + mes + "/");
                    DirectoryInfo directorioAño = new DirectoryInfo(folder + "/facturas/" + año + "/");
                    if (directorioAño.Exists == false)
                        directorioAño.Create();
                    if (directorioMes.Exists == false)
                        directorioMes.Create();
                    FileInfo[] archivos2 = directorioMes.GetFiles();
                    int id2 = 2;
                    try
                    {
                        XmlDocument actual = new XmlDocument();
                        actual.Load(folder + "/cantidadF.cnt");
                        XmlNode nodo = actual.DocumentElement;
                        id2 = Convert.ToInt32(nodo.FirstChild.InnerText);
                        id2++;
                        nodo.FirstChild.InnerText = "" + id2;
                        actual.Save(folder + "/cantidadF.cnt");
                    }
                    catch
                    {
                        XElement cant = new XElement("cantidad");
                        cant.Add(new XElement("id", id2));
                        cant.Save(folder + "/cantidadF.cnt");
                    } XmlNode ID = file.CreateElement("ID");
                    ID.InnerText = "" + id2;
                    foreach (XmlNode nodo in file.DocumentElement.ChildNodes)
                    {
                        if (nodo.Name == "MetaData")
                            nodo.InnerText = "I" + nodo.InnerText;
                    }
                    try
                    {
                        File.Delete(folder + "/cotizaciones/" + archivos[i].Name);
                    }
                    catch { }
                    ruta = folder + "/facturas/" + año + "/" + mes + "/factura" + id2 + ".xml";
                    file.Save(ruta);
                    return ruta;
                }
            }
            return ruta;
        }
        public XmlElement facturasResumen(int año, int mes)
        {
            string folder = Environment.CurrentDirectory;//folder+"
            XElement nuevo = new XElement("factura");
            nuevo.Save(folder + "/LIBRO.xml");
            DirectoryInfo directorioMes = new DirectoryInfo(folder + "/facturas/" + año + "/" + mes + "/");
            if (directorioMes.Exists)
            {
                FileInfo[] archivos = directorioMes.GetFiles();
                int id = archivos.Length;
                for (int i = 0; i < id; i++)
                {
                    XElement factura = new XElement("Factura" + (i + 1));
                    XmlDocument file = new XmlDocument();
                    file.Load(folder + "/facturas/" + año + "/" + mes + "/" + archivos[i].Name);
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
                                    elemento2.Value = detalleProductos.InnerText;
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
            nuevo.Save(folder + "/LIBRO.xml");
            XmlDocument libroFinal = new XmlDocument();
            libroFinal.Load(folder + "/LIBRO.xml");
            return libroFinal.DocumentElement;
        }
        public string consultarActualizacion()
        {
            string folder = Environment.CurrentDirectory;//folder+"
            string rutaCarpetaPublica = folder + "/Programa/";
            try
            {
                XmlDocument file = new XmlDocument();
                file.Load(rutaCarpetaPublica + "Actualizacion.xml");
                return file.DocumentElement.FirstChild.FirstChild.InnerText;
            }
            catch
            {
                return "NULL";
            }
        }
        public bool subirActualizacionAServidor(string versionNueva, string fecha, int CantArchivos)
        {
            string folder = Environment.CurrentDirectory;//folder+"
            
            try
            {
                string rutaCarpetaPublica = folder + "/Programa/";
                //asegurarse de la existencia de los directorios
                DirectoryInfo directorioVer = new DirectoryInfo(rutaCarpetaPublica);
                if (directorioVer.Exists == false)
                    directorioVer.Create();
                rutaCarpetaPublica = rutaCarpetaPublica + "/Actualizacion/";
                directorioVer = new DirectoryInfo(rutaCarpetaPublica);
                if (directorioVer.Exists == false)
                    directorioVer.Create();
                //descarga de la actualizacion
                new WebClient().DownloadFile("http://25.76.227.80/Varelec/Programa/" + versionNueva + ".zip", rutaCarpetaPublica +versionNueva + ".zip");
                //creacion de xml con la version nueva, fecha y la cantidad de datos.
                XElement nuevo = new XElement("Actualizacion");
                nuevo.Add(new XElement("Version", versionNueva));
                nuevo.Add(new XElement("Fecha", fecha)); //formato DD-MM-AA
                nuevo.Add(new XElement("CantArchivo", CantArchivos));
                nuevo.Save(rutaCarpetaPublica+"Actualizacion.xml");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool IngresarFacturaIngreso(XElement factura)
        {
            string folder = Environment.CurrentDirectory;//folder+"
            
            try
            {
                int id = 1;
                try
                {
                    XmlDocument actual = new XmlDocument();
                    actual.Load(folder + "/cantidadFI.cnt");
                    XmlNode nodo = actual.DocumentElement;
                    id = Convert.ToInt32(nodo.FirstChild.InnerText);
                    id++;
                    nodo.FirstChild.InnerText = "" + id;
                    actual.Save(folder + "/cantidadFI.cnt");
                }
                catch
                {
                    XElement cant = new XElement("cantidad");
                    cant.Add(new XElement("id", id));
                    cant.Save(folder + "/cantidadFI.cnt");
                }
                string mes = "" + DateTime.Now.Month;
                string año = "" + DateTime.Now.Year;
                DirectoryInfo directorioMes = new DirectoryInfo(folder + "/facturasIngreso/" + año + "/" + mes + "/");
                DirectoryInfo directorioAño = new DirectoryInfo(folder + "/facturasIngreso/" + año + "/");
                if (!directorioAño.Exists)
                    directorioAño.Create();
                if (!directorioMes.Exists)
                    directorioMes.Create();
                directorioMes.Create();
                factura.Save(folder + "/facturasIngreso/" + año + "/" + mes + "/factura" + id + ".xml");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public XmlElement facturasIngresoResumen(int año, int mes)
        {
            string folder = Environment.CurrentDirectory;//folder+"
            
            XElement nuevo = new XElement("factura");
            nuevo.Save(folder + "/LIBROIngreso.xml");
            DirectoryInfo directorioMes = new DirectoryInfo(folder + "/facturasIngreso/" + año + "/" + mes + "/");
            if (directorioMes.Exists)
            {
                FileInfo[] archivos = directorioMes.GetFiles();
                int id = archivos.Length;
                for (int i = 0; i < id; i++)
                {
                    XElement factura = new XElement(archivos[i].Name);
                    XmlDocument file = new XmlDocument();
                    file.Load(folder + "/facturasIngreso/" + año + "/" + mes + "/" + archivos[i].Name);
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
            nuevo.Save(folder + "/LIBROIngreso.xml");
            XmlDocument libroFinal = new XmlDocument();
            libroFinal.Load(folder + "/LIBROIngreso.xml");
            //Console.WriteLine(libroFinal.DocumentElement.InnerXml); 
            return libroFinal.DocumentElement;
        }
        public bool ingresoPMP(string NP, int NoE, int EoS, int cantidad, int precioFTR, int precioEXP, string moneda)
        {
            string folder = Environment.CurrentDirectory;//folder+"
            
            try
            {
                if (File.Exists(folder + "/HistorialPMP.xml") == false)
                {
                    XElement test = new XElement("PMPTOTAL");
                    test.Save(folder + "/HistorialPMP.xml");
                }
                XmlDocument actual = new XmlDocument();
                actual.Load(folder + "/HistorialPMP.xml");
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
                preFTR.InnerText = "" + precioFTR;
                preEXP.InnerText = "" + precioEXP;
                coin.InnerText = moneda;
                elemento.AppendChild(fecha);
                elemento.AppendChild(coin);
                elemento.AppendChild(estado);
                elemento.AppendChild(entraosale);
                elemento.AppendChild(cant);
                elemento.AppendChild(preFTR);
                elemento.AppendChild(preEXP);
                nodo.AppendChild(elemento);
                actual.Save(folder + "/HistorialPMP.xml");
            }
            catch
            {
                return false;
            }
            return true;
        }
        public XmlElement obtenerPMP()
        {
            string folder = Environment.CurrentDirectory;//folder+"
            XmlDocument actual = new XmlDocument();
            try
            {
                actual.Load(folder + "/HistorialPMP.xml");
            }
            catch
            {
                XElement nuevo = new XElement("memos");
                nuevo.Save(folder + "/HistorialPMP.xml");
                actual.Load(folder + "/HistorialPMP.xml");
            }
            return actual.DocumentElement;
        }
        public bool uploadFile(string name)
        {
            string folder = Environment.CurrentDirectory;//folder+"
            try
            {
                DirectoryInfo directorioVer = new DirectoryInfo(folder + "/programa/");
                if (!directorioVer.Exists)
                    directorioVer.Create();
                new WebClient().DownloadFile("http://25.38.189.33/Actualizacion/" + name, folder + "/programa/" + name);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool registrarInicio(string usuario, string dateComplete)
        {
            try 
            {
                string folder = Environment.CurrentDirectory;//folder+"
                if (!File.Exists(folder + "/HistorialInicios.xml"))
                {
                    XElement test = new XElement("Inicios");
                    test.Save(folder + "/HistorialInicios.xml");
                }
                XmlDocument actual = new XmlDocument();
                actual.Load(folder + "/HistorialInicios.xml");
                XmlNode nodo = actual.DocumentElement;
                XmlNode elemento = actual.CreateElement("log");
                XmlNode fecha = actual.CreateElement("Fecha");
                XmlNode user = actual.CreateElement("Usuario");
                user.InnerText = usuario;
                fecha.InnerText = dateComplete;
                elemento.AppendChild(user);
                elemento.AppendChild(fecha);
                nodo.AppendChild(elemento);
                actual.Save(folder + "/HistorialInicios.xml");
                return true;
            }
            catch 
            {
                return false;
            }
        }
        public XmlElement obtenerHistorialInicios()
        {
            string folder = Environment.CurrentDirectory;//folder+"
            XmlDocument actual = new XmlDocument();
            try
            {
                actual.Load(folder + "/HistorialInicios.xml");
            }
            catch
            {
                XElement nuevo = new XElement("memos");
                nuevo.Save(folder + "/HistorialInicios.xml");
                actual.Load(folder + "/HistorialInicios.xml");
            }
            return actual.DocumentElement;
        }
    }
}
