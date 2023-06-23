using API.Controllers;
using API.Dtos;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

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



        [Fact]
        public async Task GetById_WithNoId_ReturnsNotFound()
        {
            // Arrange
            int nonExistentId = 100; // Assuming this ID does not exist in the database

            unitOfWorkMock.Setup(uow => uow.Productos.GetByIdAsync(nonExistentId)).ReturnsAsync((Producto)null);

            // Act
            var result = await controller.Get(nonExistentId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var apiResponse = Assert.IsType<ApiResponse>(notFoundResult.Value);

            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Equal("Producto solicitado no encontrado", apiResponse.Message);
        }



        [Fact]
        public async Task GetById_Returns_ProductoDto_When_ProductoExists()
        {
            // Arrange
            var expectedId = 1;
            var expectedNombre = "Product A";
            var expectedPrecio = 10.99m;
            var expectedFechaCreacion = new DateTime(2023, 1, 1);
            var expectedMarca = new MarcaDto
            {
                Id = 1,
                Nombre = "Marca A"
                // Add any other properties specific to MarcaDto
            };
            var expectedCategoria = new CategoriaDto
            {
                Id = 1,
                Nombre = "Categoria A"
                // Add any other properties specific to CategoriaDto
            };

            unitOfWorkMock.Setup(uow => uow.Productos.GetByIdAsync(expectedId))
                .ReturnsAsync(new Producto
                {
                    Id = expectedId,
                    Nombre = expectedNombre,
                    Precio = expectedPrecio,
                    FechaCreacion = expectedFechaCreacion,
                    Marca = new Marca
                    {
                        Id = expectedMarca.Id,
                        Nombre = expectedMarca.Nombre
                        // Add any other properties specific to Marca
                    },
                    Categoria = new Categoria
                    {
                        Id = expectedCategoria.Id,
                        Nombre = expectedCategoria.Nombre
                        // Add any other properties specific to Categoria
                    }
                });
            unitOfWorkMock.Setup(uow => uow.Marcas.GetByIdAsync(expectedMarca.Id))
                .ReturnsAsync(new Marca
                {
                    Id = expectedMarca.Id,
                    Nombre = expectedMarca.Nombre
                    // Add any other properties specific to Marca
                });
            unitOfWorkMock.Setup(uow => uow.Categorias.GetByIdAsync(expectedCategoria.Id))
                .ReturnsAsync(new Categoria
                {
                    Id = expectedCategoria.Id,
                    Nombre = expectedCategoria.Nombre
                    // Add any other properties specific to Categoria
                });

            mapperMock.Setup(mapper => mapper.Map<ProductoDto>(It.IsAny<Producto>()))
                .Returns<Producto>(producto => new ProductoDto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    FechaCreacion = producto.FechaCreacion,
                    Marca = expectedMarca,
                    Categoria = expectedCategoria
                });

            // Act
            var result = await controller.Get(expectedId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<ProductoDto>>(result);
            var productoDto = Assert.IsType<ProductoDto>(result.Value);
            Assert.Equal(expectedId, productoDto.Id);
            Assert.Equal(expectedNombre, productoDto.Nombre);
            Assert.Equal(expectedPrecio, productoDto.Precio);
            Assert.Equal(expectedFechaCreacion, productoDto.FechaCreacion);
            Assert.Equal(expectedMarca, productoDto.Marca);
            Assert.Equal(expectedCategoria, productoDto.Categoria);

        }

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

        [Fact]
        public async Task Post_ReturnsCreatedStatusOrBadRequest()
        {
            // Arrange
            ProductoAddUpdateDto requestDto = null; // Set the requestDto to null for an empty request

            var expectedProducto = new Producto
            {
                Id = 32,
                Nombre = "Witarra",
                Precio = 33333,
                FechaCreacion = new DateTime(2023, 03, 31),
                MarcaId = 2,
                CategoriaId = 2
            };

            // Create a mock of the repository interface
            var productoRepositoryMock = new Mock<IProductoRepository>();

            // Set up the unitOfWorkMock to return the mocked repository object
            unitOfWorkMock.Setup(uow => uow.Productos).Returns(productoRepositoryMock?.Object);

            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.FromResult(0));

            // Set up the mapper to return null for the mapping
            mapperMock.Setup(mapper => mapper.Map<Producto>(It.IsAny<ProductoAddUpdateDto>())).Returns((Producto)null);

            // Act
            var result = await controller.Post(requestDto);

            // Assert
            Assert.NotNull(result);
            var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestObjectResult.StatusCode);

            
        }

        [Fact]
        public async Task Put_Producto_ReturnOkObjectResult()
        {
            // Arrange
            int expectedId = 4;
            ProductoAddUpdateDto expectedProductoDto = new ProductoAddUpdateDto
            {
                Id = expectedId,
                Nombre = "Sample Product",
                Precio = 10.99m,
                FechaCreacion = DateTime.Now,
                MarcaId = 1,
                CategoriaId = 1
            };

            unitOfWorkMock.Setup(uow => uow.Productos.Update(It.IsAny<Producto>()));
            unitOfWorkMock.Setup(uow => uow.SaveAsync());

            // Act
            var result = await controller.Put(expectedId, expectedProductoDto);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ActionResult<ProductoAddUpdateDto>>(result);
            var updatedProductoDto = Assert.IsType<ProductoAddUpdateDto>(result.Value);
            Assert.Equal(expectedProductoDto.Id, updatedProductoDto.Id);
            Assert.Equal(expectedProductoDto.Nombre, updatedProductoDto.Nombre);
            Assert.Equal(expectedProductoDto.Precio, updatedProductoDto.Precio);
            Assert.Equal(expectedProductoDto.FechaCreacion, updatedProductoDto.FechaCreacion);
            Assert.Equal(expectedProductoDto.MarcaId, updatedProductoDto.MarcaId);
            Assert.Equal(expectedProductoDto.CategoriaId, updatedProductoDto.CategoriaId);
        }

        [Fact]
        public async Task Put_ReturnsNotFound()
        {
            // Arrange
            int id = 4;
            ProductoAddUpdateDto productoDto = null; // Set the productoDto to null for an empty request

            // Act
            var result = await controller.Put(id, productoDto);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            int id = 4;

            unitOfWorkMock.Setup(uow => uow.Productos.GetByIdAsync(id))
                .ReturnsAsync(new Producto { Id = id });

            unitOfWorkMock.Setup(uow => uow.SaveAsync()).Returns(Task.FromResult(0));

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound()
        {
            // Arrange
            int id = 4;

            unitOfWorkMock.Setup(uow => uow.Productos.GetByIdAsync(id))
                .ReturnsAsync((Producto)null);

            // Act
            var result = await controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }

}
