namespace Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

public class TelegramUser
{
    public long Id { get; set; }
    
    public long? BotId { get; set; }
    public long TelegramUserId { get; set; }
    /// <summary>
    /// null -> global user data
    /// </summary>
    public long? ChatId { get; set; }

    public List<TelegramUserAttribute> Attributes { get; set; } = new();
}