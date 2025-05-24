using AyoChat.Api.Data;
using AyoChat.Api.Entities;
using AyoChat.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace AyoChat.Api.Services
{
    public class MessageService(ChatDbContext context,IHubContext<ChatHub> chatHub) : IMessageService
    {
        public async Task<List<Message>> GetMessagesWithUser(Guid currentUserId, Guid withUserId)
        {
            var messages = await context.Messages
                    .Where(m => (m.SenderId == currentUserId && m.ReceiverId == withUserId)
                             || (m.SenderId == withUserId && m.ReceiverId == currentUserId))
                    .OrderBy(m => m.CreatedAt)
                    .ToListAsync();
            
            return messages;
        }

        public async Task SendMessageAsync(Guid senderId, Guid receiverId, string content)
        {
            var sender = await context.Users.FindAsync(senderId);
            var receiver = await context.Users.FindAsync(receiverId);
            if (sender == null || receiver == null)
                throw new Exception("Sender or receiver not found");

            var message = new Message
            {
                SenderId = senderId,
                Sender = sender,
                ReceiverId = receiverId,
                Receiver = receiver,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            context.Messages.Add(message);
            await context.SaveChangesAsync();

            await chatHub.Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage",senderId.ToString(), receiverId.ToString(), content);
        }
    }
}
