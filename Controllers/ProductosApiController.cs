using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosApiController : ControllerBase
{
    private readonly IProductoRepositorio _repo;
    public ProductosApiController(IProductoRepositorio repo) => _repo = repo;

    [HttpGet]
    public ActionResult<IEnumerable<Producto>> Get() => Ok(_repo.Listar());

    [HttpGet("{id}")]
    public ActionResult<Producto> Get(int id)
    {
        var p = _repo.ObtenerPorId(id);
        return p is null ? NotFound() : Ok(p);
    }

    [HttpGet("destacados")]
    public ActionResult<IEnumerable<Producto>> Destacados() => Ok(_repo.ListarDestacados());

    [HttpGet("ofertas")]
    public ActionResult<IEnumerable<ProductoEnOferta>> Ofertas() => Ok(_repo.ListarEnOferta());

    [HttpGet("buscar")]
    public ActionResult<IEnumerable<Producto>> Buscar([FromQuery] string q)
        => Ok(_repo.BuscarPorTexto(q ?? string.Empty));

    [HttpGet("categoria/{idCategoria}")]
    public ActionResult<IEnumerable<Producto>> PorCategoria(int idCategoria)
        => Ok(_repo.ListarPorCategoria(idCategoria));

    [HttpPost]
    public IActionResult Post([FromBody] Producto producto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        _repo.Insertar(producto);
        return CreatedAtAction(nameof(Get), new { id = producto.IdProducto }, producto);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody] Producto producto)
    {
        producto.IdProducto = id;
        _repo.Actualizar(producto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        _repo.Eliminar(id);
        return NoContent();
    }
}