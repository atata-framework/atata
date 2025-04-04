namespace Atata;

public static class CustomAttributeExtensions
{
    internal static T? GetCustomAttribute<T>(this MemberInfo element, bool inherit = true)
        where T : notnull, Attribute =>
        element.GetCustomAttributes(typeof(T), inherit).Cast<T>().FirstOrDefault();

    public static bool TryGetCustomAttribute<T>(this MemberInfo element, [NotNullWhen(true)] out T? attribute, bool inherit = true)
        where T : notnull, Attribute
    {
        attribute = element.GetCustomAttribute<T>(inherit);
        return attribute is not null;
    }
}
