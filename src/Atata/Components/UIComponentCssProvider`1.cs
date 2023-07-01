namespace Atata;

/// <summary>
/// Allows to access the component scope element's CSS property values.
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public class UIComponentCssProvider<TOwner> : UIComponentPart<TOwner>
    where TOwner : PageObject<TOwner>
{
    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of a value of the specified control's scope element CSS property.
    /// </summary>
    /// <param name="propertyName">The name of the CSS property.</param>
    /// <returns>The <see cref="ValueProvider{TValue, TOwner}"/> of the CSS property's current value.</returns>
    public ValueProvider<string, TOwner> this[string propertyName] =>
        Component.CreateValueProvider(propertyName + " CSS property", () => GetValue(propertyName));

    /// <summary>
    /// Gets the value of the specified control's scope element CSS property.
    /// </summary>
    /// <param name="propertyName">The name of the CSS property.</param>
    /// <returns>The CSS property's current value.</returns>
    public string GetValue(string propertyName) =>
        Component.Scope.GetCssValue(propertyName);
}
