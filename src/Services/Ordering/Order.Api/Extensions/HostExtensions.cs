using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Order.Api.Extensions;

public static class HostExtensions
{
    public static IHost MigrateDatabase<TContext>(this IHost host, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
    {
        int retryForAvailability = retry.Value;
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();
        var context = services.GetService<TContext>();
        string dbContextName = typeof(TContext).Name;
        try
        {
            logger.LogInformation($"Migrating database associated with context {dbContextName}");
            InvokeSeeder(seeder, context, services);

            logger.LogInformation($"Migrated database associated with context {dbContextName}");
        }
        catch (SqlException ex)
        {
            logger.LogError(ex, $"An error occurred while migrating the database used on context {dbContextName}");
            if (retryForAvailability < 50)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase(host, seeder, retryForAvailability);
            }
        }

        return host;
    }

    private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services) where TContext : DbContext
    {
        context.Database.Migrate();
        seeder(context, services);
    }
}
