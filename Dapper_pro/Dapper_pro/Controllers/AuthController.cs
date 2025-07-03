
using Dapper_pro.Interfaces;
using Dapper_pro.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Dapper_pro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth _authService;
        private readonly HttpClient _httpClient;

        public AuthController(IAuth authService, HttpClient httpClient)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }



        /// <summary>
        /// //Admin login
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>



        [HttpPost("adminlogin")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Username and password are required."
                });
            }

            var user = await _authService.GetAdminByUsername(request.Username);

            if (user == null || user.Password != request.Password)
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Invalid username or password."
                });
            }

            var token = await _authService.Admin_GenerateJwtToken(user);

            return Ok(new ResultModel<object>
            {
                Token = token,
                Message = user.username,
                ProfileImage = user.profileImage,
                Email = user.email,
            });
        }





        private async Task<IActionResult> VerifyCaptcha(CaptchaRequest request)
        {
            var response = await _httpClient.GetAsync($"https://www.google.com/recaptcha/api/siteverify?secret=6LdFs7gpAAAAABWnm08ezWXcXjZGU7iXtLKxBG7_&response={request.Token}");
            var responseBody = await response.Content.ReadAsStringAsync();

            var verificationResult = JsonConvert.DeserializeObject<CaptchaVerificationResponse>(responseBody);

            if (verificationResult.success)
            {
                return Ok(new { success = true });
            }
            else
            {
                return BadRequest(new { success = false });
            }
        }


        [HttpPost("adminregister")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Request data is invalid." });
            }

            var existingUser = await _authService.GetAdminByUsername(request.username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var newUser = new Auth
            {
                firstname = request.firstname,
                lastname = request.lastname,
                username = request.username,
                Password = request.Password,
                email = request.email,
                status = "Active",
                Type = Constants.adminrole
            };

            var userId = await _authService.RegisterAdminAsync(newUser);
            var user = await _authService.GetAdminByUsername(request.username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve the newly registered Admin.");
            }

            var token = await _authService.Admin_GenerateJwtToken(user);

            return Ok(new { Token = token, UserId = userId, Message = "Admin registered successfully." });
        }




        //++++++++++++++++++++++++++  User Login Process ++++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// //Student login
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>  



        [HttpPost("userlogin")]
        public async Task<IActionResult> UserLogin([FromBody] UserLoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Username and password are required."
                });
            }

            var user = await _authService.GetUserByUsername(request.Username);

            if (user == null || user.Password != request.Password)
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Invalid username or password."
                });
            }

            var token = await _authService.User_GenerateJwtToken(user);

            return Ok(new ResultModel<object>
            {
                Token = token,
                Message = user.username,
                ProfileImage = user.profileImage, 
                Email = user.email,
            });
        }



        [HttpPost("userregister")]
        public async Task<IActionResult> UserRegister([FromBody] UserRegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Request data is invalid." });
            }

            var existingUser = await _authService.GetUserByUsername(request.username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var newUser = new user_login
            {
                firstname= request.firstname,
                lastname= request.lastname,
                username = request.username,
                Password = request.Password,
                email = request.email,
                status = "Active",
                Type = Constants.studentrole
            };

            var userId = await _authService.RegisterUserAsync(newUser);
            var user = await _authService.GetUserByUsername(request.username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve the newly registered user.");
            }

            var token = await _authService.User_GenerateJwtToken(user);

            return Ok(new { Token = token, UserId = userId, Message = "User registered successfully." });
        }



        //++++++++++++++++++++++++++  Teacher Login Process ++++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// //Teacher login
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns> 
        /// 
        [HttpPost("teacherlogin")]
        public async Task<IActionResult> TeacherLogin([FromBody] TeacherLoginRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Username and password are required."
                });
            }

            var user = await _authService.GetTeacherByUsername(request.Username);

            if (user == null || user.Password != request.Password)
            {
                return BadRequest(new ResultModel<object>
                {
                    Message = "Invalid username or password."
                });
            }

            var token = await _authService.Teacher_GenerateJwtToken(user);

            return Ok(new ResultModel<object>
            {
                Token = token,
                Message = user.username,
                ProfileImage = user.profileImage,
                Email = user.email,
            });
        }

        [HttpPost("teacherregister")]
        public async Task<IActionResult> TeacherRegister([FromBody] TeacherRegisterRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Request data is invalid." });
            }

            var existingUser = await _authService.GetUserByUsername(request.Username);
            if (existingUser != null)
            {
                return BadRequest(new { message = "Username already exists." });
            }

            var newUser = new Teacher
            {
                username = request.Username,
                Password = request.Password,
                email = request.email,
                status = "Active",
                Type = Constants.teacherrole
            };

            var userId = await _authService.RegisterTeacherAsync(newUser);
            var user = await _authService.GetTeacherByUsername(request.Username);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve the newly registered user.");
            }

            var token = await _authService.Teacher_GenerateJwtToken(user);

            return Ok(new { Token = token, UserId = userId, Message = "User registered successfully." });
        }


    }
}

