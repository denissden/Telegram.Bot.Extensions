using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Extensions.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBot(
        this IServiceCollection services,
        TelegramBotClientOptions options,
        Action<ITelegramBotConfigurator> botConfigurator,
        HttpClient? httpClient = null)
    {
        var configurator = new TelegramBotConfigurator(services);
        botConfigurator(configurator);
        services.AddSingleton<ITelegramBotConfigurator>(configurator);
        
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(options, httpClient));
        services.AddSingleton<TelegramUpdateHandler>();

        services.AddScoped<IHandlerDispatcher, DefaultHandlerDispatcher>();
        
        return services;
    }
    
    public static IServiceCollection AddTelegramBotMiddleware<T>(
        this IServiceCollection services) where T : class, ITelegramMiddleware
    {
        services.AddScoped<ITelegramMiddleware, T>();
        return services;
    }
}