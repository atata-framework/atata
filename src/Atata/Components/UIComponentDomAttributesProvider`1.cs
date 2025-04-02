#nullable enable

namespace Atata;

/// <summary>
/// Allows to access the component scope element's DOM attribute values.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public sealed class UIComponentDomAttributesProvider<TOwner> : UIComponentPart<TOwner>
    where TOwner : PageObject<TOwner>
{
    private const string AttributeProviderNameFormat = "\"{0}\" DOM attribute";

    internal UIComponentDomAttributesProvider(IUIComponent<TOwner> component)
    {
        Component = component;
        ComponentPartName = "DOM attributes";
    }

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance for the value of the specified control's scope element attribute.
    /// </summary>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> instance for the attribute's current value.</returns>
    public ValueProvider<string?, TOwner> this[string attributeName] =>
        Get<string>(attributeName);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance for the value of the specified control's scope element attribute.
    /// </summary>
    /// <typeparam name="TValue">The type of the attribute value.</typeparam>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> instance for the attribute's current value.</returns>
    public ValueProvider<TValue?, TOwner> Get<TValue>(string attributeName)
    {
        attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

        return Component.CreateValueProvider(
            AttributeProviderNameFormat.FormatWith(attributeName),
            () => GetValue<TValue>(attributeName));
    }

    /// <summary>
    /// Gets the value of the specified control's scope element attribute.
    /// </summary>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>
    /// The attribute's current value.
    /// Returns <see langword="null"/> if the value is not set or the attribute is not found.
    /// </returns>
    public string? GetValue(string attributeName)
    {
        attributeName.CheckNotNullOrWhitespace(nameof(attributeName));

        return Component.Scope.GetDomAttribute(attributeName);
    }

    /// <summary>
    /// Gets the value of the specified control's scope element attribute.
    /// </summary>
    /// <typeparam name="TValue">The type of the attribute value.</typeparam>
    /// <param name="attributeName">The name of the attribute.</param>
    /// <returns>
    /// The attribute's current value.
    /// Returns <see langword="null"/> or default value (for value types) if the value is not set or the attribute is not found.
    /// </returns>
    public TValue? GetValue<TValue>(string attributeName)
    {
        string? valueAsString = GetValue(attributeName);

        return valueAsString is null or [] && typeof(TValue) == typeof(bool)
            ? default
            : TermResolver.FromString<TValue>(valueAsString);
    }
}
