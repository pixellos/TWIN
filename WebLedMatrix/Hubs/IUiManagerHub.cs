using WebLedMatrix.Models;

namespace WebLedMatrix.Hubs
{
    public interface IUiManagerHub
    {
        void unRegisterAllMatrices();
        void loginStatus(string userText);
        void updateMatrices(Matrix[] matrices);
        void showSections(bool matrixesSection,bool sendingSection,bool administrationSection);
        void userIsActiveStatus(bool isActive);
    }
}