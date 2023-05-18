using System.Reflection;

namespace DapperDemo;

public interface IDapperConfiguration<TEntity> where TEntity : class
{
    PropertyInfo? GetMapping(Type type, string columnName);
}
