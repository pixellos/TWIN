using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using StorageTypes.AppInterface.ClientHub;

namespace StorageTypes.HubWrappers
{
    public enum Avability
    {
        ForAll,
        AboveSpeaker,
        AboveUsers,
        Disabled
    }

    public class Matrix
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public Avability Avability { get; set; }
        public DataToDisplay DataToDisplay { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is Matrix)
            {
                return this.Name == (obj as Matrix).Name;
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }

    public interface IMatrixManager
    {
        ICollection<Matrix> Matrices { get; }
        void RegisterMatrix(Matrix matrix);
        void UnRegisterMatrix(string name);
        void SendData(Matrix matrix, DataToDisplay data);
    }

    public class MatrixManager : IMatrixManager
    {
        public ICollection<Matrix> Matrices { get { return _matrices.AsReadOnly(); } }
        private List<Matrix> _matrices;
        public IHubContext Context { get; set; }
        public MatrixManager()
        {
            _matrices = new List<Matrix>();
        }

        public void RegisterMatrix(Matrix matrix)
        {
            _matrices.RemoveAll(x => x.Name == matrix.Name);
            _matrices.Add(matrix);
            Context.Clients.All.updateMatrixes(matrix.Name);
        }

        public void UnRegisterMatrix(string name)
        {
            _matrices.RemoveAll(x => x.Name == name);
        }

        public void SendData(Matrix matrix, DataToDisplay data)
        {
            Context?.Clients.Client(matrix.ConnectionId).sendData(data);
        }
    }

    public class ClientNodeHubBase : Hub<NodeConnectionInterface>
    {
        private readonly IMatrixManager _matrixManager;
        

        public ClientNodeHubBase(IMatrixManager matrixManager)
        {
            _matrixManager = matrixManager;
        }

        //From ui
        public void ForceRefreshData(object type, object data)
        {
            DisplayDataType displayDataType;
            DisplayDataType.TryParse((string)type, out displayDataType);
            Clients.Caller.sendData(new DataToDisplay
            {
                DisplayDataType = displayDataType,
                Data = data.ToString()
            });
        }

        public void RegisterMatrix(string name)
        {
            Matrix matrix = new Matrix()
            {
                Avability = Avability.ForAll,
                ConnectionId = this.Context.ConnectionId,
                DataToDisplay = null,
                Name =  name
            };
            _matrixManager.RegisterMatrix(matrix);
        }

        public void UnregisterMatrix(string name)
        {
            _matrixManager.UnRegisterMatrix(name);
        }
    }
}