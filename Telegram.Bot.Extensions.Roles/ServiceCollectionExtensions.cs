using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Extensions.Hosting;
using Telegram.Bot.Extensions.Roles.Interfaces;
using Telegram.Bot.Extensions.Roles.Middleware;

namespace Telegram.Bot.Extensions.Roles;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBotRoles(
        this IServiceCollection services)
    {
        services.AddTelegramBotMiddleware<CurrentUserMiddleware>();
        services.AddScoped<UserAccessor>();
        services.AddScoped<IUserAccessor>(x => x.GetRequiredService<UserAccessor>());
        services.AddTransient<UserClient>();

        services.AddTelegramBotMiddleware<CurrentChatMiddleware>();
        services.AddScoped<ChatAccessor>();
        services.AddScoped<IChatAccessor>(x => x.GetRequiredService<ChatAccessor>());
        services.AddTransient<ChatClient>();
        
        services.AddTelegramBotMiddleware<RoleCheckMiddleware>();
        
        return services;
    }
}