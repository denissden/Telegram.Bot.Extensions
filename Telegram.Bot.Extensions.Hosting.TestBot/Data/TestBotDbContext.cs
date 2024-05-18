using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

namespace Telegram.Bot.Extensions.Hosting.TestBot.Data;

public class TestBotDbContext : TelegramDbContext
{
    public TestBotDbContext(DbContextOptions<TestBotDbContext> options) : base(options)
    {
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        // sqlite specific conversion
        configurationBuilder.Properties<JsonElement>().HaveConversion<JsonElementToStringConverter>();
    }
}