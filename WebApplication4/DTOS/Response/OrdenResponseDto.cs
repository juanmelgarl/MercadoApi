namespace WebApplication4.DTOS.Response
{
    public class OrdenResponseDto
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public string UsuarioNombre { get; set; }
        public List<OrderItemResponseDto> Items { get; set; }
    }

}
