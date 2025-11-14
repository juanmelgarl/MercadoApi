namespace WebApplication4.DTOS.Response
{
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public string ProductoMarca { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
    }

}
