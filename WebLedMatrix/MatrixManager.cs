using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNet.SignalR;
using StorageTypes;
using StorageTypes.MatrixServiceCallback;
using WebLedMatrix.Hubs;

namespace WebLedMatrix
{
    public class MatrixManager
    {
        private List<Matrix> matrices = new List<Matrix>();
        public List<Matrix> Matrices => new List<Matrix>(matrices);

        private IHubContext<IUiManagerHub> Context
            => GlobalHost.ConnectionManager.GetHubContext<IUiManagerHub>(typeof(UiManagerHub).Name);

        public Matrix AddMatrix(string name, IMatrixServiceCallback callbackAction )
        {
            var matrix = new Matrix() {Name = name, Callback = callbackAction};
            if (matrices.Exists(x=>x.Name == name))
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
            matrices.RemoveAll(x => x.Name == name);
            UpdateMatrices();
        }

        public void UpdateMatrices()
        {
            Context.Clients.All.unRegisterAllMatrices();
            Context.Clients.All.updateMatrices(matrices.ToArray());     
        }


        public void SendCommandToMatirx(string name, DisplayDataType displayDataType, string data)
        {
            switch (displayDataType)
            {
                    case DisplayDataType.Text:
                    matrices.Single(x => x.Name.Equals(name)).Callback.UpdateText(data);
                    break;

                    case DisplayDataType.WebPage:
                    matrices.Single(x=>x.Name.Equals(name)).Callback.UpdateWebPage(data);
                    break;
            }
        }
    }
}