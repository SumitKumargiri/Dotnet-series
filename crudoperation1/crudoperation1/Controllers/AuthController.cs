using crudoperation1.Models;
using crudoperation1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace crudoperation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            if (login == null || string.IsNullOrWhiteSpace(login.username) || string.IsNullOrWhiteSpace(login.password))
            {
                return BadRequest(new ResultModel<UserSession>
                {
                    Success = false,
                    Message = "Invalid username or password.",
                    Data = null
                });
            }

            var result = await _authService.LoginAsync(login);
            if (result.Success)
            {
                return Ok(result);
            }

            return Unauthorized(result);
        }
    }
}
