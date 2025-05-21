namespace AyoChat.Api.Services
{
    public interface IMessageService
    {
        public Task SendMessageAsync(Guid userId,Guid receiverId,string content);

    }
}
