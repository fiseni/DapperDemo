using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DapperDemo;

public class Sample10
{
    public static async Task RunAsync(string connectionString)
    {
        DapperConfiguration.ApplyConfigurationsFromAssembly(typeof(Sample10).Assembly);

        using var connection = new SqlConnection(connectionString);

        var sp = "GetMultipleTables";

        using var multi = await connection.QueryMultipleAsync(sp, commandType: CommandType.StoredProcedure);

        var customers1 = await multi.ReadAsync<Customer1>();
        var customers2 = await multi.ReadAsync<Customer2>();

        customers1.AsList().ForEach(x => Console.WriteLine(x));
        customers2.AsList().ForEach(x => Console.WriteLine(x));
    }


    public record Customer1
    {
        public int Id_1 { get; set; }
        public string? Name_1 { get; set; }
        public int? Age_1 { get; set; }
        public decimal? Price { get; set; }
    }

    public record Customer2
    {
        public int Id { get; set; }
        public string? Name_2 { get; set; }
    }

    public class Customer1Configuration : IDapperConfiguration<Customer1>
    {
        public PropertyInfo? GetMapping(Type type, string columnName)
        {
            var result = columnName switch
            {
                "Id" => type.GetProperty(nameof(Customer1.Id_1)),
                "Name" => type.GetProperty(nameof(Customer1.Name_1)),
                "Age" => type.GetProperty(nameof(Customer1.Age_1)),
                _ => type.GetProperty(columnName)
            };

            return result;
        }
    }

    public class Customer2Configuration : IDapperConfiguration<Customer2>
    {
        public PropertyInfo? GetMapping(Type type, string columnName)
        {
            var result = columnName switch
            {
                "Name" => type.GetProperty(nameof(Customer2.Name_2)),
                _ => type.GetProperty(columnName)
            };

            return result;
        }
    }
}
