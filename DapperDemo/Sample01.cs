using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperDemo;

public class Sample01
{
    public static async Task RunAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "Select * from Customer where Id = @id";

        var customers = await connection.QueryAsync<Customer>(sql, new { id = 1 });

        customers.AsList().ForEach(x => Console.WriteLine(x));
    }

    public record Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
        public double? Perimeter { get; set; }
        public float? Area { get; set; }
        public bool? IsSmoking { get; set; }
    }
}
