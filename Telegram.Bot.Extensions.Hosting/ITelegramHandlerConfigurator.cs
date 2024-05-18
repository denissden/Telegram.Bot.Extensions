using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public interface ITelegramHandlerConfigurator
{
    ITelegramHandlerCondition Condition { get; }
    void HandleIf(Func<Update, CancellationToken, ValueTask<bool>> conditionFunc);
}

internal class TelegramHandlerConfigurator(Type handlerType) : ITelegramHandlerConfigurator
{
    public ITelegramHandlerCondition Condition { get; set; } = default!;

    public void HandleIf(Func<Update, CancellationToken, ValueTask<bool>> conditionFunc)
    {
        Condition = new FunctionHandlerCondition(handlerType, conditionFunc);
    }
}

public static class TelegramHandlerConfiguratorExtensions
{
    public static void HandleText(this ITelegramHandlerConfigurator configurator, string text)
    {
        configurator.HandleIf((update, _) => 
            ValueTask.FromResult(update.Message?.Text?.Equals(text) == true)
            );
    }
}