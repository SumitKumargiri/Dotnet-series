using CoreLayer.Interfaces;
using System.Threading.Tasks;
using CoreLayer.Model;

namespace MyApplicationApplicationLayer.Services
{
    public class CrudService : ICrud
    {
        private readonly ICrudRepository _crudRepository;

        public CrudService(ICrudRepository crudRepository)
        {
            _crudRepository = crudRepository;
        }

        public async Task<ResultModel<object>> GetDataAsync()
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var data = await _crudRepository.FetchCrudDataAsync();
                result.Data = data;
                result.Message = "Data fetched successfully";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error: {ex.Message}";
            }

            return result;
        }
    }
}
