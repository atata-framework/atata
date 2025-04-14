﻿namespace Atata;

/// <summary>
/// Allows to access the component scope element's DOM property values.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public sealed class UIComponentDomPropertiesProvider<TOwner> : UIComponentPart<TOwner>
    where TOwner : PageObject<TOwner>
{
    private const string ItemProviderNameFormat = "\"{0}\" DOM property";

    internal UIComponentDomPropertiesProvider(IUIComponent<TOwner> component)
    {
        Component = component;
        ComponentPartName = "DOM properties";
    }

    public ValueProvider<string?, TOwner> Id =>
        Get<string>("id");

    public ValueProvider<string?, TOwner> Name =>
        Get<string>("name");

    public ValueProvider<string?, TOwner> Value =>
        Get<string>("value");

    public ValueProvider<string?, TOwner> Title =>
        Get<string>("title");

    public ValueProvider<string?, TOwner> Style =>
        Get<string>("style");

    public ValueProvider<bool?, TOwner> ReadOnly =>
        Get<bool?>("readOnly");

    public ValueProvider<bool?, TOwner> Required =>
        Get<bool?>("required");

    public ValueProvider<string, TOwner> TextContent =>
        Component.CreateValueProvider(
            ItemProviderNameFormat.FormatWith("textContent"),
            () => GetValue("textContent")?.Trim() ?? string.Empty);

    public ValueProvider<string, TOwner> InnerHtml =>
        Component.CreateValueProvider(
            ItemProviderNameFormat.FormatWith("innerHTML"),
            () => GetValue("innerHTML")?.Trim() ?? string.Empty);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance for the value of the specified control's scope element property.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> instance for the property's current value.</returns>
    public ValueProvider<string?, TOwner> this[string propertyName] =>
        Get<string>(propertyName);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> instance for the value of the specified control's scope element property.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> instance for the property's current value.</returns>
    public ValueProvider<TValue?, TOwner> Get<TValue>(string propertyName)
    {
        Guard.ThrowIfNullOrWhitespace(propertyName);

        return Component.CreateValueProvider(
            ItemProviderNameFormat.FormatWith(propertyName),
            () => GetValue<TValue>(propertyName));
    }

    /// <summary>
    /// Gets the value of the specified control's scope element property.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>
    /// The property's current value.
    /// Returns <see langword="null"/> if the value is not set or the property is not found.
    /// </returns>
    public string? GetValue(string propertyName)
    {
        Guard.ThrowIfNullOrWhitespace(propertyName);

        return Component.Scope.GetDomProperty(propertyName);
    }

    /// <summary>
    /// Gets the value of the specified control's scope property attribute.
    /// </summary>
    /// <typeparam name="TValue">The type of the property value.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>
    /// The property's current value.
    /// Returns <see langword="null"/> or default value (for value types) if the value is not set or the property is not found.
    /// </returns>
    public TValue? GetValue<TValue>(string propertyName)
    {
        string? valueAsString = GetValue(propertyName);

        return valueAsString is null or [] && typeof(TValue) == typeof(bool)
            ? default
            : TermResolver.FromString<TValue>(valueAsString);
    }
}
