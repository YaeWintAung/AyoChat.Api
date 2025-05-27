using AyoChat.Api.Data;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AyoChat.Api.Hubs
{
    public class ChatHub(ChatDbContext context): Hub<IChatHub>
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"User connected: {userId} with connection ID: {Context.ConnectionId}");
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.OnlineUsers[Context.ConnectionId] = userId;
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);

                await Clients.Others.ReceiveOnlineStatus(userId, true);

                var onlineUserIds = ConnectedUsers.OnlineUsers.Values.Distinct().ToList();
                await Clients.Caller.ReceiveOnlineUsers(onlineUserIds);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if(ConnectedUsers.OnlineUsers.TryRemove(Context.ConnectionId, out var userId))
            {
               if(!ConnectedUsers.OnlineUsers.Values.Contains(userId))
                {
                    var user = await context.Users.FindAsync(Guid.Parse(userId));
                    if (user != null)
                    {
                        user.LastSeen = DateTime.UtcNow;
                        await context.SaveChangesAsync();
                    }
                    await Clients.Others.ReceiveOnlineStatus(userId, false);
                }

            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string receiverId,string message)
        {
            var senderId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(senderId))
            {
                await Clients.Group(receiverId).ReceiveMessage(senderId, message);
            }
        }
    }

    public interface IChatHub
    {
        Task ReceiveMessage(string senderId, string message);
        Task ReceiveOnlineStatus(string userId, bool isOnline);
        Task ReceiveOnlineUsers(List<string> onlineUserIds);
    }
}
