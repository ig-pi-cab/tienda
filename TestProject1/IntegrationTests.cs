using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infraestructure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using MySqlConnector;
using System.Globalization;
using System.Net;
using System.Text.Json;
using TestProject1;
using Xunit.Abstractions;

namespace Tienda.Test
{
    public class IntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {


        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITestOutputHelper _output;
        private readonly DbContextOptions<TiendaContexto> _dbContextOptions;

        public IntegrationTests(CustomWebApplicationFactory factory, ITestOutputHelper output)
        {
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _output = output;

        }
        
        
        //[Fact]
        //public async Task Get_ReturnsListOfProductoDto()
        //{
        //    // Arrange
        //    var response = await _client.GetAsync("api/productos"); // Modify the URL according to your route configuration

        //    // Assert
        //    response.EnsureSuccessStatusCode();
        //    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //    var content = await response.Content.ReadAsStringAsync();
        //    var productosDto = JsonSerializer.Deserialize<List<ProductoDto>>(content, new JsonSerializerOptions
        //    {
        //        PropertyNameCaseInsensitive = true
        //    });

        //    Assert.NotNull(productosDto);
        //    // Additional assertions based on your expected data
        //}
        [Fact]
        public async Task GetDollarExchangeRate_ReturnsDecimal()
        {
            // Arrange
            var response = await _client.GetAsync("/api/dolar"); // Modify the URL according to your route configuration

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var exchangeRate = decimal.Parse(content, CultureInfo.InvariantCulture);

            Assert.NotEqual(default(decimal), exchangeRate);
            // Additional assertions based on your expected data
        }





        public void Dispose()
        {
            _client.Dispose();
            _factory.Dispose();
        }
    }
}
