using Dllprocess.Interface;
using Dllprocess.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Dllprocess.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CrudController : ControllerBase
    {
        private readonly ICrud _crudService;

        public CrudController(ICrud crudService)
        {
            _crudService = crudService;
        }

        [HttpGet("getdata")]
        public async Task<IActionResult> Getdata()
        {
            try
            {
                var result = await _crudService.GetAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("insertdata")]
        public async Task<IActionResult> Insertdata(Crud crud)
        {
            var result = await _crudService.CheckInsertAsync(crud);
            return Ok(result);
        }

        [HttpPut("updatedata")]
        public async Task<IActionResult> UpdateData(Crud crud)
        {
            var result = await _crudService.UpdateAsync(crud);
            return Ok(result);
        }

        [HttpDelete("deletedata/{id}")]
        public async Task<IActionResult> DeleteData(int id)
        {
            var result = await _crudService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
