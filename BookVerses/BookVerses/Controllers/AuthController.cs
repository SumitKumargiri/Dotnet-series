using BookVerses.Interface;
using BookVerses.Model;
using Microsoft.AspNetCore.Mvc;
namespace BookVerses.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller 
    {
        private readonly IAuth _authService;
        public AuthController(IAuth authService) 
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult>Login(login logindata)
        {
            var result = await _authService.LoginAsync(logindata);
            return Ok(result);  
        }
    }

}
