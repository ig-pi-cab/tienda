using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TestProject1
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITestOutputHelper _output;

        public IntegrationTests(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _client = _factory.CreateClient();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _output = output;
        }

        [Fact]
        public async Task Get_Productos_ReturnsOkObjectResult()
        {
            // Arrange
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJpZ25hY2lvcGlAY29ycmVvLmNvbSIsImp0aSI6IjRkNTBmZDQ0LWU3MTEtNGI3Ni04NjZmLTE1NzU0NGY0OWFlMSIsImVtYWlsIjoiaWduYWNpb3BpQGNvcnJlby5jb20iLCJ1aWQiOiIxIiwicm9sZXMiOlsiQWRtaW5pc3RyYWRvciIsIkVtcGxlYWRvIl0sImV4cCI6MTY4NDAxOTU2MywiaXNzIjoiVGllbmRhQXBpIiwiYXVkIjoiVGllbmRhQXBpVXNlciJ9.PZ4OGOG_MC-8UaLhRIntfbJ3fdk2yAu1ES4cNdosBkc");

            // Act
            var response = await _client.GetAsync("/api/productos/");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            _output.WriteLine(await response.Content.ReadAsStringAsync());
        }


        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
