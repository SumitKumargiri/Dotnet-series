using empcrudoperation.Model;

namespace empcrudoperation.Interface
{
    public interface ICrudemp
    {
        Task<ResultModel<object>> Getcrud();
        Task<ResultModel<object>> GetEmployeeById(int empid);
        Task<ResultModel<object>> InsertCrud(Crudemp model);
        Task<ResultModel<object>> UpdateCrud(Crudemp model);
        Task<ResultModel<object>> DeleteCrud(int empid);

        Task<ResultModel<object>> getattendancedata();
        Task<ResultModel<object>> UpdateAttendance(attendance model, string firstname, string lastname);
        Task<ResultModel<object>> GetEmpAttendanceData(int empid);
    }
}
