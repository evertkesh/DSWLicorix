namespace LICORIX_PROYECT.Models.ViewModels;

public class PedidoViewModel
{
    public string IdPedido { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public int EtapaActual { get; set; } = 1; 
    public string NombreCliente { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string MetodoPago { get; set; } = string.Empty;
    public decimal Total { get; set; }

    public List<CarritoItem> Items { get; set; } = new();
    public List<EtapaPedido> Etapas { get; set; } = new();
}

public class EtapaPedido
{
    public int Numero { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Icono { get; set; } = string.Empty;
    public string? HoraEstimada { get; set; }
    public bool Completado { get; set; }
    public bool Activo { get; set; }
}