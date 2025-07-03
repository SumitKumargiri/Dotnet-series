using Grpc.Net.Client;
using GrpcWebApiClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcWebApiClient.Services
{
    public class GrpcUserService
    {
        private readonly UserService.UserServiceClient _client;
        public GrpcUserService()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7274");
            _client = new UserService.UserServiceClient(channel);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var request = new EmptyRequest();
            var response = await _client.GetUsersAsync(request);
            return response.Users;
        }
    }
}
