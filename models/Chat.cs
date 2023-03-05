using Microsoft.EntityFrameworkCore;

namespace Chat.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string? Sender { get; set; }
        public string? MessageContent { get; set; }
        public string? Timestamp { get; set; }
    }

    public class UserEntity
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public byte[]? PasswordHash { get; set; } = null!;
        public byte[]? PasswordSalt { get; set; } = null!;
    }

    public class ChatDB : DbContext
    {
        public ChatDB(DbContextOptions options) : base(options) { }
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<UserEntity> Users { get; set; } = null!;

    }
}