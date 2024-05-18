using Telegram.Bot.Extensions.EntityFrameworkCore;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Entities;
using Telegram.Bot.Extensions.EntityFrameworkCore.Data.Repositories;

namespace Telegram.Bot.Extensions.Roles.Interfaces;

public interface IUserClient
{
    TelegramUser User { get; }
    bool IsGlobal { get; }
    Task<IEnumerable<string>> GetRolesAsync(CancellationToken cancellationToken = default);
    Task AddRoleAsync(string role, CancellationToken cancellationToken = default);
    Task RemoveRoleAsync(string role, CancellationToken cancellationToken = default);
    Task<string?> GetActiveRoleAsync(CancellationToken cancellationToken = default);
    Task SetActiveRoleAsync(string role, CancellationToken cancellationToken = default);
}

public class UserClient(TelegramUserRepository userRepository) : IUserClient
{
    private const string ROLE = "role";
    private const string ACTIVE_ROLE = "active_role";
    
    public TelegramUser User { get; set; }
    public bool IsGlobal { get; set; }
    public async Task<IEnumerable<string>> GetRolesAsync(CancellationToken cancellationToken = default)
    {
        var value = await userRepository
            .GetUserAttributeValueAsync(User.Id, ROLE, cancellationToken);
        var maybeRoles = value is { } v ? v.ToObject<string[]>() : null;
        return maybeRoles ?? [];
    }

    public async Task AddRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(cancellationToken);
        var enumerable = roles as string[] ?? roles.ToArray();
        if (enumerable.Contains(role))
        {
            return;
        }

        IEnumerable<string> newRoles = [..enumerable, role];
        await userRepository.SetUserAttributeAsync(User.Id, ROLE, newRoles, cancellationToken);
    }

    public async Task RemoveRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        var roles = await GetRolesAsync(cancellationToken);
        var enumerable = roles as string[] ?? roles.ToArray();
        if (!enumerable.Contains(role))
        {
            return;
        }

        IEnumerable<string> newRoles = enumerable.Where(x => x != role);
        await userRepository.SetUserAttributeAsync(User.Id, ROLE, newRoles, cancellationToken);
    }
    
    public async Task<string?> GetActiveRoleAsync(CancellationToken cancellationToken = default)
    {
        var value = await userRepository
            .GetUserAttributeValueAsync(User.Id, ACTIVE_ROLE, cancellationToken);
        return value is { } v ? v.ToObject<string>() : null;
    }

    public async Task SetActiveRoleAsync(string role, CancellationToken cancellationToken = default)
    {
        await userRepository.SetUserAttributeAsync(User.Id, ACTIVE_ROLE, role, cancellationToken);
    }
}