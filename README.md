# Telegram bot extensions

This project aims to make interactions with [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) easier.

## Getting started
Install `Telegram.Bot.Extensions.Hosting`

Add telegram bot to services.
```csharp
builder.Services.AddTelegramBot(new TelegramBotClientOptions(token), x =>
{
    // handle exact text or command
    x.AddHandler<StartHandler>(x => x.HandleText("/start")); // handler ITelegramHandler
    // handle if specified condition is satisfied
    x.AddHandler<SecretHandler>(x => x.HandleIf((update, cancellationToken) => ValueTask.FromResult(true)));
    

    x.AddMiddleware<EchoMiddleware>(); // also you can add middleware ITelegramMiddleware
});
builder.Services.AddTelegramBotMiddleware<EchoMiddleware>() // another way to add middleware
```

Remember to use the handler.
```csharp
app.UseDefaultTelegramHandler();
```

Now your application handles telegram events.

Middleware is executed in the order it was added.

## User context and authentication
Install `Telegram.Bot.Extensions.Roles`

Add storage and role services.
```csharp
builder.Services.AddTelegramBotStorage<TestBotDbContext>();
builder.Services.AddTelegramBotRoles();

// context should inherit TelegramDbContext
public class TestBotDbContext : TelegramDbContext
{
    // your models here
}
```

Specify required roles with `AllowActiveRoleAttribute`
```csharp
public class AdminHandler : ITelegramHandler
{
    [AllowActiveRole("admin")]
    public async ValueTask Handle(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        // your code here
    }
}
```

This package also adds `IChatAccessor` and `IUserAccessor` services to your handlers.

There are 2 types of users: **global** and **chat local**. 
Global user is the same for any chat. 
Chat local user is different for each chat.
User only appears in the database once the bot receives the event from them in any chat.
For each user, there is only one global user and there may be multiple chat local users.