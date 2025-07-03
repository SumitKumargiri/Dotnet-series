using Dapper_pro.Interfaces;
using Dapper_pro.Models;
using Dapper_pro.Services;
using Microsoft.AspNetCore.Mvc;
//using static Dapper_pro.Models.AddTeacherMarks;



namespace Dapper_pro.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacher _teacherService;

        public TeacherController(ITeacher teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost("getall")]
        //[Authorize]
        public async Task<IActionResult> GetAllCategory(teachersearchmodel teachersearchmodel)
        {
            var result = await _teacherService.GetAllTeacher(teachersearchmodel.pageIndex, teachersearchmodel.pageSize, teachersearchmodel.searchText);
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Insert(TeacherInter teacher)
        {
            var result = await _teacherService.Insert(teacher);
            return Ok(result);
        }
        [HttpPut("updatebyid{id}")]
        public async Task<IActionResult> Update(int id, TeacherInter teacher)
        {
            if (id != teacher.id)
            {
                return BadRequest();
            }
            var result = await _teacherService.Update(teacher);
            return Ok(result);
        }
        [HttpDelete("deletebyid{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _teacherService.Delete(id);
            return Ok(result);
        }


        /// <summary>
        /// //Teacher Attendance
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>


        [HttpPost("addteacherattendance")]
        public async Task<IActionResult> AddTeacherAttendance([FromBody] AddTeacherAttendance addteacherattendance)
        {
            var result = await _teacherService.AddTeacherAttendance(addteacherattendance);
            return Ok(result);
        }



        [HttpGet("teacherattendancesummary")]
        public async Task<IActionResult> GetTeacherAttendanceSummary(DateTime date)
        {
            try
            {
                var result = await _teacherService.GetTeacherAttendanceSummary(date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");


            }

        }

        [HttpGet("datewiseattendance{username}")]
        public async Task<ActionResult<List<TeacherAttendanceResult>>> GetTeacherAttendanceByUsernameAsync(string username)
        {
            var results = await _teacherService.GetTeacherAttendanceByUsernameAsync(username);
            if (results == null || !results.Any())
            {
                return NotFound("No attendance records found for the specified username.");
            }
            return Ok(results);
        }


        [HttpGet("coursesubjects")]
        public async Task<ActionResult<IEnumerable<TeacherCourseSubjectDto>>> GetTeacherCourseSubjects()
        {
            var data = await _teacherService.GetTeacherCourseSubjectDataAsync();
            return Ok(data);
        }


        [HttpPost("assignsubject")]
        public async Task<IActionResult> AssignSubjectToTeacher([FromBody] TeacherSubjectAssignmentDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid data.");
            }

            var result = await _teacherService.AssignSubjectToTeacher(dto);

            if (result)
            {
                return Ok(new { message = "Assignment uploaded successfully" });
            }
            else
            {
                return StatusCode(500, "An error occurred while updating the assignment.");
            }
        }


        [HttpGet("assignments")]
        public async Task<IActionResult> GetTeacherSubjectAssignments()
        {
            var assignments = await _teacherService.GetTeacherSubjectAssignments();
            return Ok(assignments);
        }



        //[HttpGet("{studentId}")]
        //public async Task<ActionResult<StudentMarksSubject>> GetStudentMarks(int studentId, int subjectId, int departmentId)
        //{
        //    var studentMarks = await _teacherService.GetStudentMarks(studentId, subjectId, departmentId);
        //    if (studentMarks == null)
        //    {
        //        return NotFound();
        //    }
        //    return studentMarks;
        //}

        [HttpPost("update")]
        public async Task<IActionResult> UpdateStudentMarks(StudentMarksSubject studentMarks)
        {
            await _teacherService.UpdateStudentMarks(studentMarks);
            return Ok();
        }

    }
}
