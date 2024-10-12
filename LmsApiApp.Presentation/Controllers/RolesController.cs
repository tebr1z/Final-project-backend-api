using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return BadRequest("Role name cannot be empty.");

            if (await _roleManager.RoleExistsAsync(roleName))
                return Conflict("Role already exists.");

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
                return Ok("Role created successfully.");

            return BadRequest(result.Errors);
        }

        [HttpGet("list")]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }

        // Diğer rol yönetim işlemleri (Güncelleme, Silme vb.) ekleyebilirsiniz.
    }
}
