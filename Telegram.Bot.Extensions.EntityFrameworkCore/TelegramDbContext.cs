using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

namespace Telegram.Bot.Extensions.EntityFrameworkCore;

public class TelegramDbContext : DbContext 
{
    public DbSet<TelegramChat> TelegramChats { get; init; }
    public DbSet<TelegramChatAttribute> TelegramChatAttributes { get; init; }

    public DbSet<TelegramUser> TelegramUsers { get; init; }
    public DbSet<TelegramUserAttribute> TelegramUserAttributes { get; init; }

    public TelegramDbContext(DbContextOptions options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TelegramChat>()
            .HasMany(c => c.Attributes)
            .WithOne()
            .HasForeignKey(a => a.InternalChatId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<TelegramUser>()
            .HasMany(u => u.Attributes)
            .WithOne()
            .HasForeignKey(a => a.InternalUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}