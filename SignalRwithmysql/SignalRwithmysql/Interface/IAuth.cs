using SignalRwithmysql.Model;

namespace SignalRwithmysql.Interface
{
    public interface IAuth
    {
        Task<Auth> GetByUsername(string username);
        Task<string> GenerateJwtToken(Auth user);
        Task<ResultModel<object>>getdata();
    }
}
