using Grpc.Core;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using GrpcServer; 

namespace GrpcServer.Services
{
    public class UserServiceImpl : UserService.UserServiceBase
    {
        private readonly string _connectionString;

        public UserServiceImpl(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString1");
        }

        public override async Task<UserListResponse> GetUsers(EmptyRequest request, ServerCallContext context)
        {
            var response = new UserListResponse();

            using var connection = new MySqlConnection(_connectionString);
            await connection.OpenAsync();

            var command = new MySqlCommand("SELECT Id, Name, Email FROM crud", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                response.Users.Add(new User
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Name = reader["Name"].ToString(),
                    Email = reader["Email"].ToString()
                });
            }
            return response;
        }
    }
}
