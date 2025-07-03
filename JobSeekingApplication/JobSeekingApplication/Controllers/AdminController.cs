using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _adminService;
        public AdminController(IAdmin adminService)
        {
            _adminService = adminService;
        }

        /// <summary>
        /// Employee Data
        /// </summary>
        /// <returns></returns>

        [HttpGet("allemployeedata")]
        public async Task<IActionResult> getemployeedata()
        {
            var result = await _adminService.GetEmployeeAsync();
            return Ok(result);
        }

        /// <summary>
        /// State_District Data
        /// </summary>
        /// <returns></returns>

        [HttpGet("allStateDistrict")]
        public async Task<IActionResult> GetAlldata()
        {
            var result = await _adminService.GetAllstateDistrictdata();
            return Ok(result);
        }

        [HttpGet("getdistrictbystatename")]
        public async Task<IActionResult> getdistrict([FromQuery] string StateName)
        {
            var result = await _adminService.GetDistrictdata(StateName);
            return Ok(result);
        }

    }
}
