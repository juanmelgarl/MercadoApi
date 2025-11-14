using WebApplication4.Clases;

public class Producto
{
    public int Id { get; set; }
    public string? Marca { get; set; }
    public int Precio { get; set; }
    public bool Comprado { get; set; }
    public int Cantidad { get; set; }

    public List<OrderItem> OrdenItems { get; set; }
}
