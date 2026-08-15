using LICORIX_PROYECT.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class NuevosIngresosController : Controller
{
    private readonly IProductoRepositorio _productos;
    public NuevosIngresosController(IProductoRepositorio productos) => _productos = productos;

    public IActionResult Index() => View("~/Views/nuevos_ingresos_licorix_es/nuevos_ingresos.cshtml", _productos.ListarNuevosIngresos());
}