namespace AyoChat.Api.Models
{
    public class SendMessageDto
    {
        public Guid ReceiverId { get; set; }
        public required string Content { get; set; }
    }
}
