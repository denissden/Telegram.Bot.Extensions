using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Extensions.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Repositories;
using Telegram.Bot.Extensions.Hosting;
using Telegram.Bot.Extensions.Roles.Interfaces;

namespace Telegram.Bot.Extensions.Roles.Middleware;

public class CurrentChatMiddleware(
    TelegramDbContext dbContext,
    TelegramChatRepository chatRepository,
    IServiceProvider serviceProvider,
    ILogger<CurrentChatMiddleware> logger) : ITelegramMiddleware
{
    public async ValueTask InvokeAsync(ITelegramMiddlewareContext context, Func<ValueTask> next)
    {
        // check
        if (context.Update.GetCurrentChatId() is not { } chatId)
        {
            await next();
            return;
        }

        // find
        var chat = await chatRepository.GetChatAsync(
            botId: context.BotClient.BotId,
            chatId: chatId,
            context.CancellationToken);

        // create if needed
        if (chat is null)
        {
            logger.LogInformation("Creating chat chat_id={ChatId}", chatId);
            chat = new TelegramChat
                {
                    BotId = context.BotClient.BotId!.Value, 
                    ChatId = chatId
                };
            dbContext.TelegramChats.Add(chat);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
        
        dbContext.Entry(chat).State = EntityState.Detached;

        // set values
        var chatAccessor = serviceProvider.GetRequiredService<ChatAccessor>();
        var chatClient = serviceProvider.GetRequiredService<ChatClient>();
        chatClient.Chat = chat;
        chatAccessor.CurrentChat = chatClient; 

        await next();
    }
}