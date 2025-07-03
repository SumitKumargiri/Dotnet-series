//using BookVerses.Interface;
//using BookVerses.Model;
//using BookVerses.utility;
//using Dapper;
//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.IdentityModel.Tokens;
//using Newtonsoft.Json;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace BookVerses.Services
//{
//    public class AuthService : IAuth
//    {
//        private readonly DBGateway _DBGateway;
//        private readonly IDistributedCache _distributedCache;
//        private readonly IConfiguration _configuration;

//        public AuthService(string connection, IDistributedCache distributedCache, IConfiguration configuration)
//        {
//            _DBGateway = new DBGateway(connection);
//            _distributedCache = distributedCache;
//            _configuration = configuration;
//        }

//        public async Task<ResultModel<object>> LoginAsync(login logindata)
//        {
//            ResultModel<object> result = new ResultModel<object>();
//            try
//            {
//                var query = "SELECT * FROM mp_login WHERE username=@username AND password=@password";
//                var parameters = new DynamicParameters();
//                parameters.Add("@username", logindata.username);
//                parameters.Add("@password", logindata.password);

//                var user = (await _DBGateway.ExeQueryList<Auth>(query, parameters)).FirstOrDefault();

//                if (user != null)
//                {
//                    await LogAudit("login", user.username, user);

//                    var token = GenerateJwtToken(user); // Generate JWT token
//                    var session = new UserSession
//                    {
//                        Username = logindata.username,
//                        Token = token,
//                        ExpiresAt = DateTime.UtcNow.AddMinutes(15) // Token expiration
//                    };

//                    var sessionData = JsonConvert.SerializeObject(session);
//                    var redisOptions = new DistributedCacheEntryOptions
//                    {
//                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
//                    };

//                    await _distributedCache.SetStringAsync(logindata.username, sessionData, redisOptions);

//                    result.Data = session.Token;
//                    result.Success = true;
//                    result.Message = "Login successful.";
//                }
//                else
//                {
//                    result.Success = false;
//                    result.Message = "Invalid username or password.";
//                }
//            }
//            catch (Exception ex)
//            {
//                result.Success = false;
//                result.Message = $"Error: {ex.Message}";
//            }
//            return result;
//        }

//        public string GenerateJwtToken(Auth user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]); // Secret key from configuration

//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.username),
//                new Claim(JwtRegisteredClaimNames.Email, user.email),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
//            };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.UtcNow.AddMinutes(15),
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//                Issuer = _configuration["Jwt:Issuer"],
//                Audience = _configuration["Jwt:Audience"]
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }

//        public async Task LogAudit(string action, string username, Auth model)
//        {
//            var query = "INSERT INTO Audit (Action, username, TableName, ActionTimestamp, ActionData) " +
//                        "VALUES (@Action, @username, @TableName, @ActionTimestamp, @ActionData)";

//            var actionData = $"Username: {model.username}, Email: {model.email}";
//            var parameters = new DynamicParameters();
//            parameters.Add("@Action", action);
//            parameters.Add("@username", username);
//            parameters.Add("@TableName", "login");
//            parameters.Add("@ActionTimestamp", DateTime.UtcNow);
//            parameters.Add("@ActionData", actionData);

//            await _DBGateway.ExecuteAsync(query, parameters);
//        }
//    }
//}








using BookVerses.Interface;
using BookVerses.Model;
using BookVerses.utility;
using Dapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookVerses.Services
{
    public class AuthService : IAuth
    {
        private readonly DBGateway _DBGateway;
        private readonly IConfiguration _configuration;

        public AuthService(string connection, IConfiguration configuration)
        {
            _DBGateway = new DBGateway(connection);
            _configuration = configuration;
        }

        public async Task<ResultModel<object>> LoginAsync(login logindata)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var query = "SELECT * FROM mp_login WHERE username=@username AND password=@password";
                var parameters = new DynamicParameters();
                parameters.Add("@username", logindata.username);
                parameters.Add("@password", logindata.password);

                var user = (await _DBGateway.ExeQueryList<Auth>(query, parameters)).FirstOrDefault();

                if (user != null)
                {
                    var token = GenerateJwtToken(user); // Generate JWT token
                    result.Data = token;
                    result.Success = true;
                    result.Message = "Login successful.";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Invalid username or password.";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }
            return result;
        }

        public string GenerateJwtToken(Auth user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]); // Secret key from configuration

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token ID
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}

