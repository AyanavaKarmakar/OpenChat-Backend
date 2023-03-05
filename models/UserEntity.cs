using Microsoft.EntityFrameworkCore;

namespace Chat.UserEntity
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public byte[]? PasswiordHash { get; set; }
        public byte[]? PasswiordSalt { get; set; }
    }

    public class ChatDB : DbContext
    {
        public ChatDB(DbContextOptions options) : base(options) { }
        public DbSet<UserEntity> Users { get; set; } = null!;
    }
}