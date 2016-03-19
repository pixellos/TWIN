using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using WebLedMatrix.Matrix.Logic;
using WebLedMatrix.Matrix.Model;

namespace WebLedMatrix.Matrix.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ServiceWrapper _serviceWrapper;

        private RelayCommand _SetName;
        public RelayCommand SetName
        {
            get	{		return _SetName;	}
            set	{		Set(ref _SetName, value);	}
        }

        private String _NodeName;
        public String NodeName
        {
            get	{		return _NodeName;	}
            set	{		Set(ref _NodeName, value);	}
        }

        public MainViewModel(IDataService dataService)
        {
            _serviceWrapper = IoCContainter.Resolve<ServiceWrapper>();
            SetName = new RelayCommand(() =>
            {
                _serviceWrapper.SetName(NodeName);
                Messenger.Default.Send(new NotificationMessage(MessageStrings.Show));
            });
        }
    }
}