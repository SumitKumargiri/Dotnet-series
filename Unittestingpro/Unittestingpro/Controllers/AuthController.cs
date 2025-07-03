using Microsoft.AspNetCore.Mvc;
using Unittestingpro.Interface;
using Unittestingpro.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Unittestingpro.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly Ilogin _lgoinService;
        public AuthController(Ilogin lgoinService)
        {
            _lgoinService = lgoinService;
        }

        [HttpPost("login")]
        public async Task<IActionResult>LoginAuthentication(Login logindata)
        {
            var result = await _lgoinService.LoginAsync(logindata);
            return Ok(result);
        }
    }
}
