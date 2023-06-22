using API.Dtos;
using API.Helpers;
using API.Helpers.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace API.Controllers;
[ApiVersion("1.0")]
//[ApiVersion("1.1")]
//[Authorize(Roles = "Administrador")]
public class ProductosController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductosController> _logger;


    public ProductosController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }



    //[HttpGet]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //public async Task<ActionResult<Pager<ProductoListDto>>> Get([FromQuery] Params productParams)
    //{
    //    var resultado = await _unitOfWork.Productos
    //                                .GetAllAsync(productParams.PageIndex, productParams.PageSize, productParams.Search);

    //    var listaProductosDto = _mapper.Map<List<ProductoListDto>>(resultado.registros);

    //    Response.Headers.Add("X-InlineCount", resultado.totalRegistros.ToString());

    //    return new Pager<ProductoListDto>(listaProductosDto, resultado.totalRegistros,
    //                productParams.PageIndex, productParams.PageSize, productParams.Search);

    //}

    [HttpGet]
    //[MapToApiVersion("1.1")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<ProductoDto>>> Get()
    {
        try
        {
            _logger.LogInformation("Get() method called.");

            var productos = await _unitOfWork.Productos.GetAllAsync();

            _logger.LogInformation($"Retrieved: {productos}.");
            return _mapper.Map<List<ProductoDto>>(productos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving products.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse(500, "Internal Server Error"));
        }
    }

    [HttpGet("mas-caros/{cantidad}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductosMasCaros(int cantidad)
    {
        var productosMasCaros = await _unitOfWork.Productos.GetProductosMasCaros(cantidad);
        return Ok(productosMasCaros);
    }

    [HttpGet("{id}")]
    //[ProducesResponseType(StatusCodes.Status200OK)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductoDto>> Get(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);
        if (producto == null)
            return NotFound(new ApiResponse(404, "Producto solicitado no encontrado"));

        return _mapper.Map<ProductoDto>(producto);
    }

    //POST: api/Productos
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Producto>> Post(ProductoAddUpdateDto productoDto)
    {
        var producto = _mapper.Map<Producto>(productoDto);
        _unitOfWork.Productos.Add(producto);
        await _unitOfWork.SaveAsync();
        if (producto == null)
        {
            return BadRequest(new ApiResponse(400));
        }
        productoDto.Id = producto.Id;
        return CreatedAtAction(nameof(Post), new { id = productoDto.Id }, productoDto);
    }


    //PUT: api/Productos/4
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProductoAddUpdateDto>> Put(int id, [FromBody] ProductoAddUpdateDto productoDto)
    {
        if (productoDto == null)
            return NotFound(new ApiResponse(404, "Producto solicitado no encontrado"));

        //Rastreo segun contexto
        var producto = _mapper.Map<Producto>(productoDto);
        _unitOfWork.Productos.Update(producto);
        await _unitOfWork.SaveAsync();
        return productoDto;
    }
        
    //delete: api/productos
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _unitOfWork.Productos.GetByIdAsync(id);
        if (producto == null)
            return NotFound(new ApiResponse(404, "Producto solicitado no encontrado"));

        _unitOfWork.Productos.Remove(producto);
        await _unitOfWork.SaveAsync();

        return NoContent();
    }


}
