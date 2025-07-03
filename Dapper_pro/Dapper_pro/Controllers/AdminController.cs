using Dapper_pro.Interfaces;
using Dapper_pro.Models;
using Dapper_pro.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Dapper_pro.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _adminService;

        public AdminController(IAdmin adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard/chartgraph")]
        public async Task<ActionResult<List<Admin>>> GetDistrictCounts()
        {
            var districtCounts = await _adminService.GetDistrictCountsAsync();
            return Ok(districtCounts);
        }

        [HttpPost("getalladmindata")]
        //[Authorize]
        public async Task<IActionResult> GetAllAdminData(searchmodel2 searchmodel)
        {
            var result = await _adminService.GetAllAdminData(searchmodel.pageIndex, searchmodel.pageSize, searchmodel.searchText);
            return Ok(result);
        }

        /// <summary>
        /// //Course Operation
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpGet("getallcourses")]
        public async Task<ActionResult<object>> GetCourses()
        {
            var courses = await _adminService.GetCoursesAsync();
            return Ok(courses);
        }


        [HttpGet("getsubject")]
        public async Task<IActionResult> AddCourse()
        {
            var result = await _adminService.getsubject();
            return Ok(result);
        }


        [HttpPut("updateCourse/{id}")]
        public async Task<IActionResult> UpdateCourse([FromBody] Updatecourse course)
        {
            var result = await _adminService.UpdateCourseAsync(course);
            return Ok(result);
        }

        [HttpPost("coursesubjectadd")]
        public async Task<IActionResult> AddCourse([FromBody] Course course)
        {
            if (course == null)
            {
                return BadRequest(new ResultModel<object> { Message = "Invalid course data" });
            }

            var result = await _adminService.InsertCourse(course);
            return Ok(result);
        }


        /// <summary>
        /// // Subject Operation
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("subjectadd")]
        public async Task<IActionResult> AddSubject(Subject subject)
        {
            var result = await _adminService.InsertSubject(subject);
            return Ok(result);
        }


        /// <summary>
        /// //Upload profile image
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>



        [HttpPost("uploadProfileImage")]
        public async Task<IActionResult> UpdateProfileImageRequest([FromBody] adminProfileImageUpdateRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.profileImage))
            {
                return BadRequest("Invalid data.");
            }

            var result = await _adminService.adminupdateprofileimage(request.username, request.profileImage);
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
        public async Task<IActionResult> UpdateProfiledetails([FromBody] adminProfileDetailUpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                bool result = await _adminService.adminupdateprofiledetail(request.username, request.email, request.profileImage);

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
        /// //Admin Reset Password
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>


        [HttpPost("Resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultModel<string> { Message = "Invalid data", Model = null });

            var result = await _adminService.UpdatePasswordAsync(model.Username, model.OldPassword, model.NewPassword);
            if (result.Success)
                return Ok(result);
            else
                return StatusCode(500, result);
        }

    }
}








       
