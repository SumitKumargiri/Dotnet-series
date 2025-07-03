using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobSeekingApplication.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployee _employeeService;

        public EmployeeController(IEmployee employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("postjob")]
        public async Task<IActionResult> InsertEmployeedata(Employee employee)
        {
            var result = await _employeeService.InsertEmpdata(employee);
            return Ok(result);
        }

        [HttpGet("getjob")]
        public async Task<IActionResult> GetEmployeedata(int userid)
        {
            var result = await _employeeService.GetEmpdata(userid);
            return Ok(result);
        }

        [HttpGet("count")]
        public async Task<IActionResult> Getcountdata()
        {
            var result = await _employeeService.Getcount();
            return Ok(result);
        }

    }
}
