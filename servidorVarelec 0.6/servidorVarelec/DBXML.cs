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
        /* 						ChangeLog:
         * Version: 1.1 del 9 - 9 -2015
			* implemento XML por tabla para ver excepciones.
			* implemento funciones para retornar XML de errores, retornar el ultimo y todos.
			* corrigio la fecha en NVersion. 
         * version: 1.0 del 6 - 8 - 2015
             * implemento el registro de LastOperation para cuando se agrega, modifica, y eliminan datos.
             * implemento el debug, para mostrar/ocultar los Console.WriteLine(), por defecto en False.
             * implemento la fecha al NVersion para obtener la fecha de la ultima modificacion.
             * corrigio el error al revisarPK en el primer ingreso.
         */
        private bool debug; //para mostrar los Console.WriteLine();
        private string nombre;//nombre de la tabla
        private bool status;//true tabla abierta, false cerrada
        private string rutaDBXML;//los datos.
        private string rutaPrimaryKeys;//las llaves primarias de la tabla.
        private string rutaNVersion;//la nversion antes de modificar.
        private string rutaStatus;//tabla abierta o cerrada, asi se evita las dobles operaciones.
        private string rutaLastOP;//ultima operacion realizada, si nversion no calza, se realiza de nuevo.
        private string rutaExcepcion;//archivo donde se guardan todos los errores posibles de las tablas.
        public DBXML(string nombre)
        //constructor (BOB)
        //instancia las rutas de los archivos
        //STATUS=false
        {
            this.nombre = nombre;
            debug = false;
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
            //para guardar las Excepciones
            rutaExcepcion = folder + "ExcepcionList.xml";
            status = false;
        }
        private int LastOperation(string Operacion, string[] argumentos)
        //FALTA MODIFICAR ESTE METODO!!!
        {
            try
            {
                string parametros = "";
                foreach (string argumento in argumentos)
                    parametros += "" + argumento;
                parametros = Operacion + "_" + parametros;
                StreamWriter LOP = new StreamWriter(rutaLastOP);
                if (debug)
                    Console.WriteLine("LastOperation-> " + parametros);
                LOP.WriteLine(parametros); //(letra mayuscula) X cerrado O abierto
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
        //escribe en un xml la fecha de hoy y el error ocurrido
        {
            string fechaHoy = DateTime.Today.Year + "-" + DateTime.Today.Month + "-" + DateTime.Today.Day;
            if (!File.Exists(rutaExcepcion))
            {
                XElement test = new XElement("Excepcions");
                test.Save(rutaExcepcion);
            }
            XmlDocument actual = new XmlDocument();
            actual.Load(rutaExcepcion);
            XmlNode nodo = actual.DocumentElement;
            XmlNode elemento = actual.CreateElement("Excepcion");
            XmlNode fn = actual.CreateElement("Funcion");
            XmlNode texto = actual.CreateElement("Mensaje");
            XmlNode fecha = actual.CreateElement("Fecha");
            fn.InnerText = funcion;
            texto.InnerText = mensaje;
            fecha.InnerText = fechaHoy;
            elemento.AppendChild(fn);
            elemento.AppendChild(texto);
            elemento.AppendChild(fecha);
            nodo.AppendChild(elemento);
            if (debug)
                Console.WriteLine("Excepcion-> " + funcion + " - " + mensaje + " - " + fechaHoy);
            actual.Save(rutaExcepcion);
        }
        public XmlDocument excepcionResumen()
        //retorna el xml con las excepciones producidas.
        {
            XmlDocument salida = new XmlDocument();
            salida.Load(rutaExcepcion);
            if (debug)
                Console.WriteLine("excepcionResumen-> enviando Excepciones");
            return salida;
        }
        public string excepcionLastError()
        //retorna el ultimo error, formato "funcion" "+" "error"
        {
            XmlDocument data = new XmlDocument();
            data.Load(rutaExcepcion);
            XmlNode metaData = data.DocumentElement.LastChild;
            string retorno = metaData.FirstChild.InnerText;
            foreach (XmlNode nodo in metaData)
                if (nodo.Name.Equals("Mensaje"))
                    retorno += " + " + nodo.InnerText;
            return retorno;
        }
        private string nVersion(string version)
        //recive el actual y entrega el sig, EJ: 0->1, 9->a, Z->00
        {
            char[] nuevo = new char[version.Length];
            for (int i = version.Length - 1; i >= 0; i--)
                nuevo[i] = version[i];
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
                        if (debug)
                            Console.WriteLine("Nversion in-> " + version + " out->" + salida);
                        return salida;
                    }
                }
            }
            string salida1 = "";
            foreach (char letra in nuevo)
                salida1 = salida1 + letra;
            if (debug)
                Console.WriteLine("Nversion in-> " + version + " out->" + salida1);
            return salida1;
        }
        public int crearTabla(int columnas, string[] nombreColumnas)
        //crea los 6 archivos ultilizados y los inicializa
        {
            try
            {
                if (nombre.Contains(' ') == false)
                {
                    if (columnas >= 1)
                    {
                        //llenado de dbxml
                        if (debug)
                            Console.WriteLine("crearTabla 0/6");
                        XElement data = new XElement("DBXML");
                        XElement metaData = new XElement("MetaData");
                        XElement Nversion = new XElement("Nversion");
                        XElement cantColumnas = new XElement("Cant" + columnas);
                        for (int i = 0; i < columnas; i++)
                            cantColumnas.Add(new XElement(nombreColumnas[i]));
                        metaData.Add(cantColumnas);
                        metaData.Add(new XElement("ID", "0"));
                        Nversion.Add(new XElement("NVersion", "0"));
                        string fecha = DateTime.Today.Date.ToString().Split(' ')[0];
                        Nversion.Add(new XElement("Date", fecha));
                        metaData.Add(Nversion);
                        data.Add(metaData);
                        data.Add(new XElement("Data"));
                        data.Save(rutaDBXML);
                        if (debug)
                            Console.WriteLine("crearTabla 1/6");
                        //llenado de nversion
                        StreamWriter nversion = new StreamWriter(rutaNVersion);
                        nversion.WriteLine("0");
                        nversion.Close();
                        if (debug)
                            Console.WriteLine("crearTabla 2/6");
                        //se crea archivo de llaves primarias vacio
                        StreamWriter PrimaryKeys = new StreamWriter(rutaPrimaryKeys);
                        PrimaryKeys.Write("");
                        PrimaryKeys.Close();
                        //status cerrado
                        if (debug)
                            Console.WriteLine("crearTabla 3/6");
                        StreamWriter estado = new StreamWriter(rutaStatus);
                        estado.WriteLine("O"); //(letra mayuscula) X cerrado O abierto
                        estado.Close();
                        status = true;
                        if (debug)
                            Console.WriteLine("crearTabla 4/6");
                        //ultima operacion fue crear la tabla
                        string[] parametros = new string[nombreColumnas.Length + 1];
                        parametros[0] = "" + columnas;
                        for (int i = 1; i < parametros.Length; i++)
                            parametros[i] = nombreColumnas[i - 1];
                        LastOperation("crearTabla", parametros);
                        if (debug)
                            Console.WriteLine("crearTabla 5/6");
                        excepcion("Constructor", "No existen Errores");
                        if (debug)
                            Console.WriteLine("crearTabla 6/6");
                        return 1;
                    }
                    else
                        return 2;
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
                    if (debug)
                        Console.WriteLine("AbrirTabla-> tabla abierta");
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
                        if (debug)
                            Console.WriteLine("AbrirTabla-> cerrada, pero respaldo indica abierto");
                        //comparar los 2 nversion.
                        string nversionDBXML = obtenerVersion();
                        StreamReader sr = new StreamReader(rutaNVersion);
                        string nversionRespaldo = sr.ReadLine();
                        if (debug)
                            Console.WriteLine("abrirTabla-> comparando Nversion actual: " + nversionDBXML + " con: " + nversionRespaldo);
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
                    if (debug)
                        Console.WriteLine("cerrarTabla-> tabla cerrada");
                    StreamWriter sw = new StreamWriter(rutaStatus);
                    sw.WriteLine("X"); //escribe el cerrado
                    sw.Close();
                    status = false; //cierra el objeto
                    return 1;
                }
                else // si la tabla esa cerrada
                {
                    if (debug)
                        Console.WriteLine("cerrarTabla-> la tabla ya esta cerrada");
                    StreamReader sr = new StreamReader(rutaStatus);
                    if (sr.ReadLine().Equals("X")) //revisa que ya este cerrada
                    {
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
                    if (debug)
                        Console.WriteLine("RevisionPrimaryKey->" + line + "Con ->" + match);
                    if (line.Equals(match))
                    {
                        sr.Close();
                        return 2; //si la linea existe
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
                    if (debug)
                        Console.WriteLine("compara->" + line + "Con ->" + match);
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
                    return 1; //encontrada y borrada
                else
                    return 2; //no encontrada 
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
                    if (debug)
                        Console.WriteLine("agregarFila-> 0/4 comparando filas DBXML: " + cantidadColumnas + ", con filas nuevas: " + largoFilaNueva);
                    if (cantidadColumnas == largoFilaNueva)
                    {
                        int retorno;
                        if ((retorno = RevisionPrimaryKey(fila[0])) != 1)
                            return retorno; //retorno que se encontro la llave primatria
                        StreamWriter swpk = new StreamWriter(rutaPrimaryKeys, true);
                        swpk.WriteLine(fila[0]);
                        swpk.Close();
                        if (debug)
                            Console.WriteLine("agregarFila-> 1/4 se escribio la PK: " + fila[0]);
                        string NVersionTemporal = "0";
                        XmlNode nodoModelo = metaData.FirstChild.FirstChild.Clone();
                        foreach (XmlNode nodo in metaData.FirstChild.ChildNodes) //para tomar el metadata
                        {
                            if (nodo.Name.Equals("ID")) //tomo el id para el Data, y lo aumento
                            {
                                id = Convert.ToInt32(nodo.InnerText);
                                nodo.InnerText = "" + (id + 1);
                            }
                            else if (nodo.Name.Equals("Nversion")) //encuentro el nversion, y modifico el de respaldo
                            {
                                NVersionTemporal = nVersion(nodo.FirstChild.InnerText);
                                nodo.LastChild.InnerText = DateTime.Today.Date.ToString().Split(' ')[0];
                                nodo.FirstChild.InnerText = NVersionTemporal;
                                StreamWriter sw = new StreamWriter(rutaNVersion);
                                sw.WriteLine(NVersionTemporal);
                                sw.Close();
                                if (debug)
                                    Console.WriteLine("agregarFila-> 2/4 nuevo Nversion: " + NVersionTemporal);
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
                        if (debug)
                            Console.WriteLine("agregarFila-> 3/4 se agrego la fila");
                        metaData.FirstChild.LastChild.FirstChild.InnerText = NVersionTemporal;
                        metaData.LastChild.AppendChild(nodoNuevo);
                        data.Save(rutaDBXML);
                        if (debug)
                            Console.WriteLine("agregarFila-> 4/4 se guardo la tabla");
                        LastOperation("agregarFila", fila);
                    }
                    else
                        return 3; //error, no calzan los elementos.
                    return 1; //realizado con exito.
                }
                return 4; // no se agrego, la tabla esta cerrada
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
                    if ((retorno = RevisionPrimaryKey(fila[0])) != 2) //retorna 1 si es nueva,2 si existe, 0 exepcion
                        return 10 + retorno;
                    int largoFilaNueva = fila.Length;
                    XmlDocument data = new XmlDocument();
                    data.Load(rutaDBXML);
                    XmlNode metaData = data.DocumentElement;
                    int cantidadColumnas = Convert.ToInt32(metaData.FirstChild.FirstChild.Name.Split('t')[1]);
                    if (debug)
                        Console.WriteLine("editarFila-> 0/3 comparando filas DBXML: " + cantidadColumnas + ", con filas nuevas: " + largoFilaNueva);
                    if (cantidadColumnas == largoFilaNueva)
                    {
                        //modificando los metadatos
                        string NVersionTemporal = nVersion(metaData.FirstChild.LastChild.FirstChild.InnerText);
                        StreamWriter sw = new StreamWriter(rutaNVersion);
                        sw.WriteLine(NVersionTemporal);
                        sw.Close();
                        if (debug)
                            Console.WriteLine("editarFila-> 1/3 nuevo Nversion: " + NVersionTemporal);
                        //modificando los datos
                        foreach (XmlNode nodo in metaData.LastChild.ChildNodes)
                        {
                            if (nodo.FirstChild.InnerText == fila[0])
                            {
                                if (debug)
                                    Console.WriteLine("editarFila-> 2/3 se encontro la pk: " + fila[0]);
                                int i = 0;
                                foreach (XmlNode DATO in nodo.ChildNodes)
                                {
                                    DATO.InnerText = fila[i];
                                    i++;
                                }
                                //una vez que realizo el cambio, modifica el nversion del dbxml
                                metaData.FirstChild.LastChild.FirstChild.InnerText = NVersionTemporal;
                                metaData.FirstChild.LastChild.LastChild.InnerText = DateTime.Today.Date.ToString().Split(' ')[0];
                                data.Save(rutaDBXML);
                                if (debug)
                                    Console.WriteLine("editarFila-> 3/3 se guarda la tabla");
                                LastOperation("editarFila", fila);
                                return 1;//realizado
                            }
                        }
                    }
                    else
                        return 2;//no calza con la cant de elementos
                    return 3;//no se encontro el elemento a modificar
                }
                return 4; //la tabla se encuentra cerrada
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
                        if (debug)
                            Console.WriteLine("eliminarFila-> 0/4 " + dato + "ha sido eliminado del listado PK");
                        foreach (XmlNode nodo in metaData.LastChild.ChildNodes)
                        {
                            if (nodo.FirstChild.InnerText == dato)
                            {
                                if (debug)
                                    Console.WriteLine("eliminarFila-> 1/4 se encontro el nodo con PK:" + dato);
                                //elimina el nodo, modifica el nversion, y lo guarda
                                metaData.LastChild.RemoveChild(nodo);
                                if (debug)
                                    Console.WriteLine("eliminarFila-> 2/4 se elimino el nodo");
                                string version = nVersion(metaData.FirstChild.LastChild.FirstChild.InnerText);
                                metaData.FirstChild.LastChild.FirstChild.InnerText = version;
                                metaData.FirstChild.LastChild.LastChild.InnerText = DateTime.Today.Date.ToString().Split(' ')[0];
                                if (debug)
                                    Console.WriteLine("eliminarFila-> 3/4 nuevo Nversion: " + version);
                                data.Save(rutaDBXML);
                                if (debug)
                                    Console.WriteLine("eliminarFila-> 4/4 Tabla guardada");
                                string[] parametro = { dato };
                                LastOperation("eliminarFila", parametro);
                                return 1;
                            }
                        }
                        return 3;
                    }
                    return 4;//primarykey no encontrada en respaldo
                }
                return 5;//tabla cerrada
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
                if (debug)
                    Console.WriteLine("obtenerTabla-> enviando Tabla");
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
                if (debug)
                    Console.WriteLine("obtenerFilasFiltroColumnas-> Tabla cargada");
                foreach (XmlNode nodo in salida.DocumentElement.LastChild.ChildNodes)//todos los nodos
                {
                    foreach (XmlNode nodo2 in nodo)//todas las columnas del nodo
                    {
                        if (nodo2.Name.Equals(columna))//si la columna es igual a donde busca
                        {
                            if (debug)
                                Console.WriteLine("obtenerFilasFiltroColumnas-> comparando: " + nodo2.InnerText + " con: " + termino);
                            if (!nodo2.InnerText.Equals(termino)) //y el termino en esa columna no es igual
                            {
                                salida.DocumentElement.LastChild.RemoveChild(nodo); //es removiso
                            }
                        }
                    }
                }
                if (debug)
                    Console.WriteLine("obtenerFilasFiltroColumnas-> enviando tabla filtrada");
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
                string version = salida.DocumentElement.FirstChild.LastChild.FirstChild.InnerText;
                if (debug)
                    Console.WriteLine("obtenerVersion-> Nversion: " + version);
                return version;
            }
            catch (Exception e)
            {
                excepcion("obtenerVersion", e.Message);
                return "-1";
            }
        }
        public string obtenerFecha()
        //retorna el NVersion de la tabla.
        {
            try
            {
                if (!status)
                    return "-2";
                XmlDocument salida = new XmlDocument();
                salida.Load(rutaDBXML);
                string retorno = salida.DocumentElement.FirstChild.LastChild.FirstChild.InnerText;
                if (debug)
                    Console.WriteLine("obtenerFecha-> Fecha: " + retorno);
                return retorno;
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
                if (debug)
                    Console.WriteLine("existe-> DBXML si existe");
                if (File.Exists(rutaLastOP))
                {
                    if (debug)
                        Console.WriteLine("existe-> LastOP si existe");
                    if (File.Exists(rutaNVersion))
                    {
                        if (debug)
                            Console.WriteLine("existe-> NVersion si existe");
                        if (File.Exists(rutaPrimaryKeys))
                        {
                            if (debug)
                                Console.WriteLine("existe-> PrimaryKeys si existe");
                            if (File.Exists(rutaStatus))
                            {
                                if (debug)
                                    Console.WriteLine("existe-> Status si existe");
                                return 1;
                            }
                            if (debug)
                                Console.WriteLine("existe-> Status NO existe");
                            return 2;
                        }
                        if (debug)
                            Console.WriteLine("existe-> PrimaryKeys NO existe");
                        return 3;
                    }
                    if (debug)
                        Console.WriteLine("existe-> NVersion NO existe");
                    return 4;
                }
                if (debug)
                    Console.WriteLine("existe-> LastOP NO existe");
                return 5;
            }
            if (debug)
                Console.WriteLine("existe-> DBXML NO existe");
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
                if (debug)
                    Console.WriteLine("revisionTabla-> comparando Nversion DBXML: " + nversionDBXML + " con Nversion Respaldo: " + nversionRespaldo);
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
                if (debug)
                    Console.WriteLine("revisionTabla-> comparando largo PK: " + largoPK + " con cant Nodos DBXML: " + cantNodos);
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
        public void debugOnOff()
        {
            debug = !debug;
        }
        public bool debugStatus()
        {
            return debug;
        }
    }
}
