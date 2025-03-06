namespace Atata;

/// <summary>
/// Contains methods to create types of objects.
/// To create an instance uses type's parameterless constructor or constructor containing only optional arguments.
/// </summary>
public static class ActivatorEx
{
    public static T CreateInstance<T>(string typeName)
    {
        typeName.CheckNotNullOrEmpty(nameof(typeName));

        Type type = Type.GetType(typeName, true);

        return CreateInstance<T>(type);
    }

    public static T CreateInstance<T>(Type type = null) =>
        (T)CreateInstance(type ?? typeof(T));

    public static object CreateInstance(Type type)
    {
        var constructorData = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            .Select(x => new { Constructor = x, Parameters = x.GetParameters() })
            .OrderByDescending(x => x.Constructor.IsPublic)
            .ThenBy(x => x.Parameters.Length)
            .FirstOrDefault(x => x.Parameters.Length == 0 || x.Parameters.All(param => param.IsOptional || param.GetCustomAttributes(true).Any(attr => attr is ParamArrayAttribute)))
            ?? throw new MissingMethodException($"No parameterless constructor or constructor without non-optional parameters defined for the {type.FullName} type.");

        object[] parameters = [.. constructorData.Parameters.Select(x => x.IsOptional ? x.DefaultValue : null)];

        return constructorData.Constructor.Invoke(parameters);
    }
}
