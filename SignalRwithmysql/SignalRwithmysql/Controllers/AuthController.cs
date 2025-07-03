using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRwithmysql.Interface;
using SignalRwithmysql.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SignalRwithmysql.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        public AuthController(IAuth authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginrequest)
        {
            if(loginrequest == null)
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Username and password are required."
                });
            }
            var user = await _authService.GetByUsername(loginrequest.username);
            if (user == null)
            {
                return BadRequest(new ResultModel<object>
                { 
                    Message = "Invalid Username and Password."
                });                
            }
            var token = await _authService.GenerateJwtToken(user);
            return Ok(new ResultModel<object>
            {
                Token = token,
                Message = user.username,
                Email = user.email
            });
        }

        [Authorize]
        [HttpGet("getdata")]
        public async Task<IActionResult>Getdata()
        {
            var result = await _authService.getdata();
            return Ok(result);
        }

    }
}
