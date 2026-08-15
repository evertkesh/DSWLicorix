using LICORIX_PROYECT.Data.Interfaces;
using LICORIX_PROYECT.Models;
using LICORIX_PROYECT.Models.ViewModels;
using LICORIX_PROYECT.Services;
using Microsoft.AspNetCore.Mvc;

namespace LICORIX_PROYECT.Controllers;

public class CuentaController : Controller
{
    private readonly IUsuarioRepositorio _usuarios;
    private readonly SesionUsuario _sesion;

    public CuentaController(IUsuarioRepositorio usuarios, SesionUsuario sesion)
    {
        _usuarios = usuarios;
        _sesion = sesion;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (_sesion.EstaAutenticado) return RedirectToAction("Index", "Inicio");
        return View(new LoginViewModel { ReturnUrl = returnUrl });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var usuario = _usuarios.ValidarLogin(vm.Correo.Trim(), vm.Contrasena);
        if (usuario == null)
        {
            vm.MensajeError = "Correo o contraseña incorrectos.";
            return View(vm);
        }

        _sesion.Iniciar(usuario);

        if (!string.IsNullOrEmpty(vm.ReturnUrl) && Url.IsLocalUrl(vm.ReturnUrl))
            return Redirect(vm.ReturnUrl);

        return RedirectToAction("Index", "Inicio");
    }

    [HttpGet]
    public IActionResult Registro() => View(new RegistroViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Registro(RegistroViewModel vm)
    {
        
        var nombres = (vm.Nombres ?? string.Empty).Trim();
        var apellidos = (vm.Apellidos ?? string.Empty).Trim();
        var correo = (vm.Correo ?? string.Empty).Trim().ToLowerInvariant();
        var contrasena = vm.Contrasena ?? string.Empty;

        if (nombres.Length < 2 || !System.Text.RegularExpressions.Regex.IsMatch(nombres, "^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\\s]+$"))
        {
            vm.MensajeError = "Los nombres solo pueden contener letras y espacios (mínimo 2 caracteres).";
            return View(vm);
        }
        if (apellidos.Length < 2 || !System.Text.RegularExpressions.Regex.IsMatch(apellidos, "^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\\s]+$"))
        {
            vm.MensajeError = "Los apellidos solo pueden contener letras y espacios (mínimo 2 caracteres).";
            return View(vm);
        }
        if (contrasena.Length < 8)
        {
            vm.MensajeError = "La contraseña debe tener al menos 8 caracteres.";
            return View(vm);
        }
        if (contrasena != vm.ConfirmarContrasena)
        {
            vm.MensajeError = "Las contraseñas no coinciden.";
            return View(vm);
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            vm.MensajeError = "El formato del correo es inválido.";
            return View(vm);
        }
        if (!string.IsNullOrWhiteSpace(vm.Telefono) &&
            !System.Text.RegularExpressions.Regex.IsMatch(vm.Telefono, "^[0-9+\\-\\s()]+$"))
        {
            vm.MensajeError = "El teléfono solo puede contener números, espacios, +, - y paréntesis.";
            return View(vm);
        }

        if (!ModelState.IsValid) return View(vm);

        if (_usuarios.ExisteCorreo(correo))
        {
            vm.MensajeError = "Ya existe una cuenta registrada con ese correo.";
            return View(vm);
        }

        var nuevo = new Usuario
        {
            Nombres = nombres,
            Apellidos = apellidos,
            Correo = correo,
            Contrasena = contrasena, 
            Telefono = vm.Telefono,
            Direccion = vm.Direccion,
            IdRol = 2 
        };
        _usuarios.Insertar(nuevo);

        TempData["Exito"] = "Cuenta creada correctamente. Ya puedes iniciar sesión.";
        return RedirectToAction(nameof(Login));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        _sesion.Cerrar();
        TempData["Exito"] = "Sesión cerrada.";
        return RedirectToAction(nameof(Login));
    }
}