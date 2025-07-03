using JobSeekingApplication.Model;

namespace JobSeekingApplication.Interface
{
    public interface IAdmin
    {
        Task<ResultModel<Object>> GetEmployeeAsync();
        Task<ResultModel<Object>> GetAllstateDistrictdata();
        Task<ResultModel<Object>> GetDistrictdata(string StateName);

    }
}
