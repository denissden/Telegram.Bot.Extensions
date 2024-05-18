namespace Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

public class TelegramChat
{
    public long Id { get; set; }
    
    public long? BotId { get; set; }
    public long ChatId { get; set; }

    public List<TelegramChatAttribute> Attributes { get; set; } = new();
}