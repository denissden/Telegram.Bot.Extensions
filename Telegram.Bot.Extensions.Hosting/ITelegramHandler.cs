using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Hosting;

public interface ITelegramHandler
{
    ValueTask Handle(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken);
}