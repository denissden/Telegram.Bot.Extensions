using Telegram.Bot.Extensions.Roles.Interfaces;
using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting.TestBot.Handlers;

public class HelloHandler(
    IUserAccessor userAccessor, 
    IChatAccessor chatAccessor) : ITelegramHandler
{
    public async ValueTask Handle(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(update.Message!.Chat.Id, 
            $"""
             Global {userAccessor.GlobalUser.User.Display()}
             Chat local {userAccessor.ChatLocalUser.User.Display()}
             {chatAccessor.CurrentChat.Chat.Display()}
             /start
             /secret
             """,
            cancellationToken: cancellationToken);
    }
}