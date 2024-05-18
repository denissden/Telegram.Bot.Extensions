using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public interface ITelegramMiddlewareContext
{
    ITelegramBotClient BotClient { get; }
    Update Update { get; }
    Type HandlerType { get; }
    ITelegramHandlerCondition HandlerCondition { get; }
    CancellationToken CancellationToken { get; }
}

internal record TelegramMiddlewareContext(
    ITelegramBotClient BotClient, 
    Update Update, 
    Type HandlerType,
    ITelegramHandlerCondition HandlerCondition,
    CancellationToken CancellationToken) 
    : ITelegramMiddlewareContext;