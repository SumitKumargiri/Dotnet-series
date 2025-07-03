
using Dapper_pro.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper_pro.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dapper_pro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IRepoBase _repoBase;

        public HomeController(IRepoBase repoBase)
        {
            _repoBase = repoBase;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var students = await Task.Run(() => _repoBase.GetAll());
                return Ok(students);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var student = _repoBase.GetById(id);
                if (student == null)
                {
                    return NotFound();
                }
                else {
                    return Ok(student);
                }
                
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent([FromBody] Student _student)
        {
            try
            {
                await _repoBase.AddStudent(_student);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student _student)
        {
            try
            {
                _student.id = id; // Ensure the ID matches the route parameter
                await _repoBase.UpdateStudent(_student);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                await _repoBase.DeleteStudent(id);
                return Ok();
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, ex.Message);
            }
        }

    }
}
