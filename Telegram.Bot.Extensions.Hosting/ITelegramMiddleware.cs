using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public interface ITelegramMiddleware
{
    ValueTask InvokeAsync(ITelegramMiddlewareContext context, Func<ValueTask> next);
}