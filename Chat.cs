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

    public class ChatDB : DbContext
    {
        public ChatDB(DbContextOptions options) : base(options) { }
        public DbSet<Message> Messages { get; set; } = null!;
    }
}