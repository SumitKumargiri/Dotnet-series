
using Dapper;
using Dapper_pro.Models;
using Dapper_pro.Interfaces;
using Dapper_pro.Utility;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Dapper_pro.Services
{
    public class AuthService : IAuth
    {
        private readonly IConfiguration _config;
        private string _connectionString;

        public AuthService(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("ConnectionString1");
        }


        public async Task<Auth> GetAdminByUsername(string username)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT mp.username, mp.password, mp.email, ms.profileImage FROM mp_login mp JOIN md_admin ms ON mp.username = ms.username
            WHERE mp.username = @Username AND mp.type = @Type";

                return await db.QueryFirstOrDefaultAsync<Auth>(query, new { Username = username, Type = Constants.adminrole });
            }
        }


        public async Task<int> RegisterAdminAsync(Auth user)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                try
                {
                    string query = @"CALL sp_admin_register(@firstname, @lastname, @username, @email, @Password, @status, @Type);";
                    var parameters = new
                    {
                        firstname = user.firstname,
                        lastname = user.lastname,
                        username = user.username,
                        email = user.email,
                        Password = user.Password,
                        Status = user.status,
                        Type = user.Type
                    };

                    Console.WriteLine($"Executing SQL: {query} with parameters: {JsonConvert.SerializeObject(parameters)}");
                    return await db.ExecuteScalarAsync<int>(query, parameters);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"MySQL Error: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error: {ex.Message}");
                    throw;
                }
            }
        }



        public async Task<string> Admin_GenerateJwtToken(Auth user)
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("role", Constants.adminrole.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        /// <summary>
        /// //Student login Service
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns> 
        /// 


        public async Task<user_login> GetUserByUsername(string username)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"SELECT mp.username, mp.password, mp.email, ms.profileImage FROM mp_login mp JOIN md_students ms ON mp.username = ms.username
            WHERE mp.username = @Username AND mp.type = @Type";

                return await db.QueryFirstOrDefaultAsync<user_login>(query, new { Username = username, Type = Constants.studentrole });
            }
        }



        public async Task<int> RegisterUserAsync(user_login user)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                try
                {
                    string query = @"CALL sp_register(@firstname, @lastname, @username, @email, @Password, @status, @Type);";
                    var parameters = new
                    {
                        firstname = user.firstname,
                        lastname = user.lastname,
                        username = user.username,
                        email = user.email,
                        Password = user.Password,
                        Status = user.status,
                        Type = user.Type
                    };

                    Console.WriteLine($"Executing SQL: {query} with parameters: {JsonConvert.SerializeObject(parameters)}");
                    return await db.ExecuteScalarAsync<int>(query, parameters);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"MySQL Error: {ex.Message}");
                    throw;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"General Error: {ex.Message}");
                    throw;
                }
            }
        }
    

    public async Task<string> User_GenerateJwtToken(user_login user)
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("role", Constants.studentrole.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// //Teacher login Service
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns> 
        /// 

        public async Task<Teacher> GetTeacherByUsername(string username)
        {
            using IDbConnection db = new MySqlConnection(_connectionString);
            string query = "SELECT * FROM mp_login WHERE username=@Username AND type=@Type";
            return await db.QueryFirstOrDefaultAsync<Teacher>(query, new { Username = username, Type = Constants.teacherrole });
        }


        public async Task<int> RegisterTeacherAsync(Teacher user)
        {
            using (IDbConnection db = new MySqlConnection(_connectionString))
            {
                string query = @"INSERT INTO mp_login (username, Password, email, status, type)
                                 VALUES (@Username, @Password, @Email, @Status, @Type);
                                 SELECT LAST_INSERT_ID();";
                return await db.ExecuteScalarAsync<int>(query, user);
            }
        }


        public async Task<string> Teacher_GenerateJwtToken(Teacher user)
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.username),
                    new Claim("role", Constants.teacherrole.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
