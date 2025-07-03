using crudoperation1.Data;
using crudoperation1.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace crudoperation1.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _jwtSecret;

        public AuthService(ApplicationDbContext dbContext, string jwtSecret)
        {
            _dbContext = dbContext;
            _jwtSecret = jwtSecret;
        }

        public async Task<ResultModel<UserSession>> LoginAsync(Login login)
        {
            var user = await _dbContext.Logins.FirstOrDefaultAsync(u => u.username == login.username && u.password == login.password);

            if (user == null)
            {
                return new ResultModel<UserSession>
                {
                    Success = false,
                    Message = "Invalid username or password.",
                    Data = null
                };
            }

            var token = GenerateJwtToken(user);
            var userSession = new UserSession
            {
                Username = user.username,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(1) 
            };

            return new ResultModel<UserSession>
            {
                Success = true,
                Message = "Login successful.",
                Data = userSession
            };
        }

        private string GenerateJwtToken(Login user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
            //var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddDays(1));
              //  signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
