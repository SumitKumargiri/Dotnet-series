using JobSeekingApplication.Model;

namespace JobSeekingApplication.Interface
{
    public interface IAuth
    {
        Task<ResultModel<Object>> LoginAsync(login logindata);
        Task<ResultModel<Object>>RegisterAsync(register registerationdata);

        Task<ResultModel<Object>> AdminLoginAsync(login logindata);
        Task<ResultModel<Object>> AdminRegisterAsync(register registerationdata);

        Task<ResultModel<Object>> JobSeekerLoginAsync(login logindata);
        Task<ResultModel<Object>> JobSeekerRegisterAsync(register registerjobseeker);



        string GenerateJwtToken(login loginData);
        Task<Auth> GetUserByEmailAsync(string email);
        Task AddUserAsync(Auth user);

    }
}
