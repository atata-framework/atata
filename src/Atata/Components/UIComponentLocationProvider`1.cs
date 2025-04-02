#nullable enable

namespace Atata;

/// <summary>
/// Allows to access the component scope element's location (X and Y).
/// </summary>
/// <typeparam name="TOwner">The type of the owner page object.</typeparam>
public sealed class UIComponentLocationProvider<TOwner> : ValueProvider<Point, TOwner>
    where TOwner : PageObject<TOwner>
{
    private readonly UIComponent<TOwner> _component;

    internal UIComponentLocationProvider(
        UIComponent<TOwner> component,
        Func<Point> valueGetFunction,
        string providerName)
        : base(
            component.Owner,
            DynamicObjectSource.Create(valueGetFunction),
            providerName,
            component.Session.ExecutionUnit) =>
        _component = component;

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the X location coordinate of the component.
    /// </summary>
    public ValueProvider<int, TOwner> X =>
        _component.CreateValueProvider("X location", GetX);

    /// <summary>
    /// Gets the <see cref="ValueProvider{TValue, TOwner}"/> of the Y location coordinate of the component.
    /// </summary>
    public ValueProvider<int, TOwner> Y =>
        _component.CreateValueProvider("Y location", GetY);

    private int GetX() =>
        Value.X;

    private int GetY() =>
        Value.Y;
}
