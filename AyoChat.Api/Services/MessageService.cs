using AyoChat.Api.Data;
using AyoChat.Api.Entities;
using AyoChat.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AyoChat.Api.Services
{
    public class MessageService(ChatDbContext context,ChatHub chatHub) : IMessageService
    {
        public async Task SendMessageAsync(Guid senderId, Guid receiverId, string content)
        {
            var sender = await context.Users.FirstOrDefaultAsync(u=>u.Id == senderId);
            var receiver = await context.Users.FirstOrDefaultAsync(u=>u.Id == receiverId);
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

            await chatHub.Clients.Group(receiverId.ToString()).SendAsync("ReceiveMessage",senderId.ToString(),content);

        }
    }
}
