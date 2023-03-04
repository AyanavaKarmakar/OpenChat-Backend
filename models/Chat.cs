using System.ComponentModel.DataAnnotations;
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

    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [Compare(nameof(Password))]
        public string? ConfirmPassword { get; set; }
    }
}