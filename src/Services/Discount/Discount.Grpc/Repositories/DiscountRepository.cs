using System.Threading.Tasks;
using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Utils;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DatabaseSettings _databaseSettings;

        public DiscountRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
            {
                var sql = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
                var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>(sql, new { ProductName = productName });

                return coupon;
            }
        }

        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
            {
                var sql =
                    "INSERT INTO Coupon (ProductName,Description,Amount) VALUES (@ProductName,@Description,@Amount)";
                var result = await connection.ExecuteAsync(sql, coupon);

                return result > 0;
            }
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
            {
                var sql = "UPDATE Coupon SET ProductName = @ProductName, Description = @Description,Amount = @Amount WHERE Id = @Id";
                var result = await connection.ExecuteAsync(sql, coupon);

                return result > 0;
            }
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using (var connection = new NpgsqlConnection(_databaseSettings.ConnectionString))
            {
                var sql = "DELETE FROM Coupon WHERE ProductName = @ProductName";
                var result = await connection.ExecuteAsync(sql, new { ProductName = productName });

                return result > 0;
            }
        }
    }
}