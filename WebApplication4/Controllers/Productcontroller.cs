using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DTOS.Response;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Productcontroller : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public Productcontroller(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Buscarproducto()
        {
            
            var Productos = _dbContext.Productos
                .Select(p => new ProductoResponseDto
                {
                    Id = p.Id,
                    Marca = p.Marca ?? "desconocida",
                    Precio = p.Precio,
                    Stock = p.Cantidad,
                     

                })
                .ToList();
            return Ok(Productos);

        }
    }
}
