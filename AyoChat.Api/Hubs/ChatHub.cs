using Microsoft.AspNetCore.SignalR;

namespace AyoChat.Api.Hubs
{
    public class ChatHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(userId))
            {
                Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }
            return base.OnConnectedAsync();
        }

        public async Task SendMessage(string receiverId,string message)
        {
            var senderId = Context.UserIdentifier;
            if (!string.IsNullOrEmpty(senderId))
            {
                await Clients.Group(receiverId).SendAsync("ReceiveMessage", senderId, message);
            }
        }
    }
}
