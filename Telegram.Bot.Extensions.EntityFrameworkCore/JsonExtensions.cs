using System.Text.Json;

namespace Telegram.Bot.Extensions.EntityFrameworkCore;

public static class JsonExtensions
{
    public static JsonElement ToJsonElement(this object o)
    {
        return JsonSerializer.SerializeToElement(o);
    }
    
    public static T? ToObject<T>(this JsonElement element)
    {
        var json = element.GetRawText();
        return JsonSerializer.Deserialize<T>(json);
    }
    
    public static T ToRequiredObject<T>(this JsonElement element)
    {
        return element.ToObject<T>() ?? throw new ArgumentNullException();
    }
}