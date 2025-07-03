using crudoperation.utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cms;
using SignalRwithmysql.Interface;
using SignalRwithmysql.Model;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalRwithmysql.Services
{
    public class AuthService : IAuth
    {
        private readonly IConfiguration _config;
        private string _connectionString;
        private readonly IDistributedCache _distributedCache;
        private readonly DBGateway _dBGateway;

        public AuthService(IConfiguration config,IDistributedCache distributedCache,DBGateway dBGateway)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("ConnectionString1");
            _distributedCache = distributedCache;
            _dBGateway = dBGateway;
        }


        public async Task<Auth> GetByUsername(string username)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"select username,password, email from mp_login where username = @username";
                return await db.QueryFirstOrDefaultAsync<Auth>(query, new { username = username });
            }
        }



        public async Task<string> GenerateJwtToken(Auth user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT configuration values are missing.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.username),
                new Claim(ClaimTypes.Email, user.email),
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwttoken = tokenHandler.WriteToken(token);

            var redischache = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            await _distributedCache.SetStringAsync(user.username, jwttoken, redischache);
            return jwttoken;

        }

        /// <summary>
        /// getdata after Authentication.
        /// </summary>
        /// <returns></returns>

        public async Task<ResultModel<object>>getdata()
        {
            ResultModel<object>result = new ResultModel<object>();
            try
            {
                var data = await _dBGateway.ExeQueryList<Auth>("select * from mp_login");
                if (data!= null)
                {
                    result.Message = "data received successfully";
                    result.Data = data;
                }
                else
                {
                    result.Message = "Data received error";
                }
            }catch (Exception ex)
            {
                result.Message = $"Error: {ex.Message}";
            }
            return result;
        }

    }
}
