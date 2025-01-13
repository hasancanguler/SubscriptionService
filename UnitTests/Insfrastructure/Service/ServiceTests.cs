using Infrastructure.Repository;
using Infrastructure.Repository.Entities;
using Infrastructure.Service.Interfaces;
using Moq;

namespace UnitTests.Insfrastructure.Service
{
    public class ServiceTests : TestBase
    {
        private readonly IService _service;
        public ServiceTests()
        {
            _service = Create<Infrastructure.Service.Service>();
        }
        [Fact]
        public async Task GetService_Should_Return_Service()
        {
            int Id = 1;
            var expectation = new ServiceModel { Id = Id, Name="Some Service" };

            CreateMock<IServiceRepository>().
                Setup(x => x.GetService(Id)).
                ReturnsAsync(expectation).
                Verifiable();

            var actual = await _service.GetService(Id);

            Assert.Equal(expectation, actual);
        }

    }
}
