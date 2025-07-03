using Unittestingpro.Model;

namespace Unittestingpro.Interface
{
    public interface Ihome
    {
        Task<ResultModel<object>> GetAsync();
        Task<ResultModel<object>> InsertdataAsync(home homedata);
        Task<ResultModel<object>> UpdatedataAsync(home homedata);
        Task<ResultModel<object>>DeletedataAsync(int id);

        Task<ResultModel<object>> GetDatabyname(string name);
    }
}
