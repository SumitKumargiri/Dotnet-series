namespace SignalRwithmysql.Model
{
    public class Auth
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public int mobilenumber { get; set; }
        public string password { get; set; }
    }
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
    }

    public class CaptchaRequest
    {
        public string Token { get; set; }
    }
}
