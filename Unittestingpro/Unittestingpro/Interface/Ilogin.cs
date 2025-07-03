using Unittestingpro.Model;

namespace Unittestingpro.Interface
{
    public interface Ilogin
    {
        Task <ResultModel<object>> LoginAsync (Login logindata);
    }
}
