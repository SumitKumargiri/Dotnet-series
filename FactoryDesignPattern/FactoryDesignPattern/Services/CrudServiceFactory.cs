using FactoryDesignPattern.Services;
using FactoryDesignPattern.Interface;

namespace FactoryDesignPattern.Factory
{
    public class CrudServiceFactory : ICrudFactory
    {
        private readonly string _connectionString;
        public CrudServiceFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public ICrud CreateCrudService(string type)
        {
            switch (type.ToLower())
            {
                case "crud":
                    return new CrudService(_connectionString);
                default:
                    throw new ArgumentException("Invalid service type");
            }
        }
    }
}
