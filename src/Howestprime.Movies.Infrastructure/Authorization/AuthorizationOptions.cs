namespace Howestprime.Movies.Infrastructure.Authorization;

public class AuthorizationOptions
{
    public List<RolePermissions> PermissionsByRole { get; set; } = new();
}

public class RolePermissions
{
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}