using Dapper;
using FactoryDesignPattern.Interface;
using FactoryDesignPattern.Model;
using FactoryDesignPattern.utility;

namespace FactoryDesignPattern.Services
{
    public class CrudService : ICrud
    {
        private readonly DBGateway _dBGateway;

        public CrudService(string connectionString)
        {
            _dBGateway = new DBGateway(connectionString);
        }

        public async Task<ResultModel<object>> GetAsync()
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var data = await _dBGateway.ExeQueryList<Crud>("select *from crud");
                if (data != null)
                {
                    result.Message = "Data received";
                    result.Data = data;
                }
                else
                {
                    result.Message = "Error";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<object>> CheckInsertAsync(Crud crud)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var query = "INSERT INTO crud (name, email, country) VALUES (@Name, @Email, @Country)";
                var par = new DynamicParameters();
                par.Add("@Name", crud.name);
                par.Add("@Email", crud.email);
                par.Add("@Country", crud.country);

                var data = await _dBGateway.ExecuteAsync(query, par);
                if (data != 0)
                {
                    result.Message = "Data insert Successfully";
                    result.Data = crud;
                }
                else
                {
                    result.Message = "Name already Exists";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
