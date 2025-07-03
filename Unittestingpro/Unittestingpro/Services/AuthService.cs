using Dapper;
using Unittestingpro.utility;
using Unittestingpro.Interface;
using Unittestingpro.Model;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Unittestingpro.Services
{
    public class AuthService : Ilogin
    {
        private readonly DBGateway _DBGateway;
        private readonly string _jwtSecret;

        public AuthService(string connection)
        {
            _DBGateway = new DBGateway(connection);
            _jwtSecret = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9"; 
        }

        public async Task<ResultModel<object>> LoginAsync(Login logindata)
        {
            var result = new ResultModel<object>();
            try
            {
                var par = new DynamicParameters();
                par.Add("@Username", logindata.username);
                par.Add("@Password", logindata.password);

                var data = await _DBGateway.ExeScalarQuery<Login>("SELECT * FROM mp_login WHERE username = @Username AND password = @Password", par);

                if (data != null)
                {
                    var token = GenerateJwtToken(data);
                    result.Data = new { Token = token };
                    result.Success = true;
                    result.Message = "Login Successfully";
                }
                else
                {
                    result.Message = "Invalid username or password";
                }
            }
            catch (Exception ex)
            {
                result.Message = $"An error occurred during login: {ex.Message}";
                Console.WriteLine(ex.StackTrace); 
            }
            return result;
        }

        public string GenerateJwtToken(Login user)
        {
            var claim = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.username),
                new Claim(ClaimTypes.Name, user.username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claim,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
