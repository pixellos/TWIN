using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace WebLedMatrix.Matrix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            IoCContainter.Build();
            DispatcherHelper.Initialize();
        }
    }
}
