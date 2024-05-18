using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

namespace Telegram.Bot.Extensions.EntityFrameworkCore.Data.Repositories;

public class TelegramChatRepository(
    TelegramDbContext dbContext)
{
    public async Task<TelegramChat?> GetChatAsync(
        long? botId, 
        long chatId, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.TelegramChats
            .SingleOrDefaultAsync(x => x.BotId == botId 
                                       && x.ChatId == chatId,
                cancellationToken: cancellationToken);
    }
}