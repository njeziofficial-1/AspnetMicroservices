using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<T>(this IHost host, Action<T, IServiceProvider> seeder, int? retry = 0) where T : DbContext
        {
            int retrtForAvailability = retry.Value;

            using (var scope =  host.Services.CreateAsyncScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<T>>();
                var context = services.GetService<T>();

                try
                {
                    logger.LogInformation("Migrating database associated with {DbContextName}", typeof(T).Name);
                    InvokeSeeder(seeder, context, services);
                    logger.LogInformation("Migrated database associated with {DbContextName}", typeof(T).Name);
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database used on context. The is the error: {ex.Message}");
                    if (retrtForAvailability < 50)
                    {
                        retrtForAvailability++;
                        Thread.Sleep(2000);
                        MigrateDatabase<T>(host, seeder, retrtForAvailability);
                    }
                }
                return host;
            }
        }

        private static void InvokeSeeder<T>(Action<T, IServiceProvider> seeder, T context, IServiceProvider services) where T : DbContext
        {
                context.Database.Migrate();
                seeder(context, services);
        }
    }
}
