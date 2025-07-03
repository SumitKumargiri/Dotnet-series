using CoreLayer.Interfaces;
using InfrastructureLayer.utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfrastructureLayer.Repositories
{
    public class CrudRepository : ICrudRepository
    {
        private readonly DBGateway _dbGateway;

        public CrudRepository(string connection)
        {
            _dbGateway = new DBGateway(connection);
        }

        public async Task<IEnumerable<object>> FetchCrudDataAsync()
        {
            string query = "SELECT * FROM crud";
            return await _dbGateway.ExeQueryList<object>(query);
        }
    }
}
