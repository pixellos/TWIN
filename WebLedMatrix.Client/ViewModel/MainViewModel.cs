using System.Windows;
using GalaSoft.MvvmLight;
using WebLedMatrix.Client.Model;

namespace WebLedMatrix.Client.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;

        /// <summary>
        /// The <see cref="WelcomeTitle" /> property's name.
        /// </summary>
        public const string WelcomeTitlePropertyName = "WelcomeTitle";

        private string _welcomeTitle = string.Empty;

        /// <summary>
        /// Gets the WelcomeTitle property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
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


        void HideAll()
        {
            TextVisible = Visibility.Hidden;
            ImageVisibility = Visibility.Hidden;
        }

        private System.Windows.Visibility _textVisible = Visibility.Hidden;
        public System.Windows.Visibility TextVisible
        {
            get { return _textVisible; }
            set { Set(ref _textVisible, value); }
        }

        private string _displayedText = "";
        public string DisplayedText
        {
            get { return _displayedText; }
            set
            {
                HideAll();
                TextVisible = Visibility.Visible;
                Set(ref _displayedText, value);
            }
        }

        private System.Windows.Visibility _imageVisibility = Visibility.Hidden;
        public System.Windows.Visibility ImageVisibility
        {
            get { return _imageVisibility; }
            set { Set(ref _imageVisibility, value); }
        }

        private string _displayedPicture = "http://pre03.deviantart.net/b285/th/pre/f/2013/257/6/8/grumpy_cat__nope_by_imwithstoopid13-d624kvl.png";
        public string DisplayedPicture
        {
            get { return _displayedPicture; }
            set
            {
                HideAll();
                ImageVisibility = Visibility.Visible;
                Set(ref _displayedPicture, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }
                    DisplayedText = "Hello!\n" +
                                    "TWIN\n" +
                                    "Application\n" +
                                    "Display\n" +
                                    "Client\n" +
                                    "\n" +
                                    "Have fun!\n";
                    WelcomeTitle = item.Title;


                });
        }

    }
}