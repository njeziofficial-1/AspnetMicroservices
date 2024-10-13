using Npgsql;

namespace Discount.Grpc.Extensions;

public static class HostExtensions
{
    public static void MigrateDatabase<T>(this IHost host, int? retry = 0)
    {
        int retryForAvailability = retry.Value;
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;
        var configuration = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<T>>();
        try
        {
            logger.LogInformation("Migrating Postgres Database");
            using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            connection.Open();
            using var command = new NpgsqlCommand
            {
                Connection = connection
            };

            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            command.ExecuteNonQuery();

            command.CommandText = @"
            CREATE TABLE Coupon(Id SERIAL PRIMARY KEY,
                                ProductName VARCHAR(24) NOT NULL,
                                Description TEXT,
                                Amount DECIMAL NOT NULL)
";
            command.ExecuteNonQuery();

            command.CommandText = @"
INSERT INTO Coupon (ProductName, Description, Amount)
                        VALUES ('iPhone 15', 'iPhone Discount', 750)
";
            command.ExecuteNonQuery();

            command.CommandText = @"
INSERT INTO Coupon (ProductName, Description, Amount)
                        VALUES ('Samsung S24', 'Samsung Discount', 850)
";
            command.ExecuteNonQuery();

            logger.LogInformation("Migrated Postgres Database");
        }
        catch (NpgsqlException ex)
        {
            logger.LogError(ex, "An error occurred while migrating the postgres database");
            if (retryForAvailability < 50)
            {
                retryForAvailability++;
                Thread.Sleep(2000);
                MigrateDatabase<T>(host, retryForAvailability);
            }
        }
    }
}
