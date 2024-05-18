namespace Telegram.Bot.Extensions.Roles;

[AttributeUsage(AttributeTargets.Method)]
public class AllowActiveRoleAttribute(string[] roles, bool global = false) : Attribute
{
    public AllowActiveRoleAttribute(string role, bool global = false) : this([role], global) { }
    
    public string[] Roles { get; } = roles;
    public bool Global { get; } = global;
}