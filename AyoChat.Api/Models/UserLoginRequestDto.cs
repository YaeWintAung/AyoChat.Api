namespace AyoChat.Api.Models
{
    public class UserLoginRequestDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
