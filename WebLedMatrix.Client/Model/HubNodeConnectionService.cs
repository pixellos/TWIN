using System;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using StorageTypes;
using StorageTypes.AppInterface.ClientHub;
using StorageTypes.HubWrappers;
using WebLedMatrix.Hubs;

namespace WebLedMatrix.ClientNode.Model
{
    public interface IDisplayCallbacks
    {
        Action<string> DisplayTextAction { get; }
        Action<string> DisplayImageAction { get; }
    }

    public class HubNodeConnectionService : NodeConnectionInterface
    {
        private string Name = "DefaultNode";
        private HubWrapper<ClientNodeHubBase> _hubWrapper;
        private ClientNodeHubBase _typingClientNodeHubBase;

        /// <summary>
        /// For testing purposes - I couldnt make nsubstitute call ctor(null);
        /// </summary>
        public HubNodeConnectionService()
        {
            _typingClientNodeHubBase = new ClientNodeHubBase(null);
            _hubWrapper = new HubWrapper<ClientNodeHubBase>(new ClientNodeHubBase(null),null);
            ServicePointManager.DefaultConnectionLimit = 10; // For WPF App
        }

        private void ConfigureHubMethods()
        {
            _hubWrapper.RegisterAndInvoke<NodeConnectionInterface,HubNodeConnectionService>(this);   
        }

        public async Task Start(string Url = null)
        {
            try
            {
                await _hubWrapper.Start(Url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        public virtual void sendData(DataToDisplay data, object[] args = null)
        {
            
        }

        public virtual async Task Hello()
        {
            Action<string> @delegate = _typingClientNodeHubBase.RegisterMatrix;
            await _hubWrapper.InvokeAtServer(@delegate, new object[]{Name});
        }
    }
}
