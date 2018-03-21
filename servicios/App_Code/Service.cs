using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SqlClient;

[WebService(Namespace = "http://localhost/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
// [System.Web.Script.Services.ScriptService]

public class Service :   System.Web.Services.WebService
{
    public Service () {

        //Uncomment the following line if using designed components
        //InitializeComponent();
    }

    [WebMethod]
    public int saturar()
    {
        int i = 0;
        DateTime x1 = DateTime.Now;
        DateTime x2 = DateTime.Now;
        while (x1.Minute.Equals(x2.Minute))
        {
            x2 = DateTime.Now;
            ++i;
        }
        return i;
    }
    [WebMethod]
    public int estado()
    {
        return 1;
    }
    [WebMethod]
    public string holaMundo(string hola)
    {
        return "hal9600 dice:"+hola;
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
    public bool escribirMemo(string usuario, string entrada)
    {
        try
        {
            XmlDocument actual = new XmlDocument();
            actual.Load("C:/wwwroot/servicios/MemosGenerales.xml");
            XmlNode nodo = actual.DocumentElement;
            XmlNode memo = actual.CreateElement(""+usuario);
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
}
