using System.Windows;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WebLedMatrix.ClientNode.Model;

namespace WebLedMatrix.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ConnectionViewModel : ViewModelBase
    {
        private HubNodeConnectionService connectionService;
        /// <summary>
        /// Initializes a new instance of the ConnectionViewModel class.
        /// </summary>
        /// H
        public ConnectionViewModel(HubNodeConnectionService connectionService)
        {
            this.connectionService = connectionService;
            ConnectCommand = new RelayCommand(async () =>
            {
                int x = 5;
                await connectionService.Start();
                
                await connectionService.Hello();
            });
        }

        private string _url;
        public string Url
        {
            get { return _url; }
            set { Set(() => Url,ref _url, value); }
        }

        public RelayCommand ConnectCommand { get; private set; }
    }
}