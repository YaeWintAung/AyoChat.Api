using AyoChat.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AyoChat.Api.Data
{
    public class ChatDbContext(DbContextOptions<ChatDbContext> optioins):DbContext(optioins)
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

}

