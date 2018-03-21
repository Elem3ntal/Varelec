using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "InoASMX" in both code and config file together.
[ServiceContract]
public interface InoASMX
{
	[OperationContract]
	void DoWork();
    [OperationContract]
    bool escribirMemo(string usuario, string entrada);
    [OperationContract]
    XmlDocument obtenerMemosGenrales();
}
