using System.Threading.Tasks;

namespace WebLedMatrix.ScreenControler.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}