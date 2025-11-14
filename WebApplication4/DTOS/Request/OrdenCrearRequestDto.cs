namespace WebApplication4.DTOS.Request
{
    public class OrdenCrearRequestDto
    {
        public int UsuarioId { get; set; }
        public List<OrderItemCrearDto> Items { get; set; }
    }

}
