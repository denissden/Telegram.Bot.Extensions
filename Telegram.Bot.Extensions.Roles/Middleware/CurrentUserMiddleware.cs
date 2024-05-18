using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Extensions.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Repositories;
using Telegram.Bot.Extensions.Hosting;
using Telegram.Bot.Extensions.Roles.Interfaces;

namespace Telegram.Bot.Extensions.Roles.Middleware;

public class CurrentUserMiddleware(
    TelegramDbContext dbContext,
    TelegramUserRepository userRepository,
    IServiceProvider serviceProvider,
    ILogger<CurrentUserMiddleware> logger) : ITelegramMiddleware
{
    public async ValueTask InvokeAsync(ITelegramMiddlewareContext context, Func<ValueTask> next)
    {
        // check
        if (context.Update.GetCurrentActionUserId() is not { } messageFromId)
        {
            await next();
            return;
        }
        var chatId = context.Update.GetCurrentChatId();

        // find
        var globalUser = await userRepository.GetGlobalUserAsync(
            botId: context.BotClient.BotId,
            telegramId: messageFromId,
            context.CancellationToken);
        
        var chatLocalUser = await userRepository.GetChatLocalUserAsync(
            botId: context.BotClient.BotId,
            telegramId: messageFromId,
            chatId: chatId ?? throw new InvalidOperationException("Could not determine chat id"),
            context.CancellationToken);

        // create if needed
        if (globalUser is null)
        {
            logger.LogInformation("Creating global user user_id={UserId}", messageFromId);
            globalUser = new TelegramUser
                {
                    TelegramUserId = messageFromId, 
                    BotId = context.BotClient.BotId!.Value, 
                    ChatId = null
                };
            dbContext.TelegramUsers.Add(globalUser);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
        if (chatLocalUser is null)
        {
            logger.LogInformation("Creating chat local user user_id={UserId} chat_id={ChatId}", messageFromId, chatId);
            chatLocalUser = new TelegramUser
                {
                    TelegramUserId = messageFromId, 
                    BotId = context.BotClient.BotId!.Value, 
                    ChatId = chatId
                };
            dbContext.TelegramUsers.Add(chatLocalUser);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
        
        dbContext.Entry(globalUser).State = EntityState.Detached;
        dbContext.Entry(chatLocalUser).State = EntityState.Detached;

        // set values
        var userAccessor = serviceProvider.GetRequiredService<UserAccessor>();
        var globalUserClient = serviceProvider.GetRequiredService<UserClient>();
        var chatLocalUserClient = serviceProvider.GetRequiredService<UserClient>();
        globalUserClient.User = globalUser;
        globalUserClient.IsGlobal = true;
        chatLocalUserClient.User = chatLocalUser;
        userAccessor.GlobalUser = globalUserClient;
        userAccessor.ChatLocalUser = chatLocalUserClient;

        await next();
    }
}