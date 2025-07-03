namespace JobSeekingApplication.Model
{
    public class Auth
    {
        public int id {  get; set; }
        public string firstname {  get; set; }
        public string lastname { get; set; }
        public string username {  get; set; }
        public string email { get;set; }
        public string password { get; set; }    
        public int role {  get; set; }
    }

    public class login
    {
        public string username { get; set; }
        public string password { get; set; }
    }


    public class register
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        //public int role { get; set; } = Constants.emprole;
        public int mobilenumber {  get; set; }
    }
    public class GoogleLoginDto
    {
        public string IdToken { get; set; }
    }

    public class UserSession
    {
        public string Username { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }


    public class Audit
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public DateTime ActionTimestamp { get; set; }
        public string ActionData { get; set; }
        public int username { get; set; }
    }

}
