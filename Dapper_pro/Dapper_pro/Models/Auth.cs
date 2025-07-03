namespace Dapper_pro.Models
{
    public class Auth
    {
        public string firstname {  get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string Password { get; set; }
        public string status { get; set; }
        public int Type { get; set; }
        public string profileImage { get; set; }
    }


    public class user_login
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public int Type { get; set; }
        public string profileImage { get; set; }

    }


    public class Teacher
    {
        public string username { get; set; }
        public string email { get; set; }
        public string Password { get; set; }
        public string status { get; set; }
        public int Type { get; set; }
        public string profileImage { get; set; }
    }



    public class CaptchaRequest
    {
        public string Token { get; set; }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }

    public class UserLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }

    public class TeacherLoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
    }


    public class RegisterRequest
    {
        public string firstname {  get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
    }

    public class UserRegisterRequest
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
        //public int Type { get; set; }
    }

    public class TeacherRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string email { get; set; }
    }


    public class CaptchaVerificationResponse
    {
        public bool success { get; set; }
    }

}
