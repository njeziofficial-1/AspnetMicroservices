using Dapper;
using Discount.Grpc.Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Api.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private NpgsqlConnection _connection;

    public DiscountRepository(IConfiguration configuration)
    {
        _connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {

        string sql = @"
            CREATE TABLE IF NOT EXISTS Coupon (
                Id SERIAL PRIMARY KEY,
                ProductName VARCHAR(255) NOT NULL,
                Description TEXT,
                Amount DECIMAL NOT NULL
            )";

        await _connection.ExecuteAsync(sql);

        sql = @"
                        INSERT INTO Coupon (ProductName, Description, Amount)
                        VALUES (@ProductName, @Description, @Amount)
";
        var affected = await _connection.ExecuteAsync(sql, new {coupon.ProductName, coupon.Description, coupon.Amount});
        Dispose(_connection);
        return affected switch
        {
            0 => false,
            _ => true,
        };
    }

    public Task<bool> DeleteDiscount(string productName)
    {
        throw new NotImplementedException();
    }

    public async Task<Coupon> GetDiscount(string productName)
    {
        string sql = "SELECT * FROM Coupon WHERE LOWER(ProductName) = LOWER(@productName)";
        var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>(sql, new {productName });
        Dispose(_connection);
        return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };
    }

    public Task<bool> UpdateDiscount(Coupon coupon)
    {
        throw new NotImplementedException();
    }

    void Dispose(NpgsqlConnection connection)
    => connection.Dispose();
}
