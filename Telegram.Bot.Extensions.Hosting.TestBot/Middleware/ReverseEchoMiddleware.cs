namespace Telegram.Bot.Extensions.Hosting.TestBot.Middleware;

public class ReverseEchoMiddleware : ITelegramMiddleware
{
    public async ValueTask InvokeAsync(ITelegramMiddlewareContext context, Func<ValueTask> next)
    {
        await next();
        
        if (context.Update.Message?.Text is not { } text)
        {
            return;
        }

        await context.BotClient.SendTextMessageAsync(
            context.Update.Message.Chat.Id,
            text.ReverseGraphemeClusters(),
            cancellationToken: context.CancellationToken);

    }
}