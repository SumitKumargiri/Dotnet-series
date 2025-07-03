using Dllprocess.Model;

namespace Dllprocess.Interface
{
    public interface ICrud
    {
        Task<ResultModel<object>> GetAsync();
        Task<ResultModel<object>> CheckInsertAsync(Crud crud);
        Task<ResultModel<object>> UpdateAsync(Crud crud);
        Task<ResultModel<object>> DeleteAsync(int id);
    }
}
