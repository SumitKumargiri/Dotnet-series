using FactoryDesignPattern.Model;

namespace FactoryDesignPattern.Interface
{
    public interface ICrud
    {
        Task<ResultModel<object>> GetAsync();
        Task<ResultModel<object>> CheckInsertAsync(Crud crud);
    }
}
