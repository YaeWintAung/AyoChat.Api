using AyoChat.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AyoChat.Api.Data
{
    public class ChatDbContext(DbContextOptions<ChatDbContext> optioins):DbContext(optioins)
    {
        public DbSet<User> Users { get; set; } 
    }
}
