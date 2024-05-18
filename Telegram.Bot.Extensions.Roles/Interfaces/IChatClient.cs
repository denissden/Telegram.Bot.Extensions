using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;

namespace Telegram.Bot.Extensions.Roles.Interfaces;

public interface IChatClient
{
    TelegramChat Chat { get; }
}

public class ChatClient : IChatClient
{
    public TelegramChat Chat { get; set; }
}