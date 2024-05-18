using Telegram.Bot;
using Telegram.Bot.Extensions.EntityFrameworkCore;
using Telegram.Bot.Extensions.Hosting;
using Telegram.Bot.Extensions.Hosting.TestBot.Data;
using Telegram.Bot.Extensions.Hosting.TestBot.Handlers;
using Telegram.Bot.Extensions.Hosting.TestBot.Middleware;
using Telegram.Bot.Extensions.Roles;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlite<TestBotDbContext>("Data Source=test.db");

string token = builder.Configuration.GetSection("Telegram:Token").Value
               ?? throw new ArgumentNullException();

builder.Services.AddTelegramBot(new TelegramBotClientOptions(token), x =>
{
    x.AddHandler<StartHandler>(x => x.HandleText("/start"));
    x.AddHandler<HelloHandler>(x => x.HandleText("/hello"));
    x.AddHandler<AdminHandler>(x => x.HandleText("/admin"));
    x.AddHandler<SecretHandler>(x => x.HandleIf(async (update, cancellationToken) => 
        update.Message?.Text?.Contains("secret") == true));

    x.AddMiddleware<EchoMiddleware>();
    x.AddMiddleware<ReverseEchoMiddleware>();
});
builder.Services.AddTelegramBotStorage<TestBotDbContext>();
builder.Services.AddTelegramBotRoles();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<TestBotDbContext>().Database.EnsureCreated();
}

app.UseDefaultTelegramHandler();

app.Run();