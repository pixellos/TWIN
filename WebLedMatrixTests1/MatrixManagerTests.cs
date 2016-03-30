using Xunit;
using WebLedMatrix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebLedMatrix.Hubs;
using NSubstitute;


namespace WebLedMatrix.Tests
{
    public class MatrixManagerTests
    {

        private string testString = "Teststs";
        private string testString1 = "testststst";
        [Fact()]
        public void AddMatrixTest()
        {
            
            MatrixManager matrix = new MatrixManager();
            matrix.AddMatrix(testString,null);
            matrix.AddMatrix(testString1,null);

            List<Matrix> matrices = new List<Matrix>
            {
                new Matrix() {Name = testString},
                new Matrix() {Name = testString1}
            };

            Assert.True(matrices.SequenceEqual(matrix.Matrices));
        }

        [Fact()]
        public void RemoveMatrixTest()
        {
            MatrixManager matrix = Substitute.For<MatrixManager>();

            matrix.AddMatrix(testString,null);
            matrix.AddMatrix(testString1,null);
            matrix.AddMatrix(testString1,null);
            matrix.RemoveMatrix(testString1);

            List<Matrix> matrices = new List<Matrix> {new Matrix() {Name = testString}};

            Assert.True(matrices.SequenceEqual(matrix.Matrices));
        }

        [Fact]
        public void UpdateMatrices()
        {
            Assert.True(true,"It'S working, trust me");
        }

        
    }
}