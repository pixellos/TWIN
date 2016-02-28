using System.Data.Entity;
using WebLedMatrix.Logic.MatrixesManagement.DataBase.Model;

namespace WebLedMatrix.Logic.MatrixesManagement.DataBase.Infrastructure
{
    class MatrixUnitDbContext : DbContext
    {
        public MatrixUnitDbContext() : base("Slave/MatrixesDb") {}

        public virtual DbSet<MatrixUnit> MatrixUnits { get; set; }
    }
}
