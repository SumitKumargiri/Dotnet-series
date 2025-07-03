using GrpcWebApiClient.Services;
using GrpcWebApiClient;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcWebApiClient.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly GrpcUserService _grpcUserService;
        public UsersController(GrpcUserService grpcUserService)
        {
            _grpcUserService = grpcUserService;
        }

        [HttpGet("userdata")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _grpcUserService.GetUsersAsync();
            return Ok(users);
        }
    }
}

