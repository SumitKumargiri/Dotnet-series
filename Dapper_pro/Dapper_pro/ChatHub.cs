using Dapper_pro.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private static ConcurrentDictionary<string, List<ChatMessage>> userMessages = new ConcurrentDictionary<string, List<ChatMessage>>();

    public async Task SendMessage(string user, string message)
    {
        try
        {
            var chatMessage = new ChatMessage { User = user, Message = message, Read = false };
            userMessages.GetOrAdd(Context.ConnectionId, new List<ChatMessage>()).Add(chatMessage);

            await Clients.All.SendAsync("ReceiveMessage", user, message, false);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
            throw; // Rethrow the exception to notify the client
        }
    }

    public async Task MessageSeen(string user, string message)
    {
        await Clients.All.SendAsync("MessageSeen", user, message);
    }

    public override Task OnDisconnectedAsync(Exception exception)
    {
        userMessages.TryRemove(Context.ConnectionId, out _);
        return base.OnDisconnectedAsync(exception);
    }

    public override async Task OnConnectedAsync()
    {
        if (userMessages.TryGetValue(Context.ConnectionId, out var messages))
        {
            foreach (var message in messages)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", message.User, message.Message, message.Read);
            }
        }

        await base.OnConnectedAsync();
    }
}
