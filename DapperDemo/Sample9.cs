using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace DapperDemo;

public class Sample9
{
    public static async Task RunAsync(string connectionString)
    {
        DapperConfiguration.ApplyConfigurationsFromAssembly(typeof(Sample9).Assembly);

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

    public interface IDapperConfiguration<TEntity> where TEntity : class
    {
        PropertyInfo? GetMapping(Type type, string columnName);
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

    public class DapperConfiguration
    {
        private static readonly MethodInfo _applyConfigurationMethod = typeof(DapperConfiguration)
            .GetMethods()
            .Single(
                e => e.Name == nameof(ApplyConfiguration)
                    && e.ContainsGenericParameters
                    && e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition()
                    == typeof(IDapperConfiguration<>));

        public static void ApplyConfiguration<TEntity>(IDapperConfiguration<TEntity> configuration) where TEntity : class
        {
            if (configuration is not null)
            {
                var func = configuration.GetMapping;
                var typeMap = new CustomPropertyTypeMap(typeof(TEntity), func);
                Dapper.SqlMapper.SetTypeMap(typeof(TEntity), typeMap);
            }
        }

        public static void ApplyConfigurationsFromAssembly(Assembly assembly)
        {
            var dapperConfigInterface = typeof(IDapperConfiguration<>);

            var types = assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract);
            foreach (var type in types)
            {
                foreach (var @interface in type.GetInterfaces())
                {
                    if (!@interface.IsGenericType)
                        continue;

                    if (@interface.GetGenericTypeDefinition() == dapperConfigInterface)
                    {
                        var target = _applyConfigurationMethod.MakeGenericMethod(@interface.GenericTypeArguments[0]);
                        target.Invoke(null, new[] { Activator.CreateInstance(type) });
                    }
                }
            }
        }
    }
}
