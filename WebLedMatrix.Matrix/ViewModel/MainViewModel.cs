using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Autofac;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using WebLedMatrix.Matrix.Model;

namespace WebLedMatrix.Matrix.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
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
            });

            Task x = new Task(() =>
            {
                while (true)
                {
                    Task.Delay(1000);
                    WelcomeTitle = DateTime.Now.ToString();
                }
            });
            x.Start();
        }

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        public string WelcomeTitle
        {
            get
            {
                return _welcomeTitle;
            }
            set
            {
                Set(ref _welcomeTitle, value);
            }
        }
    }
}