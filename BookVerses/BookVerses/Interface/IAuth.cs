using BookVerses.Model;

namespace BookVerses.Interface
{
    public interface IAuth
    {
        Task<ResultModel<Object>> LoginAsync(login logindata);
    }
}
