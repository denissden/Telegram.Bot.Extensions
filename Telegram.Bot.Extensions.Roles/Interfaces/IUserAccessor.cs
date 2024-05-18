namespace Telegram.Bot.Extensions.Roles.Interfaces;

public interface IUserAccessor
{
    IUserClient GlobalUser { get; } 
    IUserClient ChatLocalUser { get; } 
}

public class UserAccessor : IUserAccessor
{
    public IUserClient GlobalUser { get; set; }
    public IUserClient ChatLocalUser { get; set; }
}