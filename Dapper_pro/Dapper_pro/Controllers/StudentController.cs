using Microsoft.AspNetCore.Mvc;
using Dapper_pro.Models;
using Dapper_pro.Services;
using System.Threading.Tasks;
using Dapper_pro.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Dapper_pro.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;

namespace Dapper_pro.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudent _studentService;

        public StudentController(IStudent studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _studentService.GetById(id);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Insert(Student student)
        {
            var result = await _studentService.Insert(student);
            return Ok(result);
        }

        [HttpPost("getall")]
        //[Authorize]
        public async Task<IActionResult> GetAllCategory(searchmodel searchmodel)
        {
            var result = await _studentService.GetAllCategory(searchmodel.pageIndex, searchmodel.pageSize, searchmodel.searchText);
            return Ok(result);
        }

        [HttpPut("updatebyid{id}")]
        public async Task<IActionResult> Update(int id, Student student)
        {
            if (id != student.id)
            {
                return BadRequest();
            }
            var result = await _studentService.Update(student);
            return Ok(result);
        }
        [HttpDelete("deletebyid{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _studentService.Delete(id);
            return Ok(result);
        }

        /// <summary>
        /// //Upload profile image
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>


        [HttpPost("uploadProfileImage")]
        public async Task<IActionResult> UpdateProfileImageRequest([FromBody] ProfileImageUpdateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.profileImage))
            {
                return BadRequest("Invalid data.");
            }

            var result = await _studentService.UpdateProfileImage(request.username, request.profileImage);
            if (result)
            {
                return Ok(new { message = "Profile image updated successfully." });
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the profile image.");
            }
        }


        [HttpPost("updateProfiledetails")]
        public async Task<IActionResult> UpdateProfiledetails([FromBody] ProfileDetailUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                bool result = await _studentService.UpdateProfileDetail(request.username, request.email, request.profileImage);
                if (result)
                {
                    return Ok(new { message = "Profile updated successfully" });
                }
                else
                {
                    return StatusCode(500, "An error occurred while updating the profile.");
                }
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// //Student Reset Password
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("Resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<string> { Message = "Invalid data", Model = null });

            var result = await _studentService.StdUpdatePasswordAsync(model.Username, model.OldPassword, model.NewPassword);
            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result);
        }




        /// <summary>
        /// //Student Attendance
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// 
        [HttpPost("addstudentattendance")]
        public async Task<IActionResult> AddAttendanceAsync(Attendance attendance)
        {
            var result = await _studentService.AddAttendanceAsync(attendance);
            return Ok(result);
        }



        [HttpGet("studentsattendancesummary")]
        public async Task<IActionResult> GetStudentAttendanceSummary(DateTime date)
        {
            try
            {
                var result = await _studentService.GetStudentAttendanceSummary(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");


            }

        }

        [HttpGet("datewiseattendance{username}")]
        public async Task<ActionResult<List<AttendanceResult>>> GetAttendanceByUsername(string username)
        {
            var results = await _studentService.GetAttendanceByUsernameAsync(username);
            if (results == null || !results.Any())
            {
                return NotFound("No attendance records found for the specified username.");
            }
            return Ok(results);
        }


        [HttpGet("marks{username}")]
        public async Task<ActionResult<List<StudentSubjectMarks>>> GetStudentSubjectMarks(string username)
        {
            var marks = await _studentService.GetStudentSubjectMarks(username);
            if (marks == null)
            {
                return NotFound();
            }
            return Ok(marks);
        }


        [HttpGet("details")]
        public async Task<ActionResult<List<StudentDetails>>> GetStudentDetails()
        {
            var studentDetails = await _studentService.GetStudentDetails();
            return Ok(studentDetails);
        }
    }

}

