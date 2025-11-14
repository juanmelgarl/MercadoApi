namespace WebApplication4.Clases
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public long Numerotelefono { get; set; }
        public string? Correoelectronico { get; set; }
        public List<Orden> orders { get; set; }
     }
}
