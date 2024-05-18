using Telegram.Bot.Extensions.Roles.Interfaces;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting.TestBot.Handlers;

public class SecretHandler(IUserAccessor userAccessor) : ITelegramHandler
{
    public async ValueTask Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (await userAccessor.ChatLocalUser.GetActiveRoleAsync(cancellationToken) == "admin")
        {
            await userAccessor.ChatLocalUser.SetActiveRoleAsync("user", cancellationToken);
            await botClient.SendTextMessageAsync(update.Message!.Chat.Id, 
                "You are now a user",
                cancellationToken: cancellationToken);
            return;
        }
        
        await userAccessor.ChatLocalUser.SetActiveRoleAsync("admin", cancellationToken);
        await botClient.SendTextMessageAsync(update.Message!.Chat.Id, 
            "You are now an admin /admin",
            cancellationToken: cancellationToken);
    }
}