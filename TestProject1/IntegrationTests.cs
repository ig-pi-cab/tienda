using AutoMapper;
using Core.Interfaces;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class IntegrationTests : IDisposable
    {
        private readonly CustomWebApplicationFactory _factory;
        private readonly HttpClient _client;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITestOutputHelper _output;

        public IntegrationTests(ITestOutputHelper output)
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _output = output;
        }

        //[Fact]
        //public async Task Get_Productos_ReturnsOkObjectResult()
        //{
        //    // Arrange
        //    _client.DefaultRequestHeaders.Authorization = null;

        //    var response = await _client.GetAsync("/api/productos/");
        //    // Assert
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    _output.WriteLine(await response.Content.ReadAsStringAsync());
        //}

        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}



