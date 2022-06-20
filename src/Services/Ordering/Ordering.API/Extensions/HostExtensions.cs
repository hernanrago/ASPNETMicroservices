using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(
            this IHost host,
            Action<TContext,
            IServiceProvider> seeder,
            int? retry = 0)
            where TContext : DbContext
        {
            int retryForAvailibility = retry.Value;
            
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var logger = services.GetRequiredService<ILogger<TContext>>();

            var context = services.GetRequiredService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
                
                InvokeSeeder(seeder, context, services);
                
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (SqlException ex)
            {
                 logger.LogError(ex, "An error ocurred while migrating the database used on context.");

                 if (retryForAvailibility < 50)
                 {
                     retryForAvailibility++;

                     Thread.Sleep(2000);

                     MigrateDatabase<TContext>(host, seeder, retryForAvailibility);
                 }
            }

            return host;
        }

        private static void InvokeSeeder<TContext>(
            Action<TContext,
            IServiceProvider> seeder,
            TContext context,
            IServiceProvider services)
            where TContext : DbContext
        {
            context.Database.Migrate();

            seeder(context, services);
        }
    }
}