using System.ComponentModel.DataAnnotations;

namespace LICORIX_PROYECT.Models.ViewModels;

public class RegistroViewModel
{
    [Required(ErrorMessage = "Tus nombres son obligatorios.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Los nombres deben tener entre 2 y 80 caracteres.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\\s]+$", ErrorMessage = "Los nombres solo pueden contener letras y espacios.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Tus apellidos son obligatorios.")]
    [StringLength(80, MinimumLength = 2, ErrorMessage = "Los apellidos deben tener entre 2 y 80 caracteres.")]
    [RegularExpression("^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\\s]+$", ErrorMessage = "Los apellidos solo pueden contener letras y espacios.")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
    [StringLength(120)]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres.")]
    [StringLength(64, ErrorMessage = "La contraseña no puede tener más de 64 caracteres.")]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirma tu contraseña.")]
    [DataType(DataType.Password)]
    [Compare("Contrasena", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmarContrasena { get; set; } = string.Empty;

    [StringLength(20)]
    [RegularExpression("^[0-9+\\-\\s()]{0,20}$", ErrorMessage = "El teléfono solo puede contener números, espacios, +, - y paréntesis.")]
    public string? Telefono { get; set; }

    [StringLength(200, MinimumLength = 5, ErrorMessage = "La dirección debe tener entre 5 y 200 caracteres.")]
    public string? Direccion { get; set; }

    public string? MensajeError { get; set; }
}