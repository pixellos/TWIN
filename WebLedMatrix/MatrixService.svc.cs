using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.SignalR;
using WebLedMatrix.Hubs;

namespace WebLedMatrix
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MatrixService" in code, svc and config file together.
    // NOTE: In order to launch WCF MatrixService Client for testing this service, please select MatrixService.svc or MatrixService.svc.cs at the Solution Explorer and start debugging.
    public class MatrixService : IMatrixService
    {
        public void RegisterMatrix(string name)
        {
            GlobalHost.ConnectionManager.GetHubContext<UiManagerHub>(typeof(UiManagerHub).Name).Clients.All.Clients.All.updateMatrixes(name);
        }
    }
}
