using Dapper_pro.Models;

namespace Dapper_pro.Interfaces
{
    public interface IAuth
    {
        Task<Auth> GetAdminByUsername(string username);
        Task<int> RegisterAdminAsync(Auth user);
        Task<string> Admin_GenerateJwtToken(Auth user);



        Task<user_login> GetUserByUsername(string username);
        Task<int> RegisterUserAsync(user_login user);
        Task<string> User_GenerateJwtToken(user_login user);


        Task<Teacher> GetTeacherByUsername(string username);
        Task<int> RegisterTeacherAsync(Teacher user);
        Task<string> Teacher_GenerateJwtToken(Teacher user);
    }

}
