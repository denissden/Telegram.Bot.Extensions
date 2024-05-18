using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

public class TelegramChatAttribute
{
    public long Id { get; set; }
    
    [ForeignKey("TelegramChat")]
    public long InternalChatId { get; set; }
    
    public string Name { get; set; }
    
    public JsonElement Value { get; set; }
}