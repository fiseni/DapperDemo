using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo;

public class Sample08
{
    public static async Task RunAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var sp = "InsertCustomer";

        var customer = new Customer() { Id = 1, Name_X = "Name1", Age = 30, Price = 10.1m };

        var param = new
        {
            @Name = customer.Name_X,
            @Age = customer.Age,
        };

        var rows = await connection.ExecuteAsync(sp, param, commandType: CommandType.StoredProcedure);

        Console.WriteLine($"Affected ros: {rows}");
    }

    public record Customer
    {
        public int Id { get; set; }
        public string? Name_X { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
    }
}
