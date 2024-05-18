namespace Telegram.Bot.Extensions.Hosting.TestBot.Middleware;

public class EchoMiddleware : ITelegramMiddleware
{
    public async ValueTask InvokeAsync(ITelegramMiddlewareContext context, Func<ValueTask> next)
    {
        if (context.Update.Message?.Text is not { } text)
        {
            return;
        }

        await context.BotClient.SendTextMessageAsync(
            context.Update.Message.Chat.Id,
            text,
            cancellationToken: context.CancellationToken);

        await next();
    }
}