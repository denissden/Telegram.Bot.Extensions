using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

public class TelegramUserAttribute
{
    public long Id { get; set; }
    
    [ForeignKey("TelegramUser")]
    public long InternalUserId { get; set; }
    
    public string Name { get; set; }
    
    public JsonElement Value { get; set; }
}