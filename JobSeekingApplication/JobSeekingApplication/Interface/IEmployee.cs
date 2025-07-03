using JobSeekingApplication.Model;

namespace JobSeekingApplication.Interface
{
    public interface IEmployee
    {
        Task<ResultModel<Object>>InsertEmpdata(Employee employee);
        Task<ResultModel<Object>> GetEmpdata(int userid);
        Task<ResultModel<Object>> Getcount();
    }
}
