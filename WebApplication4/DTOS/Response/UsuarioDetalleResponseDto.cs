namespace WebApplication4.DTOS.Response
{
    public class UsuarioDetalleResponseDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string CorreoElectronico { get; set; }
        public List<Orden> Ordenes { get; set; }
    }

}
