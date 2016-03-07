using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using NSubstitute;
using NSubstitute.Routing.Handlers;
using StorageTypes.AppInterface.ClientHub;
using StorageTypes.HubWrappers;
using WebLedMatrix.Hubs;
using Xunit;

namespace StorageTypesTests.HubWrappers
{
    public class ConnectionHubWrapperTests
    {
        public class ClientNodeHub : ClientNodeHubBase
        {
            public ClientNodeHub(IMatrixManager matrixManager) : base(matrixManager)
            {
            }
        }

        private HubWrapper<ClientNodeHubBase> baseConnectionHubWrapper =
            new HubWrapper<ClientNodeHubBase>(new ClientNodeHub(new MatrixManager()),null); 
        public void ConnectionHubWrapperTest()
        {
        }

       
    /*    [Fact()]
        public  void GetConnectionTest()
        {
            Assert.True(false,"This must be checked by integration test - i cannot get access to signalr internals.");
            /*
            var hubConnection = Substitute.For<HubConnection>("");

            object[] argsObjects = {""};
            HubProxy proxy = Substitute.For<HubProxy>(hubConnection, "1");
            HubWrapper<ClientNodeHubBase> baseWrapper =
                new HubWrapper<ClientNodeHubBase>(new ClientNodeHub(),hubConnection,proxy);
            
            Action hardTypedFromHubFunction = (new ClientNodeHub()).Hello;

             baseWrapper.InvokeAtServer<object>(hardTypedFromHubFunction,argsObjects);
            
        }*/

        [Fact()]
        public void GetHubNameTest()
        {
            Assert.Equal(baseConnectionHubWrapper.GetHubName(),"ClientNodeHub");
        }

        [Fact()]
        public void InvokeMethodTest()
        {
            var hub = new ClientNodeHub(new MatrixManager());
            Action<object, object> method = hub.ForceRefreshData;
           Assert.Equal(baseConnectionHubWrapper.GetMethodName(method),"ForceRefreshData"); 
        }

        [Fact()]
        public void GetMethodNameMethodTestNOTEQUAL()
        {
            var x = "";
            Func<string, bool> predicate = x.Contains;
            try
            {
                baseConnectionHubWrapper.GetMethodName(predicate);
            }
            catch (MethodDoesntBelongToDesiredTypeException e)
            {
                Assert.True(true, e.Message);
                return;
            }
            Assert.True(false);
        }
    }
}