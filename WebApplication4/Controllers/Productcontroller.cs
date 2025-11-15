using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
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

        [HttpGet("buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Buscarproducto()
        {

            var productos = _dbContext.Productos
                  .Select(x => new ProductoResponseDto
                  {
                      Id = x.Id,
                      Marca = x.Marca,
                      Precio = x.Precio,
                      Stock = x.Cantidad
                  })
                  .ToList();
            return Ok(productos);

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
                return BadRequest();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Actualizarcategoria(int id, ProductoPatchDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Crearproducto(ProductoRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Buscarporprecio(int precio)
        {
            var producto = _dbContext.Productos.Where(x => x.Precio == precio);
            if (!producto.Any())
            {
                return NotFound();
            }
            return Ok(producto);
        }
        [HttpGet("promedio")]
        public ActionResult Promedioprecio()
        {
            var Promedio = _dbContext.Productos.Average(x => x.Precio);

            return Ok(Promedio.ToString("C"));
        }
        [HttpGet("Total")]
        public ActionResult Calculartotal()
        {
            var Total = _dbContext.Productos.Sum(x => x.Precio * x.Cantidad);

            return Ok(Total.ToString("C"));
        }
        [HttpDelete("EliminarSinstock")]
        public ActionResult Delete()
        {
            var buscar = _dbContext.Productos.Where(x => x.Cantidad == 0).ToList();
            _dbContext.Productos.RemoveRange(buscar);


            return Ok("Se borraron los Productos con stock 0");

        }
        [HttpDelete("eliminarpormarca/{marca}")]
        public ActionResult DeleteFormarca(string marca)
        {
            var producto = _dbContext.Productos.Where(p => p.Marca == marca);
            if (!producto.Any())
            {
                return NotFound();
            }
            _dbContext.Productos.RemoveRange(producto);
            return Ok();
        }
        [HttpGet("Exportar")]
        public ActionResult Exportar()
        {
            var productos = _dbContext.Productos.ToList();
            var csv = new StringBuilder();
            csv.AppendLine("Id,Marca,Precio,Cantidad,Comprado");

            foreach (var p in productos)
            {
                csv.AppendLine($"{p.Id},{p.Marca},{p.Precio},{p.Cantidad},{p.Comprado}");
            }

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", "productos.csv");
        }
        [HttpGet("masbarato")]
        public ActionResult Productomasbarato()
        {
            var producto = _dbContext.Productos
                .OrderBy(x => x.Precio)
                .FirstOrDefault();
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }
        [HttpGet("Mascaro")]
        public ActionResult Mascaro()
        {
            var productos = _dbContext.Productos
                .OrderByDescending(x => x.Precio)
                .FirstOrDefault();
            if (productos == null)
            {
                return NotFound();
            }
            return Ok(productos);
        }
      
    }
}

