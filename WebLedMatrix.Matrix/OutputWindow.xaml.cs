using System;
using System.Dynamic;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using WebLedMatrix.Matrix.ViewModel;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;

namespace WebLedMatrix.Matrix
{
    /// <summary>
    /// Description for OutputWindow.
    /// </summary>
    public partial class OutputWindow : Window
    {
       
        private static OutputWindow _outputWindow;
        public static Window Create()
        {
            if (_outputWindow == null)
            {
                _outputWindow = new OutputWindow();
            }
            return _outputWindow;
        }

        /// <summary>
        /// Initializes a new instance of the OutputWindow class.
        /// </summary>
        public OutputWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
            ((ViewModelBase)this.DataContext).PropertyChanged += OutputWindow_PropertyChanged;
            var appName = System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";

            var Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            Key.SetValue(appName, 11001, RegistryValueKind.DWord);
            Key.Close();
        }

        private void OutputWindow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Image"))
            {

            }
            if (e.PropertyName.Equals("WebAddress"))
            {
                var address = ((ShowerViewModel)this.DataContext).WebAddress;
                var isImage = address.AbsoluteUri.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase);
                if (isImage)
                {
                    Image.Source = new BitmapImage(address);
                    Image.Visibility = Visibility.Visible;
                    Text.Visibility = Visibility.Collapsed;
                    Browser.Visibility = Visibility.Collapsed;
                }
                else
                {
                    Browser.Source = address;
                    Image.Visibility = Visibility.Collapsed;
                    Text.Visibility = Visibility.Collapsed;
                    Browser.Visibility = Visibility.Visible;
                }
            }
            if (e.PropertyName.Equals("Text"))
            {
                Image.Visibility = Visibility.Collapsed;
                Browser.Visibility = Visibility.Collapsed;
                Text.Visibility = Visibility.Visible;
            }
        }
    }
}