using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;
using SignalRwithmysql.Model;

namespace SignalRwithmysql
{
    public class ChatHub : Hub
    {
        private readonly IDistributedCache _distributedCache;

        public ChatHub(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task SendMessage(string username, string message)
        {
            var connectionId = Context.ConnectionId;

            string cacheKey = $"user:{username}";
            var cachedMessage = await _distributedCache.GetAsync(cacheKey);
            if (cachedMessage == null)
            {
                var messageData = new { Username = username, Message = message, ConnectionId = connectionId };
                var serializedMessage = JsonSerializer.Serialize(messageData);
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await _distributedCache.SetStringAsync(cacheKey, serializedMessage, cacheOptions);
                await Clients.All.SendAsync("ReceiveMessage", username, message, connectionId);
            }
            else
            {
                var cachedMessageString = Encoding.UTF8.GetString(cachedMessage);
                var cachedMessageData = JsonSerializer.Deserialize<MessageData>(cachedMessageString);
                await Clients.All.SendAsync("ReceiveMessage", cachedMessageData.Username, cachedMessageData.Message, cachedMessageData.ConnectionId);
            }
        }
    }
}
