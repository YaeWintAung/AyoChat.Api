using AyoChat.Api.Data;
using AyoChat.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AyoChat.Api.Services
{
    public class UserService(ChatDbContext context) : IUserService
    {
        public async Task<User?> GetUserById(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            if(user is null)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> GetUserByPhone(string phone)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=> u.PhoneNumber == phone);
            if (user is null) return null;
            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            return await context.Users.ToListAsync();
        }
    }
}
