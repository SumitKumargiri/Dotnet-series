using Dapper_pro.Models;

namespace Dapper_pro.Interfaces
{
    public interface IStudent  
    {

        Task<ResultModel<object>> GetById(int id);
        Task<ResultModel<object>> GetAllCategory(int pageIndex, int pageSize, string searchValue);
        Task<ResultModel<object>> Insert(Student student);
        Task<ResultModel<object>> Update(Student student);
        Task<ResultModel<object>> Delete(int Id);

        Task<bool> UpdateProfileImage(string username, string profileImage);
        Task<bool> UpdateProfileDetail(string username, string email, string profileImage);
        Task<ResultModel<string>> StdUpdatePasswordAsync(string username, string oldPassword, string newPassword);
        Task<ResultModel<object>> AddAttendanceAsync(Attendance attendance);
        Task<AttendanceSummary> GetStudentAttendanceSummary(DateTime date);
        Task<List<AttendanceResult>> GetAttendanceByUsernameAsync(string username);

        Task<List<StudentSubjectMarks>> GetStudentSubjectMarks(string username);
        Task<List<StudentDetails>> GetStudentDetails();

    }
}
