using Telegram.Bot.Extensions.Roles;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting.TestBot.Handlers;

public class AdminHandler : ITelegramHandler
{
    [AllowActiveRole("admin")]
    public async ValueTask Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(update.Message!.Chat.Id, 
            $"""
             Hello admin
             /start
             """,
            cancellationToken: cancellationToken);
    }
}