using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebLedMatrix.Hubs
{
    public interface IMatrixManager {}

    public class MatrixManager : IMatrixManager
    {
       //2 parts - Reciving for request, and invoking request
    }

    public class ManagerHub : Hub
    {
        private IMatrixManager _matrixManager;

        public ManagerHub(IMatrixManager matrixManager)
        {
            _matrixManager = matrixManager;
        }

        public void Hello()
        {
            Clients.All.hello();
        }

        public void SubscribeConnectionWithClients()
        {
            
        }
    }
}