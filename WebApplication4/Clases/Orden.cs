using WebApplication4.Clases;

public class Orden
{
    public int Id { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }

    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }

    public List<OrderItem> Items { get; set; }
}
