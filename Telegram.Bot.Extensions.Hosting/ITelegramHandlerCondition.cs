using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public interface ITelegramHandlerCondition
{
    Type HandlerType { get; }
    ValueTask<bool> CanHandle(Update update, CancellationToken cancellationToken);
}

internal class FunctionHandlerCondition(
    Type handlerType,
    Func<Update, CancellationToken, ValueTask<bool>> func) : ITelegramHandlerCondition
{
    public Type HandlerType { get; } = handlerType;

    public ValueTask<bool> CanHandle(Update update, CancellationToken cancellationToken)
    {
        return func(update, cancellationToken);
    }
}