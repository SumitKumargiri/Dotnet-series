using Dapper;
using Dllprocess.Interface;
using Dllprocess.Model;
using Dllprocess.utility;
using System.Threading.Tasks;

namespace Dllprocess.Services
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
                var data = await _dBGateway.ExeQueryList<Crud>("SELECT * FROM crud");
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
                var existingdata = await _dBGateway.ExeScalarQuery<int>("SELECT COUNT(1) FROM crud WHERE name = @Name", new DynamicParameters(new { Name = crud.name }));

                if (existingdata > 0)
                {
                    result.Message = "Name already exists.";
                    return result;
                }

                var query = "INSERT INTO crud (name, email, country) VALUES (@Name, @Email, @Country)";
                var data = await _dBGateway.ExecuteAsync(query, new DynamicParameters(new { Name = crud.name, Email = crud.email, Country = crud.country }));

                if (data > 0)
                {
                    result.Message = "Data inserted successfully.";
                    result.Data = crud;
                }
                else
                {
                    result.Message = "Data insertion failed.";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<object>> UpdateAsync(Crud crud)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var query = "UPDATE crud SET name = @Name, email = @Email, country = @Country WHERE id = @Id";
                var data = await _dBGateway.ExecuteAsync(query, new DynamicParameters(new { Id = crud.id, Name = crud.name, Email = crud.email, Country = crud.country }));

                if (data > 0)
                {
                    result.Message = "Data updated successfully.";
                    result.Data = crud;
                }
                else
                {
                    result.Message = "Data update failed.";
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<ResultModel<object>> DeleteAsync(int id)
        {
            ResultModel<object> result = new ResultModel<object>();
            try
            {
                var query = "DELETE FROM crud WHERE id = @Id";
                var data = await _dBGateway.ExecuteAsync(query, new DynamicParameters(new { Id = id }));

                if (data > 0)
                {
                    result.Message = "Data deleted successfully.";
                }
                else
                {
                    result.Message = "Data deletion failed.";
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
