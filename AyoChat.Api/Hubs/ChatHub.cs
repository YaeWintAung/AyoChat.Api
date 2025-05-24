using Microsoft.AspNetCore.SignalR;

namespace AyoChat.Api.Hubs
{
    public class ChatHub: Hub<IChatHub>
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            await Clients.All.ReceiveMessage($"Thank you for connection {Context.User?.Identity?.Name}");
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
        Task ReceiveMessage(string message);
        Task ReceiveMessage(string senderId, string message);
    }
}
