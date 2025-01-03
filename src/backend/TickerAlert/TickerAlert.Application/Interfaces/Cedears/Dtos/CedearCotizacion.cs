namespace TickerAlert.Application.Interfaces.Cedears.Dtos;

public class CedearCotizacion
{
    public bool HasCedear { get; set; } 
    public string Ratio { get; set; } 
    public decimal CedearCompra { get; set; }
    public decimal CedearVenta { get; set; }
}
