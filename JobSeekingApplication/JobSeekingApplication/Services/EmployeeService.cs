using Dapper;
using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using JobSeekingApplication.utility;

namespace JobSeekingApplication.Services
{
    public class EmployeeService:IEmployee
    {
        private readonly DBGateway _DBGateway;
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        public EmployeeService(DBGateway dBGateway, string connectionString, IConfiguration configuration)
        {
            _DBGateway = dBGateway;
        }

        /// <summary>
        /// Employee Post Job Data.
        /// </summary>
        /// <param name="Employee Post Job Data"></param>
        /// <returns></returns>

        public async Task<ResultModel<object>> InsertEmpdata(Employee employee)
        {
            var result = new ResultModel<object>();
            try
            {
                string adddata = @"insert into md_postjob(userid,role,company_name,description,location,package,mode,skill,noofhiring,image) values(@userid,@role,@company_name,@description,@location,@package,@mode,@skill,@noofhiring,@image);";
                var par = new DynamicParameters();
                par.Add("@userid", employee.userid);
                par.Add("@role", employee.role);
                par.Add("@company_name", employee.company_name);
                par.Add("@description", employee.description);
                par.Add("@location", employee.location);
                par.Add("@package", employee.package);
                par.Add("@mode", employee.mode);
                par.Add("@skill", employee.skill);
                par.Add("@noofhiring", employee.noofhiring);
                par.Add("@image", employee.image);

                var employeedata = await _DBGateway.ExeQueryList<int>(adddata, par);


                if (employeedata != null)
                {
                    result.Success = true;
                    result.Message = "Insert Data SuccessFully";
                }
                else
                {
                    result.Success = false;
                    result.Message = "Insertion failed";
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
        /// Employee get Job Data.
        /// </summary>
        /// <param name="Employee Get Job Data"></param>
        /// <returns></returns>

        public async Task<ResultModel<object>> GetEmpdata(int userid)
        {
            var result = new ResultModel<object>();
            try
            {
                string query = "SELECT * FROM md_postjob where userid=@id";
                var parameters = new DynamicParameters();
                parameters.Add("@id", userid);


                var employeedata = await _DBGateway.ExeQueryList<Employee>(query, parameters);

                if (employeedata != null && employeedata.Any())
                {
                    result.Success = true;
                    result.Message = "Data retrieved successfully.";
                    result.Data = employeedata; 
                }
                else
                {
                    result.Success = false;
                    result.Message = "No data found.";
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
        /// Count Graph
        /// </summary>
        /// <param name="Count Graph"></param>
        /// <returns></returns>

        public async Task<ResultModel<Object>> Getcount()
        {
            var result = new ResultModel<Object>();
            try
            {
                var exitdata = @"SELECT role, COUNT(*) AS role_count FROM mp_login WHERE role IN (2, 1, 3) GROUP BY role;";

                var data = await _DBGateway.ExeQueryList<count>(exitdata);

                if(data != null && data.Any())
                {
                    result.Success = true;
                    result.Message = "Data received";
                    result.Data = data;
                }
                else
                {
                    result.Success = false;
                }
            }catch(Exception ex) 
            { 
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }

    }
}
