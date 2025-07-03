using Google.Apis.Auth;
using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace JobSeekingApplication.Controllers
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

        /// <summary>
        /// EmployeeLogin Process
        /// </summary>
        /// <param name="Employeelogindata"></param>
        /// <returns></returns>

        [HttpPost("employeelogin")]
        public async Task<IActionResult> Login(login logindata)
        {
            var result = await _authService.LoginAsync(logindata);
            return Ok(result);
        }

        /// <summary>
        /// Login Process
        /// </summary>
        /// <param name="Google Login"></param>
        /// <returns></returns>

        [HttpPost("googlelogin")]
        public async Task<ActionResult> GoogleLogin([FromBody] GoogleLoginDto model)
        {
            var idToken = model.IdToken;
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { "749967038180-2p2smervn8tbq46dv5tgfdvbtr8cd1dv.apps.googleusercontent.com" }
            };

            var result = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            if (result == null)
            {
                return BadRequest("Invalid Google Token");
            }

            var nameParts = result.Name.Split(' ');
            var firstName = nameParts.Length > 0 ? nameParts[0] : "";
            var lastName = nameParts.Length > 1 ? nameParts[1] : "";

            var existingUser = await _authService.GetUserByEmailAsync(result.Email);
            if (existingUser == null)
            {
                var newUser = new Auth
                {
                    username = result.Name,
                    email = result.Email,
                    firstname = firstName,
                    lastname = lastName,
                };
                await _authService.AddUserAsync(newUser);
            }
            var token = _authService.GenerateJwtToken(new login { username = result.Email });
            return Ok(new { token });
        }

        /// <summary>
        /// EmployeeRegister Process
        /// </summary>
        /// <param name="EmployeeRegister Data"></param>
        /// <returns></returns>

        [HttpPost("employeeregistration")]
        public async Task<IActionResult>Register(register registerdata)
        {
            var result = await _authService.RegisterAsync(registerdata);
            return Ok(result);
        }


        /// <summary>
        /// AdminLogin Process
        /// </summary>
        /// <param name="Admin Login"></param>
        /// <returns></returns>

        [HttpPost("adminlogin")]
        public async Task<IActionResult> adminLogin(login logindata)
        {
            var result = await _authService.AdminLoginAsync(logindata);
            return Ok(result);
        }
        /// <summary>
        /// AdminRegister Process
        /// </summary>
        /// <param name="AdminRegister Data"></param>
        /// <returns></returns>

        [HttpPost("adminregistration")]
        public async Task<IActionResult> AdminRegister(register registerdata)
        {
            var result = await _authService.AdminRegisterAsync(registerdata);
            return Ok(result);
        }

        /// <summary>
        /// JobSeekereLogin Process
        /// </summary>
        /// <param name="JobSeeker Login"></param>
        /// <returns></returns>

        [HttpPost("jobseekerlogin")]
        public async Task<IActionResult> JobSeekerLogin(login logindata)
        {
            var result = await _authService.JobSeekerLoginAsync(logindata);
            return Ok(result);
        }

        /// <summary>
        /// JobSeekereRegister Process
        /// </summary>
        /// <param name="JobSeeker Register"></param>
        /// <returns></returns>
        
        [HttpPost("jobseekerregister")]
        public async Task<IActionResult>JobSeekerRegister(register registerjobseeker)
        {
            var result = await _authService.JobSeekerRegisterAsync(registerjobseeker);
            return Ok(result);
        }
    }

}
