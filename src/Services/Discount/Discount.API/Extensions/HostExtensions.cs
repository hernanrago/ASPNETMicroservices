using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvaibility = retry.Value;

            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;

            var configuration = services.GetRequiredService<IConfiguration>();

            var logger = services.GetRequiredService<ILogger<TContext>>();

            try
            {
                logger.LogInformation("Migrating PostgreSQL database...");

                using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

                connection.Open();

                using var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                command.CommandText = "DROP TABLE IF EXISTS Coupon";
                command.ExecuteNonQuery();

                command.CommandText = @"CREATE TABLE Coupon(
                    Id SERIAL PRIMARY KEY,
                    ProductName VARCHAR(24) NOT NULL,
                    Description TEXT,
                    Amount INT)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('iPhone X', 'iPhone Discount', 150)";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES ('Samsung 10', 'Samsung Discount', 100)";
                command.ExecuteNonQuery();

                logger.LogInformation("PostgreSQL database migrated.");
            }
            catch (NpgsqlException ex)
            {
                logger.LogError("An error ocurred while migrating the PostgreSQL database: {0}", ex.Message);

                if (retry < 50)
                {
                    retryForAvaibility++;

                    Thread.Sleep(2000);

                    MigrateDatabase<TContext>(host, retryForAvaibility);
                }
            }
        
            return host;
        }
    }
}