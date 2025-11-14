using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApplication4.Clases;
using WebApplication4.DTOS.Request;
using WebApplication4.DTOS.Response;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Userscontroller : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public Userscontroller(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
                [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Obtenertodo()
        {
           
            var usuarios = _dbContext.Usuarios
                .Select(u => new UsuarioResponseDto
                {
                    NombreCompleto = u.Nombre,
                    CorreoElectronico = u.Correoelectronico,
                      Id = u.Id,
                    


                })

                .ToList();
            if (!usuarios.Any())
            {
                return NotFound("no hay usuarios");
            }
            return Ok(usuarios);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Buscarporid(int id)
        {
            var buscar = _dbContext.Usuarios.FirstOrDefault(x => x.Id == id);
                if (buscar == null)
            {
                return NotFound("no se encontro al usuario.");
            }
            return Ok(buscar);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Borrarusuario(int id)
        {
            var usuario = _dbContext.Usuarios.FirstOrDefault(x => x.Id == id);
            if (usuario == null)
            {
                return NotFound("no se encontro el usuario");
            }
            _dbContext.Usuarios.Remove(usuario);
            _dbContext.SaveChanges();
            return Ok("se ha borrado exitosamente");

        }
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Actualizarusuario(int id,UsuarioRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var usuarios = _dbContext.Usuarios.FirstOrDefault(x => x.Id == id);
            if (usuarios == null)
            {
                return NotFound("no se encontro al usuario");
            }
            usuarios.Nombre = dto.Nombre;
            usuarios.Numerotelefono = dto.NumeroTelefono;
            usuarios.Correoelectronico = dto.CorreoElectronico;
            usuarios.Apellido = dto.Apellido;
            _dbContext.SaveChanges();
            return Ok();
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Crearusuario(UsuarioRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                Numerotelefono = dto.NumeroTelefono,
                Correoelectronico = dto.CorreoElectronico
            };
            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();
            return Ok();

        }

      



    }

}
