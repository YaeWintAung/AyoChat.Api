using AyoChat.Api.Entities;
using AyoChat.Api.Models;

namespace AyoChat.Api.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto request);

        Task<LoginResponseDto?> LoginAsync(UserLoginRequestDto request);

        Task<LoginResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);

        Task<User?> GetLoggedUser(Guid userId);
    }
}
