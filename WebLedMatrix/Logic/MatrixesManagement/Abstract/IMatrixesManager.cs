using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using WebLedMatrix.Logic.MatrixesManagement.DataBase.Model;

namespace WebLedMatrix.Logic.MatrixesManagement.Abstract
{

    public interface IMatrixesManager
    {
        void Create(MatrixUnit unit);
        bool TryLogin(string name);
        void Delete(MatrixUnit unit);
        MatrixUnit FindByName(string name);
        List<MatrixUnit> UpdateStatus();
    }                                      
}