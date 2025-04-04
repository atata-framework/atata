#nullable enable

namespace Atata;

/// <summary>
/// Provides a set of extension methods for <see cref="FindAttribute"/>.
/// </summary>
public static class FindAttributeExtensions
{
    public static TAttribute OfAnyVisibility<TAttribute>(this TAttribute attribute)
        where TAttribute : FindAttribute
    {
        attribute.Visibility = Visibility.Any;
        return attribute;
    }

    public static TAttribute Visible<TAttribute>(this TAttribute attribute)
        where TAttribute : FindAttribute
    {
        attribute.Visibility = Visibility.Visible;
        return attribute;
    }

    public static TAttribute Hidden<TAttribute>(this TAttribute attribute)
        where TAttribute : FindAttribute
    {
        attribute.Visibility = Visibility.Hidden;
        return attribute;
    }
}
