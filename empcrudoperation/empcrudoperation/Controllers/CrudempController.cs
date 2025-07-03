using empcrudoperation.Interface;
using empcrudoperation.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace empcrudoperation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CrudempController : ControllerBase
    {
        private readonly ICrudemp _crudempService;
        public CrudempController(ICrudemp crudempService)
        {
            _crudempService = crudempService;
        }

        [HttpGet("getempdata")]
        public async Task<IActionResult> getempdata()
        {
            var result = await _crudempService.Getcrud();
            return Ok(result);
        }
        [HttpGet("getempdata/{empid}")]
        public async Task<IActionResult> GetEmpById(int empid)
        {
            var result = await _crudempService.GetEmployeeById(empid);
            return Ok(result);
        }

        [HttpPost("insertempdata")]
        public async Task<IActionResult> Insertempdata([FromBody]Crudemp model)
        {
            var result = await _crudempService.InsertCrud(model);
            return Ok(result);
        }

        [HttpPut("updateempdata")]
        public async Task<IActionResult> Updateempdata([FromBody] Crudemp model)
        {
            var result = await _crudempService.UpdateCrud(model);
            return Ok(result);
        }

        [HttpDelete("deleteempdata/{empid}")]
        public async Task<IActionResult> Deleteempdata(int empid)
        {
            var result = await _crudempService.DeleteCrud(empid);
            return Ok(result);
        }


        /// <summary>
        /// Attendance data operation
        /// </summary>
        /// <returns></returns>
        

        [HttpGet("getattendancedata")]
        public async Task<IActionResult> Attendancedata()
        {
            var result = await _crudempService.getattendancedata();
            return Ok(result);
        }

        [HttpPut("updateattendancedata")]
        public async Task<IActionResult> UpdateAttendancedata(attendance model, string firstname, string lastname)
        {
            var result = await _crudempService.UpdateAttendance(model, firstname, lastname);
            return Ok(result);
        }
        [HttpGet("getemployeeattendancedata/{empid}")]
        public async Task<IActionResult>GetEmployeeattendancedata(int empid)
        {
            var result = await _crudempService.GetEmpAttendanceData(empid);
            return Ok(result);
        }

    }
}
