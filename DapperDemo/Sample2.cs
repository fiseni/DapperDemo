using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo;

public class Sample2
{
    public static async Task RunAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var sp = "GetAllCustomers";

        var customers = await connection.QueryAsync<Customer>(sp, new { Name = "Name1" }, commandType: CommandType.StoredProcedure);

        customers.AsList().ForEach(x => Console.WriteLine(x));
    }

    public record Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
    }
}
