using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "noASMX" in code, svc and config file together.
public class noASMX : InoASMX
{
	public void DoWork()
	{
	}
    public bool escribirMemo(string usuario, string entrada)
    {
        try
        {
            XmlDocument actual = new XmlDocument();
            actual.Load("C:/wwwroot/service/MemosGenerales.xml");
            XmlNode nodo = actual.DocumentElement;
            XmlNode memo = actual.CreateElement("" + usuario);
            memo.InnerText = entrada;
            nodo.AppendChild(memo);
            actual.Save("C:/wwwroot/service/MemosGenerales.xml");
        }
        catch
        {
            XElement actual = new XElement("Memos");
            actual.Add(new XElement(usuario, entrada));
            actual.Save("C:/wwwroot/service/MemosGenerales.xml");
        }
        return true;
    }
    public XmlDocument obtenerMemosGenrales()
    {
        XmlDocument actual = new XmlDocument();
        try
        {
            actual.Load("C:/wwwroot/service/memosGenerales.xml");
        }
        catch
        {
            XElement nuevo = new XElement("memos");
            nuevo.Save("C:/wwwroot/service/MemosGenerales.xml");
            actual.Load("C:/wwwroot/service/MemosGenerales.xml");
        }
        return actual;
    }
}
