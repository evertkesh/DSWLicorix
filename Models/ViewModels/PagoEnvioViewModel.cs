using System.ComponentModel.DataAnnotations;

namespace LICORIX_PROYECT.Models.ViewModels;

public class PagoEnvioViewModel
{
    public List<CarritoItem> Items { get; set; } = new();
    public decimal Subtotal { get; set; }
    public decimal Envio { get; set; } = 4.90m;
    public decimal Total => Subtotal + Envio;

    
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    public string Direccion { get; set; } = string.Empty;

    [Required(ErrorMessage = "La ciudad es obligatoria.")]
    public string Ciudad { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El email es obligatorio.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string MetodoEnvio { get; set; } = "standard"; 
    public string MetodoPago { get; set; } = "tarjeta";   
    public string? NumeroTarjeta { get; set; }
    public string? NombreTarjeta { get; set; }
    public string? Vencimiento { get; set; }
    public string? Cvv { get; set; }
}