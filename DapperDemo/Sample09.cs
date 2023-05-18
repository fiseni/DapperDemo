using Dapper;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations.Schema;

namespace DapperDemo;

public class Sample09
{
    public static async Task RunAsync(string connectionString)
    {
        // Call it only once
        Mapper.MapManual();

        using var connection = new SqlConnection(connectionString);

        var sql = "Select * from Customer";

        var customers = await connection.QueryAsync<Customer>(sql);

        customers.AsList().ForEach(x => Console.WriteLine(x));
    }

    public record Customer
    {
        [Column("Id")]
        public int Id_X { get; set; }

        [Column("Name")]
        public string? Name_X { get; set; }

        [Column("Age")]
        public int? Age_X { get; set; }

        public decimal Price { get; set; }
    }

    public class Mapper
    {
        public static void MapWithAttributes()
        {
            SqlMapper.SetTypeMap(typeof(Customer), new CustomPropertyTypeMap(typeof(Customer), (type, columnName)
                => type.GetProperties().FirstOrDefault(prop =>
                    prop.GetCustomAttributes(false)
                        .OfType<ColumnAttribute>()
                        .Any(attr => attr.Name == columnName))));
        }

        public static void MapManual()
        {
            var customerMap = new CustomPropertyTypeMap(typeof(Customer), (type, columnName) => columnName switch
            {
                "Id" => type.GetProperty(nameof(Customer.Id_X)),
                "Name" => type.GetProperty(nameof(Customer.Name_X)),
                "Age" => type.GetProperty(nameof(Customer.Age_X)),
                _ => type.GetProperty(columnName)
            });

            SqlMapper.SetTypeMap(typeof(Customer), customerMap);
        }
    }
}
