using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperDemo;

public class Sample3
{
    public static async Task RunAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "Insert into Customer (Name) Values (@Name)";

        var rows = await connection.ExecuteAsync(sql, new { Name = "Name4" });

        Console.WriteLine($"Affected ros: {rows}");
    }

    public record Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
    }
}
