using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Microsoft.AspNet.SignalR;
using WebLedMatrix.Hubs;
using WebLedMatrix.Models;
using WebLedMatrix.WebService;

namespace WebLedMatrix
{
    public class MatrixManager
    {
        private HashSet<Matrix> matrices = new HashSet<Matrix>();
        public List<Matrix> Matrices => new List<Matrix>(matrices);

        IMatrixServiceCallback matrixCallback(string name) => matrices.Single(x => x.Name.Equals(name)).Callback;

        private IHubContext<IUiManagerHub> Context { get; set; }

        public MatrixManager(IHubContext<IUiManagerHub> hubContext)
        {
            Context = hubContext;
        }

        public MatrixManager()
        {
            Context = GlobalHost.ConnectionManager.GetHubContext<IUiManagerHub>(typeof(UiManagerHub).Name);
        }

        public Matrix AddMatrix(string name, IMatrixServiceCallback callbackAction)
        {
            
            var matrix = new Matrix() {Name = name, Callback = callbackAction};
            if (matrices.Any(x=>x.Name == name))
            {
                matrix = matrices.Single(x => x.Name == name);
            }
            else
            {
                matrices.Add(matrix);
            }

            UpdateMatrices();
            return matrix;
        }

        public void RemoveMatrix(string name)
        {
            matrices.RemoveWhere(x => x.Name == name);
            UpdateMatrices();
        }

        public void UpdateMatrices()
        {
            Context.Clients.All.unRegisterAllMatrices();
            Context.Clients.All.updateMatrices(matrices.ToArray());     
        }

        public void SendWebPage(string name, string data)
        {
            
            matrixCallback(name).UpdateWebPage(data);
        }

        public void SendToAll(string text)
        {
            foreach (var matrix in this.matrices)
            {
                matrix.Callback.UpdateText(text);
            }
        }

        public void SendText(string name, string text)
        {
            matrixCallback(name).UpdateText(text);
        }
    }
}