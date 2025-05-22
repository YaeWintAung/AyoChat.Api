using AyoChat.Api.Entities;

namespace AyoChat.Api.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsers();

        Task<User?> GetUserById(Guid userId);

        Task<User?> GetUserByPhone(string  phone);
    }
}
