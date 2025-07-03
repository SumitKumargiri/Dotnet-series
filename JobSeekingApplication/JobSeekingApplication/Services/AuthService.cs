using Dapper;
using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using JobSeekingApplication.utility;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using MySqlX.XDevAPI.Common;
using StackExchange.Redis;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobSeekingApplication.Services
{
    public class AuthService : IAuth
    {
        private readonly DBGateway _DBGateway;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _distributedCache;

        public AuthService(string connection, IConfiguration configuration, IDistributedCache distributedCache)
        {
            _DBGateway = new DBGateway(connection);
            _configuration = configuration;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// EmployeeLogin Process
        /// </summary>
        /// <param name="EmployeeloginData"></param>
        /// <returns></returns>
        public async Task<ResultModel<object>> LoginAsync(login loginData)
        {
            var result = new ResultModel<object>();
            try
            {
                string query = "SELECT * FROM mp_login WHERE (username = @usernameOrEmail OR email = @usernameOrEmail) AND password = @password";
                var parameters = new DynamicParameters();
                parameters.Add("@usernameOrEmail", loginData.username);
                parameters.Add("@password", loginData.password);

                var user = (await _DBGateway.ExeQueryList<Auth>(query, parameters)).FirstOrDefault();

                if (user != null && user.role == Constants.emprole)
                {

                    if (user.role != Constants.emprole) 
                    {
                        result.Success = false;
                        result.Message = "You do not have the required role to log in.";
                        return result;  
                    }
                    //+++++++++++++++++++++ Audit Log ++++++++++++++++++++++++++++++
                    await LogAudit("Authentication", user.username, user);
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    string token = GenerateJwtToken(loginData);

                    //+++++++++++++++++ Cache the JWT token in Redis for 30 minutes ++++++++++++++++++++++
                    var tokenCacheKey = $"{loginData.username}_token";
                    await _distributedCache.SetStringAsync(tokenCacheKey, token, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    });
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    result.Token = token;
                    result.UserId = user.id;
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


        // +++++++++++++++++++++++++++++++++++ JWT TOKEN +++++++++++++++++++++++++++++++++++++++
        public string GenerateJwtToken(login loginData)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            var claims = new[]
            {
                new Claim("username", loginData.username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
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

        // ++++++++++++++++++++ check token are available in chache +++++++++++++++++++++++++++++++++++++
        public async Task<string> GetTokenFromCacheAsync(string username)
        {
            var tokenCacheKey = $"{username}_token";
            var token = await _distributedCache.GetStringAsync(tokenCacheKey);
            return token;
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public async Task LogAudit(string action, string username, Auth model)
        {
            var query = "INSERT INTO mp_audit (Action, username, TableName, ActionTimestamp, ActionData) " +
                        "VALUES (@Action, @username, @TableName, @ActionTimestamp, @ActionData)";

            var actionData = $"Username: {model.username}, Email: {model.email}";
            var parameters = new DynamicParameters();
            parameters.Add("@Action", action);
            parameters.Add("@username", username);
            parameters.Add("@TableName", "mp_login");
            parameters.Add("@ActionTimestamp", DateTime.UtcNow);
            parameters.Add("@ActionData", actionData);

            await _DBGateway.ExecuteAsync(query, parameters);
        }


       // ++++++++++++++++++++++++  Signin Google data pass in database +++++++++++++++++++++++++++++++++++

        public async Task<Auth> GetUserByEmailAsync(string email)
        {
            string query = "SELECT * FROM mp_login WHERE email = @Email";
            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            var user = (await _DBGateway.ExeQueryList<Auth>(query, parameters)).FirstOrDefault();
            if (user != null)
            {
                await LogAudit("GetUserByEmail", user.username, user);
            }
            return user;
        }

        public async Task AddUserAsync(Auth user)
        {
            if (user.role != Constants.emprole)  
            {
                var Message = "You do not have the required role to log in.";  
            }
            string query = @"INSERT INTO mp_login (username, email, firstname, lastname) VALUES (@Username, @Email, @FirstName, @LastName)";
            var parameters = new DynamicParameters();
            parameters.Add("@Username", user.username);
            parameters.Add("@Email", user.email);
            parameters.Add("@FirstName", user.firstname);
            parameters.Add("@LastName", user.lastname);
            parameters.Add("@Role", user.role);
            await _DBGateway.ExecuteAsync(query, parameters);

            await LogAudit("AddUser", user.username, user);
        }

        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        /// <summary>
        /// Employee Registration Process
        /// </summary>
        /// <param name="Employee registrationdata"></param>
        /// <returns></returns>

        public async Task<ResultModel<object>> RegisterAsync(register registerationdata)
        {
            var result = new ResultModel<object>();
            try
            {
                string checkQuery = "SELECT COUNT(1) FROM mp_login WHERE username = @Username OR email = @Email";
                var par = new DynamicParameters();
                par.Add("@Username", registerationdata.username);
                par.Add("@Email", registerationdata.email);
                var exists = await _DBGateway.ExecuteScalarQueryAsync<int>(checkQuery, par);

                if (exists > 0)
                {
                    result.Success = false;
                    result.Message = "Username and Email already exist";
                    return result;
                }

                string dataInsertQuery = @"INSERT INTO mp_login (firstname, lastname, username, email, password, role) VALUES (@FirstName, @LastName, @Username, @Email, @Password, @Role); SELECT LAST_INSERT_ID();";
                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", registerationdata.firstname);
                parameters.Add("@LastName", registerationdata.lastname);
                parameters.Add("@Username", registerationdata.username);
                parameters.Add("@Email", registerationdata.email);
                parameters.Add("@Password", registerationdata.password);
                parameters.Add("@Role",  Constants.emprole);

                var userId = await _DBGateway.ExecuteScalarQueryAsync<int>(dataInsertQuery, parameters);

                if (registerationdata != null)
                {
                    string insertEmployeeQuery = @"INSERT INTO md_employee (userid, mobilenumber) VALUES (@userid, @MobileNumber)";
                    var employeeParameters = new DynamicParameters();
                    employeeParameters.Add("@userid", userId);
                    employeeParameters.Add("@MobileNumber", registerationdata.mobilenumber);

                    await _DBGateway.ExecuteAsync(insertEmployeeQuery, employeeParameters);
                }

                var loginData = new login
                {
                    username = registerationdata.username,
                    password = registerationdata.password
                };
                string token = GenerateJwtToken(loginData);
               // await LogAudit("Register", loginData.username, loginData);

                result.Token = token;
                result.Success = true;
                result.Message = "Registered Successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }
            return result;
        }



        /// <summary>
        /// AdminLogin Process
        /// </summary>
        /// <param name="AdminloginData"></param>
        /// <returns></returns>
        public async Task<ResultModel<object>> AdminLoginAsync(login loginData)
        {
            var result = new ResultModel<object>();
            try
            {
                string query = "SELECT * FROM mp_login WHERE (username = @usernameOrEmail OR email = @usernameOrEmail) AND password = @password";
                var parameters = new DynamicParameters();
                parameters.Add("@usernameOrEmail", loginData.username);
                parameters.Add("@password", loginData.password);

                var user = (await _DBGateway.ExeQueryList<Auth>(query, parameters)).FirstOrDefault();

                if (user != null && user.role == Constants.adminrole)
                {

                    if (user.role != Constants.adminrole)
                    {
                        result.Success = false;
                        result.Message = "You do not have the required role to log in.";
                        return result;
                    }
                    //+++++++++++++++++++++ Audit Log ++++++++++++++++++++++++++++++
                    await LogAudit("Authentication", user.username, user);
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    string token = GenerateJwtToken(loginData);

                    //+++++++++++++++++ Cache the JWT token in Redis for 30 minutes ++++++++++++++++++++++
                    var tokenCacheKey = $"{loginData.username}_token";
                    await _distributedCache.SetStringAsync(tokenCacheKey, token, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    });
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    result.UserId = user.id;
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


        /// <summary>
        /// AdminRegistration Process
        /// </summary>
        /// <param name="AdminRegistrationdata"></param>
        /// <returns></returns>

        public async Task<ResultModel<object>> AdminRegisterAsync(register registerationdata)
        {
            var result = new ResultModel<object>();
            try
            {
                string checkQuery = "SELECT COUNT(1) FROM mp_login WHERE username = @Username OR email = @Email";
                var par = new DynamicParameters();
                par.Add("@Username", registerationdata.username);
                par.Add("@Email", registerationdata.email);
                var exists = await _DBGateway.ExecuteScalarQueryAsync<int>(checkQuery, par);

                if (exists > 0)
                {
                    result.Success = false;
                    result.Message = "Username and Email already exist";
                    return result;
                }

                string dataInsertQuery = @"INSERT INTO mp_login (firstname, lastname, username, email, password, role) VALUES (@FirstName, @LastName, @Username, @Email, @Password, @Role); SELECT LAST_INSERT_ID();";
                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", registerationdata.firstname);
                parameters.Add("@LastName", registerationdata.lastname);
                parameters.Add("@Username", registerationdata.username);
                parameters.Add("@Email", registerationdata.email);
                parameters.Add("@Password", registerationdata.password);
                parameters.Add("@Role", Constants.adminrole);

                var userId = await _DBGateway.ExecuteScalarQueryAsync<int>(dataInsertQuery, parameters);

                if (registerationdata != null)
                {
                    string insertEmployeeQuery = @"INSERT INTO md_admin (userid, mobilenumber) VALUES (@userid, @MobileNumber)";
                    var employeeParameters = new DynamicParameters();
                    employeeParameters.Add("@userid", userId);
                    employeeParameters.Add("@MobileNumber", registerationdata.mobilenumber);

                    await _DBGateway.ExecuteAsync(insertEmployeeQuery, employeeParameters);
                }

                var loginData = new login
                {
                    username = registerationdata.username,
                    password = registerationdata.password
                };
                string token = GenerateJwtToken(loginData);
                // await LogAudit("Register", loginData.username, loginData);

                result.Token = token;
                result.Success = true;
                result.Message = "Registered Successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }
            return result;
        }


        /// <summary>
        /// JobSeekerLogin Process
        /// </summary>
        /// <param name="JobSeekerloginData"></param>
        /// <returns></returns>
        public async Task<ResultModel<object>> JobSeekerLoginAsync(login loginData)
        {
            var result = new ResultModel<object>();
            try
            {
                string query = "SELECT * FROM mp_login WHERE (username = @usernameOrEmail OR email = @usernameOrEmail) AND password = @password";
                var parameters = new DynamicParameters();
                parameters.Add("@usernameOrEmail", loginData.username);
                parameters.Add("@password", loginData.password);

                var user = (await _DBGateway.ExeQueryList<Auth>(query, parameters)).FirstOrDefault();

                if (user != null && user.role == Constants.jobseeker)
                {

                    //+++++++++++++++++++++ Audit Log ++++++++++++++++++++++++++++++
                    await LogAudit("Authentication", user.username, user);
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    string token = GenerateJwtToken(loginData);

                    //+++++++++++++++++ Cache the JWT token in Redis for 30 minutes ++++++++++++++++++++++
                    //var tokenCacheKey = $"{loginData.username}_token";
                    //await _distributedCache.SetStringAsync(tokenCacheKey, token, new DistributedCacheEntryOptions
                    //{
                    //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
                    //});
                    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

                    result.UserId = user.id;
                    result.Token = token;
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

        /// <summary>
        /// JobSeekerRegistration Process
        /// </summary>
        /// <param name="JobSeekerRegistrationdata"></param>
        /// <returns></returns>
        
        public async Task<ResultModel<Object>> JobSeekerRegisterAsync(register registerjobseeker)
        {
            var result = new ResultModel<Object>();
            try
            {
                string query = "select count(1) from mp_login where usernmae = @Username OR email = @Email";
                var par = new DynamicParameters();
                par.Add("@Username", registerjobseeker.username);
                par.Add("@Email", registerjobseeker.email);

                var exitdata = await _DBGateway.ExecuteScalarQueryAsync<int>(query, par);

                if (exitdata > 0)
                {
                    result.Message = "Username and Email is already exit";
                    result.Success = true;
                    return result;
                }

                string insertdata = @"insert into mp_login(firstname, lastname, username, email, password, role) VALUES(@FirstName, @LastName, @Username, @Email, @Password, @Role); SELECT LAST_INSERT_ID()";
                var parameters = new DynamicParameters();
                parameters.Add("@FirstName", registerjobseeker.firstname);
                parameters.Add("@LastName", registerjobseeker.lastname);
                parameters.Add("@Username", registerjobseeker.username);
                parameters.Add("@Email", registerjobseeker.email);
                parameters.Add("@Password", registerjobseeker.password);
                parameters.Add("@Role", Constants.jobseeker);

                var userId = await _DBGateway.ExecuteScalarQueryAsync<int>(insertdata, parameters);

                if (registerjobseeker != null)
                {
                    string insertjobseekerdata = @"insert into md_jobseeker(userid,mobilenumber) values (@Userid,@Mobilenumber)";
                    var jobseekerdata = new DynamicParameters();
                    jobseekerdata.Add("@Userid", userId);
                    jobseekerdata.Add("@Mobilenumber", registerjobseeker.mobilenumber);
                    await _DBGateway.ExecuteAsync(insertjobseekerdata, jobseekerdata);
                }

                var logindata = new login
                {
                    username = registerjobseeker.username,
                    password = registerjobseeker.password
                };

                string token = GenerateJwtToken(logindata);
                result.Token = token;
                result.Message = "Register Successfully";
                result.Success = true;
            }catch(Exception ex)
            {
                result.Message = ex.Message;
                result.Message = "Registration failed!";
                result.Success = false;
            }
            return result;
        }

    }
}
