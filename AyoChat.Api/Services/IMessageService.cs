using AyoChat.Api.Entities;

namespace AyoChat.Api.Services
{
    public interface IMessageService
    {
        public Task SendMessageAsync(Guid userId,Guid receiverId,string content);
        public Task<List<Message>> GetMessagesWithUser(Guid currentUserId, Guid withUserId);
    }
}
