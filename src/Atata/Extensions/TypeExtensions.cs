namespace Atata;

public static class TypeExtensions
{
    internal static bool IsClassOrNullable(this Type type)
    {
        Guard.ThrowIfNull(type);

        return type.IsClass || Nullable.GetUnderlyingType(type) is not null;
    }

    public static Enum[] GetIndividualEnumFlags(this Type type)
    {
        Guard.ThrowIfNull(type);

        if (type.IsDefined(typeof(FlagsAttribute), false))
        {
            Enum[] allFlags = Enum.GetValues(type)
                .Cast<Enum>()
                .Where(x => Convert.ToDecimal(x, CultureInfo.InvariantCulture) != 0m)
                .ToArray();

            List<Enum> individualFlags = [allFlags[0]];

            for (int i = 1; i < allFlags.Length; i++)
            {
                Enum currentFlag = allFlags[i];
                if (allFlags.Take(i).All(x => !currentFlag.HasFlag(x)))
                    individualFlags.Add(currentFlag);
            }

            return [.. individualFlags];
        }
        else
        {
            return Enum.GetValues(type).Cast<Enum>().ToArray();
        }
    }

    internal static MethodInfo GetMethodWithThrowOnError(this Type type, string name, params Type[] types)
    {
        Guard.ThrowIfNull(type);
        Guard.ThrowIfNullOrWhitespace(name);

        return type.GetMethod(name, types)
            ?? throw new MissingMethodException(type.FullName, name);
    }

    internal static MethodInfo GetMethodWithThrowOnError(this Type type, string name, BindingFlags bindingFlags)
    {
        Guard.ThrowIfNull(type);
        Guard.ThrowIfNullOrWhitespace(name);

        return type.GetMethod(name, bindingFlags)
            ?? throw new MissingMethodException(type.FullName, name);
    }

    internal static MethodInfo GetMethodWithThrowOnError(this Type type, string name, BindingFlags bindingFlags, params Type[] types)
    {
        Guard.ThrowIfNull(type);
        Guard.ThrowIfNullOrWhitespace(name);

        return type.GetMethod(name, bindingFlags, null, types, null)
            ?? throw new MissingMethodException(type.FullName, name);
    }

    internal static PropertyInfo GetPropertyWithThrowOnError(this Type type, string name, BindingFlags bindingFlags = BindingFlags.Default)
    {
        Guard.ThrowIfNull(type);
        Guard.ThrowIfNullOrWhitespace(name);

        PropertyInfo[] properties = bindingFlags == BindingFlags.Default
            ? type.GetProperties()
            : type.GetProperties(bindingFlags);

        return properties.FirstOrDefault(x => x.Name == name)
            ?? throw new MissingMemberException(type.FullName, name);
    }

    internal static PropertyInfo GetPropertyWithThrowOnError(this Type type, string name, Type? propertyType, BindingFlags bindingFlags = BindingFlags.Default)
    {
        if (propertyType is null)
            return type.GetPropertyWithThrowOnError(name, bindingFlags);

        Guard.ThrowIfNull(type);
        Guard.ThrowIfNullOrWhitespace(name);

        PropertyInfo[] properties = bindingFlags == BindingFlags.Default
            ? type.GetProperties()
            : type.GetProperties(bindingFlags);

        return properties.FirstOrDefault(x => x.Name == name && x.PropertyType.IsAssignableFrom(propertyType))
            ?? throw new MissingMemberException(type.FullName, name);
    }

    internal static FieldInfo GetFieldWithThrowOnError(this Type type, string name, BindingFlags bindingFlags = BindingFlags.Default)
    {
        Guard.ThrowIfNull(type);
        Guard.ThrowIfNullOrWhitespace(name);

        FieldInfo[] fields = bindingFlags == BindingFlags.Default
            ? type.GetFields()
            : type.GetFields(bindingFlags);

        return fields.FirstOrDefault(x => x.Name == name)
            ?? throw new MissingMemberException(type.FullName, name);
    }

    public static int? GetDepthOfInheritance(this Type type, Type baseType)
    {
        Guard.ThrowIfNull(type);

        if (baseType is null)
            return null;
        else if (baseType.IsGenericTypeDefinition)
            return type.GetDepthOfInheritanceOfRawGeneric(baseType);
        else
            return GetDepthOfInheritanceOfRegularType(type, baseType);
    }

    private static int? GetDepthOfInheritanceOfRegularType(Type type, Type baseType)
    {
        Type? typeToCheck = type;

        for (int depth = 0; typeToCheck is not null; depth++)
        {
            if (typeToCheck == baseType)
                return depth;

            typeToCheck = typeToCheck.BaseType;
        }

        return null;
    }

    public static bool IsInheritedFromOrIs(this Type type, Type baseType)
    {
        Guard.ThrowIfNull(type);

        if (baseType is null)
            return false;
        else if (baseType.IsGenericTypeDefinition)
            return type.GetDepthOfInheritanceOfRawGeneric(baseType) != null;
        else
            return baseType.IsAssignableFrom(type);
    }

    /// <summary>
    /// Gets the base type of the specified raw generic type (e.g. <c>typeof(List&lt;&gt;)</c>).
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="genericType">Type of the generic.</param>
    /// <returns>A <see cref="Type"/> or <see langword="null"/>.</returns>
    public static Type? GetBaseTypeOfRawGeneric(this Type type, Type genericType)
    {
        if (genericType is null)
            return null;

        Type? typeToCheck = type;
        int depth = 0;

        while (typeToCheck is not null && typeToCheck != typeof(object))
        {
            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
                return typeToCheck;

            typeToCheck = typeToCheck.BaseType;
            depth++;
        }

        return null;
    }

    /// <summary>
    /// Returns a full type name in a short form, without namespace(s).
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns>A string representing type name.</returns>
    public static string ToStringInShortForm(this Type type)
    {
        Guard.ThrowIfNull(type);

        return Stringifier.ResolveSimplifiedTypeName(type);
    }
}
