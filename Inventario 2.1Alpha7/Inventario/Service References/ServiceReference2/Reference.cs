﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inventario.ServiceReference2 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference2.IVarelecService")]
    public interface IVarelecService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/status", ReplyAction="http://tempuri.org/IVarelecService/statusResponse")]
        bool status();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/login", ReplyAction="http://tempuri.org/IVarelecService/loginResponse")]
        int login(string MD5);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/editarUsuario", ReplyAction="http://tempuri.org/IVarelecService/editarUsuarioResponse")]
        bool editarUsuario(string[] usuario, string oldPass);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/crearUsuario", ReplyAction="http://tempuri.org/IVarelecService/crearUsuarioResponse")]
        bool crearUsuario(string[] usuario);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/eliminarUsuario", ReplyAction="http://tempuri.org/IVarelecService/eliminarUsuarioResponse")]
        bool eliminarUsuario(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/crearProducto", ReplyAction="http://tempuri.org/IVarelecService/crearProductoResponse")]
        bool crearProducto(string[] producto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/editarProducto", ReplyAction="http://tempuri.org/IVarelecService/editarProductoResponse")]
        bool editarProducto(string[] producto);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/eliminarProducto", ReplyAction="http://tempuri.org/IVarelecService/eliminarProductoResponse")]
        bool eliminarProducto(string id);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/crearCliente", ReplyAction="http://tempuri.org/IVarelecService/crearClienteResponse")]
        bool crearCliente(string[] data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/editarCliente", ReplyAction="http://tempuri.org/IVarelecService/editarClienteResponse")]
        bool editarCliente(string[] data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/eliminarCliente", ReplyAction="http://tempuri.org/IVarelecService/eliminarClienteResponse")]
        bool eliminarCliente(string whereID);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/registrarTransaccion", ReplyAction="http://tempuri.org/IVarelecService/registrarTransaccionResponse")]
        bool registrarTransaccion(string[] datos);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/getInventarioXML", ReplyAction="http://tempuri.org/IVarelecService/getInventarioXMLResponse")]
        System.Xml.Linq.XElement getInventarioXML();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/getClientesXML", ReplyAction="http://tempuri.org/IVarelecService/getClientesXMLResponse")]
        System.Xml.Linq.XElement getClientesXML();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/getUsersXML", ReplyAction="http://tempuri.org/IVarelecService/getUsersXMLResponse")]
        System.Xml.Linq.XElement getUsersXML();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IVarelecService/getFacturasPorClienteXML", ReplyAction="http://tempuri.org/IVarelecService/getFacturasPorClienteXMLResponse")]
        System.Xml.Linq.XElement getFacturasPorClienteXML(string rutCliente);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IVarelecServiceChannel : Inventario.ServiceReference2.IVarelecService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class VarelecServiceClient : System.ServiceModel.ClientBase<Inventario.ServiceReference2.IVarelecService>, Inventario.ServiceReference2.IVarelecService {
        
        public VarelecServiceClient() {
        }
        
        public VarelecServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public VarelecServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VarelecServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public VarelecServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public bool status() {
            return base.Channel.status();
        }
        
        public int login(string MD5) {
            return base.Channel.login(MD5);
        }
        
        public bool editarUsuario(string[] usuario, string oldPass) {
            return base.Channel.editarUsuario(usuario, oldPass);
        }
        
        public bool crearUsuario(string[] usuario) {
            return base.Channel.crearUsuario(usuario);
        }
        
        public bool eliminarUsuario(string id) {
            return base.Channel.eliminarUsuario(id);
        }
        
        public bool crearProducto(string[] producto) {
            return base.Channel.crearProducto(producto);
        }
        
        public bool editarProducto(string[] producto) {
            return base.Channel.editarProducto(producto);
        }
        
        public bool eliminarProducto(string id) {
            return base.Channel.eliminarProducto(id);
        }
        
        public bool crearCliente(string[] data) {
            return base.Channel.crearCliente(data);
        }
        
        public bool editarCliente(string[] data) {
            return base.Channel.editarCliente(data);
        }
        
        public bool eliminarCliente(string whereID) {
            return base.Channel.eliminarCliente(whereID);
        }
        
        public bool registrarTransaccion(string[] datos) {
            return base.Channel.registrarTransaccion(datos);
        }
        
        public System.Xml.Linq.XElement getInventarioXML() {
            return base.Channel.getInventarioXML();
        }
        
        public System.Xml.Linq.XElement getClientesXML() {
            return base.Channel.getClientesXML();
        }
        
        public System.Xml.Linq.XElement getUsersXML() {
            return base.Channel.getUsersXML();
        }
        
        public System.Xml.Linq.XElement getFacturasPorClienteXML(string rutCliente) {
            return base.Channel.getFacturasPorClienteXML(rutCliente);
        }
    }
}
