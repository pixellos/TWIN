using System;
using System.Diagnostics.CodeAnalysis;
using WebLedMatrix.Hubs;
using Xunit;

namespace WebLedMatrix.ServerTests2.Hubs
{
    [ExcludeFromCodeCoverage]
    public class MatrixTests
    {
        string matrixName = nameof(matrixName);
        string anotherMatrixName = nameof(anotherMatrixName);

        [Fact()] public void EqualsTest_ThatSameNamed_AreEqual()
        {
            var Matrix = new Matrix() {Name = matrixName};
            var Matrix2 = new Matrix() {Name = matrixName};

            Assert.Equal(Matrix,Matrix2);
        }

        [Fact()]
        public void EqualsTest_RandomObject_AreNotEqual()
        {
            var Matrix = new Matrix() { Name = matrixName };
            var randomObject = new Random().Next().GetHashCode().ToString();

            Assert.NotEqual(Matrix, (object)randomObject);
        }

        [Fact()]
        public void EqualsTest_DiffrentNamed_AreNotEqual()
        {
            var Matrix = new Matrix() { Name = matrixName };
            var Matrix2 = new Matrix() { Name = anotherMatrixName};

            Assert.NotEqual(Matrix, Matrix2);
        }

        [Fact()]
        public void GetHashCode_NameIsHashCode_ExpectedBehavior() //Needed for 
        {
            var matrix = new Matrix {Name = matrixName};

            Assert.Equal(matrix.GetHashCode(),matrixName.GetHashCode());
        }

        [Fact()]
        public void GetHashCode_NameIsHashCode_ExpectedBehavior_PropertiesAreAssigned() //Needed for 
        {
            var matrix = new Matrix
            {
                Connected = true,
                Enabled = false,
                Name = matrixName
            };

            Assert.Equal(matrix.GetHashCode(), matrixName.GetHashCode());
        }
    }
}