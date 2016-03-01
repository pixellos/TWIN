using System;
using System.Windows;
using NLog;
using WebLedMatrix.Client.ViewModel;

namespace WebLedMatrix.Client
{
    public partial class MainWindow : Window
    {
        enum DisplayedTypes
        {
            Text, Image, Picture, WebPage
        }
            private Logger _logger = LogManager.GetCurrentClassLogger();

            const string TooLowValue = "Value is too low";
            const string TooBigValueMessage = "{0} value is too big";

            const int MarginFromRightEdge = 100;
            const int MarginFromDownEdge = 250;

            private int _selectedXResolution
            {
                get { return parseWithExceptionHandling(resolutionXTextBox.Text); }
            }
            private int _selectedYResolution
            {
                get { return parseWithExceptionHandling(resolutionYTextBox.Text); }
            }

            public MainWindow()
            {
                InitializeComponent();
                Closing += (s, e) => ViewModelLocator.Cleanup();
            }

            int parseWithExceptionHandling(string valueToParse)
            {
                try
                {
                    return int.Parse(valueToParse);
                }
                catch (Exception exception) when (exception is FormatException || exception is ArgumentNullException)
                {
                    MessageBox.Show("Typed format is unsupported - type valid number");
                    //_logger.Trace(exception.Message);
                }
                return 0;
            }

            void TextDisplay(string text)
            {
                textInGrid.Visibility = Visibility.Visible;
            }

            void ImageDisplay()
            {
                imageInGrid.Visibility = Visibility.Visible;
            }

            private void changeButton_Click(object sender, RoutedEventArgs e)
            {
                if (SystemParameters.PrimaryScreenWidth < _selectedXResolution)
                {
                    MessageBox.Show(string.Format(TooBigValueMessage, "Width"));
                }
                else if (SystemParameters.PrimaryScreenHeight < _selectedYResolution)
                {
                    MessageBox.Show(string.Format(TooBigValueMessage, "Height"));
                }
                else if (_selectedXResolution <= MarginFromRightEdge || _selectedYResolution <= MarginFromDownEdge)
                {
                    MessageBox.Show(TooLowValue);
                }
                else
                {
                    ChangeResolution(_selectedXResolution, _selectedYResolution);
                }
            }

            private void ChangeResolution(int x, int y)
            {
                this.Width = x + MarginFromRightEdge;
                this.Height = y + MarginFromDownEdge;
                textInGrid.Width = viewbox.Width = commonGrid.Width = x;
                textInGrid.Height = viewbox.Height = commonGrid.Height = y;
            }
        }
    }