using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class SobreNosotrosController : Controller
{
    public IActionResult Index() => View("~/Views/sobre_nosotros_licorix_es/sobre_nosotros.cshtml");
}