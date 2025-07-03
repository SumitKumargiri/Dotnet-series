using Dapper_pro.Models;
//using static Dapper_pro.Models.AddTeacherMarks;

namespace Dapper_pro.Interfaces
{
    public interface ITeacher
    {
        Task<ResultModel<object>> GetAllTeacher(int pageIndex, int pageSize, string searchValue);
        Task<ResultModel<object>> Insert(TeacherInter teacher);
        Task<ResultModel<object>> Update(TeacherInter teacher);
        Task<ResultModel<object>> Delete(int id);

        Task<ResultModel<object>> AddTeacherAttendance(AddTeacherAttendance addteacherattendance);
        Task<TeacherAttendanceSummary> GetTeacherAttendanceSummary(DateTime date);
        Task<List<TeacherAttendanceResult>> GetTeacherAttendanceByUsernameAsync(string username);

        Task<IEnumerable<TeacherCourseSubjectDto>> GetTeacherCourseSubjectDataAsync();

        Task<bool> AssignSubjectToTeacher(TeacherSubjectAssignmentDto dto);

        Task<List<TeacherSubjectAssignment>> GetTeacherSubjectAssignments();



        //Task<StudentMarksSubject> GetStudentMarks(int studentId, int subjectId, int departmentId);
        Task UpdateStudentMarks(StudentMarksSubject studentMarks);

    }
}
