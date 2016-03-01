using Microsoft.AspNet.SignalR;
using StorageTypes;
using StorageTypes.AppInterface.ClientHub;

namespace WebLedMatrix.Hubs
{
    public class ClientNodeHubBase : Hub<NodeConnectionInterface>
    {
        public async void ClientIsReady()
        {
            //RegisterAndInvoke call - to UI information "Hi, i'm there and i can receive commands"
        }

        //From ui
        public async void DisplayData(object type, object data)
        {
            DisplayDataType displayDataType;
            DisplayDataType.TryParse((string)type, out displayDataType);
            Clients.Caller.sendData(new DataToDisplay
            {
                DisplayDataType = displayDataType,
                Data = data.ToString()
            });
        }
    }
}