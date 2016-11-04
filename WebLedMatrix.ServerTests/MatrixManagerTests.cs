using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNet.SignalR;
using Moq;
using WebLedMatrix;
using WebLedMatrix.Hubs;
using WebLedMatrix.Models;
using Xunit;

namespace Test.WebLedMatrix.Server
{
    [ExcludeFromCodeCoverage]
    public static class MatrixManagerTestsExtensions
    {
        public static Mock<IHubContext<IUiManagerHub>> AssertIfCallUpdateMatrices(
            this Mock<IHubContext<IUiManagerHub>>mock)
        {
            mock.Setup(x=>x.Clients.All.unRegisterAllMatrices())
                .Verifiable("This should be called");

            mock.Setup(x=>x.Clients.All.updateMatrices(It.IsAny<Matrix[]>()))
                .Verifiable("This should be called");
            return mock;
        }
    }

    [ExcludeFromCodeCoverage]
    public class MatrixManagerFixture
    {
        public Mock<IHubContext<IUiManagerHub>> IHubMock;
        public MatrixManager MatrixManager;

        public MatrixManagerFixture()
        {
            IHubMock = new Mock<IHubContext<IUiManagerHub>>();
        }

        public void SetupMock(Action<Mock<IHubContext<IUiManagerHub>>> setupParameters)
        {
            IHubMock = new Mock<IHubContext<IUiManagerHub>>();
            setupParameters.Invoke(IHubMock);
            MatrixManager = new MatrixManager(IHubMock.Object);
        }
    }

    [ExcludeFromCodeCoverage]
    public class MatrixManagerTests :IClassFixture<MatrixManagerFixture>
    {
        private MatrixManagerFixture _fixture;

        string nameOfMatrix = nameof(nameOfMatrix);

        public MatrixManagerTests(MatrixManagerFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact()]
        public void AddMatrixTest_IsCallingService_IsAddingMatrix()
        {
            _fixture.SetupMock(x=>x.AssertIfCallUpdateMatrices());

            _fixture.MatrixManager.AddMatrix(nameOfMatrix);
            
            _fixture.IHubMock.Verify(x=>x.Clients.All.unRegisterAllMatrices());
            Assert.True(
                _fixture.MatrixManager.Matrices.Exists(
                    x => x.Name.Equals(nameOfMatrix)));
        }

        [Fact()]
        public void RemoveMatrixTest()
        {
            _fixture.SetupMock(x => x.AssertIfCallUpdateMatrices());

            _fixture.MatrixManager.AddMatrix(nameOfMatrix);
            _fixture.MatrixManager.RemoveMatrix(nameOfMatrix);

            _fixture.IHubMock.Verify(x => x.Clients.All.unRegisterAllMatrices());
            Assert.False(
                _fixture.MatrixManager.Matrices.Exists(
                    x => x.Name.Equals(nameOfMatrix)));
        }

        [Fact()]
        public void UpdateMatricesTest()
        {
            _fixture.SetupMock(x=>x.AssertIfCallUpdateMatrices());

            _fixture.MatrixManager.UpdateMatrices();

            _fixture.IHubMock.Verify(x=>x.Clients.All.unRegisterAllMatrices());
        }


        [Fact()]
        public void UpdateMatricesTest_IsNotCalled()
        {
            _fixture.SetupMock(x => x.AssertIfCallUpdateMatrices());

            _fixture.IHubMock.Verify(x => x.Clients.All.unRegisterAllMatrices(),Times.Never);
        }

    }
}