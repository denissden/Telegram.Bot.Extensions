using System.Globalization;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

namespace Telegram.Bot.Extensions.Hosting.TestBot;

public static class StringExtensions
{
    public static IEnumerable<string> GraphemeClusters(this string s) {
        var enumerator = StringInfo.GetTextElementEnumerator(s);
        while(enumerator.MoveNext()) {
            yield return (string)enumerator.Current;
        }
    }
    public static string ReverseGraphemeClusters(this string s) {
        return string.Join("", s.GraphemeClusters().Reverse().ToArray());
    }

    public static string Display(this TelegramChat chat)
    {
        return $"Chat {chat.ChatId}";
    }
    
    public static string Display(this TelegramUser user)
    {
        return $"User {user.TelegramUserId} chat_id={user.ChatId}";
    }
}