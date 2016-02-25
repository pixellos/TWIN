using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLedMatrix.Logic.MatrixsManagement.Model;

namespace WebLedMatrix.Logic.MatrixsManagement.Infrastructure
{
    class MatrixUnitDbContext : DbContext
    {
        public MatrixUnitDbContext() : base("Slave/MatrixesDb") {}

        public virtual DbSet<MatrixUnit> MatrixUnits { get; set; }
    }
}
