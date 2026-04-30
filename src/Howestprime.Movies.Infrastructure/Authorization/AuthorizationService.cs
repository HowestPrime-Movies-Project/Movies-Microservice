using Howestprime.Movies.Application.Contracts.Ports;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Howestprime.Movies.Infrastructure.Authorization;

public class AuthorizationService(
    IOptions<AuthorizationOptions> options,
    ILogger<AuthorizationService> logger
): IAuthorizationService
{
    public void Authorize(string userRole, string requestedPermission)
    {
        var rolePermissions = options.Value.PermissionsByRole
            .FirstOrDefault(r => r.Name.Equals(userRole, StringComparison.OrdinalIgnoreCase));
        
        if (rolePermissions == null)
        {
            throw new UnauthorizedAccessException("No permissions found for the provided role");
        }
        
        if (!rolePermissions.Permissions.Contains(requestedPermission, StringComparer.OrdinalIgnoreCase))
        {
            logger.LogDebug("Authorization failed for role {Role} on use case {UseCase}", userRole, requestedPermission);
            throw new UnauthorizedAccessException($"Role {userRole} does not have the correct permissions to access this resource");
        }
        
    }
}