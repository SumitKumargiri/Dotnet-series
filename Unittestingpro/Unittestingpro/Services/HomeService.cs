using Dapper;
using Unittestingpro.utility;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Xml.Linq;
using Unittestingpro.Interface;
using Unittestingpro.Model;

namespace Unittestingpro.Services
{
    public class HomeService : Ihome
    {
        private readonly DBGateway _dBGateway;
        private readonly IDistributedCache _distributedCache;
        public HomeService(string connection,IDistributedCache cache)
        {
            _dBGateway = new DBGateway(connection);
            _distributedCache = cache;
        }

        /// <summary>
        /// <para>[Get Data from database]</para>
        /// </summary>
        /// <returns></returns>
       
        public async Task<ResultModel<object>> GetAsync() 
        {
            var result = new ResultModel<object>();
            string cachekey = "data";
            string cachedata = await _distributedCache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                result.Data = JsonSerializer.Deserialize<List<home>>(cachedata);
                result.Message = "Data received from redis";
                result.Success = true;
            }
            try
            {
                var data = await _dBGateway.ExeQueryList<home>("SELECT * FROM crud");
                if (data != null)
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };

                    await _distributedCache.SetStringAsync(cachekey, JsonSerializer.Serialize(data), cacheOptions);

                    result.Data = data;
                    result.Message = "Data received from database";
                    result.Success = true;
                }
                else
                {
                    result.Message = "No data";
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                result.Message = $"Error: {ex.Message}";
                result.Success = false;
            }

            return result;
        }

        /// <summary>
        /// <para>[Insert Data in database]</para>
        /// </summary>
        /// <returns></returns>
        
        public async Task<ResultModel<object>>InsertdataAsync(home homedata)
        {
            var result = new ResultModel<object>();
            string cachekey = "data";
            try
            {
                var query = "insert into crud(name,email,country) values (@Name,@Email,@Country)";
                var par = new DynamicParameters();
                par.Add("@Name", homedata.name);
                par.Add("@Email", homedata.email);
                par.Add("@Country", homedata.country);

                int data = await _dBGateway.ExecuteAsync(query, par);

                if(data!= null)
                {
                    await _distributedCache.RemoveAsync(cachekey);
                    result.Message = "Insertdata Successfully";
                    result.Success = true;
                }
                else
                {
                    result.Message = "Insertdata Failed";
                    result.Success = false;
                }
            }catch (Exception ex) 
            {
                result.Message = $"Error: {ex.Message}";
                result.Success = false;
            }
            return result;
        }

        /// <summary>
        /// <para>[Update Data in database]</para>
        /// </summary>
        /// <returns></returns>

        public async Task<ResultModel<object>> UpdatedataAsync(home homedata)
        {
            var result = new ResultModel<object>();
            string cachekey = "data";
            try 
            {
                var query = "update crud set name = @Name, email = @Email, country = @Country where id = @Id";
                var par = new DynamicParameters();
                par.Add("@Id", homedata.id);
                par.Add("@Name", homedata.name);
                par.Add("@Email",homedata.email);
                par.Add("@Country",homedata.country);

                int data = await _dBGateway.ExecuteAsync(query, par);

                if (data != null)
                {
                    await _distributedCache.RemoveAsync(cachekey);
                    result.Message = "Data Update Successfully";
                    result.Success = true;
                }
                else
                {
                    result.Message = "Data Update Fail";
                    result.Success = false;
                }
            }
            catch (Exception ex) 
            {
                result.Message = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// <para>[Delete Data in database]</para>
        /// </summary>
        /// <returns></returns>
        
        public async Task<ResultModel<object>> DeletedataAsync(int id)
        {
            var result = new ResultModel<object>();
            string cachekey = "data";
            try 
            {
                var query = "delete from crud where id=@Id";
                var par = new DynamicParameters();
                par.Add("@Id", id);

                int data = await _dBGateway.ExecuteAsync(query, par);

                if (data != null)
                {
                    await _distributedCache.RemoveAsync(cachekey);
                    result.Message = "Data Deleted Successfully";
                    result.Success = true;
                }
                else
                {
                    result.Message = "Data Not Deleting";
                }
            }
            catch(Exception ex) 
            {
                result.Message = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// <para>[Get Data By Name]</para>
        /// </summary>
        /// <returns></returns>

        public async Task<ResultModel<object>> GetDatabyname(string name)
        {
            var result = new ResultModel<object>();
            var cachekey = "data";
            string cachedata = await _distributedCache.GetStringAsync(cachekey);
            if (!string.IsNullOrEmpty(cachedata))
            {
                result.Data = JsonSerializer.Deserialize<List<home>>(cachedata);
                result.Success = true;
                result.Message = "Data received Successfully from redis";
            }
            try
            {
                var par = new DynamicParameters();
                par.Add("@Name", name);
                var data = await _dBGateway.ExeQueryList<home>("SELECT * FROM crud WHERE name = @Name", par);
                if (data != null)
                {
                    var cacheOptions = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    };
                    await _distributedCache.SetStringAsync(cachekey, JsonSerializer.Serialize(data), cacheOptions);

                    result.Data = data;
                    result.Success = true;
                    result.Message = "Data received from database";
                }
                else
                {
                    result.Data =data;
                    result.Success = false;
                    result.Message = "Data Not received";
                }
            }catch(Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }


    }
}


