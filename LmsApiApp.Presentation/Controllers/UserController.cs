using LmsApiApp.Application.Interfaces;
using LmsApiApp.Application.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LmsApiApp.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User?search=name
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers([FromQuery] string search)
        {
            var users = await _userService.GetUsersAsync(search);
            return Ok(users);
        }


    }
}
