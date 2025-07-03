using Dapper;
using JobSeekingApplication.Interface;
using JobSeekingApplication.Model;
using JobSeekingApplication.utility;

namespace JobSeekingApplication.Services
{
    public class AdminService:IAdmin
    {
        private readonly DBGateway _DBGateway;
        public AdminService(DBGateway dBGateway)
        {
            _DBGateway = dBGateway;
        }

        public async Task<ResultModel<Object>> GetEmployeeAsync()
        {
            var result = new ResultModel<Object>();
            try
            {
                var query = "select ml.firstname,ml.lastname,ml.username,ml.email,emp.profileimg,emp.mobilenumber,emp.address from mp_login ml inner join md_employee emp on ml.id = emp.userid";

                var exitsdata = await _DBGateway.ExeQueryList<employeedata>(query);

                if (exitsdata != null)
                {
                    result.Success = true;
                    result.Message = "Data received Successfully";
                    result.Data = exitsdata;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data received failed";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<ResultModel<Object>> GetAllstateDistrictdata()
        {
            var result = new ResultModel<Object>();
            try
            {
                var query = @"select * from md_state_district";
                var exitsstatedistrictdata = await _DBGateway.ExeQueryList<state_district>(query);

                if(exitsstatedistrictdata != null)
                {
                    result.Success = true;
                    result.Message = "Data received Successfully";
                    result.Data = exitsstatedistrictdata;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data received failed";
                }
            }
            catch(Exception ex) 
            { result.Success = false;
              result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<Object>> GetDistrictdata(string StateName)
        {
            var result = new ResultModel<Object>();
            try
            {
                var query = @"SELECT StateName,DistrictCode,DistrictName FROM md_state_district WHERE StateName = @StateName";
                var par = new DynamicParameters();
                par.Add("@StateName", StateName);

                var exitdistrictdata = await _DBGateway.ExeQueryList<statedistrictdata>(query, par);

                if (exitdistrictdata != null)
                {
                    result.Success = true;
                    result.Message = "Data received Successfully";
                    result.Data = exitdistrictdata;
                }
                else
                {
                    result.Success = false;
                    result.Message = "Data Not Found";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
