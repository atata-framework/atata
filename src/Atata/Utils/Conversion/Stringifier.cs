namespace Atata;

/// <summary>
/// Provides a set of static methods for object conversion to readable string.
/// The methods are useful for the formatting of objects for log messages.
/// </summary>
public static class Stringifier
{
    public const string NullString = "null";

    public const string Indent = "  ";

    private static readonly Lazy<Func<WebElement, string>> s_elementIdRetrieveFunction = new(() =>
    {
        var idProperty = typeof(WebElement).GetPropertyWithThrowOnError(
            "Id",
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        var parameterExpression = Expression.Parameter(typeof(WebElement), "item");

        var propertyExpression = Expression.Property(parameterExpression, idProperty);

        return Expression.Lambda<Func<WebElement, string>>(propertyExpression, parameterExpression)
            .Compile();
    });

    public static string ToString(IEnumerable collection) =>
        ToString(collection?.Cast<object>());

    public static string ToString<T>(IEnumerable<T> collection)
    {
        if (collection == null)
            return NullString;

        string[] itemStringValues = collection.Select(x => ToString(x)).ToArray();

        return itemStringValues.Any(x => x.Contains('\n'))
            ? $"[{Environment.NewLine}{string.Join($",{Environment.NewLine}", itemStringValues.Select(AddIndent))}{Environment.NewLine}]"
            : $"[{string.Join(", ", itemStringValues)}]";
    }

    public static string ToString(Expression expression) =>
        $"({ObjectExpressionStringBuilder.ExpressionToString(expression)})";

    public static string ToString(object value) =>
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
            ValueType =>
                value.ToString(),
            IEnumerable enumerableValue =>
                ToString(enumerableValue),
            Expression expressionValue =>
                ToString(expressionValue),
            WebElement asWebElement =>
                $"Element {{ Id={s_elementIdRetrieveFunction.Value.Invoke(asWebElement)} }}",
            _ =>
                AnyObjectToString(value)
        };

    private static string AnyObjectToString(object value)
    {
        string valueAsString = value.ToString();
        if (valueAsString.Contains('\n'))
        {
            string indentedValue = AddIndent(valueAsString);
            return $"{{{Environment.NewLine}{indentedValue}{Environment.NewLine}}}";
        }
        else
        {
            return $"{{ {value} }}";
        }
    }

    private static string AddIndent(string value) =>
        Indent + value.Replace("\r\n", "\n").Replace("\n", Environment.NewLine + Indent);

    public static string ToStringInFormOfOneOrMany<T>(IEnumerable<T> collection)
    {
        if (collection == null)
            return NullString;

        int count = collection.Count();

        return count == 1
            ? ToString(collection.First())
            : ToString(collection);
    }

    public static string ToStringInSimpleStructuredForm(object value, Type excludeBaseType = null)
    {
        if (Equals(value, null))
            return NullString;

        Type valueType = value.GetType();

        IEnumerable<PropertyInfo> properties = valueType.GetProperties(
            BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.Public);

        if (excludeBaseType != null)
            properties = properties.Where(
                x => excludeBaseType.IsAssignableFrom(x.DeclaringType) && x.DeclaringType != excludeBaseType);

        string[] propertyStrings = (from prop in properties
                                    let val = prop.GetValue(value)
                                    where TakeValueForSimpleStructuredForm(val)
                                    select $"{prop.Name}={ToString(val)}").ToArray();

        string simplifiedTypeName = ResolveSimplifiedTypeName(valueType);
        StringBuilder builder = new StringBuilder(simplifiedTypeName);

        if (propertyStrings.Length > 0)
            builder.Append($" {{ {string.Join(", ", propertyStrings)} }}");

        return builder.ToString();
    }

    private static string ResolveSimplifiedTypeName(Type type)
    {
        string name = type.Name;

        if (type.IsGenericType)
        {
            Type[] genericArgumentTypes = type.GetGenericArguments();
            string genericArgumentsString = string.Join(", ", genericArgumentTypes.Select(ResolveSimplifiedTypeName));

            return $"{name.Substring(0, name.IndexOf('`'))}<{genericArgumentsString}>";
        }

        return name;
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
