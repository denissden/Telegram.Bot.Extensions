using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Extensions.Hosting;

public static class HostExtensions 
{
    public static IHost UseDefaultTelegramHandler(this IHost host, ReceiverOptions? receiverOptions = null)
    {
        return UseTelegramHandler<TelegramUpdateHandler>(host, receiverOptions);
    }
    
    public static IHost UseTelegramHandler<T>(this IHost host, ReceiverOptions? receiverOptions = null) where T: IUpdateHandler
    {
        var bot = host.Services.GetRequiredService<ITelegramBotClient>();
        var handler = host.Services.GetRequiredService<T>();
        
        bot.StartReceiving(
            receiverOptions: receiverOptions,
            updateHandler: handler,
            cancellationToken: host.Services
                .GetService<IHostApplicationLifetime>()?.ApplicationStopping 
                               ?? CancellationToken.None
            );
        
        return host;
    }
}