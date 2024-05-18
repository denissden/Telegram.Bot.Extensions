using Microsoft.Extensions.DependencyInjection;

namespace Telegram.Bot.Extensions.Hosting;

public interface ITelegramBotConfigurator
{
     /// <summary>
     /// Add update handler.
     /// </summary>
     /// <param name="handlerType"></param>
     /// <param name="configureHandler"></param>
     /// <returns></returns>
     ITelegramBotConfigurator AddHandler(Type handlerType, Action<ITelegramHandlerConfigurator> configureHandler);
     
     /// <summary>
     /// Add middleware. Middlewares are executed in the order they were added.
     /// </summary>
     /// <typeparam name="T">Middleware implementation</typeparam>
     /// <returns></returns>
     ITelegramBotConfigurator AddMiddleware<T>() where T: class, ITelegramMiddleware;
}

internal class TelegramBotConfigurator(IServiceCollection services) : ITelegramBotConfigurator
{
     public ITelegramBotConfigurator AddHandler(Type handlerType, Action<ITelegramHandlerConfigurator> configureHandler)
     {
          var configurator = new TelegramHandlerConfigurator(handlerType);
          configureHandler(configurator);
          services.AddSingleton<ITelegramHandlerCondition>(configurator.Condition);
          
          services.AddScoped(handlerType);
          services.AddScoped(handlerType);
          return this;
     }

     public ITelegramBotConfigurator AddMiddleware<T>() where T : class, ITelegramMiddleware
     {
          services.AddTelegramBotMiddleware<T>();
          return this;
     }
}

public static class TelegramBotConfiguratorExtensions
{
     public static ITelegramBotConfigurator AddHandler<T>(this ITelegramBotConfigurator configurator,
        Action<ITelegramHandlerConfigurator> configureHandler) where T : class, ITelegramHandler
    {
        configurator.AddHandler(typeof(T), configureHandler);
        return configurator;
    }
}