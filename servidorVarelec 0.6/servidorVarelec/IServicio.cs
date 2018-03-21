using System;
using System.Xml;
using System.Data;
using System.Text;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.ServiceModel;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ServiceModel.Description;
using System.Xml.Linq;

namespace servidorVarelec
{
    [ServiceContract]
    public interface IServicio
    {
        [OperationContract]
        bool online();
        //---------------Contratos de Usuarios
        [OperationContract]
        int inventarioAgregar(string[] fila);
        [OperationContract]
        int inventarioModificar(string[] fila);
        [OperationContract]
        int inventarioEliminar(string fila);
        [OperationContract]
        string inventarioNVersion();
        [OperationContract]
        XmlElement inventarioObtener();
        [OperationContract]
        XmlElement inventarioErrores();
        [OperationContract]
        string inventarioErrorUltimo();
        //---------------Contratos de Clientes
        [OperationContract]
        int clientesAgregar(string[] fila);
        [OperationContract]
        int clientesModificar(string[] fila);
        [OperationContract]
        int clientesEliminar(string fila);
        [OperationContract]
        string clientesNVersion();
        [OperationContract]
        XmlElement clientesObtener();
        [OperationContract]
        XmlElement clientesErrores();
        [OperationContract]
        string clientesErrorUltimo();
        //---------------Contratos de Usuarios
        [OperationContract]
        int usuariosAgregar(string[] fila);
        [OperationContract]
        int usuariosModificar(string[] fila);
        [OperationContract]
        int usuariosEliminar(string fila);
        [OperationContract]
        string usuariosNVersion();
        [OperationContract]
        XmlElement usuariosObtener();
        [OperationContract]
        XmlElement usuariosErrores();
        [OperationContract]
        string usuariosErrorUltimo();
        //---------------Contratos de Memos
        [OperationContract]
        bool escribirMemo(string usuario, string entrada);
        [OperationContract]
        XmlElement obtenerMemosGenrales();
        [OperationContract]
        int guardarCotizacion(XElement cotizacion);
        [OperationContract]
        XmlElement listaCotizaciones();
        [OperationContract]
        XmlElement obtenerFactura(int numero);
        [OperationContract]
        XmlElement indicadoresEconomicos();
        [OperationContract]
        bool setearIndicadoresEconomicos(string dolar, string euro, string importe);
        [OperationContract]
        bool ingresarFacturaLiberada(XElement factura);
        [OperationContract]
        string ingresarFacturaPorCotizacion(int identificacion);
        [OperationContract]
        XmlElement facturasResumen(int año, int mes);
        [OperationContract]
        string consultarActualizacion();
        [OperationContract]
        bool subirActualizacionAServidor();
        [OperationContract]
        bool IngresarFacturaIngreso(XElement factura);
        [OperationContract]
        XmlElement facturasIngresoResumen(int año, int mes);
        [OperationContract]
        bool ingresoPMP(string NP, int NoE, int EoS, int cantidad, int precioFTR, int precioEXP, string moneda);
        [OperationContract]
        XmlElement obtenerPMP();
        [OperationContract]
        bool uploadFile(string name);
        [OperationContract]
        bool registrarInicio(string usuario, string dateComplete);
        [OperationContract]
        XmlElement obtenerHistorialInicios();
    }
}
