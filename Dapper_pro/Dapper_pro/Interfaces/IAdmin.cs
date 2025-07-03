
using Dapper_pro.Models;
using System.Threading.Tasks;

namespace Dapper_pro.Interfaces
{
    public interface IAdmin
    {
        Task<ResultModel<List<Admin>>> GetDistrictCountsAsync();
        Task<ResultModel<object>> GetAllAdminData(int pageIndex, int pageSize, string searchValue);

        Task<bool> adminupdateprofileimage(string username, string profileImage);
        Task<bool> adminupdateprofiledetail(string username, string email, string profileImage);
        Task<ResultModel<string>> UpdatePasswordAsync(string username, string oldPassword, string newPassword);
        Task<ResultModel<object>> getsubject();
        Task<ResultModel<object>> InsertCourse(Course course);
        Task<ResultModel<object>> GetCoursesAsync();
        Task<ResultModel<object>> UpdateCourseAsync(Updatecourse course);
        Task<ResultModel<object>> InsertSubject(Subject subject);

    }
}
