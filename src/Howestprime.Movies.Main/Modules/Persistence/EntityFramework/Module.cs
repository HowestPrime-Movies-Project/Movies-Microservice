
namespace Howestprime.Movies.Main.Modules.Persistence.EntityFramework;

public static class PersistenceModule
{
    public static IServiceCollection AddEFCoreModule(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services.AddEFCoreServices(configuration);
    }

    public static async Task<WebApplication> UseEFCoreModule(this WebApplication app)
    {
        app.ApplyMigrations();
        await app.SeedData();

        return app; 
    }
}
