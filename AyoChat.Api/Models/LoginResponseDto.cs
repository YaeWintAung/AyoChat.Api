﻿namespace AyoChat.Api.Models
{
    public class LoginResponseDto
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
