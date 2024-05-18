using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting.TestBot.Handlers;

public class StartHandler : ITelegramHandler
{
    public async ValueTask Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(update.Message!.Chat.Id, 
            "Start /hello",
            cancellationToken: cancellationToken);
    }
}