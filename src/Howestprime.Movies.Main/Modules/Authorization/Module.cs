using Howestprime.Movies.Application.Contracts.Ports;
using Howestprime.Movies.Infrastructure.Authorization;

namespace Howestprime.Movies.Main.Modules.Authorization;

public static class AuthorizationModule
{
    public static IServiceCollection AddAuthorizationModule(this IServiceCollection services, IConfiguration configuration)
    {
        // Register authorization services, policies, handlers, etc.
        services
            .AddOptions<AuthorizationOptions>()
            .Bind(configuration.GetSection("Authorization"));

        services.AddScoped<IAuthorizationService, AuthorizationService>();
        return services;
    }
}