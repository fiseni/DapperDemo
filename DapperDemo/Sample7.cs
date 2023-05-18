using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperDemo;

public class Sample7
{
    public static async Task RunAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var sp = "GetMultipleTables";

        using var multi = await connection.QueryMultipleAsync(sp, commandType: CommandType.StoredProcedure);

        var customers1 = await multi.ReadAsync<Customer1>();
        var customers2 = await multi.ReadAsync<Customer2>();
        var count = await multi.ReadFirstAsync<int>();

        customers1.AsList().ForEach(x => Console.WriteLine(x));
        customers2.AsList().ForEach(x => Console.WriteLine(x));
    }

    public record Customer1
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public decimal? Price { get; set; }
        public double? Perimeter { get; set; }
        public float? Area { get; set; }
        public bool? IsSmoking { get; set; }
    }

    public record Customer2
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
