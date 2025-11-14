using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DTOS.Request;
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
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Buscarporid(int id)
        {
            var productos = _dbContext.Productos.FirstOrDefault(x => x.Id == id);
            if (productos == null)
            {
                return NotFound("no se encontro el producto");
            }
            return Ok(productos);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult BorrarProducto(int id)
        {
            var borrar = _dbContext.Productos.FirstOrDefault(x => x.Id == id);
            if (borrar == null)
            {
                return NotFound();
            }

            _dbContext.Productos.Remove(borrar);
            _dbContext.SaveChanges();

            return NoContent();
        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Actualizarproducto(int id, ProductoRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var producto = _dbContext.Productos.FirstOrDefault(x => x.Id == id);
            if (producto == null)
            {
                return NotFound();
            }
            producto.Marca = dto.Marca;
            producto.Cantidad = dto.Cantidad;
            producto.Comprado = dto.Comprado;
            producto.Precio = dto.Precio;

            _dbContext.SaveChanges();
            return Ok();


        }
        [HttpPatch("{id}")]

        public ActionResult Actualizarcategoria(int id, ProductoPatchDto dto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var categoria = _dbContext.Productos.FirstOrDefault(x => x.Id == id);
            if (categoria == null)
            {
                return NotFound();
            }
            categoria.Cantidad = dto.Cantidad;
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public ActionResult Crearproducto(ProductoRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var producto = new Producto
            {
                Marca = dto.Marca,
                Cantidad = dto.Cantidad,
                Precio = dto.Precio,
                Comprado = dto.Comprado

            };
            _dbContext.Productos.Add(producto);
            _dbContext.SaveChanges();
            return Ok();

        }
        [HttpGet("filtrarporprecio")]
        public ActionResult Filtrarporprecio()
        {

            var producto = _dbContext.Productos
                .OrderByDescending(x => x.Precio)
                .Select(p => new ProductoResponseDto
                {
                    Marca = p.Marca,
                    Precio = p.Precio,




                })
                .ToList();
            return Ok(producto);
        }
        [HttpGet("pormarca/{marca}/")]

        public ActionResult Buscarpornombre(string? marca)
        {
            var productos = _dbContext.Productos.Where(p => p.Marca == marca);
            if (!productos.Any())
            {
                return NotFound();
            }
            return Ok(productos);
        }
        [HttpGet("porprecio/{precio:int}")]
        public ActionResult Buscarporprecio(int precio)
        {
            var producto = _dbContext.Productos.Where(x => x.Precio == precio);
            if (!producto.Any())
            {
                return NotFound();
            }
            return Ok(producto);
        }
    }
}

