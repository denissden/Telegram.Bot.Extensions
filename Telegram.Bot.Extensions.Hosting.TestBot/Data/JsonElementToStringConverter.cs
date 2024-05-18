using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Telegram.Bot.Extensions.Hosting.TestBot.Data;

public class JsonElementToStringConverter : ValueConverter<JsonElement, string>
{
    public JsonElementToStringConverter() : base(
        x => x.GetRawText(),
        x => JsonSerializer.Deserialize<JsonElement>(x, new JsonSerializerOptions())
    )
    {
        
    }
}