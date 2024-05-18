using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Repositories;

namespace Telegram.Bot.Extensions.EntityFrameworkCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBotStorage<T>(
        this IServiceCollection services)
    where T: TelegramDbContext
    {
        services.AddScoped<TelegramDbContext>(x => x.GetRequiredService<T>());

        services.AddScoped<TelegramChatRepository>();
        services.AddScoped<TelegramUserRepository>();
        
        return services;
    }
}