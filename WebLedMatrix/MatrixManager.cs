using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using WebLedMatrix.Hubs;
using WebLedMatrix.Models;
using WebLedMatrix.Logic;
using System;
using System.Collections;

namespace WebLedMatrix
{
    public class Clients : IEnumerable<Client>
    {
        private HashSet<Client> Collection = new HashSet<Client>();

        private IHubContext<IUiManagerHub> Context { get; set; }

        public Clients(IHubContext<IUiManagerHub> hubContext)
        {
            this.Context = hubContext;
        }

        public Clients()
        {
            this.Context = GlobalHost.ConnectionManager.GetHubContext<IUiManagerHub>(typeof(UiManagerHub).Name);
        }

        public Client Matrix(string name)
        {
            var matrix = new Client() {Name = name};
            if (Collection.Any(x=>x.Name == name))
            {
                matrix = Collection.Single(x => x.Name == name);
            }
            else
            {
                this.Collection.Add(matrix);
            }
            UpdateMatrices();
            return matrix;
        }

        public void RemoveMatrix(string name)
        {
            this.Collection.RemoveWhere(x => x.Name == name);
            this.UpdateMatrices();
        }

        public void SendToAll(string username, string text)
        {
            foreach (var matrix in this.Collection)
            {
                matrix.AppendData(username,text);
            }
        }

        public void AppendData(string user, string clientName, string text)
        {
            var searchedMatrice = Collection.SingleOrDefault(x => x.Name.Equals(clientName));
            if (searchedMatrice == null)
            {
                throw new Exception($"Matrice named {clientName} does not exist at our cache. Please consider refreshing browser");
            }
            searchedMatrice.AppendData(user, text);
        }

        public void UpdateMatrices()
        {
            Context.Clients.All.unRegisterAllMatrices();
            Context.Clients.All.updateMatrices(Collection.ToArray());
        }

        public IEnumerator<Client> GetEnumerator()
        {
            return this.Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }
    }
}