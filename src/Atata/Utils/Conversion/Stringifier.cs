﻿namespace Atata;

/// <summary>
/// Provides a set of static methods for object conversion to readable string.
/// The methods are useful for the formatting of objects for log messages.
/// </summary>
public static class Stringifier
{
    private static readonly ConcurrentDictionary<Type, string> s_typeNamesInShortForm = new()
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
        [typeof(string)] = "string",
        [typeof(char)] = "char"
    };

    private static readonly Dictionary<Type, string> s_typeNamesInAliasForm = new()
    {
        [typeof(bool)] = "bool",
        [typeof(byte)] = "byte",
        [typeof(sbyte)] = "sbyte",
        [typeof(short)] = "short",
        [typeof(ushort)] = "ushort",
        [typeof(int)] = "int",
        [typeof(uint)] = "uint",
        [typeof(long)] = "long",
        [typeof(ulong)] = "ulong",
        [typeof(float)] = "float",
        [typeof(double)] = "double",
        [typeof(decimal)] = "decimal",
        [typeof(string)] = "string",
        [typeof(char)] = "char"
    };

    public const string NullString = "null";

    public const string Indent = "  ";

    private const int EnumerableSingleLinePresentationMaxLength = 70;

    public static string ToString(IEnumerable? collection) =>
        ToString(collection?.Cast<object>());

    public static string ToString<T>(IEnumerable<T>? collection)
    {
        if (collection is null)
            return NullString;

        string[] itemStringValues = [.. collection.Select(x => ToString(x))];

        return itemStringValues.Any(x => x.Contains('\n')
            || itemStringValues.Sum(x => x.Length) + (itemStringValues.Length * 2) > EnumerableSingleLinePresentationMaxLength)
            ? $"[{Environment.NewLine}{string.Join($",{Environment.NewLine}", itemStringValues.Select(AddIndent))}{Environment.NewLine}]"
            : $"[{string.Join(", ", itemStringValues)}]";
    }

    public static string ToString(Expression expression) =>
        $"({ObjectExpressionStringBuilder.ExpressionToString(expression)})";

    public static string ToString(string? value) =>
        ToString(value as object);

    public static string ToString(object? value) =>
        value switch
        {
            null =>
                NullString,
            string =>
                $"\"{value}\"",
            char =>
                $"'{value}'",
            bool =>
                value.ToString().ToLowerInvariant(),
            IEnumerable enumerableValue =>
                ToString(enumerableValue),
            Expression expressionValue =>
                ToString(expressionValue),
            _ =>
                AnyObjectToString(value)
        };

    private static string AnyObjectToString(object value)
    {
        string valueAsString = value.ToString();

        return valueAsString.Contains('\n')
            ? ReplaceNewLines(valueAsString)
            : valueAsString;
    }

    private static string AddIndent(string value) =>
        Indent + value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine + Indent);

    private static string ReplaceNewLines(string value) =>
        value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);

    public static string ToStringInFormOfOneOrMany<T>(IEnumerable<T>? collection)
    {
        if (collection is null)
            return NullString;

        int count = collection.Count();

        return count == 1
            ? ToString(collection.First())
            : ToString(collection);
    }

    public static string ToStringInSimpleStructuredForm(object? value, Type? excludeBaseType = null)
    {
        if (value is null)
            return NullString;

        Type valueType = value.GetType();

        IEnumerable<PropertyInfo> properties = valueType.GetProperties(
            BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

        if (excludeBaseType is not null)
            properties = properties.Where(
                x => excludeBaseType.IsAssignableFrom(x.DeclaringType) && x.DeclaringType != excludeBaseType);

        string[] propertyStrings = [.. from prop in properties
                                       let val = prop.GetValue(value)
                                       where TakeValueForSimpleStructuredForm(val)
                                       select $"{prop.Name}={ToString(val)}"];

        string simplifiedTypeName = ResolveSimplifiedTypeName(valueType);

        if (propertyStrings.Length > 0)
        {
            var builder = new StringBuilder(simplifiedTypeName)
                .Append(" { ");

            builder.Append(string.Join(", ", propertyStrings));
            builder.Append(" }");

            return builder.ToString();
        }
        else
        {
            return simplifiedTypeName;
        }
    }

    public static string ToStringInShortForm(Type? type)
    {
        if (type is null)
            return NullString;

        return ResolveSimplifiedTypeName(type);
    }

    internal static string ResolveSimplifiedTypeName(Type type) =>
        s_typeNamesInShortForm.GetOrAdd(type, DoResolveSimplifiedTypeName);

    private static string DoResolveSimplifiedTypeName(Type type)
    {
        Type[] genericArguments = type.GetGenericArguments();
        Queue<Type>? genericArgumentTypeQueue = genericArguments.Length > 0
            ? new(genericArguments)
            : null;

        StringBuilder outputBuilder = new();
        PrintSimplifiedTypeName(type, genericArgumentTypeQueue, outputBuilder);
        return outputBuilder.ToString();
    }

    private static void PrintSimplifiedTypeName(Type type, Queue<Type>? genericArgumentTypeQueue, StringBuilder outputBuilder)
    {
        Type declaringType = type.DeclaringType;

        if (declaringType is not null)
        {
            PrintSimplifiedTypeName(declaringType, genericArgumentTypeQueue, outputBuilder);
            outputBuilder.Append('.');
        }

        string name = type.Name;
        int indexOfGenericIndicator = name.IndexOf('`');

        if (indexOfGenericIndicator >= 0)
        {
            if (name == "Nullable`1" && Nullable.GetUnderlyingType(type) is { } nullableUnderlyingType)
            {
                PrintSimplifiedTypeName(nullableUnderlyingType, [], outputBuilder);
                outputBuilder.Append('?');
            }
            else
            {
                outputBuilder.Append(name, 0, indexOfGenericIndicator);
                outputBuilder.Append('<');

                int numberOfGenericArguments = int.Parse(name[(indexOfGenericIndicator + 1)..]);

                for (int i = 0; i < numberOfGenericArguments; i++)
                {
                    if (i != 0)
                        outputBuilder.Append(", ");

                    if (genericArgumentTypeQueue is null || genericArgumentTypeQueue.Count == 0)
                        throw new InvalidOperationException("There are no generic argument types to use.");

                    Type genericArgumentType = genericArgumentTypeQueue.Dequeue();
                    Type[] genericArgumentTypeGenericArguments = genericArgumentType.GetGenericArguments();
                    Queue<Type>? genericArgumentTypeGenericArgumentTypeQueue = genericArgumentTypeGenericArguments.Length > 0
                        ? new(genericArgumentTypeGenericArguments)
                        : null;

                    PrintSimplifiedTypeName(genericArgumentType, genericArgumentTypeGenericArgumentTypeQueue, outputBuilder);
                }

                outputBuilder.Append('>');
            }
        }
        else
        {
            outputBuilder.Append(
                declaringType is null && s_typeNamesInAliasForm.TryGetValue(type, out string cachedName)
                    ? cachedName
                    : name);
        }
    }

    private static bool TakeValueForSimpleStructuredForm(object value) =>
        value switch
        {
            null => false,
            Array valueAsArray => valueAsArray.Length > 0,
            IReadOnlyCollection<object> valueAsCollection => valueAsCollection.Count > 0,
            IEnumerable<object> valueAsGenericEnumerable => valueAsGenericEnumerable.Any(),
            IEnumerable valueAsEnumerable => valueAsEnumerable.Cast<object>().Any(),
            _ => true
        };
}
