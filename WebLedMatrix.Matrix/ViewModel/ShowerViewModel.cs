using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using WebLedMatrix.Matrix.Logic;

namespace WebLedMatrix.Matrix.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ShowerViewModel : ViewModelBase
    {
        private readonly ServiceWrapper _serviceWrapper;

        public ShowerViewModel()
        {
            _serviceWrapper = IoCContainter.Resolve<ServiceWrapper>();

            Messenger.Default.Register<NotificationMessage>(this, (notification) =>
            {
                if (notification.Notification.Equals(MessageStrings.Show))
                {
                    var window = OutputWindow.Create();

                    try
                    {
                        window.Show();
                    }
                    catch (Exception)
                    {
                        window = new OutputWindow();
                    }
                }
                if (notification.Notification.Equals(MessageStrings.Hide))
                {

                }
            });
        }

        const string DefauleWebPage = "http://Zsp5.krosno.pl";
        private Uri _webAddress = new Uri(DefauleWebPage);

        public Uri WebAddress
        {
            get { return _webAddress; }
            set { Set(ref _webAddress, value); }
        }

        private string _text = "TWIN";

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }
    }
}