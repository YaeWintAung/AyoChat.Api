namespace AyoChat.Api.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public Guid SenderId { get; set; }
        public required User Sender { get; set; }
        public Guid ReceiverId { get; set; }
        public required User Receiver { get; set; }


    }
}
