using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Unittestingpro.Interface;
using Unittestingpro.Model;

namespace Unittestingpro.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly Ihome _homeService;
        public HomeController(Ihome homeService)
        {
            _homeService = homeService;
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> Getdata()
        {
            var result = await _homeService.GetAsync();
            return Ok(result);
        }

        [HttpPost("Insertdata")]
        public async Task<IActionResult>InsertData(home homedata)
        {
            var result = await _homeService.InsertdataAsync(homedata);
            return Ok(result);
        }

        [HttpPut("Updatedata{id}")]
        public async Task<IActionResult>UpdateData(home homedata)
        {
            var result = await _homeService.UpdatedataAsync(homedata);
            return Ok(result);
        }

        [HttpDelete("Deletedata{id}")]
        public async Task<IActionResult>DeleteData(int id)
        {
            var result = await _homeService.DeletedataAsync(id);
            return Ok(result);
        }

        [HttpGet("getdata{name}")]
        public async Task<IActionResult>Getdataname(string name)
        {
            var result = await _homeService.GetDatabyname(name);
            return Ok(result);
        }
      
        
    }
}
