using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
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
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;

        public Userscontroller(AppDbContext dbContext,IConfiguration config, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _config = config;
            _userManager = userManager;
        }
        [HttpGet]
                [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Obtenertodo(Paginationrequest pagination)
        {
           
            var usuarios = _dbContext.Usuarios
                 .Skip((pagination.Pagenumber - 1) * pagination.Pagesize)
           .Take(pagination.Pagesize)
                .Select(u => new UsuarioResponseDto
                {
                    NombreCompleto = u.Nombre ?? "null",
                    CorreoElectronico = u.Correoelectronico ?? "null",
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
        public ActionResult Buscarporid(int id, Paginationrequest pagination)
        {
            var buscar = _dbContext.Usuarios.FirstOrDefault(x => x.Id == id);
                if (buscar == null)
            {
                return BadRequest("no se encontro al usuario.");
            }
            return Ok(buscar);
        }
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Admin")]
        public ActionResult Borrarusuario(int id)
        {
            var usuario = _dbContext.Usuarios.FirstOrDefault(x => x.Id == id);
            if (usuario == null)
            {
                return BadRequest("no se encontro el usuario");
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
                return BadRequest();
            }
            var usuarios = _dbContext.Usuarios.FirstOrDefault(x => x.Id == id);
            if (usuarios == null)
            {
                return BadRequest("no se encontro al usuario");
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
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Loginrequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
                return Unauthorized("Credenciales inválidas");

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id)
    };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }






    }

}
