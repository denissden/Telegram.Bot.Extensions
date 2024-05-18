
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public interface IHandlerDispatcher
{
    Task HandleUpdateAsync(ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken);
}

public class DefaultHandlerDispatcher(
    IEnumerable<ITelegramHandlerCondition> handlerConditions,
    IEnumerable<ITelegramMiddleware> telegramMiddlewares,
    IServiceProvider serviceProvider,
    ILogger<DefaultHandlerDispatcher> logger) 
    : IHandlerDispatcher
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        bool routed = false;
        
        foreach (var condition in handlerConditions)
        {
            if (!await condition.CanHandle(update, cancellationToken))
            {
                continue;
            }

            routed = true;
            
            var handlerService = serviceProvider.GetRequiredService(condition.HandlerType);
            var typedHandler = (handlerService as ITelegramHandler)
                               ?? throw new InvalidOperationException(handlerService.GetType().FullName +
                                                                      " does not implement ITelegramHandler");
            
            logger.LogInformation("Handling event {EventId}", update.Id);
            
            await InvokeMiddlewaresAsync(new TelegramMiddlewareContext(
                BotClient: botClient,
                Update: update,
                HandlerType: condition.HandlerType,
                HandlerCondition: condition,
                CancellationToken: cancellationToken
            ), async () => 
                await typedHandler.Handle(botClient, update, cancellationToken));
            
            break;
        }

        if (!routed)
        {
            logger.LogWarning("Event {EventId} was not routed", update.Id);
        }
    }
    
    private async Task InvokeMiddlewaresAsync(ITelegramMiddlewareContext context, Func<ValueTask> next)
    {
        var middlewareEnumerator = telegramMiddlewares.GetEnumerator();

        try
        {
            async ValueTask Next()
            {
                if (middlewareEnumerator.MoveNext())
                {
                    await middlewareEnumerator.Current.InvokeAsync(context, Next);
                }
                else
                {
                    await next();
                }
            }

            await Next();
        }
        finally
        {
            middlewareEnumerator.Dispose();
        }
    }
}