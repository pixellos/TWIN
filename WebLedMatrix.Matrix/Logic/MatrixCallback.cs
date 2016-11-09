using System;
using Microsoft.Practices.ServiceLocation;
using WebLedMatrix.Matrix.ViewModel;

namespace WebLedMatrix.Matrix.Logic
{
    public class MatrixCallback
    {
        private ShowerViewModel viewModelInstance => ServiceLocator.Current.GetInstance<ShowerViewModel>();

        private ITextFormater textFormater = new BasicUnEscaper();

        public void UpdateWebPage(string text)
        {
            Uri Uri = null;
            try
            {
               Uri = new UriBuilder(text).Uri;
               viewModelInstance.WebAddress = Uri;
            }
            catch (UriFormatException)
            {
                viewModelInstance.Text = "Cant display webpage \n " + text;
            }
        }

        public void UpdateText(string text)
        {
            viewModelInstance.Text = textFormater.Format(text);
        }
    }
}