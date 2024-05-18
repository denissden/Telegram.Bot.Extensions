using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public class TelegramUpdateHandler(
    IServiceProvider serviceProvider,
    ILogger<TelegramUpdateHandler> logger) 
    : IUpdateHandler
{
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var dispatcher = scope.ServiceProvider.GetRequiredService<IHandlerDispatcher>();
        await dispatcher.HandleUpdateAsync(botClient, update, cancellationToken);
    }

    public Task HandlePollingErrorAsync(
        ITelegramBotClient botClient, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Telegram update error");
        return Task.CompletedTask;
    }
}