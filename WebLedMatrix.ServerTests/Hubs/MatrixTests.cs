using Xunit;
using System;
using System.Diagnostics.CodeAnalysis;
using WebLedMatrix.Models;

namespace WebLedMatrix.Hubs.Tests
{
    [ExcludeFromCodeCoverage]
    public class MatrixTests
    {
        string matrixName = nameof(matrixName);
        string anotherMatrixName = nameof(anotherMatrixName);

        [Fact()] public void EqualsTest_ThatSameNamed_AreEqual()
        {
            var Matrix = new Client() {Name = matrixName};
            var Matrix2 = new Client() {Name = matrixName};

            Assert.Equal(Matrix,Matrix2);
        }

        [Fact()]
        public void EqualsTest_RandomObject_AreNotEqual()
        {
            var Matrix = new Client() { Name = matrixName };
            var randomObject = new Random().Next().GetHashCode().ToString();

            Assert.NotEqual(Matrix, (object)randomObject);
        }

        [Fact()]
        public void EqualsTest_DiffrentNamed_AreNotEqual()
        {
            var Matrix = new Client() { Name = matrixName };
            var Matrix2 = new Client() { Name = anotherMatrixName};

            Assert.NotEqual(Matrix, Matrix2);
        }

        [Fact()]
        public void GetHashCode_NameIsHashCode_ExpectedBehavior() //Needed for 
        {
            var matrix = new Client {Name = matrixName};

            Assert.Equal(matrix.GetHashCode(),matrixName.GetHashCode());
        }

        [Fact()]
        public void GetHashCode_NameIsHashCode_ExpectedBehavior_PropertiesAreAssigned() //Needed for 
        {
            var matrix = new Client
            {
                Connected = true,
                Enabled = false,
                Name = matrixName
            };

            Assert.Equal(matrix.GetHashCode(), matrixName.GetHashCode());
        }
    }
}