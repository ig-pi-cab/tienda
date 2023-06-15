using API.Controllers;
using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infraestructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestProject1
{
    public class UnitTest1
    {
        private Mock<IUnitOfWork> unitOfWorkMock;
        private Mock<IMapper> mapperMock;
        private ProductosController controller;
        //private DolarController dolarController;

        public UnitTest1()
        {
            // Initialize mock objects
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mapperMock = new Mock<IMapper>();

            // Create the controller object with the mock objects
            controller = new ProductosController(unitOfWorkMock.Object, mapperMock.Object);
        }

        [Fact]
        public async Task Get_Productos_ReturnsOkObjectResult()
        {
            // Arrange
            var expectedProductos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Producto 1", Precio = 100, FechaCreacion = DateTime.Now },
                new Producto { Id = 2, Nombre = "Producto 2", Precio = 90, FechaCreacion = DateTime.Now }
                // Add more sample Producto objects if needed
            };

            unitOfWorkMock.Setup(uow => uow.Productos.GetAllAsync()).ReturnsAsync(expectedProductos);

            var expectedProductoDtos = new List<ProductoDto>
            {
                new ProductoDto { Id = 1, Nombre = "Producto 1", Precio = 100, FechaCreacion = DateTime.Now },
                new ProductoDto { Id = 2, Nombre = "Producto 2", Precio = 90, FechaCreacion = DateTime.Now }
                // Add more expected ProductoDto objects if needed
            };

            mapperMock.Setup(mapper => mapper.Map<List<ProductoDto>>(expectedProductos)).Returns(expectedProductoDtos);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = result as ActionResult<IEnumerable<ProductoDto>>;
            Assert.NotNull(okResult.Value);
            Assert.IsType<List<ProductoDto>>(okResult.Value);
        }

        [Fact]
        public async Task Get_ReturnsNull()
        {
            // Arrange
            var expectedProductos = new List<Producto>(); // Empty list

            unitOfWorkMock.Setup(uow => uow.Productos.GetAllAsync()).ReturnsAsync(expectedProductos);

            mapperMock.Setup(mapper => mapper.Map<List<ProductoDto>>(expectedProductos)).Returns((List<ProductoDto>)null);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = result as ActionResult<IEnumerable<ProductoDto>>;
            Assert.Null(okResult.Value); // Expecting null value
        }

        //[Fact]
        //public async Task Get_WithValidId_ReturnsOkObjectResult()
        //{
        //    // Arrange
        //    int productId = 1;
        //    var expectedProducto = new Producto
        //    {
        //        Id = 1,
        //        Nombre = "d'Addario EJ27N Cuerda Guitarra Acústica",
        //        Precio = 200.00m,
        //        FechaCreacion = new DateTime(2022, 01, 01),
        //        MarcaId = 1,
        //        CategoriaId = 1,
        //        Marca = new Marca { Id = 1, Nombre = "Addario" },
        //        Categoria = new Categoria { Id = 1, Nombre = "Accesorios Guitarra" }
        //    };

        //    unitOfWorkMock.Setup(uow => uow.Productos.GetByIdAsync(productId)).ReturnsAsync(expectedProducto);

        //    // Act
        //    var result = await controller.Get(productId);

        //    // Assert that the result is not null
        //    Assert.NotNull(result);

        //    // Assert that the result is of type ActionResult<ProductoDto>
        //    var actionResult = Assert.IsType<ActionResult<ProductoDto>>(result);
        //    Assert.NotNull(actionResult);

        //    // Assert that the value of the ActionResult is not null
        //    var productoDto = actionResult.Value;
        //    Assert.NotNull(productoDto);

        //    // Perform the assertions on productoDto
        //    Assert.Equal(expectedProducto.Id, productoDto.Id);
        //    Assert.Equal(expectedProducto.Nombre, productoDto.Nombre);
        //    Assert.Equal(expectedProducto.Precio, productoDto.Precio);
        //    Assert.Equal(expectedProducto.FechaCreacion, productoDto.FechaCreacion);
        //    Assert.Equal(expectedProducto.Marca.Id, productoDto.Marca.Id);
        //    Assert.Equal(expectedProducto.Marca.Nombre, productoDto.Marca.Nombre);
        //    Assert.Equal(expectedProducto.Categoria.Id, productoDto.Categoria.Id);
        //    Assert.Equal(expectedProducto.Categoria.Nombre, productoDto.Categoria.Nombre);

        //}

        

        [Fact]
        public async Task Post_ReturnsCreatedStatusAndProductoDto()
        {
            // Arrange
            var requestDto = new ProductoAddUpdateDto
            {
                Nombre = "Witarra",
                Precio = 33333,
                FechaCreacion = new DateTime(2023, 03, 31),
                MarcaId = 2,
                CategoriaId = 2
            };

            var expectedProducto = new Producto
            {
                Id = 32,
                Nombre = "Witarra",
                Precio = 33333,
                FechaCreacion = new DateTime(2023, 03, 31),
                MarcaId = 2,
                CategoriaId = 2
            };

            unitOfWorkMock.Setup(uow => uow.Productos.Add(It.IsAny<Producto>())).Callback<Producto>(producto =>
            {
                producto.Id = expectedProducto.Id;
            });

            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.FromResult(0));

            mapperMock.Setup(mapper => mapper.Map<Producto>(It.IsAny<ProductoAddUpdateDto>())).Returns(expectedProducto);

            // Act
            var result = await controller.Post(requestDto);

            // Assert
            Assert.NotNull(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, createdResult.StatusCode);
            var productoDto = Assert.IsType<ProductoAddUpdateDto>(createdResult.Value);
            Assert.Equal(expectedProducto.Id, productoDto.Id);
            Assert.Equal(expectedProducto.Nombre, productoDto.Nombre);
            Assert.Equal(expectedProducto.Precio, productoDto.Precio);
            Assert.Equal(expectedProducto.FechaCreacion, productoDto.FechaCreacion);
            Assert.Equal(expectedProducto.MarcaId, productoDto.MarcaId);
            Assert.Equal(expectedProducto.CategoriaId, productoDto.CategoriaId);
        }

        //[Fact]
        //public async Task Post_ReturnsBadRequestForEmptyProducto()
        //{
        //    // Arrange
        //    var requestDto = new ProductoAddUpdateDto();

        //    // Act
        //    var result = await controller.Post(requestDto);

        //    // Assert
        //    Assert.NotNull(result);
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        //    Assert.Equal(400, badRequestResult.StatusCode);
        //    var errorResponse = Assert.IsType<Dictionary<string, List<string>>>(badRequestResult.Value);
        //    Assert.NotNull(errorResponse);
        //    Assert.Contains("Nombre", errorResponse.Keys);
        //    var nombreErrors = errorResponse["Nombre"];
        //    Assert.NotNull(nombreErrors);
        //    Assert.NotEmpty(nombreErrors);
        //    Assert.Equal("El nombre del producto es requerido.", nombreErrors[0]);
        //}

        //[Fact]
        //public async Task Post_WithNullProducto_ReturnsBadRequest()
        //{
        //    // Arrange
        //    ProductoAddUpdateDto? requestDto = null;

        //    // Act
        //    var result = await controller.Post(requestDto);

        //    // Assert
        //    Assert.NotNull(result);
        //    var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        //    Assert.Equal(400, badRequestResult.StatusCode);
        //    var apiResponse = Assert.IsType<ApiResponse>(badRequestResult.Value);
        //    Assert.Equal(400, apiResponse.StatusCode);
        //}




    }

}
