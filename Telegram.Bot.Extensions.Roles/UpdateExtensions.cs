using Telegram.Bot.Types;

namespace Telegram.Bot.Extensions.Roles;

public static class UpdateExtensions
{
    /// <summary>
    /// Get Id of user that did some action, for actions that are not ambiguous
    /// </summary>
    public static long? GetCurrentActionUserId(this Update update)
    {
        return update.Message?.From?.Id
               ?? update.EditedMessage?.From?.Id
               ?? update.InlineQuery?.From.Id
               ?? update.ChosenInlineResult?.From.Id
               ?? update.CallbackQuery?.From.Id
               ?? update.ShippingQuery?.From.Id
               ?? update.PreCheckoutQuery?.From.Id
               ?? update.PollAnswer?.User.Id
               ?? update.ChatJoinRequest?.From.Id;
    }
    
    /// <summary>
    /// Get any not null chat id
    /// </summary>
    public static long? GetCurrentChatId(this Update update)
    {
        // todo: do something with callback queries
        return update.Message?.Chat.Id
               ?? update.EditedMessage?.Chat.Id
               ?? update.ChannelPost?.Chat.Id
               ?? update.EditedChannelPost?.Chat.Id
               ?? update.MyChatMember?.Chat.Id
               ?? update.ChatMember?.Chat.Id
               ?? update.ChatJoinRequest?.Chat.Id;
    }
}