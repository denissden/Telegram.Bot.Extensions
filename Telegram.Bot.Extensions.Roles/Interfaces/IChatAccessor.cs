namespace Telegram.Bot.Extensions.Roles.Interfaces;

public interface IChatAccessor
{
    public IChatClient CurrentChat { get; }
}

public class ChatAccessor : IChatAccessor
{
    public IChatClient CurrentChat { get; set; }
}