using Microsoft.AspNetCore.Identity;

namespace crudoperation1.Models
{
    public class User : IdentityUser
    {
    }

    public class Login
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }

    public class UserSession
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }

    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResultModel()
        {
            Success = true;
            Message = "Success";
        }
    }

}
