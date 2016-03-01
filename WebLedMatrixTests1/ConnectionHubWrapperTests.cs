using Xunit;
using WebLedMatrix.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
/*
namespace WebLedMatrix.Hubs.Tests
{
    public class ConnectionHubWrapperTests
    {
        public class ClientNodeHub : Hub
        {
            public async void RequestForData()
            {
                //Register call - to UI information "Hi, i'm there and i can receive commands"
            }

            //From ui
            public async void DisplayData(object type, object data)
            {
                DisplayDataType displayDataType;
                DisplayDataType.TryParse((string) type, out displayDataType);
                Clients.Caller.sendData(new DataToDisplay
                {
                    DisplayDataType = displayDataType,
                    Data = data.ToString()
                });
            }


            public void Hello()
            {
                Clients.All.hello();
            }
        }

         [Fact]
            public void GetHubNameTest()
            {
                Assert.Equal("ClientNodeHub", new ConnectionHubWrapper<ClientNodeHub>(new ClientNodeHub()).GetHubName());
            }
        
    }
}*/