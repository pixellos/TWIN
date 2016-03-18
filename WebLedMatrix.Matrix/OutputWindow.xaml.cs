using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using WebLedMatrix.Matrix.ViewModel;

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
        }

        private void OutputWindow_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("WebAddress"))
            {
                Browser.Source = ((ShowerViewModel) this.DataContext).WebAddress;
                Text.Visibility = Visibility.Collapsed;
                Browser.Visibility = Visibility.Visible;
            }
            if (e.PropertyName.Equals("Text"))
            {
                Browser.Visibility = Visibility.Collapsed;
                Text.Visibility = Visibility.Visible;
            }
        }
    }
}