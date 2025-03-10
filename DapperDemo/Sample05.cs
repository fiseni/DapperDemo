﻿using Dapper;
using Microsoft.Data.SqlClient;

namespace DapperDemo;

public class Sample05
{
    public static async Task RunAsync(string connectionString)
    {
        using var connection = new SqlConnection(connectionString);

        var sql = "Insert into Customer (Name, Age) Values (@Name, @Age)";

        var values = new List<object>()
        {
            new { Name = "Name1", Age = 30 },
            new { Name = "Name2", Age = 32 },
            new { Name = "Name3", Age = 33 },
        };

        var rows = await connection.ExecuteAsync(sql, values);

        Console.WriteLine($"Affected ros: {rows}");
    }

    public record Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
    }
}
