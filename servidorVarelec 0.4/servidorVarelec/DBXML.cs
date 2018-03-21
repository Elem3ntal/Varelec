using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace servidorVarelec
{
    class DBXML
    {
        private string nombre;//nombre de la tabla
        private bool status;//true tabla abierta, false cerrada
        private string rutaDBXML;//los datos
        private string rutaPrimaryKeys;//las llaves primarias de la tabla
        private string rutaNVersion;//la nversion antes de modificar
        private string rutaStatus;//tabla abierta o cerrada, asi se evita las dobles operaciones
        private string rutaLastOP;//ultima operacion realizada, si nversion no calza, se realiza de nuevo
        public DBXML(string nombre)
        //constructor (BOB)
        //instancia las rutas de los archivos
        //STATUS=false
        {
            this.nombre = nombre;
            string directorioTablas = Environment.CurrentDirectory + @"\Tablas\";
            DirectoryInfo directorioVer = new DirectoryInfo(directorioTablas);
            if (directorioVer.Exists == false)
                directorioVer.Create();//me aseguro que exista la carpeta donde se almacenan las tablas
            //creo la carpeta de la tabla
            string folder = directorioTablas + nombre + @"\";
            DirectoryInfo carpetaTabla = new DirectoryInfo(folder);
            if (carpetaTabla.Exists == false)
                carpetaTabla.Create();
            //una tabla se compone de 4 archivos
            //donde se guardan todos los datos
            rutaDBXML = folder + nombre + ".dbxml";
            //donde se guardan las llaves primarias
            rutaPrimaryKeys = folder + "PrimaryKeys";
            //donde se guarda en forma externa la NVersion
            rutaNVersion = folder + "NVersion";
            //si la tabla esta abierta o cerrada, es para verificar en caso de caidas
            rutaStatus = folder + "Status";
            //ultima operacion, para en caso de fallos
            rutaLastOP = folder + "LastOperation";
            status = false;
        }
        private int LastOperation(string Operacion, string[] argumentos)
        //FALTA MODIFICAR ESTE METODO!!!
        {
            try
            {
                StreamWriter LOP = new StreamWriter(rutaLastOP);
                LOP.WriteLine(Operacion); //(letra mayuscula) X cerrado O abierto
                LOP.Close();
                return 1;
            }
            catch (Exception e)
            {
                excepcion("LastOperation", e.Message);
                return 0;
            }
        }
        private void excepcion(string funcion, string mensaje)
        //escribe un archivo con la fecha de hoy y pone el nombre de la funcion y el error
        {
            string folder = Environment.CurrentDirectory;
            string path = folder + @"\error" + DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day + ".txt";
            StreamWriter sw = new StreamWriter(path, true);
            Console.WriteLine(funcion + " - " + mensaje);
            sw.WriteLine(funcion + " - " + mensaje);
            sw.Close();
        }
        private string nVersion(string version)
        //recive el actual y entrega el sig, EJ: 0->1, 9->a, Z->00
        {
            char[] nuevo = new char[version.Length];
            for (int i = version.Length - 1; i >= 0; i--)
            {
                nuevo[i] = version[i];
            }
            for (int i = version.Length - 1; i >= 0; i--)
            {
                nuevo[i] = Convert.ToChar(nuevo[i] + 1);
                if (nuevo[i] >= 48 && nuevo[i] <= 57)
                    break;
                else if (nuevo[i] >= 65 && nuevo[i] <= 90)
                    break;
                else if (nuevo[i] >= 97 && nuevo[i] <= 122)
                    break;
                if (nuevo[i] == 58)
                {
                    nuevo[i] = Convert.ToChar(65);
                    break;
                }
                if (nuevo[i] == 91)
                {
                    nuevo[i] = Convert.ToChar(97);
                    break;
                }
                if (nuevo[i] == 123)
                {
                    nuevo[i] = Convert.ToChar(48);
                    if (i == 0)
                    {
                        char[] nuevo2 = new char[version.Length + 1];
                        for (int i2 = version.Length - 1; i2 >= 0; i2--)
                        {
                            nuevo2[i2] = nuevo[i2];
                        }
                        nuevo2[version.Length] = Convert.ToChar(48);
                        string salida = "";
                        foreach (char letra in nuevo2)
                            salida = salida + letra;
                        return salida;
                    }
                }
            }
            string salida1 = "";
            foreach (char letra in nuevo)
                salida1 = salida1 + letra;
            return salida1;
        }
        public int crearTabla(int columnas, string[] nombreColumnas)
        //crea los 5 archivos ultilizados y los inicializa
        {
            try
            {
                if (nombre.Contains(' ') == false)
                {
                    if (columnas >= 1)
                    {
                        //llenado de dbxml
                        XElement data = new XElement("DBXML");
                        XElement metaData = new XElement("MetaData");
                        XElement cantColumnas = new XElement("Cant" + columnas);
                        for (int i = 0; i < columnas; i++)
                            cantColumnas.Add(new XElement(nombreColumnas[i]));
                        metaData.Add(cantColumnas);
                        metaData.Add(new XElement("ID", "0"));
                        metaData.Add(new XElement("NVersion", "0"));
                        data.Add(metaData);
                        data.Add(new XElement("Data"));
                        data.Save(rutaDBXML);
                        //llenado de nversion
                        StreamWriter nversion = new StreamWriter(rutaNVersion);
                        nversion.WriteLine("0");
                        nversion.Close();
                        //se crea archivo de llaves primarias vacio
                        StreamWriter primaryKeys = new StreamWriter(rutaPrimaryKeys);
                        primaryKeys.Close();
                        //status cerrado
                        StreamWriter estado = new StreamWriter(rutaStatus);
                        estado.WriteLine("O"); //(letra mayuscula) X cerrado O abierto
                        estado.Close();
                        status = true;
                        //ultima operacion fue crear la tabla
                        LastOperation("" + columnas, nombreColumnas);
                        Thread.Sleep(500);
                        return 1;
                    }
                    else
                        return 5;
                }
                else
                    return 3;

            }
            catch (Exception e)
            {
                excepcion("crearTabla", e.Message);
                return 0;
            }
        }
        public int abrirTabla()
        //toma el status, y abre la tabla
        {
            try
            {
                if (!status) //en caso de que la tabla este cerrada (negacion de false)
                {
                    StreamReader estatus = new StreamReader(rutaStatus);
                    if (estatus.ReadLine().Equals("X"))//si la tabla estaba cerrada.
                    {
                        status = true;
                        estatus.Close();
                        StreamWriter ponerAbierto = new StreamWriter(rutaStatus);
                        ponerAbierto.WriteLine("O");
                        ponerAbierto.Close();
                    }
                    else//en caso de que la tabla este abierta, esto puede implicar un cierre inesperado
                    {
                        //comparar los 2 nversion.
                        string nversionDBXML = obtenerVersion();
                        StreamReader sr = new StreamReader(rutaNVersion);
                        string nversionRespaldo = sr.ReadLine();
                        if (nversionDBXML.Equals(nversionRespaldo))//mal cerrada pero sin perdida de datos
                            return 2; //mal cerrado pero SIN perdida de datos
                        else
                            return 3;//mal cerrado pero CON perdida de datos, aqui se opera con LastOperation
                    }
                    return 1;
                }
                else
                    return 4; //la tabla ya se encuentra abierta
            }
            catch (Exception e)
            {
                excepcion("abrirTabla", e.Message);
                return 0;
            }
        }
        public int cerrarTabla()
        //toma el status, y cierra la tabla
        {
            try
            {
                if (status) //si esta abierto
                {
                    StreamWriter sw = new StreamWriter(rutaStatus);
                    sw.WriteLine("X"); //escribe el cerrado
                    sw.Close();
                    status = false; //cierra el objeto
                    return 1;
                }
                else // si la tabla esa cerrada
                {
                    StreamReader sr = new StreamReader(rutaStatus);
                    if (sr.ReadLine().Equals("X")) //revisa que ya este cerrada
                    {
                        Console.WriteLine("Tabla - " + nombre + "- ya se encuentra cerrada");
                        return 2;
                    }
                    else
                        return 3;
                }
            }
            catch (Exception e)
            {
                excepcion("cerrarTabla", e.Message);
                return 0;
            }
        }
        private int RevisionPrimaryKey(string match)
        //busca que el "match" no este en el listado de llaves primarias
        {
            try
            {
                StreamReader sr = new StreamReader(rutaPrimaryKeys);
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine("compara->" + line + "Con ->" + match);
                    if (line.Equals(match))
                    {
                        sr.Close();
                        return 5; //si la linea existe
                    }
                }
                sr.Close();
                return 1;//si la linea no existe
            }
            catch (Exception e)
            {
                excepcion("RevisionPrimaryKey", e.Message);
                return 0;//excepcion
            }
        }
        private int eliminarPrimaryKey(string match)
        //para eliminar la pk copia el resto a un archivo temp y lo cambia
        {
            try
            {
                StreamReader sr = new StreamReader(rutaPrimaryKeys);
                StreamWriter sw = new StreamWriter("temp");
                string line;
                bool pk = false;
                while ((line = sr.ReadLine()) != null)
                {
                    //Console.WriteLine("compara->" + line + "Con ->" + match);
                    if (!line.Equals(match))
                    {
                        sw.WriteLine(line); //si encuentra la pk no la copia como el resto
                        pk = true;
                    }
                }
                sr.Close();
                sw.Close();
                new FileInfo(rutaPrimaryKeys).MoveTo(rutaPrimaryKeys + ".old");
                new FileInfo("temp").MoveTo(rutaPrimaryKeys);
                new FileInfo(rutaPrimaryKeys + ".old").Delete();
                if (pk)
                    return 1;
                else
                    return 2;
            }
            catch (Exception e)
            {
                excepcion("eliminarPrimaryKey", e.Message);
                return 0;//excepcion
            }
        }
        public int agregarFila(string[] fila)
        //con la tabla abierta, asegura que no se repita la llave primaria, y pone los datos, etc...
        {
            try
            {
                if (status)//si la tabla esta abierta, puede agregar
                {
                    int largoFilaNueva = fila.Length;
                    XmlDocument data = new XmlDocument();
                    data.Load(rutaDBXML);
                    XmlNode metaData = data.DocumentElement;
                    int id = 0;
                    int cantidadColumnas = Convert.ToInt32(metaData.FirstChild.FirstChild.Name.Split('t')[1]);
                    if (cantidadColumnas == largoFilaNueva)
                    {
                        int retorno;
                        if ((retorno = RevisionPrimaryKey(fila[0])) != 1)
                            return retorno; //retorno que se encontro la llave primatria
                        StreamWriter swpk = new StreamWriter(rutaPrimaryKeys, true);
                        swpk.WriteLine(fila[0]);
                        swpk.Close();
                        string NVersionTemporal = "0";
                        XmlNode nodoModelo = metaData.FirstChild.FirstChild.Clone();
                        foreach (XmlNode nodo in metaData.FirstChild.ChildNodes) //para tomar el metadata
                        {
                            if (nodo.Name.Equals("ID")) //tomo el id para el Data, y lo aumento
                            {
                                id = Convert.ToInt32(nodo.InnerText);
                                nodo.InnerText = "" + (id + 1);
                            }
                            else if (nodo.Name.Equals("NVersion")) //encuentro el nversion, y modifico el de respaldo
                            {
                                NVersionTemporal = nVersion(nodo.InnerText);
                                nodo.InnerText = NVersionTemporal;
                                StreamWriter sw = new StreamWriter(rutaNVersion);
                                sw.WriteLine(NVersionTemporal);
                                sw.Close();
                            }
                        }
                        //aca se escribe el nodo nuevo
                        XmlNode nodoNuevo = data.CreateElement("DATA" + id);
                        int i = 0;
                        foreach (XmlNode nombres in nodoModelo.ChildNodes)
                        {
                            Console.WriteLine("" + i + " ->" + nombres.Name);
                            XmlNode dat = data.CreateElement(nombres.Name);
                            dat.InnerText = fila[i];
                            nodoNuevo.AppendChild(dat);
                            i++;
                        }
                        metaData.LastChild.AppendChild(nodoNuevo);
                        data.Save(rutaDBXML);
                    }
                    else
                    {
                        return 3;
                    }
                    return 1;
                }
                return 2;
            }
            catch (Exception e)
            {
                excepcion("agregarFila", e.Message);
                return 0;
            }
        }
        public int editarFila(string[] fila)
        //con la tabla abierta, modifica el nodo del dbxml y el Nversion
        {
            try
            {
                if (status)//si la tabla esta abierta, puede modificar
                {
                    int retorno;
                    if ((retorno = RevisionPrimaryKey(fila[0])) != 5) //retorna 1 si es nueva,5 si existe, 0 exepcion
                        return retorno;
                    int largoFilaNueva = fila.Length;
                    XmlDocument data = new XmlDocument();
                    data.Load(rutaDBXML);
                    XmlNode metaData = data.DocumentElement;
                    int cantidadColumnas = Convert.ToInt32(metaData.FirstChild.FirstChild.Name.Split('t')[1]);
                    if (cantidadColumnas == largoFilaNueva)
                    {
                        //modificando los metadatos
                        string NVersionTemporal = nVersion(metaData.FirstChild.LastChild.InnerText);
                        StreamWriter sw = new StreamWriter(rutaNVersion);
                        sw.WriteLine(NVersionTemporal);
                        sw.Close();
                        //modificando los datos
                        foreach (XmlNode nodo in metaData.LastChild.ChildNodes)
                        {
                            if (nodo.FirstChild.InnerText == fila[0])
                            {
                                int i = 0;
                                foreach (XmlNode DATO in nodo.ChildNodes)
                                {
                                    DATO.InnerText = fila[i];
                                    i++;
                                }
                                //una vez que realizo el cambio, modifica el nversion del dbxml
                                metaData.FirstChild.LastChild.InnerText = NVersionTemporal;
                                data.Save(rutaDBXML);
                                return 1;
                            }
                        }
                    }
                    else
                        return 2;
                    return 3;
                }
                return 4;
            }
            catch (Exception e)
            {
                excepcion("editarFila", e.Message);
                return 0;
            }
        }
        public int eliminarFila(string dato)
        //con la tabla abierta, elimina el nodo, modifica el nversion, 
        {
            try
            {
                if (status) //si la tabla esta abierta
                {
                    XmlDocument data = new XmlDocument();
                    data.Load(rutaDBXML);
                    XmlNode metaData = data.DocumentElement;
                    if (eliminarPrimaryKey(dato) == 1) //si elimina correctamente el nodo
                    {
                        foreach (XmlNode nodo in metaData.LastChild.ChildNodes)
                        {
                            if (nodo.FirstChild.InnerText == dato)
                            {
                                //elimina el nodo, modifica el nversion, y lo guarda
                                metaData.LastChild.RemoveChild(nodo);
                                string version = nVersion(metaData.FirstChild.LastChild.InnerText);
                                metaData.FirstChild.LastChild.InnerText = version;
                                data.Save(rutaDBXML);
                                return 1;
                            }
                        }
                        return 3;
                    }
                    return 4;
                }
                return 5;
            }
            catch (Exception e)
            {
                excepcion("eliminarFila", e.Message);
                return 0;
            }
        }
        public XmlDocument obtenerTabla()
        //con la tabla abierta, manda la tabla completa, sino, tabla nula
        {
            try
            {
                XmlDocument salida = new XmlDocument();
                if (status)
                    salida.Load(rutaDBXML);
                return salida;
            }
            catch (Exception e)
            {
                excepcion("obtenerTabla", e.Message);
                return null;
            }
        }
        public XmlDocument obtenerFilasFiltroColumnas(string termino, string columna)
        //si la tabla esta abierta, manda la tabla, sino, tabla nula
        {
            try
            {
                if (!status)
                {
                    XmlDocument nulo = new XmlDocument();
                    return nulo;
                }
                XmlDocument salida = new XmlDocument();
                salida.Load(rutaDBXML);
                foreach (XmlNode nodo in salida.DocumentElement.LastChild.ChildNodes)//todos los nodos
                {
                    foreach (XmlNode nodo2 in nodo)//todas las columnas del nodo
                    {
                        if (nodo2.Name.Equals(columna))//si la columna es igual a donde busca
                        {
                            if (nodo2.InnerText.Equals(termino) == false) //y el termino en esa columna no es igual
                            {
                                salida.DocumentElement.LastChild.RemoveChild(nodo); //es removiso
                            }
                        }
                    }
                }
                return salida; //se manda la tabla final
            }
            catch (Exception e)
            {
                excepcion("obtenerFilasFiltroColumnas", e.Message);
                return null;
            }
        }
        public string obtenerVersion()
        //retorna el NVersion de la tabla.
        {
            try
            {
                if (!status)
                    return "-2";
                XmlDocument salida = new XmlDocument();
                salida.Load(rutaDBXML);
                return salida.DocumentElement.FirstChild.LastChild.InnerText;
            }
            catch (Exception e)
            {
                excepcion("obtenerVersion", e.Message);
                return "-1";
            }
        }
        public int existe()
        //revisa la existencia de los 5 archivos de la tabla
        {
            if (File.Exists(rutaDBXML))
            {
                if (File.Exists(rutaLastOP))
                {
                    if (File.Exists(rutaNVersion))
                    {
                        if (File.Exists(rutaPrimaryKeys))
                        {
                            if (File.Exists(rutaStatus))
                                return 1;
                            return 2;
                        }
                        return 3;
                    }
                    return 4;
                }
                return 5;
            }
            return 0;
        }
        public int revisionTabla()
        //comprueba que los NVersion calcen, que todas las pk calcen entre los dos archivos.
        {
            try
            {
                //revision del nversion
                string nversionDBXML = obtenerVersion();
                StreamReader sr = new StreamReader(rutaNVersion);
                string nversionRespaldo = sr.ReadLine();
                if (nversionDBXML.Equals(nversionRespaldo))
                    return 2;
                sr.Close();
                int largoPK = 0;
                StreamReader srpk = new StreamReader(rutaPrimaryKeys);
                string line;
                while ((line = srpk.ReadLine()) != null)
                    largoPK++;
                srpk.Close();
                XmlDocument dbxml = new XmlDocument();
                dbxml.Load(rutaDBXML);
                int cantNodos = dbxml.DocumentElement.LastChild.ChildNodes.Count;
                if (largoPK != cantNodos)
                    return 3;
                return 1;
            }
            catch (Exception e)
            {
                excepcion("RevisionTabla", e.Message);
                return 0;
            }
        }
    }
}