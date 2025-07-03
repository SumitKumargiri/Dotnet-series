using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Tls;
using System;
using System.Threading.Tasks;

namespace crudoperation
{
    public class ChatHub : Hub
    {
        public async Task SendMessagesToAll(string username, string message)
        {
            string connectionId = GenerateConnectionId();
            await Clients.All.SendAsync("ReceiveMessage", username, message, connectionId);
        }

        private string GenerateConnectionId()
        {
            return Guid.NewGuid().ToString();
        }

        public override Task OnConnectedAsync()
        {
            string connectionId = Context.ConnectionId;
            return base.OnConnectedAsync();
        }
    }
}
