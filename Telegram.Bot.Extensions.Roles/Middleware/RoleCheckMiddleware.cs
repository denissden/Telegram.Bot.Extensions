using System.Reflection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Extensions.Hosting;
using Telegram.Bot.Extensions.Roles.Interfaces;

namespace Telegram.Bot.Extensions.Roles.Middleware;

public class RoleCheckMiddleware(
    IUserAccessor userAccessor,
    ILogger<RoleCheckMiddleware> logger) : ITelegramMiddleware
{
    public async ValueTask InvokeAsync(ITelegramMiddlewareContext context, Func<ValueTask> next)
    {
        var handlerType = context.HandlerType;

        MethodInfo? handleMethod = handlerType.GetMethod("Handle");
        var methodAttribute = handleMethod?
            .GetCustomAttribute<AllowActiveRoleAttribute>(inherit: true);

        var role = await userAccessor.ChatLocalUser.GetActiveRoleAsync(cancellationToken: context.CancellationToken);
        
        if (methodAttribute is not null)
        {
            var allowedRoles = methodAttribute.Roles;
            if (!allowedRoles.Contains(role))
            {
                logger.LogInformation("User ({UserRole}) does not have allowed active roles ({AllowedRoles})", 
                    role, 
                    allowedRoles);
                return;
            }
        }

        await next();
    }
}