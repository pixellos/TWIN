﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebLedMatrix.Matrix.Service_References.MatrixService {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MatrixService.IMatrixService", CallbackContract=typeof(IMatrixServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IMatrixService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatrixService/RegisterMatrix")]
        void RegisterMatrix(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatrixService/RegisterMatrix")]
        System.Threading.Tasks.Task RegisterMatrixAsync(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatrixService/UnRegisterMatrix")]
        void UnRegisterMatrix(string name);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatrixService/UnRegisterMatrix")]
        System.Threading.Tasks.Task UnRegisterMatrixAsync(string name);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMatrixServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatrixService/UpdateWebPage")]
        void UpdateWebPage(string text);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IMatrixService/UpdateText")]
        void UpdateText(string text);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMatrixServiceChannel : IMatrixService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MatrixServiceClient : System.ServiceModel.DuplexClientBase<IMatrixService>, IMatrixService {
        
        public MatrixServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public MatrixServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public MatrixServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public MatrixServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public MatrixServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void RegisterMatrix(string name) {
            base.Channel.RegisterMatrix(name);
        }
        
        public System.Threading.Tasks.Task RegisterMatrixAsync(string name) {
            return base.Channel.RegisterMatrixAsync(name);
        }
        
        public void UnRegisterMatrix(string name) {
            base.Channel.UnRegisterMatrix(name);
        }
        
        public System.Threading.Tasks.Task UnRegisterMatrixAsync(string name) {
            return base.Channel.UnRegisterMatrixAsync(name);
        }
    }
}
