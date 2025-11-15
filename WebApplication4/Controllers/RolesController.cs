using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserRolesController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("create-role")]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole(roleName));
        }
        return Ok($"Rol {roleName} creado");
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole(string username, string roleName)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null) return NotFound("Usuario no encontrado");

        if (!await _roleManager.RoleExistsAsync(roleName))
            return BadRequest("El rol no existe");

        await _userManager.AddToRoleAsync(user, roleName);
        return Ok($"Rol {roleName} asignado a {username}");
    }
}
