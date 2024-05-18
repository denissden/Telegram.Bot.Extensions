using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

namespace Telegram.Bot.Extensions.EntityFrameworkCore.Data.Repositories;

public class TelegramUserRepository(
    TelegramDbContext dbContext)
{
    public async Task<TelegramUser?> GetChatLocalUserAsync(
        long? botId, 
        long telegramId, 
        long chatId,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.TelegramUsers
            .SingleOrDefaultAsync(x => x.BotId == botId 
                                       && x.TelegramUserId == telegramId
                                       && x.ChatId == chatId, 
                cancellationToken: cancellationToken);
    }
    
    public async Task<TelegramUser?> GetGlobalUserAsync(
        long? botId, 
        long telegramId, 
        CancellationToken cancellationToken = default)
    {
        return await dbContext.TelegramUsers
            .SingleOrDefaultAsync(x => x.BotId == botId 
                                       && x.TelegramUserId == telegramId 
                                       && x.ChatId == null,
                cancellationToken: cancellationToken);
    }

    public async Task<TelegramUserAttribute?> GetUserAttributeAsync(
        long internalUserId, 
        string attributeName,
        CancellationToken cancellationToken = default)
    {
        return await dbContext.TelegramUserAttributes
            .SingleOrDefaultAsync(x => x.InternalUserId == internalUserId && x.Name == attributeName, 
                cancellationToken: cancellationToken);
    }
    
    public async Task<JsonElement?> GetUserAttributeValueAsync(
        long internalUserId, 
        string attributeName,
        CancellationToken cancellationToken = default)
    {
        var attribute = await GetUserAttributeAsync(internalUserId, attributeName, cancellationToken);
        return attribute?.Value;
    }
    
    public async Task SetUserAttributeAsync(
        long internalUserId, 
        string attributeName,
        object value,
        CancellationToken cancellationToken = default)
    {
        var attribute = await GetUserAttributeAsync(internalUserId, attributeName, cancellationToken);
        
        if (attribute == null)
        {
            attribute = new TelegramUserAttribute
            {
                InternalUserId = internalUserId,
                Name = attributeName,
                Value = value.ToJsonElement()
            };

            dbContext.TelegramUserAttributes.Add(attribute);
        }
        else
        {
            attribute.Value = value.ToJsonElement();
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAttributeAsync(
        long internalUserId,
        string attributeName,
        CancellationToken cancellationToken = default)
    {
        var attribute = await GetUserAttributeAsync(internalUserId, attributeName, cancellationToken);

        if (attribute is null)
        {
            return;
        }

        dbContext.TelegramUserAttributes.Remove(attribute);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}