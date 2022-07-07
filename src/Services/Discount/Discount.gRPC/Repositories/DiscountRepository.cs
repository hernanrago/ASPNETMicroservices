using Dapper;
using Discount.gRPC.Entities;
using Npgsql;

namespace Discount.gRPC.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Coupon> Get(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
            (
                "SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName }
            );

            if (coupon == null)
            {
                return new Coupon() { ProductName = "No discount", Amount = 0, Description = "No discount" };
            }

            return coupon;
        }

        public async Task<bool> Create(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
            (
                "INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount }
            );

            return affected > 0;
        }

        public async Task<bool> Update(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
            (
                "UPDATE Coupon SET ProductName=@ProductName , Description=@Description, Amount=@Amount)WHERE Id=@Id",
                new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id }
            );

            return affected > 0;
        }

        public async Task<bool> Delete(string productName)
        {
            using var connection = new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));

            var affected = await connection.ExecuteAsync
            (
                "DELETE FROM Coupon WHERE ProductName=@ProductName",
                new { ProductName = productName }
            );

            return affected > 0;        }

    }
}