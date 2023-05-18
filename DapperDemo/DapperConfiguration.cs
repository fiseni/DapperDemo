using Dapper;
using System.Data;
using System.Reflection;

namespace DapperDemo;

internal static class DapperConfiguration
{
    private static readonly MethodInfo _applyConfigurationMethod = typeof(DapperConfiguration)
        .GetMethods(BindingFlags.Static | BindingFlags.NonPublic)
        .Single(
            e => e.Name == nameof(ApplyConfiguration)
                && e.ContainsGenericParameters
                && e.GetParameters().SingleOrDefault()?.ParameterType.GetGenericTypeDefinition()
                == typeof(IDapperConfiguration<>));

    internal static void ApplyConfigurationsFromAssembly(Assembly assembly)
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

    private static void ApplyConfiguration<TEntity>(IDapperConfiguration<TEntity> configuration) where TEntity : class
    {
        if (configuration is not null)
        {
            var func = configuration.GetMapping;
            var typeMap = new CustomPropertyTypeMap(typeof(TEntity), func);
            SqlMapper.SetTypeMap(typeof(TEntity), typeMap);
        }
    }
}
